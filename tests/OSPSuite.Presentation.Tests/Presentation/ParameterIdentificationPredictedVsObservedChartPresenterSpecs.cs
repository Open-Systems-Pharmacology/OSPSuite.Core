using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationPredictedVsObservedChartPresenter : ContextSpecification<ParameterIdentificationPredictedVsObservedChartPresenter>
   {
      private IParameterIdentificationSingleRunAnalysisView _view;
      private IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private ICurveNamer _curveNamer;
      private IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;
      private IChartTemplatingTask _chartTemplatingTask;
      private IPresentationSettingsTask _presentationSettingsTask;
      private IDimensionFactory _dimensionFactory;
      protected ParameterIdentificationPredictedVsObservedChart _predictedVsObservedChart;
      protected ParameterIdentification _parameterIdentification;
      private ParameterIdentificationRunResult _parameterIdentificationRunResult;
      private ResidualsResult _residualResults;
      protected OptimizationRunResult _optimizationRunResult;
      protected DataColumn _noDimensionColumnForSimulation;
      protected IPredictedVsObservedChartService _predictedVsObservedService;
      protected DataRepository _observationData;
      private IChartEditorLayoutTask _chartEditorLayoutTask;
      private IProjectRetriever _projectRetreiver;
      protected ChartPresenterContext _chartPresenterContext;
      protected DataRepository _simulationData;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _curveNamer = A.Fake<ICurveNamer>();
         _dataColumnToPathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _predictedVsObservedService = A.Fake<IPredictedVsObservedChartService>();
         _chartEditorLayoutTask = A.Fake<IChartEditorLayoutTask>();
         _projectRetreiver = A.Fake<IProjectRetriever>();

         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.EditorAndDisplayPresenter).Returns(_chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.CurveNamer).Returns(_curveNamer);
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(_dataColumnToPathElementsMapper);
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(_chartTemplatingTask);
         A.CallTo(() => _chartPresenterContext.PresenterSettingsTask).Returns(_presentationSettingsTask);
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(_chartEditorLayoutTask);
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(_projectRetreiver);

         _observationData = DomainHelperForSpecs.ObservedData();

         _simulationData = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Simulation");
         _noDimensionColumnForSimulation = _simulationData.FirstDataColumn();

         _predictedVsObservedChart = new ParameterIdentificationPredictedVsObservedChart().WithAxes();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut = new ParameterIdentificationPredictedVsObservedChartPresenter(_view, _chartPresenterContext, _predictedVsObservedService);

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _parameterIdentification.Results).Returns(new[] {_parameterIdentificationRunResult});

         _residualResults = new ResidualsResult();
         _optimizationRunResult = new OptimizationRunResult {ResidualsResult = _residualResults, SimulationResults = new List<DataRepository> {_simulationData}};
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult;

         sut.InitializeAnalysis(_predictedVsObservedChart);
      }
   }

   public class When_the_simulation_results_have_preferred_and_non_preferred_dimensions : concern_for_ParameterIdentificationPredictedVsObservedChartPresenter
   {
      private IReadOnlyList<OutputMapping> _outputMappings;
      private IQuantity _quantityWithNoDimension;
      private IQuantity _quantityWithConcentration;
      private DataColumn _noDimensionDataColumn;
      private DataColumn _concentrationDataColumn;
      private DataColumn _concentrationColumnForSimulation;

      protected override void Context()
      {
         base.Context();
         _noDimensionColumnForSimulation.Dimension = DomainHelperForSpecs.NoDimension();
         _concentrationColumnForSimulation = DomainHelperForSpecs.ConcentrationColumnForSimulation("Simulation", _simulationData.BaseGrid);
         _simulationData.Add(_concentrationColumnForSimulation);
         

         _quantityWithNoDimension = A.Fake<IQuantity>();
         _quantityWithConcentration = A.Fake<IQuantity>();
         A.CallTo(() => _quantityWithNoDimension.Dimension).Returns(DomainHelperForSpecs.NoDimension());
         A.CallTo(() => _quantityWithConcentration.Dimension).Returns(DomainHelperForSpecs.ConcentrationDimensionForSpecs());

         var simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         var anotherQuantitySelection = A.Fake<SimulationQuantitySelection>();
         var noDimensionOutputMapping = new OutputMapping {OutputSelection = simulationQuantitySelection};
         var concentrationOutputMapping = new OutputMapping {OutputSelection = anotherQuantitySelection};
         A.CallTo(() => simulationQuantitySelection.Quantity).Returns(_quantityWithNoDimension);
         A.CallTo(() => anotherQuantitySelection.Quantity).Returns(_quantityWithConcentration);


         _outputMappings = new[] {noDimensionOutputMapping, concentrationOutputMapping};
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_outputMappings);
         _noDimensionDataColumn = _observationData.FirstDataColumn();
         _noDimensionDataColumn.Dimension = DomainHelperForSpecs.NoDimension();

         _concentrationDataColumn = new DataColumn("newColumn", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _observationData.BaseGrid);
         _observationData.Add(_concentrationDataColumn);

         A.CallTo(() => _parameterIdentification.AllObservationColumnsFor(_noDimensionColumnForSimulation.QuantityInfo.PathAsString)).Returns(new List<DataColumn> {_noDimensionDataColumn, _concentrationDataColumn});
         A.CallTo(() => _parameterIdentification.AllObservedData).Returns(new[] { _observationData });
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_predictedVsObservedChart, _parameterIdentification);
      }

      [Observation]
      public void the_x_axis_dimension_is_updated()
      {
         A.CallTo(() => _predictedVsObservedService.SetXAxisDimension(A<IEnumerable<DataColumn>>.That.Contains(_observationData.FirstDataColumn()), _predictedVsObservedChart)).MustHaveHappened();
      }

      [Observation]
      public void adds_curve_for_no_dimension_column()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>.That.Contains(_noDimensionDataColumn), _noDimensionColumnForSimulation, _predictedVsObservedChart, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>.That.Contains(_concentrationDataColumn), _noDimensionColumnForSimulation, _predictedVsObservedChart, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
      }

      [Observation]
      public void adds_curve_for_concentration_column()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>.That.Contains(_noDimensionDataColumn), _concentrationColumnForSimulation, _predictedVsObservedChart, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>.That.Contains(_concentrationDataColumn), _concentrationColumnForSimulation, _predictedVsObservedChart, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
      }
   }

   public class When_editing_a_parameter_identification_with_results : concern_for_ParameterIdentificationPredictedVsObservedChartPresenter
   {
      protected override void Because()
      {
         sut.InitializeAnalysis(_predictedVsObservedChart, _parameterIdentification);
      }

      [Observation]
      public void should_call_the_chart_service_to_create_a_curve_for_each_simulation_output()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>._, _noDimensionColumnForSimulation, A<ParameterIdentificationPredictedVsObservedChart>._, A<Action<DataColumn, Curve>>._)).MustHaveHappened();
      }
   }

   public class When_updating_the_selected_output_used_to_display_predicted_vs_observed_data: concern_for_ParameterIdentificationPredictedVsObservedChartPresenter
   {
      private CurveChartTemplate _template;

      protected override void Context()
      {
         base.Context();
         _template = new CurveChartTemplate();
         sut.InitializeAnalysis(_predictedVsObservedChart, _parameterIdentification);
         A.CallTo(() => _chartPresenterContext.TemplatingTask.TemplateFrom(sut.Chart, false)).Returns(_template);
      }

      protected override void Because()
      {
         sut.SelectedRunResults = _parameterIdentification.Results.First();
      }

      [Observation]
      public void should_save_the_current_chart_to_template()
      {
         _template.ShouldNotBeNull();
      }

      [Observation]
      public void should_reload_the_chart_from_template()
      {
         A.CallTo(() => _chartPresenterContext.TemplatingTask.InitializeChartFromTemplate(sut.Chart, A<IEnumerable<DataColumn>>._, 
            _template, 
            A<Func<DataColumn, string>>._,
            false,
            true)).MustHaveHappened();
      }
   }
}