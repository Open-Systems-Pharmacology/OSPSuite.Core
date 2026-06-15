using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SelectedCurveValues : ContextSpecification<SelectedCurveValues>
   {
      protected override void Context()
      {
         sut = new SelectedCurveValues();
      }
   }

   public class When_validating_selected_curve_values_without_a_line_thickness : concern_for_SelectedCurveValues
   {
      [Observation]
      public void should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_selected_curve_values_with_a_line_thickness_within_the_allowed_range : concern_for_SelectedCurveValues
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

   public class When_validating_selected_curve_values_with_a_line_thickness_below_the_allowed_range : concern_for_SelectedCurveValues
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

   public class When_validating_selected_curve_values_with_a_line_thickness_above_the_allowed_range : concern_for_SelectedCurveValues
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
}
