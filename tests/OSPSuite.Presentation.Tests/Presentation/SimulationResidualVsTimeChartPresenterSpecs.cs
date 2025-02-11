using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NPOI.HPSF;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;


namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimulationResidualVsTimeChartPresenter : ContextSpecification<ISimulationResidualVsTimeChartPresenter>
   {
      protected ISimulationVsObservedDataView _view;
      protected IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      protected IDataColumnToPathElementsMapper _pathElementsMapper;
      protected IChartTemplatingTask _chartTemplatingTask;
      protected IObservedDataRepository _observedDataRepository;
      protected IResidualCalculatorFactory _residualCalculatorFactory;
      protected IResidualCalculator _residualCalculator;
      protected SimulationResidualVsTimeChart _residualVsTimeChart;
      protected ISimulation _simulation;
      protected IDimensionFactory _dimensionFactory;
      protected ResidualsResult _residualResults;
      protected ResidualsVsTimeChartService _residualsVsTimeChartService;
      protected IChartEditorLayoutTask _chartEditorLayoutTask;
      protected IProjectRetriever _projectRetriever;
      protected ChartPresenterContext _chartPresenterContext;
      protected OutputResiduals _outputResiduals1;
      protected OutputResiduals _outputResiduals3;
      protected OutputResiduals _outputResiduals2;
      protected OutputMapping _outputMapping1;
      protected OutputMapping _outputMapping2;
      protected OutputMapping _outputMapping3;
      protected OutputMappings _outputMappings;

      protected IChartEditorPresenter ChartEditorPresenter => _chartEditorAndDisplayPresenter.EditorPresenter;

      protected override void Context()
      {
         _view = A.Fake<ISimulationVsObservedDataView>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _residualCalculatorFactory = A.Fake<IResidualCalculatorFactory>();
         _pathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _chartEditorLayoutTask = A.Fake<IChartEditorLayoutTask>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _residualCalculator = A.Fake<IResidualCalculator>();
         _residualsVsTimeChartService = A.Fake<ResidualsVsTimeChartService>();

         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.EditorAndDisplayPresenter).Returns(_chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(_pathElementsMapper);
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(_chartTemplatingTask);
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(_chartEditorLayoutTask);
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(_projectRetriever);
         A.CallTo(() => _residualCalculatorFactory.CreateFor(A<ParameterIdentificationConfiguration>._)).Returns(_residualCalculator);




         _residualVsTimeChart = new SimulationResidualVsTimeChart().WithAxes();
         _simulation = A.Fake<ISimulation>();
         _residualResults = new ResidualsResult();

         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();
         _outputMapping3 = A.Fake<OutputMapping>();

         var observation1 = DomainHelperForSpecs.ObservedData("OBS1");
         A.CallTo(() => _outputMapping1.WeightedObservedData.ObservedData).Returns(observation1);
         var observation2 = DomainHelperForSpecs.ObservedData("OBS2");
         A.CallTo(() => _outputMapping2.WeightedObservedData.ObservedData).Returns(observation2);
         var observation3 = DomainHelperForSpecs.ObservedData("OBS3");
         A.CallTo(() => _outputMapping3.WeightedObservedData.ObservedData).Returns(observation3);


         _outputResiduals1 = new OutputResiduals("OutputPath1", _outputMapping1.WeightedObservedData, new[] { new Residual(11f, 12f, 1), new Residual(21f, 22f, 1) });
         _outputResiduals2 = new OutputResiduals("OutputPath2", _outputMapping2.WeightedObservedData, new[] { new Residual(31f, 32f, 1), new Residual(41f, 42f, 1) });
         _outputResiduals3 = new OutputResiduals("OutputPath1", _outputMapping3.WeightedObservedData, new[] { new Residual(51f, 52f, 1), new Residual(61f, 62f, 1) });

         _residualResults.AddOutputResiduals(_outputResiduals1);
         _residualResults.AddOutputResiduals(_outputResiduals2);
         _residualResults.AddOutputResiduals(_outputResiduals3);


         _outputMappings = new OutputMappings();
         _outputMappings.Add(_outputMapping1);
         _outputMappings.Add(_outputMapping2);
         _outputMappings.Add(_outputMapping3);

         A.CallTo(() => _outputMapping1.FullOutputPath).Returns("OutputPath1");
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns("OutputPath2");
         A.CallTo(() => _outputMapping3.FullOutputPath).Returns("OutputPath1");


         A.CallTo(() => _simulation.OutputMappings).Returns(_outputMappings);
         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(A<ISimulation>._)).Returns(new List<DataRepository>() { observation3, observation1, observation2 });
         A.CallTo(() => _residualCalculator.Calculate(A<DataRepository>._, A<List<OutputMapping>>._)).Returns(_residualResults);
         sut = new SimulationResidualVsTimeChartPresenter(_view, _chartPresenterContext, _observedDataRepository, _residualCalculatorFactory, _residualsVsTimeChartService);
      }
   }

   public class When_displaying_the_results_of_a_given_simulation_as_residual_vs_time : concern_for_SimulationResidualVsTimeChartPresenter
   {
      protected override void Because()
      {
         sut.InitializeAnalysis(_residualVsTimeChart, _simulation);
      }

      [Observation]
      public void should_create_one_repository_for_each_output_containing_the_value_of_the_residuals_and_for_the_marker()
      {
         A.CallTo(() => ChartEditorPresenter.AddDataRepositories(A<IEnumerable<DataRepository>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_added_one_curve_for_each_output_residual_plus_one_for_the_zero_marker()
      {
         _residualVsTimeChart.Curves.Count.ShouldBeEqualTo(_residualResults.AllOutputResiduals.Count + 1);
      }

   }

   public class When_clearing_the_simulation_residual_vs_time_chart_presenter : concern_for_SimulationResidualVsTimeChartPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.InitializeAnalysis(_residualVsTimeChart, _simulation);

         //residual results plus zero marker
         _residualVsTimeChart.Curves.Count.ShouldBeEqualTo(_residualResults.AllOutputResiduals.Count + 1);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_remove_the_zero_maker_curve_that_was_added_to_the_chart()
      {
         //zero marker removed
         _residualVsTimeChart.Curves.Count.ShouldBeEqualTo(_residualResults.AllOutputResiduals.Count);
      }
   }

   public class When_clearing_the_simulation_residual_vs_time_chart_presenter_that_was_initialized_without_observed_data : concern_for_SimulationResidualVsTimeChartPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(A<ISimulation>._)).Returns(Enumerable.Empty<DataRepository>());

         sut.InitializeAnalysis(_residualVsTimeChart, _simulation);
      }

      [Observation]
      public void should_not_crash()
      {
         sut.Clear();
      }
   }
}