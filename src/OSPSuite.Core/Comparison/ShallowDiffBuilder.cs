using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   public abstract class ShallowDiffBuilder<T> : DiffBuilder<T> where T : class
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      protected ShallowDiffBuilder(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      /// <summary>
      ///    This method compares two lists of named objects extracted from the specified property of
      ///    two objects. It identifies objects that are present in one list but not the other and records these
      ///    differences in the provided <paramref name="comparison" /> context. The order is not a relevant difference
      /// </summary>
      protected void AddPresentByNameDifference<TNamedObject>(Expression<Func<T, IReadOnlyList<TNamedObject>>> enumerationExpression, IComparison<T> comparison) where TNamedObject : class, IWithName
      {
         var func = enumerationExpression.Compile();
         var list1 = func(comparison.Object1);
         var list2 = func(comparison.Object2);

         var presentOnlyInList1 = list1.Where(x => !list2.AllNames().Contains(x.Name));
         var presentOnlyInList2 = list2.Where(x => !list1.AllNames().Contains(x.Name));

         presentOnlyInList1.Each(x =>
         {
            comparison.Add(new MissingDiffItem
            {
               Object1 = x,
               Object2 = null,
               MissingObject1 = x,
               MissingObject2 = null,
               MissingObjectType = _objectTypeResolver.TypeFor(x),
               MissingObjectName = x.Name,
               Description = Captions.Diff.OneObjectIsNull,
               CommonAncestor = comparison.CommonAncestor
            });
         });

         presentOnlyInList2.Each(x =>
         {
            comparison.Add(new MissingDiffItem
            {
               Object1 = null,
               Object2 = x,
               MissingObject1 = null,
               MissingObject2 = x,
               MissingObjectType = _objectTypeResolver.TypeFor(x),
               MissingObjectName = x.Name,
               Description = Captions.Diff.OneObjectIsNull,
               CommonAncestor = comparison.CommonAncestor
            });
         });
      }

      /// <summary>
      ///    Adds a difference item to the comparison if either object is null, but not if both are null, or if the names do not
      ///    match.
      ///    The comparison is not extended to child objects
      /// </summary>
      protected void AddShallowDifference<TNamedObject>(Expression<Func<T, TNamedObject>> namedElementRetriever, IComparison<T> comparison) where TNamedObject : class, IWithName
      {
         var propertyName = NameFrom(namedElementRetriever);
         var func = namedElementRetriever.Compile();
         var namedElement1 = func(comparison.Object1);
         var namedElement2 = func(comparison.Object2);

         // Only if one element is null and the other isn't
         if ((namedElement1 == null) ^ (namedElement2 == null))
            comparison.Add(missingDiffItem(namedElement1, namedElement2, comparison, propertyName));

         else if (shouldCompareNames(namedElement2, namedElement1))
            comparison.Add(nameDiffItem(namedElement1, namedElement2, comparison, propertyName));
      }

      private static bool shouldCompareNames<TNamedObject>(TNamedObject namedElement2, TNamedObject namedElement1) where TNamedObject : class, IWithName =>
         namedElement2 != null && namedElement1 != null && !string.Equals(namedElement1.Name, namedElement2.Name);

      private DiffItem missingDiffItem<TNamedObject>(TNamedObject namedElement1, TNamedObject namedElement2, IComparison<T> comparison, string objectName) where TNamedObject : class, IWithName
      {
         var presentItem = namedElement1 ?? namedElement2;
         return new MissingDiffItem
         {
            Object1 = namedElement1,
            Object2 = namedElement2,
            MissingObject1 = namedElement1,
            MissingObject2 = namedElement2,
            MissingObjectType = _objectTypeResolver.TypeFor(presentItem),
            Description = Captions.Diff.OneObjectIsNull,
            MissingObjectName = objectName,
            CommonAncestor = comparison.CommonAncestor
         };
      }

      private DiffItem nameDiffItem<TNamedObject>(TNamedObject namedElement1, TNamedObject namedElement2, IComparison<T> comparison, string propertyName) where TNamedObject : class, IWithName
      {
         var formattedValue1 = namedElement1.Name;
         var formattedValue2 = namedElement2.Name;
         return new PropertyValueDiffItem
         {
            Object1 = comparison.Object1,
            Object2 = comparison.Object2,
            CommonAncestor = comparison.CommonAncestor,
            FormattedValue1 = formattedValue1,
            FormattedValue2 = formattedValue2,
            PropertyName = propertyName,
            Description = Captions.Diff.PropertyDiffers(propertyName, formattedValue1, formattedValue2)
         };
      }
   }
}