namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class ParameterFeedbackDTO : ConstraintParameterDTO
   {
      public ValueDTO Best { get; set; }
      public ValueDTO Current { get; set; }

      public override double ValueForBoundaryCheck => Best.Value;
   }
}