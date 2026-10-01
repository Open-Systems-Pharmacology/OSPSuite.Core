using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter : ContextSpecification<ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter>
   {
      protected IParameterIdentificationSingleRunAnalysisView _view;
      protected IChartEditorPresenter _chartEditorPresenter;
      protected ChartPresenterContext _chartPresenterContext;
      protected IDimensionFactory _dimensionFactory;
      protected ITimeProfileConfidenceIntervalCalculator _timeProfileConfidenceIntervalCalculator;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _parameterIdentificationRunResult;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         var chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _chartEditorPresenter = A.Fake<IChartEditorPresenter>();
         A.CallTo(() => chartEditorAndDisplayPresenter.EditorPresenter).Returns(_chartEditorPresenter);
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _timeProfileConfidenceIntervalCalculator = A.Fake<ITimeProfileConfidenceIntervalCalculator>();

         _chartPresenterContext = A.Fake<ChartPresenterContext>();
         A.CallTo(() => _chartPresenterContext.EditorAndDisplayPresenter).Returns(chartEditorAndDisplayPresenter);
         A.CallTo(() => _chartPresenterContext.CurveNamer).Returns(A.Fake<ICurveNamer>());
         A.CallTo(() => _chartPresenterContext.DataColumnToPathElementsMapper).Returns(A.Fake<IDataColumnToPathElementsMapper>());
         A.CallTo(() => _chartPresenterContext.TemplatingTask).Returns(A.Fake<IChartTemplatingTask>());
         A.CallTo(() => _chartPresenterContext.PresenterSettingsTask).Returns(A.Fake<IPresentationSettingsTask>());
         A.CallTo(() => _chartPresenterContext.DimensionFactory).Returns(_dimensionFactory);
         A.CallTo(() => _chartPresenterContext.EditorLayoutTask).Returns(A.Fake<IChartEditorLayoutTask>());
         A.CallTo(() => _chartPresenterContext.ProjectRetriever).Returns(A.Fake<IProjectRetriever>());

         sut = new ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter(_view, _chartPresenterContext, _timeProfileConfidenceIntervalCalculator);

         _parameterIdentification = A.Fake<ParameterIdentification>();
         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _parameterIdentification.Results).Returns(new[] { _parameterIdentificationRunResult });
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(new OutputMapping[0]);
      }
   }

   public class When_initializing_a_cloned_confidence_interval_chart_that_already_contains_curves : concern_for_ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter
   {
      private const string SOURCE_ID = "SOURCE_ID";
      private const string CLONED_ID = "CLONED_ID";
      private static readonly Color CLONED_COLOR = Color.Magenta;
      private DataRepository _simulationResults;
      private ParameterIdentificationTimeProfileConfidenceIntervalChart _sourceChart;
      private ParameterIdentificationTimeProfileConfidenceIntervalChart _clonedChart;

      protected override void Context()
      {
         base.Context();
         var outputMapping = A.Fake<OutputMapping>();
         A.CallTo(() => outputMapping.FullOutputPath).Returns("Sim|Comp|Liver|Cell|Concentration");
         var runResult = A.Fake<OptimizationRunResult>();
         _simulationResults = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim");
         A.CallTo(() => runResult.SimulationResultFor(outputMapping.FullOutputPath)).Returns(_simulationResults.FirstDataColumn());
         var confidenceIntervalRepository = new ConfidenceIntervalDataRepositoryCreator().CreateFor("Confidence Interval", new[] { 1d, 2d, 3d }, outputMapping, runResult);
         A.CallTo(() => _timeProfileConfidenceIntervalCalculator.CalculateConfidenceIntervalFor(_parameterIdentification, _parameterIdentificationRunResult)).Returns(new[] { confidenceIntervalRepository });

         _sourceChart = new ParameterIdentificationTimeProfileConfidenceIntervalChart().WithAxes().WithId(SOURCE_ID);
         new ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter(_view, _chartPresenterContext, _timeProfileConfidenceIntervalCalculator).InitializeAnalysis(_sourceChart, _parameterIdentification);
         _sourceChart.Curves.Single().Color = CLONED_COLOR;

         var idGenerator = A.Fake<IIdGenerator>();
         A.CallTo(() => idGenerator.NewId()).Returns(CLONED_ID);
         var objectBaseFactory = new ObjectBaseFactory(A.Fake<OSPSuite.Utility.Container.IContainer>(), _dimensionFactory, idGenerator, A.Fake<ICreationMetaDataFactory>());
         _clonedChart = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>()).Clone(_sourceChart);

         Fake.ClearRecordedCalls(_timeProfileConfidenceIntervalCalculator);
         Fake.ClearRecordedCalls(_chartEditorPresenter);
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_clonedChart, _parameterIdentification);
      }

      [Observation]
      public void should_not_recompute_the_confidence_intervals()
      {
         A.CallTo(() => _timeProfileConfidenceIntervalCalculator.CalculateConfidenceIntervalFor(A<ParameterIdentification>._, A<ParameterIdentificationRunResult>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_add_the_repositories_of_the_clone_to_the_editor()
      {
         A.CallTo(() => _chartEditorPresenter.AddDataRepositories(A<IEnumerable<DataRepository>>.That.Contains(_clonedChart.DataRepositories.Single()))).MustHaveHappened();
      }

      [Observation]
      public void should_keep_the_interval_curve_with_its_color_without_adding_duplicates()
      {
         var intervalCurve = _clonedChart.Curves.Single();
         intervalCurve.Color.ShouldBeEqualTo(CLONED_COLOR);
         intervalCurve.yData.Repository.ShouldBeEqualTo(_clonedChart.DataRepositories.Single());
      }

      [Observation]
      public void should_leave_the_source_chart_unchanged()
      {
         _sourceChart.Curves.Single().yData.Repository.ShouldBeEqualTo(_sourceChart.DataRepositories.Single());
      }
   }
}
