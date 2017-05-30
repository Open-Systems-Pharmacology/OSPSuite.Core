using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationPredictedVsObservedChartPresenter : ContextSpecification<ParameterIdentificationPredictedVsObservedChartPresenter>
   {
      private IParameterIdentificationSingleRunAnalysisView _view;
      private IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      private IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;
      private IChartTemplatingTask _chartTemplatingTask;
      private IPresentationSettingsTask _presentationSettingsTask;
      private IDimensionFactory _dimensionFactory;
      protected ParameterIdentificationPredictedVsObservedChart _predictedVsObservedChart;
      protected ParameterIdentification _parameterIdentification;
      private ParameterIdentificationRunResult _parameterIdentificationRunResult;
      private ResidualsResult _residualResults;
      protected OptimizationRunResult _optimizationRunResult;
      protected DataColumn _simulationColumn;
      protected IPredictedVsObservedChartService _predictedVsObservedService;
      protected DataRepository _observationData;
      private IChartEditorLayoutTask _chartEditorLayoutTask;
      private IProjectRetriever _projectRetreiver;
      private ChartPresenterContext _chartPresenterContext;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _quantityDisplayPathMapper = A.Fake<IQuantityPathToQuantityDisplayPathMapper>();
         _dataColumnToPathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _predictedVsObservedService = A.Fake<IPredictedVsObservedChartService>();
         _chartEditorLayoutTask = A.Fake<IChartEditorLayoutTask>();
         _projectRetreiver = A.Fake<IProjectRetriever>();

         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.ChartEditorAndDisplayPresenter).Returns(_chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.QuantityDisplayPathMapper).Returns(_quantityDisplayPathMapper);
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(_dataColumnToPathElementsMapper);
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(_chartTemplatingTask);
         A.CallTo(() => _chartPresenterContext.PresenterSettingsTask).Returns(_presentationSettingsTask);
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(_chartEditorLayoutTask);
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(_projectRetreiver);

         _observationData = DomainHelperForSpecs.ObservedData();

         var simulationData = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Simulation");
         _simulationColumn = simulationData.FirstDataColumn();

         _predictedVsObservedChart = new ParameterIdentificationPredictedVsObservedChart().WithAxes();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut = new ParameterIdentificationPredictedVsObservedChartPresenter(_view, _chartPresenterContext, _predictedVsObservedService);

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _parameterIdentification.Results).Returns(new[] { _parameterIdentificationRunResult });

         _residualResults = new ResidualsResult();
         _optimizationRunResult = new OptimizationRunResult { ResidualsResult = _residualResults, SimulationResults = new List<DataRepository> { simulationData } };
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult;

         sut.InitializeAnalysis(_predictedVsObservedChart);
      }
   }

   public class When_the_simulation_results_do_not_contain_a_concentration_calculation : concern_for_ParameterIdentificationPredictedVsObservedChartPresenter
   {
      private IReadOnlyList<OutputMapping> _outputMappings;
      private OutputMapping _outputMapping;
      private IQuantity _quantity;
      private DataColumn _firstObservationDataColumn;

      protected override void Context()
      {
         base.Context();
         _simulationColumn.Dimension = DomainHelperForSpecs.NoDimension();

         _quantity = A.Fake<IQuantity>();
         A.CallTo(() => _quantity.Dimension).Returns(DomainHelperForSpecs.NoDimension());

         var simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         _outputMapping = new OutputMapping { OutputSelection = simulationQuantitySelection };
         A.CallTo(() => simulationQuantitySelection.Quantity).Returns(_quantity);


         _outputMappings = new[] { _outputMapping };
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_outputMappings);
         _firstObservationDataColumn = _observationData.FirstDataColumn();
         _firstObservationDataColumn.Dimension = DomainHelperForSpecs.NoDimension();
         A.CallTo(() => _parameterIdentification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString)).Returns(new List<DataColumn> { _firstObservationDataColumn });
      }

      protected override void Because()
      {
         sut.UpdateAnalysisBasedOn(_parameterIdentification);
      }

      [Observation]
      public void the_chart_should_use_the_dimension_of_the_first_output_mapping_to_determine_the_x_axis_dimension()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>.That.Contains(_firstObservationDataColumn), _simulationColumn, _predictedVsObservedChart, A<Action<DataColumn, ICurve>>._)).MustHaveHappened();
      }
   }

   public class When_editing_a_parameter_identification_with_results : concern_for_ParameterIdentificationPredictedVsObservedChartPresenter
   {
      protected override void Because()
      {
         sut.UpdateAnalysisBasedOn(_parameterIdentification);
      }

      [Observation]
      public void should_call_the_chart_service_to_create_a_curve_for_each_simulation_output()
      {
         A.CallTo(() => _predictedVsObservedService.AddCurvesFor(A<IEnumerable<DataColumn>>._, _simulationColumn, A<ParameterIdentificationPredictedVsObservedChart>._, A<Action<DataColumn, ICurve>>._)).MustHaveHappened();
      }
   }
}
