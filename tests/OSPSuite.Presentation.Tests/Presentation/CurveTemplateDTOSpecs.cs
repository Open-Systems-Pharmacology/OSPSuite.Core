using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CurveTemplateDTO : ContextSpecification<CurveTemplateDTO>
   {
      protected override void Context()
      {
         sut = new CurveTemplateDTO(new CurveTemplate())
         {
            Name = "Curve Template",
            xDataPath = "Organism|Time",
            yDataPath = "Organism|Concentration"
         };
      }
   }

   public class When_validating_a_curve_template_dto_with_a_line_thickness_within_the_allowed_range : concern_for_CurveTemplateDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.LineThickness = Constants.MAX_LINE_THICKNESS;
      }

      [Observation]
      public void should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_a_curve_template_dto_with_a_line_thickness_outside_the_allowed_range : concern_for_CurveTemplateDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.LineThickness = Constants.MAX_LINE_THICKNESS + 1;
      }

      [Observation]
      public void should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_a_curve_template_dto_with_a_line_thickness_below_the_allowed_range : concern_for_CurveTemplateDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.LineThickness = Constants.MIN_LINE_THICKNESS - 1;
      }

      [Observation]
      public void should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
