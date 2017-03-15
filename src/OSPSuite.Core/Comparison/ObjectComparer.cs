using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   /// <summary>
   ///    Comparison services in charge of comparing two object base objects.
   /// </summary>
   public interface IObjectComparer
   {
      /// <summary>
      ///    Compare the object <paramref name="object1" /> and <paramref name="object2" /> using the default settings and
      ///    returns a report containg the difference between the two objects
      /// </summary>
      /// <param name="object1"> First object to compare </param>
      /// <param name="object2"> Second object to compare </param>
      /// <returns> The report containing the difference between the two objects </returns>
      DiffReport Compare(object object1, object object2);

      /// <summary>
      ///    Compare the object <paramref name="object1" /> and <paramref name="object2" /> using the given
      ///    <paramref
      ///       name="settings" />
      ///    and returns a report containg the difference between the two objects
      /// </summary>
      /// <param name="object1"> First object to compare </param>
      /// <param name="object2"> Second object to compare </param>
      /// <param name="settings"> Settings used for the comparison </param>
      /// <returns> The report containing the difference between the two objects </returns>
      DiffReport Compare(object object1, object object2, ComparerSettings settings);

      /// <summary>
      ///    Starts a comparison using the parameter defined in <paramref name="comparison" />
      /// </summary>
      void Compare<TObject>(IComparison<TObject> comparison) where TObject : class;
   }

   public class ObjectComparer : IObjectComparer
   {
      private readonly IDiffBuilderRepository _diffBuilderRepository;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ObjectComparer(IDiffBuilderRepository diffBuilderRepository, IObjectTypeResolver objectTypeResolver)
      {
         _diffBuilderRepository = diffBuilderRepository;
         _objectTypeResolver = objectTypeResolver;
      }

      public DiffReport Compare(object object1, object object2)
      {
         return Compare(object1, object2, new ComparerSettings());
      }

      public DiffReport Compare(object object1, object object2, ComparerSettings settings)
      {
         var report = new DiffReport();
         compare(object1, object2, settings, report);
         return report;
      }

      public void Compare<TObject>(IComparison<TObject> comparison) where TObject : class
      {
         compare(comparison.Object1, comparison.Object2, comparison.Settings, comparison.Report, comparison.CommonAncestor);
      }

      private void compare(object x1, object x2, ComparerSettings settings, DiffReport report, object commonAncestor = null)
      {
         //If both null return true
         if (x1 == null && x2 == null)
            return;

         //Check if one of them is null
         if (x1 == null || x2 == null)
         {
            var missingObject = x1 ?? x2;
            report.Add(new MissingDiffItem
            {
               Object1 = x1,
               Object2 = x2,
               MissingObject1 = x1,
               MissingObject2 = x2,
               MissingObjectType = _objectTypeResolver.TypeFor(missingObject),
               Description = Captions.Diff.OneObjectIsNull,
               CommonAncestor = commonAncestor
            });
            return;
         }

         var type1 = x1.GetType();
         var type2 = x2.GetType();

         if (type1 != type2)
         {
            report.Add(new MismatchDiffItem
            {
               Object1 = x1,
               Object2 = x2,
               Description = Captions.Diff.DifferentTypes(_objectTypeResolver.TypeFor(x1), _objectTypeResolver.TypeFor(x2)),
               CommonAncestor = commonAncestor
            });
            return;
         }

         var diffBuilder = _diffBuilderRepository.DiffBuilderFor(type1);
         diffBuilder?.CompareObjects(x1, x2, settings, report, commonAncestor);
      }
   }
}