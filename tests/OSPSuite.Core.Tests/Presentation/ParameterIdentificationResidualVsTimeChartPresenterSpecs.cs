using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
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
   public abstract class concern_for_ParameterIdentificationResidualVsTimeChartPresenter : ContextSpecification<IParameterIdentificationResidualVsTimeChartPresenter>
   {
      protected IPresentationSettingsTask _presentationSettingsTask;
      protected IParameterIdentificationSingleRunAnalysisView _view;
      protected IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      protected IDataColumnToPathElementsMapper _pathElementsMapper;
      protected IChartTemplatingTask _chartTemplatingTask;

      protected ParameterIdentificationResidualVsTimeChart _residualVsTimeChart;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _parameterIdentificationRunResult;
      protected OptimizationRunResult _optimizationRunResult;
      protected IDimensionFactory _dimensionFactory;
      protected ResidualsResult _residualResults;
      private IChartEditorLayoutTask _chartEditorLayoutTask;
      private IProjectRetriever _projectRetreiver;
      private ChartPresenterContext _chartPresenterContext;
      private ICurveNamer _curveNamer;

      protected IChartEditorPresenter ChartEditorPresenter => _chartEditorAndDisplayPresenter.EditorPresenter;

      protected override void Context()
      {
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _chartEditorAndDisplayPresenter = A.Fake<IChartEditorAndDisplayPresenter>();
         _curveNamer = A.Fake<ICurveNamer>();
         _pathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
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


         sut = new ParameterIdentificationResidualVsTimeChartPresenter(_view, _chartPresenterContext);

         _residualVsTimeChart = new ParameterIdentificationResidualVsTimeChart().WithAxes();
         _parameterIdentification = A.Fake<ParameterIdentification>();

         _parameterIdentificationRunResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _parameterIdentification.Results).Returns(new[] {_parameterIdentificationRunResult});

         _residualResults = new ResidualsResult();
         _optimizationRunResult = new OptimizationRunResult {ResidualsResult = _residualResults};
         _parameterIdentificationRunResult.BestResult = _optimizationRunResult;


         A.CallTo(() => _parameterIdentification.MinObservedDataTime).Returns(10);
         A.CallTo(() => _parameterIdentification.MaxObservedDataTime).Returns(50);
      }
   }

   public class When_displaying_the_results_of_a_given_parameter_identification_as_residual_vs_time : concern_for_ParameterIdentificationResidualVsTimeChartPresenter
   {
      private OutputResiduals _outputResiduals1;
      private OutputResiduals _outputResiduals3;
      private OutputResiduals _outputResiduals2;
      private OutputMapping _outputMapping1;
      private OutputMapping _outputMapping2;
      private OutputMapping _outputMapping3;

      protected override void Context()
      {
         base.Context();
         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();
         _outputMapping3 = A.Fake<OutputMapping>();

         var observation1 = DomainHelperForSpecs.ObservedData("OBS1");
         A.CallTo(() => _outputMapping1.WeightedObservedData.ObservedData).Returns(observation1);
         var observation2 = DomainHelperForSpecs.ObservedData("OBS2");
         A.CallTo(() => _outputMapping2.WeightedObservedData.ObservedData).Returns(observation2);
         var observation3 = DomainHelperForSpecs.ObservedData("OBS3");
         A.CallTo(() => _outputMapping3.WeightedObservedData.ObservedData).Returns(observation3);


         _outputResiduals1 = new OutputResiduals("OutputPath1", _outputMapping1.WeightedObservedData, new[] {new Residual(11f, 12f, 1), new Residual(21f, 22f, 1)});
         _outputResiduals2 = new OutputResiduals("OutputPath2", _outputMapping2.WeightedObservedData, new[] {new Residual(31f, 32f, 1), new Residual(41f, 42f, 1)});
         _outputResiduals3 = new OutputResiduals("OutputPath1", _outputMapping3.WeightedObservedData, new[] {new Residual(51f, 52f, 1), new Residual(61f, 62f, 1)});

         _residualResults.AddOutputResiduals(_outputResiduals1);
         _residualResults.AddOutputResiduals(_outputResiduals2);
         _residualResults.AddOutputResiduals(_outputResiduals3);


         A.CallTo(() => _outputMapping1.FullOutputPath).Returns("OutputPath1");
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns("OutputPath2");
         A.CallTo(() => _outputMapping3.FullOutputPath).Returns("OutputPath1");


         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(new[] {_outputMapping1, _outputMapping2, _outputMapping3});
         A.CallTo(() => _parameterIdentification.AllObservedData).Returns(new[] {observation3, observation1, observation2});
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_residualVsTimeChart, _parameterIdentification);
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

      [Observation]
      public void all_observed_data_mapped_to_the_same_output_should_have_the_same_color()
      {
         _residualVsTimeChart.Curves.ElementAt(0).Color.ShouldBeEqualTo(_residualVsTimeChart.Curves.ElementAt(1).Color);
      }

      [Observation]
      public void only_one_residual_scatter_curve_should_be_visible_in_legend_per_observed_data()
      {
         _residualVsTimeChart.Curves.ElementAt(0).VisibleInLegend.ShouldBeTrue();
         _residualVsTimeChart.Curves.ElementAt(1).VisibleInLegend.ShouldBeFalse();
      }

      [Observation]
      public void should_have_added_the_name_of_the_observed_data_to_the_output_path_as_one_before_last_item()
      {
         var firstCurve = _residualVsTimeChart.Curves.ElementAt(0);
         var yData = firstCurve.yData;
         var pathArray = yData.QuantityInfo.Path.ToArray();
         pathArray[pathArray.Length - 2].ShouldBeEqualTo(_outputResiduals1.ObservedDataName);
      }
   }

   public class When_clearing_the_parameter_identification_residual_vs_time_chart_presenter : concern_for_ParameterIdentificationResidualVsTimeChartPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.AllObservedData).Returns(new[] {DomainHelperForSpecs.ObservedData()});
         sut.InitializeAnalysis(_residualVsTimeChart, _parameterIdentification);

         //only zero marker
         _residualVsTimeChart.Curves.Count.ShouldBeEqualTo(1);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_remove_the_zero_maker_curve_that_was_added_to_the_chart()
      {
         //zero marker removed
         _residualVsTimeChart.Curves.Count.ShouldBeEqualTo(0);
      }
   }
}