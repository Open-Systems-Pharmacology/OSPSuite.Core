using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class PathAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> where TBuilder : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected PathAndValueEntityBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer, x => x.Path)
      {
      }
   }

   public abstract class PathAndValueEntityBuildingBlockFromPKSimDiffBuilder<TBuildingBlock, TBuilder> : PathAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TBuilder> where TBuilder : PathAndValueEntity where TBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<TBuilder>
   {
      protected PathAndValueEntityBuildingBlockFromPKSimDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }

      public override void Compare(IComparison<TBuildingBlock> comparison)
      {
         base.Compare(comparison);
         CompareValues(x => x.PKSimVersion, x => x.PKSimVersion, comparison);
      }
   }
}