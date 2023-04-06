using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameter : PathAndValueEntity
   {
      /// <summary>
      /// Only set for a parameter that is a distributed parameter.
      /// This is required in order to be able to create distributed parameter dynamically in the simulation
      /// </summary>
      public DistributionType? DistributionType { get; set; }
   }
}