using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class OriginDataItemDiffBuilder : DiffBuilder<OriginDataItem>
   {
      public override void Compare(IComparison<OriginDataItem> comparison)
      {
         CompareStringValues(x => x.Name, x => x.Name, comparison);
         CompareStringValues(x => x.Value, x => x.Value, comparison);

         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareStringValues(x => x.DisplayName, x => x.DisplayName, comparison);
         CompareStringValues(x => x.Icon, x => x.Icon, comparison);
         CompareStringValues(x => x.Description, x => x.Description, comparison);
      }
   }
}