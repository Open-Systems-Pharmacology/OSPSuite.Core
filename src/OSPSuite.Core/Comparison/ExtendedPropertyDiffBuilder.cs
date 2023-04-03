using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ExtendedPropertyDiffBuilder : DiffBuilder<IExtendedProperty>
   {
      public override void Compare(IComparison<IExtendedProperty> comparison)
      {
         CompareStringValues(x => x.Name, x => x.Name, comparison);
         CompareStringValues(x => x.ValueAsObject?.ToString(), Captions.Value, comparison);

         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareStringValues(x => x.DisplayName, x => x.DisplayName, comparison);
         CompareStringValues(x => x.Description, x => x.Description, comparison);
      }
   }
}