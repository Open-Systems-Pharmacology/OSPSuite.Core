using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Comparison
{
   public abstract class PathAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> where TBuilder : PathAndValueEntity 
      where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected PathAndValueEntityBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer, x => x.Path)
      {

      }

      protected override IEnumerable<TBuilder> GetBuilders(TBuildingBlock buildingBlock)
      {
         return base.GetBuilders(buildingBlock).Where(x => !isSubParameter(x, buildingBlock.Where(parameter => parameter.IsDistributed()).Select(parameter => parameter.Path)));
      }

      private bool isSubParameter(TBuilder pathAndValueEntity, IEnumerable<ObjectPath> distributedPaths)
      {
         return distributedPaths.Any(x => pathAndValueEntity.ContainerPath.Equals(x));
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