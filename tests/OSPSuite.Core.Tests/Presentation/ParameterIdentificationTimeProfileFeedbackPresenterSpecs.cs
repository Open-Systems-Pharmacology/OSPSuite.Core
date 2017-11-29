using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationTimeProfileFeedbackPresenter : ContextSpecification<IParameterIdentificationTimeProfileFeedbackPresenter>
   {
      protected IParameterIdentificationChartFeedbackView _view;
      protected IChartDisplayPresenter _chartDisplayPresenter;
      protected IDimensionFactory _dimensionFactory;
      protected ParameterIdentificationRunState _runState;
      protected OptimizationRunResult _bestResult;
      protected OptimizationRunResult _currentResult;
      protected ParameterIdentification _parameterIdentification;
      protected OutputMapping _outputMapping1;
      protected OutputMapping _outputMapping2;
      protected OutputMapping _outputMapping3;
      private DataColumn _obsCol1;
      private DataColumn _obsCol2;
      private DataColumn _obsCol3;
      private IDisplayUnitRetriever _displayUnitRetriever;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationChartFeedbackView>();
         _chartDisplayPresenter = A.Fake<IChartDisplayPresenter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _displayUnitRetriever= A.Fake<IDisplayUnitRetriever>();

         sut = new ParameterIdentificationTimeProfileFeedbackPresenter(_view, _chartDisplayPresenter, _dimensionFactory,_displayUnitRetriever);

         _runState = A.Fake<ParameterIdentificationRunState>();
         _bestResult = A.Fake<OptimizationRunResult>();
         _currentResult = A.Fake<OptimizationRunResult>();

         A.CallTo(() => _runState.BestResult).Returns(_bestResult);
         A.CallTo(() => _runState.CurrentResult).Returns(_currentResult);

         _parameterIdentification = A.Fake<ParameterIdentification>();
         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();
         _outputMapping3 = A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping1.FullOutputPath).Returns("A|B|C");
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns("A|B|C");
         A.CallTo(() => _outputMapping3.FullOutputPath).Returns("A|B|C|D");
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(new[] {_outputMapping1, _outputMapping2, _outputMapping3});

         var baseGrid1 = new BaseGrid("TimeCol1", DomainHelperForSpecs.TimeDimensionForSpecs());
         _obsCol1 = new DataColumn("ObsCol1", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid1);

         var baseGrid2 = new BaseGrid("TimeCol2", DomainHelperForSpecs.TimeDimensionForSpecs());
         _obsCol2 = new DataColumn("ObsCol2", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid2);

         var baseGrid3 = new BaseGrid("TimeCol3", DomainHelperForSpecs.TimeDimensionForSpecs());
         _obsCol3 = new DataColumn("ObsCol3", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid3);

         A.CallTo(() => _parameterIdentification.AllObservationColumnsFor(_outputMapping1.FullOutputPath)).Returns(new[] {_obsCol1, _obsCol2});
         A.CallTo(() => _parameterIdentification.AllObservationColumnsFor(_outputMapping3.FullOutputPath)).Returns(new[] { _obsCol3 });
      }
   }

   public class When_the_time_profile_feedback_presenter_is_editing_a_given_parameter_identification : concern_for_ParameterIdentificationTimeProfileFeedbackPresenter
   {

      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_select_the_first_mapped_output_as_selected_output()
      {
         sut.SelectedOutput.ShouldBeEqualTo(_outputMapping1);
      }

      [Observation]
      public void should_have_bind_the_selecte_output_to_the_view()
      {
         A.CallTo(() => _view.BindToSelecteOutput()).MustHaveHappened();
      }

      [Observation]
      public void should_have_added_one_curve_for_the_best_and_one_curve_for_the_current_result_as_well_as_one_curve_for_each_observed_data_map_to_the_selected_output()
      {
         sut.Chart.Curves.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_return_the_list_of_availalbe_output_with_different_output_path()
      {
         sut.AllOutputs.ShouldOnlyContain(_outputMapping1, _outputMapping3);
      }
   }

   public class When_updating_the_time_profile_feedback_for_a_given_parameter_identification : concern_for_ParameterIdentificationTimeProfileFeedbackPresenter
   {
      private DataColumn _bestCol;
      private DataColumn _currentCol;

      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);

         var baseGridBest = new BaseGrid("baseGridBest", DomainHelperForSpecs.TimeDimensionForSpecs())
         {
            Values = new[] {1f, 2f, 3f}
         };

         _bestCol = new DataColumn("BEST", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGridBest)
         {
            Values = new[] {11f, 22f, 33f}
         };

         var baseGridCurrent = new BaseGrid("baseGridCurrent", DomainHelperForSpecs.TimeDimensionForSpecs())
         {
            Values = new[] {4f, 5f, 6f, 7f}
         };

         _currentCol = new DataColumn("CURRENT", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGridCurrent)
         {
            Values = new[] {41f, 51f, 61f, 71f}
         };

         A.CallTo(() => _bestResult.SimulationResultFor(_outputMapping1.FullOutputPath)).Returns(_bestCol);
         A.CallTo(() => _currentResult.SimulationResultFor(_outputMapping1.FullOutputPath)).Returns(_currentCol);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void should_update_the_best_and_current_values_according_to_the_run_status()
      {
         var bestCurve = sut.Chart.Curves.Find(x => x.xData.Repository.IsNamed(Captions.ParameterIdentification.Best));
         var currentCurve = sut.Chart.Curves.Find(x => x.xData.Repository.IsNamed(Captions.ParameterIdentification.Current));

         bestCurve.xData.Values.ShouldBeEqualTo(_bestCol.BaseGrid.Values);
         bestCurve.yData.Values.ShouldBeEqualTo(_bestCol.Values);

         currentCurve.xData.Values.ShouldBeEqualTo(_currentCol.BaseGrid.Values);
         currentCurve.yData.Values.ShouldBeEqualTo(_currentCol.Values);
      }
   }

   public class When_switching_the_selected_output_for_time_profile_feedback : concern_for_ParameterIdentificationTimeProfileFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(A<DataColumn>.That.Matches(x => Equals(x.Dimension, _outputMapping3.Dimension)))).Returns(_outputMapping3.Dimension);
      }

      protected override void Because()
      {
         sut.SelectedOutput =_outputMapping3;
      }

      [Observation]
      public void the_dimension_of_the_y_axis_should_change_to_the_same_as_the_output_mapping()
      {
         var bestCurve = sut.Chart.Curves.Find(x => x.xData.Repository.IsNamed(Captions.ParameterIdentification.Best));
         var currentCurve = sut.Chart.Curves.Find(x => x.xData.Repository.IsNamed(Captions.ParameterIdentification.Current));
         sut.Chart.AxisBy(AxisTypes.Y).Dimension.ShouldBeEqualTo(_outputMapping3.Dimension);
         bestCurve.yDimension.ShouldBeEqualTo(_outputMapping3.Dimension);
         currentCurve.yDimension.ShouldBeEqualTo(_outputMapping3.Dimension);
      }

      [Observation]
      public void should_show_the_observed_data_for_the_newly_selected_output()
      {
         sut.Chart.Curves.Count.ShouldBeEqualTo(3);
      }
   }

   public class When_clearing_the_references_held_in_memory_by_the_parameter_identification_time_profile_feedback_presenter : concern_for_ParameterIdentificationTimeProfileFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.ClearReferences();
      }

      [Observation]
      public void should_delete_the_binding()
      {
         A.CallTo(() => _view.ClearBinding()).MustHaveHappened();   
      }
   }
}