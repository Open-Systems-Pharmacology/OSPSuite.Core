using System.Drawing;
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

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationResidualVsTimeChart : ContextSpecification<ParameterIdentificationResidualVsTimeChart>
   {
      protected const string SOURCE_ID = "SOURCE_ID";
      protected const string CLONED_ID = "CLONED_ID";
      protected const string OUTPUT_PATH = "Sim|Liver|Concentration";
      protected const string CUSTOM_CAPTION = "Custom residuals";
      protected const int RUN_INDEX = 1;
      protected static readonly Color SCATTER_COLOR = Color.Magenta;
      protected ICloneManager _cloneManager;
      protected ResidualsVsTimeChartService _residualsVsTimeChartService;
      protected DataRepository _observedData;
      protected OutputResiduals _outputResiduals;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         var idGenerator = A.Fake<IIdGenerator>();
         A.CallTo(() => idGenerator.NewId()).Returns(CLONED_ID);
         var objectBaseFactory = new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), _dimensionFactory, idGenerator, A.Fake<ICreationMetaDataFactory>());
         _cloneManager = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());
         _residualsVsTimeChartService = new ResidualsVsTimeChartService(_dimensionFactory);

         _observedData = DomainHelperForSpecs.ObservedData("OBS");
         _outputResiduals = new OutputResiduals(OUTPUT_PATH, _observedData, new[] { new Residual(1f, 0.5f, 1), new Residual(2f, -0.5f, 1) });

         sut = new ParameterIdentificationResidualVsTimeChart().WithAxes().WithId(SOURCE_ID);
      }

      protected DataRepository AddScatterAndZeroMarkerCurvesToSource()
      {
         var scatterRepository = _residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(sut, _outputResiduals, RUN_INDEX);
         addScatterCurvesFor(scatterRepository);
         _residualsVsTimeChartService.AddZeroMarkerCurveToChart(sut, 1f, 2f);
         return scatterRepository;
      }

      protected void AddScatterCurveFor(DataRepository scatterRepository)
      {
         sut.AddRepository(scatterRepository);
         addScatterCurvesFor(scatterRepository);
      }

      private void addScatterCurvesFor(DataRepository scatterRepository)
      {
         _residualsVsTimeChartService.AddCurvesFor(scatterRepository, (column, curve) =>
         {
            curve.Color = SCATTER_COLOR;
            curve.Symbol = Symbols.Diamond;
         }, sut);
      }

      protected static Curve CurveFor(CurveChart chart, DataRepository repository)
      {
         return chart.FindCurveWithSameData(repository.BaseGrid, repository.FirstDataColumn());
      }

      protected static string[] AllIdsIn(AnalysisChartWithLocalRepositories chart)
      {
         return chart.DataRepositories.Select(x => x.Id)
            .Concat(chart.DataRepositories.SelectMany(x => x.Columns).Select(x => x.Id))
            .ToArray();
      }
   }

   public class When_cloning_a_parameter_identification_residual_vs_time_chart : concern_for_ParameterIdentificationResidualVsTimeChart
   {
      private DataRepository _scatterRepository;
      private ParameterIdentificationResidualVsTimeChart _clone;

      protected override void Context()
      {
         base.Context();
         _scatterRepository = AddScatterAndZeroMarkerCurvesToSource();
         sut.YAxis.Caption = CUSTOM_CAPTION;
         sut.YAxis.Min = -2f;
         sut.YAxis.Max = 2f;
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      [Observation]
      public void should_own_a_copy_of_the_scatter_repository_with_an_id_based_on_the_clone_id()
      {
         var clonedRepository = _clone.DataRepositories.Single();
         clonedRepository.ShouldNotBeEqualTo(_scatterRepository);
         clonedRepository.Id.ShouldBeEqualTo($"{CLONED_ID}-{OUTPUT_PATH}-{_observedData.Id}-{RUN_INDEX}");
      }

      [Observation]
      public void should_plot_the_scatter_curve_from_the_copied_repository_with_the_source_color_and_symbol()
      {
         var clonedCurve = CurveFor(_clone, _clone.DataRepositories.Single());
         clonedCurve.ShouldNotBeNull();
         clonedCurve.Color.ShouldBeEqualTo(SCATTER_COLOR);
         clonedCurve.Symbol.ShouldBeEqualTo(Symbols.Diamond);
      }

      [Observation]
      public void should_not_copy_the_zero_marker_curve()
      {
         _clone.Curves.Count.ShouldBeEqualTo(1);
         _clone.Curves.Any(x => string.Equals(x.yData.Name, ResidualsVsTimeChart.ZERO)).ShouldBeFalse();
      }

      [Observation]
      public void should_not_share_any_repository_or_column_id_with_the_source()
      {
         AllIdsIn(_clone).Length.ShouldBeEqualTo(AllIdsIn(sut).Length);
         AllIdsIn(_clone).Intersect(AllIdsIn(sut)).ShouldBeEmpty();
         AllIdsIn(_clone).Distinct().Count().ShouldBeEqualTo(AllIdsIn(_clone).Length);
      }

      [Observation]
      public void should_keep_the_y_axis_caption_and_range()
      {
         _clone.YAxis.Caption.ShouldBeEqualTo(CUSTOM_CAPTION);
         _clone.YAxis.Min.ShouldBeEqualTo(-2f);
         _clone.YAxis.Max.ShouldBeEqualTo(2f);
      }

      [Observation]
      public void should_leave_the_source_chart_unchanged()
      {
         sut.DataRepositories.ShouldOnlyContain(_scatterRepository);
         sut.Curves.Count.ShouldBeEqualTo(2);
         CurveFor(sut, _scatterRepository).ShouldNotBeNull();
      }
   }

   public class When_cloning_a_residual_vs_time_chart_holding_several_scatter_repositories : concern_for_ParameterIdentificationResidualVsTimeChart
   {
      private const string OTHER_OUTPUT_PATH = "Sim|Kidney|Concentration";
      private ParameterIdentificationResidualVsTimeChart _clone;

      protected override void Context()
      {
         base.Context();
         AddScatterAndZeroMarkerCurvesToSource();
         var otherResiduals = new OutputResiduals(OTHER_OUTPUT_PATH, _observedData, new[] { new Residual(1f, 0.25f, 1), new Residual(3f, -0.25f, 1) });
         AddScatterCurveFor(_residualsVsTimeChartService.CreateScatterDataRepository($"{SOURCE_ID}-{OTHER_OUTPUT_PATH}-{_observedData.Id}-{RUN_INDEX}", "Simulation Results", otherResiduals));
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      private Curve clonedCurveFor(string outputPath)
      {
         return CurveFor(_clone, _clone.DataRepositories.FindById($"{CLONED_ID}-{outputPath}-{_observedData.Id}-{RUN_INDEX}"));
      }

      [Observation]
      public void should_plot_each_scatter_curve_from_the_copy_of_its_own_repository()
      {
         _clone.Curves.Count.ShouldBeEqualTo(2);
         clonedCurveFor(OUTPUT_PATH).ShouldNotBeNull();
         clonedCurveFor(OTHER_OUTPUT_PATH).ShouldNotBeNull();
      }
   }

   public class When_the_residual_service_updates_a_cloned_residual_vs_time_chart : concern_for_ParameterIdentificationResidualVsTimeChart
   {
      private DataRepository _scatterRepository;
      private ParameterIdentificationResidualVsTimeChart _clone;
      private DataRepository _copiedRepository;
      private OutputResiduals _updatedResiduals;
      private DataRepository _result;

      protected override void Context()
      {
         base.Context();
         _scatterRepository = AddScatterAndZeroMarkerCurvesToSource();
         _clone = _cloneManager.Clone(sut);
         _copiedRepository = _clone.DataRepositories.Single();
         _updatedResiduals = new OutputResiduals(OUTPUT_PATH, _observedData, new[] { new Residual(1f, 0.25f, 1), new Residual(2f, -0.25f, 1) });
      }

      protected override void Because()
      {
         _result = _residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(_clone, _updatedResiduals, RUN_INDEX);
         _residualsVsTimeChartService.AddCurvesFor(_result, (column, curve) => { }, _clone);
      }

      [Observation]
      public void should_return_the_copied_repository()
      {
         _result.ShouldBeEqualTo(_copiedRepository);
         _clone.DataRepositories.ShouldOnlyContain(_copiedRepository);
      }

      [Observation]
      public void should_not_add_new_scatter_curves()
      {
         _clone.Curves.Count.ShouldBeEqualTo(1);
         CurveFor(_clone, _result).Color.ShouldBeEqualTo(SCATTER_COLOR);
      }

      [Observation]
      public void should_update_the_values_of_the_copied_repository_only()
      {
         _result.FirstDataColumn().Values.ShouldOnlyContainInOrder(0.25f, -0.25f);
         _scatterRepository.FirstDataColumn().Values.ShouldOnlyContainInOrder(0.5f, -0.5f);
      }
   }

   public class When_cloning_a_residual_vs_time_chart_holding_a_repository_without_the_chart_id_prefix : concern_for_ParameterIdentificationResidualVsTimeChart
   {
      private DataRepository _repositoryOfAnotherChart;
      private ParameterIdentificationResidualVsTimeChart _clone;

      protected override void Context()
      {
         base.Context();
         _repositoryOfAnotherChart = _residualsVsTimeChartService.CreateScatterDataRepository($"ANOTHER_CHART_ID-{OUTPUT_PATH}-{_observedData.Id}-{RUN_INDEX}", "Simulation Results", _outputResiduals);
         AddScatterCurveFor(_repositoryOfAnotherChart);
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      [Observation]
      public void should_keep_the_new_id_generated_for_the_copy()
      {
         var clonedRepository = _clone.DataRepositories.Single();
         clonedRepository.Id.ShouldNotBeEqualTo(_repositoryOfAnotherChart.Id);
         clonedRepository.Id.StartsWith(CLONED_ID).ShouldBeFalse();
      }

      [Observation]
      public void should_plot_the_scatter_curve_from_the_copied_repository()
      {
         CurveFor(_clone, _clone.DataRepositories.Single()).ShouldNotBeNull();
      }
   }

   public class When_cloning_a_residual_vs_time_chart_holding_a_repository_whose_id_only_starts_with_the_chart_id : concern_for_ParameterIdentificationResidualVsTimeChart
   {
      private ParameterIdentificationResidualVsTimeChart _clone;

      protected override void Context()
      {
         base.Context();
         var repositoryOfAnotherChart = _residualsVsTimeChartService.CreateScatterDataRepository($"{SOURCE_ID}2-{OUTPUT_PATH}-{_observedData.Id}-{RUN_INDEX}", "Simulation Results", _outputResiduals);
         AddScatterCurveFor(repositoryOfAnotherChart);
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      [Observation]
      public void should_keep_the_new_id_generated_for_the_copy()
      {
         _clone.DataRepositories.Single().Id.StartsWith(CLONED_ID).ShouldBeFalse();
      }
   }
}
