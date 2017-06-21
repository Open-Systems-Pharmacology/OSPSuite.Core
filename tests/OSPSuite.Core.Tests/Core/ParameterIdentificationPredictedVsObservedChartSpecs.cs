using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationPredictedVsObservedChart : ContextSpecification<ParameterIdentificationPredictedVsObservedChart>
   {
      protected override void Context()
      {
         sut = new ParameterIdentificationPredictedVsObservedChart().WithAxes();
         sut.AddNewAxis();
         sut.AddNewAxis();
      }
   }

   public class When_updating_axis_visibility : concern_for_ParameterIdentificationPredictedVsObservedChart
   {
      protected override void Context()
      {
         base.Context();
         sut.AxisBy(AxisTypes.Y).Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         sut.AxisBy(AxisTypes.Y2).Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
         sut.AxisBy(AxisTypes.Y3).Dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         sut.AxisBy(AxisTypes.X).Dimension = sut.AxisBy(AxisTypes.Y3).Dimension;
      }

      protected override void Because()
      {
         sut.UpdateAxesVisibility();
      }

      [Observation]
      public void the_x_axis_should_be_visible()
      {
         sut.AxisBy(AxisTypes.X).Visible.ShouldBeTrue();
      }

      [Observation]
      public void the_y_axes_with_same_dimension_is_visible()
      {
         sut.AxisBy(AxisTypes.Y3).Visible.ShouldBeTrue();
      }

      [Observation]
      public void axes_with_other_dimensions_are_not_visible()
      {
         sut.AxisBy(AxisTypes.Y2).Visible.ShouldBeFalse();
         sut.AxisBy(AxisTypes.Y).Visible.ShouldBeFalse();
      }
   }
}
