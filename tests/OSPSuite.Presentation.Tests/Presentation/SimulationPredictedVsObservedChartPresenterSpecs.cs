using System;
using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimulationPredictedVsObservedChartPresenter : ContextSpecification<SimulationPredictedVsObservedChartPresenter>
   {
      private ISimulationVsObservedDataView _view;
      private IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private ICurveNamer _curveNamer;
      protected ISimulation _simulation;
      private IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;
      protected IObservedDataRepository _observedDataRepository;
      private IChartTemplatingTask _chartTemplatingTask;
      private IPresentationSettingsTask _presentationSettingsTask;
      private IDimensionFactory _dimensionFactory;
      protected SimulationPredictedVsObservedChart _predictedVsObservedChart;
      private ParameterIdentificationRunResult _parameterIdentificationRunResult;
      private ResidualsResult _residualResults;
      protected OptimizationRunResult _optimizationRunResult;
      protected DataColumn _noDimensionColumnForSimulation;
      protected IPredictedVsObservedChartService _predictedVsObservedService;
      protected DataRepository _calculationData;
      protected DataRepository _observationData;
      private IChartEditorLayoutTask _chartEditorLayoutTask;
      private IProjectRetriever _projectRetriever;
      protected ChartPresenterContext _chartPresenterContext;
      protected DataRepository _simulationData;
      protected OutputMappings _outputMappings;
      protected IQuantity _quantityWithNoDimension;
      protected IQuantity _quantityWithConcentration;
      protected DataColumn _noDimensionDataColumn;
      protected DataColumn _concentrationDataColumn;
      protected DataColumn _concentrationColumnForSimulation;
      protected IChartEditorPresenter _chartEditorPresenter;

      protected override void Context()
      {
         _view = A.Fake<ISimulationVsObservedDataView>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _curveNamer = A.Fake<ICurveNamer>();
         _dataColumnToPathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _predictedVsObservedService = A.Fake<IPredictedVsObservedChartService>();
         _chartEditorLayoutTask = A.Fake<IChartEditorLayoutTask>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _chartEditorPresenter = A.Fake<IChartEditorPresenter>();

         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.EditorAndDisplayPresenter).Returns(_chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.CurveNamer).Returns(_curveNamer);
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(_dataColumnToPathElementsMapper);
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(_chartTemplatingTask);
         A.CallTo(() => _chartPresenterContext.PresenterSettingsTask).Returns(_presentationSettingsTask);
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(_chartEditorLayoutTask);
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(_projectRetriever);
         A.CallTo(() => _chartPresenterContext.EditorPresenter).Returns(_chartEditorPresenter);
         A.CallTo(() => _chartEditorAndDisplayPresenter.EditorPresenter).Returns(_chartEditorPresenter);

         _calculationData = DomainHelperForSpecs.ObservedData();
         _calculationData.Name = "calculation observed data";
         _observationData = DomainHelperForSpecs.ObservedData();
         _observationData.Name = "simulation observed data";

         _simulationData = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Simulation");
         _noDimensionColumnForSimulation = _simulationData.FirstDataColumn();

         _predictedVsObservedChart = new SimulationPredictedVsObservedChart().WithAxes();
         _simulation = A.Fake<ISimulation>();
         sut = new SimulationPredictedVsObservedChartPresenter(_view, _chartPresenterContext, _predictedVsObservedService, _observedDataRepository);

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _simulation.ResultsDataRepository).Returns(_calculationData);

         _residualResults = new ResidualsResult();
         _optimizationRunResult = new OptimizationRunResult
            { ResidualsResult = _residualResults, SimulationResults = new List<DataRepository> { _simulationData } };
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult;
         _noDimensionColumnForSimulation.Dimension = DomainHelperForSpecs.NoDimension();
         _concentrationColumnForSimulation = DomainHelperForSpecs.ConcentrationColumnForSimulation("Simulation", _simulationData.BaseGrid);
         _simulationData.Add(_concentrationColumnForSimulation);


         _quantityWithNoDimension = A.Fake<IQuantity>();
         _quantityWithConcentration = A.Fake<IQuantity>();
         A.CallTo(() => _quantityWithNoDimension.Dimension).Returns(DomainHelperForSpecs.NoDimension());
         A.CallTo(() => _quantityWithConcentration.Dimension).Returns(DomainHelperForSpecs.ConcentrationDimensionForSpecs());

         var simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         A.CallTo(() => simulationQuantitySelection.FullQuantityPath).Returns("Path1");
         var anotherQuantitySelection = A.Fake<SimulationQuantitySelection>();
         A.CallTo(() => anotherQuantitySelection.FullQuantityPath).Returns("Path2");
         var noDimensionOutputMapping = new OutputMapping
         {
            OutputSelection = simulationQuantitySelection,
            WeightedObservedData = new WeightedObservedData(_observationData)
         };
         var concentrationOutputMapping = new OutputMapping
         {
            OutputSelection = anotherQuantitySelection,
            WeightedObservedData = new WeightedObservedData(_observationData)
         };
         A.CallTo(() => simulationQuantitySelection.Quantity).Returns(_quantityWithNoDimension);
         A.CallTo(() => anotherQuantitySelection.Quantity).Returns(_quantityWithConcentration);


         _outputMappings = new OutputMappings();
         _outputMappings.Add(noDimensionOutputMapping);
         _outputMappings.Add(concentrationOutputMapping);
         A.CallTo(() => _simulation.OutputMappings).Returns(_outputMappings);
         _noDimensionDataColumn = _calculationData.FirstDataColumn();
         _noDimensionDataColumn.Dimension = DomainHelperForSpecs.NoDimension();
         _noDimensionDataColumn.DataInfo.Origin = ColumnOrigins.Calculation;
         _noDimensionDataColumn.QuantityInfo = new QuantityInfo(new List<string>() { "Path1" }, QuantityType.OtherProtein);

         _concentrationDataColumn = new DataColumn("newColumn", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _calculationData.BaseGrid);
         _concentrationDataColumn.DataInfo.Origin = ColumnOrigins.Calculation;
         _noDimensionDataColumn.QuantityInfo = new QuantityInfo(new List<string>() { "Path2" }, QuantityType.OtherProtein);
         _calculationData.Add(_concentrationDataColumn);
         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(A<ISimulation>._)).Returns(new List<DataRepository>() { _calculationData });

         A.CallTo(() => _predictedVsObservedService.AddIdentityCurves(A<List<DataColumn>>._, _predictedVsObservedChart)).Returns(new List<DataRepository>() { DomainHelperForSpecs.ObservedData() });

         sut.InitializeAnalysis(_predictedVsObservedChart);
      }
   }

   public class When_the_simulation_does_not_have_results : concern_for_SimulationPredictedVsObservedChartPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.ResultsDataRepository).Returns(null);
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_predictedVsObservedChart, _simulation);
      }

      [Observation]
      public void the_chart_editor_must_not_be_initialized_with_null()
      {
         A.CallTo(() => _chartEditorPresenter.AddDataRepositories(A<IEnumerable<DataRepository>>._)).MustNotHaveHappened();
      }

   }

   public class When_the_results_have_preferred_and_non_preferred_dimensions : concern_for_SimulationPredictedVsObservedChartPresenter
   {
      protected override void Because()
      {
         sut.InitializeAnalysis(_predictedVsObservedChart, _simulation);
      }

      [Observation]
      public void the_x_axis_dimension_is_updated()
      {
         A.CallTo(() => _predictedVsObservedService.ConfigureAxesDimensionAndTitle(
            A<IReadOnlyList<DataColumn>>.That.Contains(_calculationData.FirstDataColumn()),
            _predictedVsObservedChart)).MustHaveHappened();
      }

      [Observation]
      public void adds_curve_for_concentration_column()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>._,
            _concentrationDataColumn, A<PredictedVsObservedChart>._, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_chart_editor_must_be_initialized_with_data_repository()
      {
         A.CallTo(() => _chartEditorPresenter.AddDataRepositories(A<IEnumerable<DataRepository>>.That.Contains(_calculationData))).MustHaveHappened();
      }
   }

   public class When_handling_the_output_mapping_changed_event : concern_for_SimulationPredictedVsObservedChartPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.UpdateAnalysisBasedOn(_simulation);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationOutputMappingsChangedEvent(_simulation));
      }

      [Observation]
      public void the_chart_editor_presenter_should_be_updated()
      {
         A.CallTo(() => _chartEditorPresenter.AddOutputMappings(A<OutputMappings>._)).MustHaveHappened(2, Times.Exactly);
      }
   }
}