using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
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

   public class When_creating_a_parameter_identifcation_history_feedback_presenter: concern_for_ParameterIdentificationErrorHistoryFeedbackPresenter
   {
      [Observation]
      public void should_initialize_the_chart_presenter_to_use_specific_display_settings_for_feedback()
      {
         A.CallTo(() => _chartDisplayPresenter.Edit(A<CurveChart>._, A<ChartFontAndSizeSettings>.That.Matches(x=>x.Fonts.TitleSize == Constants.ChartFontOptions.DEFAULT_FONT_SIZE_TITLE_FOR_PARAMETER_IDENTIFICATION_FEEDBACK))).MustHaveHappened();
      }
   }

   public class When_updating_the_error_history_feedback_for_a_given_parameter_identification : concern_for_ParameterIdentificationErrorHistoryFeedbackPresenter
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut.EditParameterIdentification(_parameterIdentification);
         _errorHistory.AddRange(new[] {10f, 11f, 12f});
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void should_update_the_error_values_according_to_the_run_status()
      {
         sut.Chart.Curves.Count.ShouldBeEqualTo(1);
         var errorCurve = sut.Chart.Curves.ElementAt(0);
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
         sut.Chart.AxisBy(AxisTypes.X).Caption.ShouldBeEqualTo(Captions.ParameterIdentification.NumberOfEvaluations);
         sut.Chart.AxisBy(AxisTypes.Y).Caption.ShouldBeEqualTo(Captions.ParameterIdentification.TotalError);
      }

      [Observation]
      public void should_have_set_the_y_axis_as_linear_scale()
      {
         sut.Chart.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_resetting_the_feedback_in_the_parameter_identification_error_feedback_presenter : concern_for_ParameterIdentificationErrorHistoryFeedbackPresenter
   {
      protected override void Because()
      {
         sut.ResetFeedback();
      }

      [Observation]
      public void should_refresh_the_chart_display_presenter()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
      }
   }
}