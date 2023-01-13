using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class IndividualBuildingBlockDiffBuilder : PathAndAndValueEntityBuildingBlockDiffBuilder<IndividualBuildingBlock, IndividualParameter>
   {
      private readonly IObjectComparer _objectComparer;

      public IndividualBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer, IObjectComparer objectComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IndividualBuildingBlock> comparison)
      {
         base.Compare(comparison);
         CompareValues(x => x.PKSimVersion, x => x.PKSimVersion, comparison);
         CompareStringValues(x => x.Name, x => x.Name, comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.OriginData));
      }
   }

   public class IndividualParameterDiffBuilder : PathAndValueEntityDiffBuilder<IndividualParameter>
   {
      public IndividualParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder) : base(objectComparer, entityDiffBuilder)
      {
      }
   }

   public class OriginDataItemsDiffBuilder : DiffBuilder<OriginDataItems>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public OriginDataItemsDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }
      public override void Compare(IComparison<OriginDataItems> comparison)
      {
         _enumerableComparer.CompareEnumerables(comparison, x => x.AllDataItems, x => x.Name);
      }
   }

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