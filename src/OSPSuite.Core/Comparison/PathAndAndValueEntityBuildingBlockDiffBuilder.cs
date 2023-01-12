using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class PathAndAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> where TBuilder : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected PathAndAndValueEntityBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer, x => x.Path)
      {
         
      }
   }
}