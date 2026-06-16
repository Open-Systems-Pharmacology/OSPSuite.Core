using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ResidualsVsTimeChartService : ContextSpecification<ResidualsVsTimeChartService>
   {
      private IDimensionFactory _dimensionFactory;
      protected AnalysisChartWithLocalRepositories _residualsVsTimeChart;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _residualsVsTimeChart = new SimulationResidualVsTimeChart();

         sut = new ResidualsVsTimeChartService(_dimensionFactory);
      }

      public class When_creating_scatter_data_repository : concern_for_ResidualsVsTimeChartService
      {
         private const string RepositoryId = "repository Id";
         private const string RepositoryName = "repository Name";
         private const string OutputPath = "Output Path";
         private OutputResiduals _outputResiduals;
         private DataRepository _resultDataRepository;

         protected override void Context()
         {
            base.Context();
            _outputResiduals = A.Fake<OutputResiduals>();
            A.CallTo(() => _outputResiduals.FullOutputPath).Returns(OutputPath);
         }

         protected override void Because()
         {
            _resultDataRepository = sut.CreateScatterDataRepository(RepositoryId, RepositoryName, _outputResiduals);
         }

         [Observation]
         public void the_resulting_repository_should_have_the_correct_id()
         {
            _resultDataRepository.Id.ShouldBeEqualTo(RepositoryId);
         }

         [Observation]
         public void the_resulting_repository_should_have_the_correct_name()
         {
            _resultDataRepository.Name.ShouldBeEqualTo(RepositoryName);
         }

         [Observation]
         public void the_resulting_repository_should_have_the_correct_output_path()
         {
            _resultDataRepository.FirstDataColumn().QuantityInfo.Path.ShouldContain(OutputPath);
         }
      }

      public class When_getting_a_scatter_repository_that_was_persisted_in_the_chart_without_base_grid_path : concern_for_ResidualsVsTimeChartService
      {
         private OutputResiduals _outputResiduals;
         private DataRepository _existingRepository;
         private DataRepository _result;

         protected override void Context()
         {
            base.Context();
            _outputResiduals = A.Fake<OutputResiduals>();
            A.CallTo(() => _outputResiduals.FullOutputPath).Returns("Output Path");
            A.CallTo(() => _outputResiduals.Residuals).Returns(new[] {new Residual(1f, 2f, 1)});

            var id = $"{_residualsVsTimeChart.Id}-{_outputResiduals.FullOutputPath}-{_outputResiduals.ObservedData.Id}";
            _existingRepository = sut.CreateScatterDataRepository(id, "Simulation Results", _outputResiduals);
            //simulates a repository persisted with the chart before the base grid path was introduced
            _existingRepository.BaseGrid.QuantityInfo.Path = new string[0];
            _residualsVsTimeChart.AddRepository(_existingRepository);
         }

         protected override void Because()
         {
            _result = sut.GetOrCreateScatterDataRepositoryInChart(_residualsVsTimeChart, _outputResiduals);
         }

         [Observation]
         public void should_reuse_the_existing_repository()
         {
            _result.ShouldBeEqualTo(_existingRepository);
         }

         [Observation]
         public void should_set_the_base_grid_path()
         {
            _result.BaseGrid.QuantityInfo.PathAsString.ShouldBeEqualTo("Time");
         }
      }

      public class When_generating_the_zero_marker : concern_for_ResidualsVsTimeChartService
      {
         private readonly float _minObservedDataTime = 1;
         private readonly float _maxObservedDataTime = 10;

         protected override void Because()
         {
            sut.AddZeroMarkerCurveToChart(_residualsVsTimeChart, _minObservedDataTime, _maxObservedDataTime);
         }

         [Observation]
         public void a_call_to_the_chart_to_add_the_curve_should_have_happened()
         {
            _residualsVsTimeChart.Curves.Count.ShouldBeEqualTo(1);
         }

         [Observation]
         public void the_zero_curve_should_be_correctly_named()
         {
            _residualsVsTimeChart.Curves.First().Name.ShouldBeEqualTo("Zero");
         }

         [Observation]
         public void the_zero_curve_should_have_correct_curve_options()
         {
            _residualsVsTimeChart.Curves.First().CurveOptions.LineStyle.ShouldBeEqualTo(LineStyles.Dash);
            _residualsVsTimeChart.Curves.First().CurveOptions.Visible.ShouldBeTrue();
         }

         [Observation]
         public void the_zero_curve_should_have_correct_start_and_end_values()
         {
            _residualsVsTimeChart.Curves.First().xData.InternalValues[0].ShouldBeEqualTo(1);
            _residualsVsTimeChart.Curves.First().xData.InternalValues[1].ShouldBeEqualTo(10);
         }

         [Observation]
         public void the_zero_curve_columns_should_have_paths_usable_in_chart_templates()
         {
            var curve = _residualsVsTimeChart.Curves.First();
            curve.yData.QuantityInfo.PathAsString.ShouldBeEqualTo("Zero");
            curve.xData.QuantityInfo.PathAsString.ShouldBeEqualTo("Time");
         }
      }

      public class When_generating_the_zero_marker_for_an_observed_data_having_only_one_data_point : concern_for_ResidualsVsTimeChartService
      {
         private readonly float _minObservedDataTime = 1;
         private readonly float _maxObservedDataTime = 1;

         protected override void Because()
         {
            sut.AddZeroMarkerCurveToChart(_residualsVsTimeChart, _minObservedDataTime, _maxObservedDataTime);
         }

         [Observation]
         public void should_have_created_the_zero_curve_in_the_chart_with_only_one_value()
         {
            _residualsVsTimeChart.Curves.Count.ShouldBeEqualTo(1);
            var curve = _residualsVsTimeChart.Curves.First();
            curve.Name.ShouldBeEqualTo("Zero");
            curve.xData.InternalValues[0].ShouldBeEqualTo(1);
         }
      }
   }
}