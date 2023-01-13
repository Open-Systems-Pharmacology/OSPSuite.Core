using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ExpressionProfileBuildingBlockDiffBuilder : PathAndValueEntityBuildingBlockDiffBuilder<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      public ExpressionProfileBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {

      }

      public override void Compare(IComparison<ExpressionProfileBuildingBlock> comparison)
      {
         base.Compare(comparison);
         CompareValues(x => x.PKSimVersion, x => x.PKSimVersion, comparison);
         CompareStringValues(x => x.Name, x => x.Name, comparison);
         CompareValues(x => x.Type, x => x.Type, comparison);
      }
   }

   internal class ExpressionParameterDiffBuilder : PathAndValueEntityDiffBuilder<ExpressionParameter>
   {
      public ExpressionParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder) : base(objectComparer, entityDiffBuilder)
      {
      }
   }
}
