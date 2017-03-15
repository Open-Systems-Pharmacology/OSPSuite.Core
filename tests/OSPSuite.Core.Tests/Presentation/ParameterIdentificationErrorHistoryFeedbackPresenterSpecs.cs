using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationErrorHistoryFeedbackPresenter : ContextSpecification<IParameterIdentificationErrorHistoryFeedbackPresenter>
   {
      private IDimensionFactory _dimensionFactory;
      private IParameterIdentificationErrorHistoryFeedbackView _view;
      protected IChartDisplayPresenter _chartDisplayPresenter;
      protected ParameterIdentificationRunState _runState;
      protected List<float> _errorHistory;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _view = A.Fake<IParameterIdentificationErrorHistoryFeedbackView>();
         _chartDisplayPresenter = A.Fake<IChartDisplayPresenter>();
         sut = new ParameterIdentificationErrorHistoryFeedbackPresenter(_view, _chartDisplayPresenter, _dimensionFactory);

         _runState = A.Fake<ParameterIdentificationRunState>();
         _errorHistory = new List<float>();
         A.CallTo(() => _runState.ErrorHistory).Returns(_errorHistory);
      }
   }

   public class When_updating_the_error_history_feedback_for_a_given_parameter_identification : concern_for_ParameterIdentificationErrorHistoryFeedbackPresenter
   {
      private ParameterIdentification _parameterIdentification;
      private ICurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut.EditParameterIdentification(_parameterIdentification);
         _errorHistory.AddRange(new[] {10f, 11f, 12f});
         _chart = _chartDisplayPresenter.DataSource;
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void should_update_the_error_values_according_to_the_run_status()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var errorCurve = _chart.Curves.ElementAt(0);
         errorCurve.xData.Values.ShouldOnlyContainInOrder(1f, 2f, 3f);
         errorCurve.yData.Values.ShouldBeEqualTo(_errorHistory);
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_have_set_the_chart_axis_captions_as_expected()
      {
         _chart.Axes[AxisTypes.X].Caption.ShouldBeEqualTo(Captions.ParameterIdentification.NumberOfEvaluations);
         _chart.Axes[AxisTypes.Y].Caption.ShouldBeEqualTo(Captions.ParameterIdentification.TotalError);
      }

      [Observation]
      public void should_have_set_the_y_axis_as_linear_scale()
      {
         _chart.Axes[AxisTypes.Y].Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }
}