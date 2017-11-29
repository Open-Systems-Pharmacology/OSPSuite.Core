using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationTimeProfileChartPresenter : ContextSpecification<IParameterIdentificationTimeProfileChartPresenter>
   {
      protected IPresentationSettingsTask _presentationSettingsTask;
      protected IParameterIdentificationMultipleRunsAnalysisView _view;
      protected IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      protected IDataColumnToPathElementsMapper _pathElementsMapper;
      protected IChartTemplatingTask _chartTemplatingTask;
      protected ParameterIdentificationTimeProfileChart _timeProfileAnalysis;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _parameterIdentificationRunResult;
      protected DataRepository _observedData1;
      protected DataRepository _observedData2;
      protected OptimizationRunResult _optimizationRunResult;
      protected OutputMapping _outputMapping1;
      protected OutputMapping _outputMapping2;
      private IDimensionFactory _dimensionFactory;
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected OptimizationRunResult _optimizationRunResult2;
      private IChartEditorLayoutTask _chartEditorLayoutTask;
      private IProjectRetriever _projectRetreiver;
      private ChartPresenterContext _chartPresenterContext;
      private ICurveNamer _curveNamer;

      protected override void Context()
      {
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _view = A.Fake<IParameterIdentificationMultipleRunsAnalysisView>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _curveNamer = A.Fake<ICurveNamer>();
         _pathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _chartEditorLayoutTask = A.Fake<IChartEditorLayoutTask>();
         _projectRetreiver = A.Fake<IProjectRetriever>();


         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.EditorAndDisplayPresenter).Returns(_chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.CurveNamer).Returns(_curveNamer);
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(_pathElementsMapper);
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(_chartTemplatingTask);
         A.CallTo(() => _chartPresenterContext.PresenterSettingsTask).Returns(_presentationSettingsTask);
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(_chartEditorLayoutTask);
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(_projectRetreiver);


         sut = new ParameterIdentificationTimeProfileChartPresenter(_view, _chartPresenterContext, _displayUnitRetriever);

         _timeProfileAnalysis = new ParameterIdentificationTimeProfileChart();
         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();
         _observedData1 = DomainHelperForSpecs.ObservedData();
         _observedData2 = DomainHelperForSpecs.ObservedData();
         A.CallTo(() => _outputMapping1.WeightedObservedData).Returns(new WeightedObservedData(_observedData1));
         A.CallTo(() => _outputMapping2.WeightedObservedData).Returns(new WeightedObservedData(_observedData2));
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         _parameterIdentification.AddResult(_parameterIdentificationRunResult);

         _optimizationRunResult = new OptimizationRunResult();
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult;

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         _parameterIdentification.AddResult(_parameterIdentificationRunResult);
         _optimizationRunResult2 = new OptimizationRunResult();
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult2;
      }

      protected IChartEditorPresenter ChartEditorPresenter => _chartEditorAndDisplayPresenter.EditorPresenter;
   }

   public class When_displaying_the_results_of_a_given_parameter_identification_as_time_profile : concern_for_ParameterIdentificationTimeProfileChartPresenter
   {
      private DataRepository _simulationResult1;
      private DataColumn _outputColumn1;
      private DataColumn _firstObservedData1;
      private DataRepository _simulationResult2;
      private DataColumn _firstObservedData2;
      private List<DataRepository> _allAddedDataRepositories;

      protected override void Context()
      {
         base.Context();
         _firstObservedData1 = _observedData1.ObservationColumns().First();
         _firstObservedData2 = _observedData2.ObservationColumns().First();

         _simulationResult1 = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("SimulationResult1");
         _simulationResult2 = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("SimulationResult2");
         _outputColumn1 = _simulationResult1.AllButBaseGrid().First();

         A.CallTo(() => _outputMapping1.FullOutputPath).Returns(_outputColumn1.QuantityInfo.PathAsString);
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns(_outputColumn1.QuantityInfo.PathAsString);
         _optimizationRunResult.AddResult(_simulationResult1);
         _optimizationRunResult.AddResult(_simulationResult2);
         _optimizationRunResult2.AddResult(DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("SimulationResult3"));
         _optimizationRunResult2.AddResult(DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("SimulationResult4"));

         _parameterIdentification.Configuration.RunMode = new MultipleParameterIdentificationRunMode();

         _allAddedDataRepositories = new List<DataRepository>();;
         A.CallTo(() => ChartEditorPresenter.AddDataRepositories(A<IEnumerable<DataRepository>>._))
            .Invokes(x => _allAddedDataRepositories.AddRange(x.GetArgument<IEnumerable<DataRepository>>(0)));

      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_timeProfileAnalysis, _parameterIdentification);
      }

      [Observation]
      public void only_best_result_should_be_shown_by_default()
      {
         _timeProfileAnalysis.Curves.Count(curve => curve.Visible).ShouldBeEqualTo(4);
         _timeProfileAnalysis.Curves.Count(curve => !curve.Visible).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_add_the_observed_data_to_the_chart_editor()
      {
         _allAddedDataRepositories.ShouldContain(_observedData1);
         _allAddedDataRepositories.ShouldContain(_observedData2);
      }

      [Observation]
      public void should_add_the_simulation_results_to_the_chart_editor()
      {
         _allAddedDataRepositories.ShouldContain(_simulationResult1);
      }

      [Observation]
      public void should_add_one_curve_for_each_observed_data_mapped_in_the_optimization()
      {
         _timeProfileAnalysis.FindCurveWithSameData(_firstObservedData1.BaseGrid, _firstObservedData1).ShouldNotBeNull();
         _timeProfileAnalysis.FindCurveWithSameData(_firstObservedData2.BaseGrid, _firstObservedData2).ShouldNotBeNull();
      }

      [Observation]
      public void should_add_one_curve_for_each_output_mapped_of_the_simulation_using_a_matching_color()
      {
         var observedDataCurve1 = _timeProfileAnalysis.FindCurveWithSameData(_firstObservedData1.BaseGrid, _firstObservedData1);
         var observedDataCurve2 = _timeProfileAnalysis.FindCurveWithSameData(_firstObservedData2.BaseGrid, _firstObservedData2);
         var outputCurve = _timeProfileAnalysis.FindCurveWithSameData(_outputColumn1.BaseGrid, _outputColumn1);
         outputCurve.ShouldNotBeNull();

         outputCurve.Color.ShouldBeEqualTo(observedDataCurve1.Color);
         observedDataCurve2.Color.ShouldBeEqualTo(observedDataCurve1.Color);
      }

      [Observation]
      public void should_hide_the_observed_data_curve_from_the_legend()
      {
         var observedDataCurve = _timeProfileAnalysis.FindCurveWithSameData(_firstObservedData1.BaseGrid, _firstObservedData1);
         observedDataCurve.VisibleInLegend.ShouldBeFalse();
      }
   }
}