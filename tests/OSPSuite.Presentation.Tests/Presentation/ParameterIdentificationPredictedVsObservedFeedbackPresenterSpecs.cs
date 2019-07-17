using System;
using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter : ContextSpecification<ParameterIdentificationPredictedVsObservedFeedbackPresenter>
   {
      protected IPredictedVsObservedChartService _predictedVsObservedChartService;
      protected IDimensionFactory _dimensionFactory;
      protected IChartDisplayPresenter _chartDisplayPresenter;
      protected IParameterIdentificationChartFeedbackView _view;
      protected ParameterIdentification _parameterIdentification;
      private OutputMapping _outputMapping;
      protected IList<DataColumn> _observationColumns;
      protected List<Curve> _curveList;
      private BaseGrid _baseGrid;
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected ParameterIdentificationPredictedVsObservedChart _chart;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationChartFeedbackView>();
         _chartDisplayPresenter = A.Fake<IChartDisplayPresenter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _predictedVsObservedChartService = A.Fake<IPredictedVsObservedChartService>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _observationColumns = new List<DataColumn>();
         _curveList = new List<Curve>();
         _baseGrid = DomainHelperForSpecs.ObservedData().BaseGrid;
         _outputMapping = new OutputMapping();

         sut = new ParameterIdentificationPredictedVsObservedFeedbackPresenter(_view, _chartDisplayPresenter, _dimensionFactory, _displayUnitRetriever, _predictedVsObservedChartService);

         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(new[] {_outputMapping});
         A.CallTo(() => _parameterIdentification.AllObservationColumnsFor(A<string>._)).Returns(_observationColumns);
         _observationColumns.Add(new DataColumn());
         _observationColumns.Add(new DataColumn());

         A.CallTo(() => _predictedVsObservedChartService.AddCurvesFor(_observationColumns, A<DataColumn>._, A<ParameterIdentificationPredictedVsObservedChart>._, A<Action<DataColumn, Curve>>._)).Invokes(x =>
         {
            var action = x.Arguments.Get<Action<DataColumn, Curve>>(3);
            _observationColumns.Each(observation =>
            {
               var curve = new Curve {Name = "Best"};

               action(new DataColumn(ShortGuid.NewGuid(), A.Fake<IDimension>(), _baseGrid), curve);
               _curveList.Add(curve);
            });
            _chart = x.GetArgument<ParameterIdentificationPredictedVsObservedChart>(2);
         });
      }
   }

   public class When_updating_the_feedback : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;
      private OptimizationRunResult _bestResult;
      private OptimizationRunResult _currentResult;
      private DataColumn _bestColumn;
      private DataColumn _currentColumn;

      protected override void Context()
      {
         base.Context();
         _bestColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();
         _currentColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();

         _bestResult = A.Fake<OptimizationRunResult>();
         _currentResult = A.Fake<OptimizationRunResult>();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.SelectedOutput = A.Fake<OutputMapping>();

         A.CallTo(() => _bestResult.SimulationResultFor(A<string>._)).Returns(_bestColumn);
         A.CallTo(() => _currentResult.SimulationResultFor(A<string>._)).Returns(_currentColumn);

         _runState = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runState.BestResult).Returns(_bestResult);
         A.CallTo(() => _runState.CurrentResult).Returns(_currentResult);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void the_chart_display_presenter_should_be_refreshed()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_updating_the_feedback_and_the_selected_output_is_not_set : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;
      private OptimizationRunResult _bestResult;
      private OptimizationRunResult _currentResult;
      private DataColumn _bestColumn;
      private DataColumn _currentColumn;

      protected override void Context()
      {
         base.Context();
         _bestColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();
         _currentColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();

         _bestResult = A.Fake<OptimizationRunResult>();
         _currentResult = A.Fake<OptimizationRunResult>();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.SelectedOutput = null;

         A.CallTo(() => _bestResult.SimulationResultFor(A<string>._)).Returns(_bestColumn);
         A.CallTo(() => _currentResult.SimulationResultFor(A<string>._)).Returns(_currentColumn);

         _runState = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runState.BestResult).Returns(_bestResult);
         A.CallTo(() => _runState.CurrentResult).Returns(_currentResult);
      }

      [Observation]
      public void should_not_crash()
      {
         sut.UpdateFeedback(_runState);
      }
   }

   public class When_updating_the_feedback_for_a_run_state_entering_sensitivity_calculation : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;
      private OptimizationRunResult _bestResult;
      private OptimizationRunResult _currentResult;
      private DataColumn _bestColumn;
      private DataColumn _currentColumn;

      protected override void Context()
      {
         base.Context();
         _bestColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();
         _currentColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();

         _bestResult = A.Fake<OptimizationRunResult>();
         _currentResult = A.Fake<OptimizationRunResult>();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.SelectedOutput = A.Fake<OutputMapping>();

         A.CallTo(() => _bestResult.SimulationResultFor(A<string>._)).Returns(_bestColumn);
         A.CallTo(() => _currentResult.SimulationResultFor(A<string>._)).Returns(_currentColumn);

         _runState = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runState.BestResult).Returns(_bestResult);
         A.CallTo(() => _runState.CurrentResult).Returns(_currentResult);
         A.CallTo(() => _runState.Status).Returns(RunStatus.CalculatingSensitivity);
         _view.OutputSelectionEnabled = true;
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void the_chart_display_presenter_should_be_refreshed()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_disable_the_output_selection()
      {
         _view.OutputSelectionEnabled.ShouldBeFalse();
      }
   }

   public class When_changing_the_selected_output_that_the_chart_is_showing_feedack_for : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         var selectedOutput = A.Fake<OutputMapping>();
         selectedOutput.Scaling = Scalings.Log;
         sut.SelectedOutput = selectedOutput;
      }

      [Observation]
      public void the_chart_service_should_have_been_used_to_replot_the_new_output_feedback()
      {
         A.CallTo(() => _predictedVsObservedChartService.AddCurvesFor(_observationColumns, A<DataColumn>._, A<ParameterIdentificationPredictedVsObservedChart>.That.Matches(chart => chart.DefaultYAxisScaling == Scalings.Log), A<Action<DataColumn, Curve>>._))
            .MustHaveHappened(4, Times.Exactly);
      }
   }

   public class When_editing_a_parameter_identification_for_predicted_vs_observed_feedback : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void there_should_be_4_curves_added_to_the_chart()
      {
         _curveList.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_plot_the_best_and_current_run_in_the_chart()
      {
         A.CallTo(() => _predictedVsObservedChartService.AddCurvesFor(_observationColumns, A<DataColumn>._, A<ParameterIdentificationPredictedVsObservedChart>._, A<Action<DataColumn, Curve>>._)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_resetting_the_feedback_in_the_parameter_identification_for_prededicted_vs_observed_feedback_presenter : concern_for_ParameterIdentificationPredictedVsObservedFeedbackPresenter
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