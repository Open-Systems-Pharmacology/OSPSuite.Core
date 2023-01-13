using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class PathAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> where TBuilder : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected PathAndValueEntityBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer, x => x.Path)
      {
         
      }
   }
}