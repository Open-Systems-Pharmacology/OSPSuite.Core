using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ExpressionProfileBuildingBlockDiffBuilder : PathAndValueEntityBuildingBlockFromPKSimDiffBuilder<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      public ExpressionProfileBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }

      public override void Compare(IComparison<ExpressionProfileBuildingBlock> comparison)
      {
         base.Compare(comparison);
         CompareValues(x => x.Type, x => x.Type, comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.InitialConditions, x => x.Path);
      }
   }

   internal class ExpressionParameterDiffBuilder : PathAndValueEntityDiffBuilder<ExpressionParameter>
   {
      public ExpressionParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder) : base(objectComparer, entityDiffBuilder)
      {
      }
   }
}