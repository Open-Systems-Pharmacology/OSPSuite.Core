using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ExpressionProfileBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<ExpressionParameter>
   {
      public ExpressionProfileBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      { 
      }
   }

   internal class ExpressionParameterDiffBuilder : StartValueDiffBuilder<ExpressionParameter>
   {

      public ExpressionParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<ExpressionParameter> valueOriginComparison) : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }
   }
}
