using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class OptimizedParameterDTO : ConstraintParameterDTO
   {
      public ValueDTO OptimalValue { get; set; }
      public ValueDTO StartValue { get; set; }

      public override double ValueForBoundaryCheck => OptimalValue.Value;
      public Scalings Scaling { get; set; }
   }
}