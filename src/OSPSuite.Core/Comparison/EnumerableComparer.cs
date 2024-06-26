﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   public class EnumerableComparer
   {
      private readonly IObjectComparer _objectComparer;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IDisplayNameProvider _displayNameProvider;

      public EnumerableComparer(IObjectComparer objectComparer, IObjectTypeResolver objectTypeResolver, IDisplayNameProvider displayNameProvider)
      {
         _objectComparer = objectComparer;
         _objectTypeResolver = objectTypeResolver;
         _displayNameProvider = displayNameProvider;
      }

      /// <summary>
      ///    Compares the enumerables defined by the <paramref name="getEnumeration" /> properties and creates
      ///    builds recursively the comparison for the item of type <typeparamref name="TItem" /> that differs
      /// </summary>
      /// <typeparam name="TParent">Type of parent</typeparam>
      /// <typeparam name="TItem">Type of item in enumeration</typeparam>
      /// <typeparam name="TResult">Type returned by the <paramref name="equalityProperty" /> object</typeparam>
      /// <param name="comparison">The current comparison</param>
      /// <param name="getEnumeration">
      ///    Function returning the enumeration of <typeparamref name="TItem" /> for which the
      ///    comparison should be built
      /// </param>
      /// <param name="equalityProperty">Method returning the property to use to compare the object</param>
      /// <param name="missingItemType">
      ///    Optional parameter. If set, the value will be used and displayed as type of the missing object.
      ///    Otherwise the type will be resolved dynamically
      /// </param>
      /// <param name="presentObjectDetailsFunc">
      ///    Optional parameter. If set, the method will be called to retrieve some information on the present object that should
      ///    help understand the context
      /// </param>
      public void CompareEnumerables<TParent, TItem, TResult>(IComparison<TParent> comparison,
         Func<TParent, IEnumerable<TItem>> getEnumeration,
         Func<TItem, TResult> equalityProperty,
         Func<TItem, string> presentObjectDetailsFunc = null,
         string missingItemType = null
      )
         where TParent : class
         where TItem : class
      {
         CompareEnumerables(comparison,
            getEnumeration,
            (item1, item2) => Equals(equalityProperty(item1), equalityProperty(item2)),
            item => equalityProperty(item).ToString(),
            presentObjectDetailsFunc,
            missingItemType
         );
      }

      public void CompareEnumerablesByIndex<TParent, TItem>(IComparison<TParent> comparison,
         Func<TParent, IEnumerable<TItem>> getEnumeration)
         where TParent : class
         where TItem : class
      {
         var object1 = comparison.Object1;
         var object2 = comparison.Object2;
         var list1 = getEnumeration(object1).ToList();
         var list2 = getEnumeration(object2).ToList();

         if (list1.Count != list2.Count)
         {
            comparison.Add( new PropertyValueDiffItem
            {
               Object1 = comparison.Object1,
               Object2 = comparison.Object2,
               CommonAncestor = comparison.CommonAncestor,
               PropertyName = Captions.Diff.Count,
               FormattedValue1 = $"{list1.Count}",
               FormattedValue2 = $"{list2.Count}",
               Description = Captions.Diff.PropertyDiffers(Captions.Diff.Count, list1.Count, list2.Count)
            });
            return;
         }

         list1.Each((entity1, index) =>
         {
            var entity2 = list2[index];
            var childComparison = new Comparison<TItem>(entity1, entity2, comparison.Settings, comparison.Report, object1);
            _objectComparer.Compare(childComparison);
         });
      }

      public void CompareEnumerables<TParent, TItem>(IComparison<TParent> comparison,
         Func<TParent, IEnumerable<TItem>> getEnumeration,
         Func<TItem, TItem, bool> equalityFunc,
         Func<TItem, string> identifierRetrieverFunc,
         Func<TItem, string> presentObjectDetailsFunc = null,
         string missingItemType = null)
         where TParent : class
         where TItem : class
      {
         var object1 = comparison.Object1;
         var object2 = comparison.Object2;
         var list1 = getEnumeration(object1).ToList();
         var list2 = getEnumeration(object2).ToList();
         string getMissingType(TItem item) => missingItemType ?? _objectTypeResolver.TypeFor(item);
         string getDetails(TItem item) => presentObjectDetailsFunc?.Invoke(item) ?? string.Empty;


         foreach (var entity1 in list1)
         {
            var entity2 = list2.FirstOrDefault(item => equalityFunc(item, entity1));
            if (entity2 != null)
            {
               var childComparison = new Comparison<TItem>(entity1, entity2, comparison.Settings, comparison.Report, object1);
               _objectComparer.Compare(childComparison);
            }
            else
               comparison.Add(missingItem(object1, object2, entity1, null, getMissingType(entity1), identifierRetrieverFunc(entity1), getDetails(entity1)));
         }

         //all common entity have been added. Add missing entity from object1 base on object2
         foreach (var entity2 in list2)
         {
            if (list1.Any(item => equalityFunc(item, entity2)))
               continue;

            comparison.Add(missingItem(object1, object2, null, entity2, getMissingType(entity2), identifierRetrieverFunc(entity2), getDetails(entity2)));
         }
      }

      private DiffItem missingItem<TMissing>(object object1, object object2, TMissing missingObjectFromObject1,
         TMissing missingObjectFromObject2, string missingObjectType, string missingObjectName, string presentObjectDetails) where TMissing : class
      {
         var containerType = _objectTypeResolver.TypeFor(object1);
         var containerName = _displayNameProvider.DisplayNameFor(object1);
         return new MissingDiffItem
         {
            Object1 = object1,
            Object2 = object2,
            MissingObject1 = missingObjectFromObject1,
            MissingObject2 = missingObjectFromObject2,
            MissingObjectName = missingObjectName,
            MissingObjectType = missingObjectType,
            Description = Captions.Diff.ObjectMissing(containerType, containerName, missingObjectType, missingObjectName),
            PresentObjectDetails = presentObjectDetails,
            CommonAncestor = object1
         };
      }
   }
}