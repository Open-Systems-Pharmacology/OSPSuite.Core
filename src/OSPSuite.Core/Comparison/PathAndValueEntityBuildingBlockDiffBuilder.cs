using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
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

      public override void Compare(IComparison<TBuildingBlock> comparison)
      {
         base.Compare(comparison);
         compareSubParametersDefinedInBothBuildingBlocks(comparison);
      }

      /// <summary>
      ///    Sub parameters of distributed parameters (mean, deviation, ...) are left out of the main comparison so that they are
      ///    not reported as missing when only one building block defines them. They are compared only when both building blocks
      ///    define them, as a different distribution is a real difference.
      /// </summary>
      protected override IEnumerable<TBuilder> GetBuilders(TBuildingBlock buildingBlock)
      {
         return base.GetBuilders(buildingBlock).Where(x => !isSubParameter(x, buildingBlock));
      }

      private void compareSubParametersDefinedInBothBuildingBlocks(IComparison<TBuildingBlock> comparison)
      {
         var subParameterPaths2 = subParametersOf(comparison.Object2).Select(x => x.Path).ToList();
         var commonPaths = subParametersOf(comparison.Object1).Select(x => x.Path).Where(path => subParameterPaths2.Any(path.Equals)).ToList();
         if (!commonPaths.Any())
            return;

         _enumerableComparer.CompareEnumerables(comparison, buildingBlock => subParametersOf(buildingBlock).Where(x => commonPaths.Any(x.Path.Equals)), _equalityProperty, _presentObjectDetailsFunc);
      }

      private IEnumerable<TBuilder> subParametersOf(TBuildingBlock buildingBlock) => buildingBlock.Where(x => isSubParameter(x, buildingBlock));

      private static bool isSubParameter(TBuilder pathAndValueEntity, TBuildingBlock buildingBlock)
      {
         var distributedPaths = buildingBlock.Where(parameter => parameter.IsDistributed()).Select(parameter => parameter.Path);
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
         CompareValues(x => x.PKSimVersion, Captions.Diff.PKSimVersion, comparison);
      }
   }
}