using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationResidualVsTimeChart : ContextSpecification<SimulationResidualVsTimeChart>
   {
      protected const string CLONED_ID = "CLONED_ID";
      protected const string OUTPUT_PATH = "Sim|Liver|Concentration";
      protected static readonly Color SCATTER_COLOR = Color.Magenta;
      protected ICloneManager _cloneManager;
      protected DataRepository _observedData;
      protected DataRepository _scatterRepository;

      protected override void Context()
      {
         var dimensionFactory = DimensionFactoryForSpecs.Factory;
         var idGenerator = A.Fake<IIdGenerator>();
         A.CallTo(() => idGenerator.NewId()).Returns(CLONED_ID);
         var objectBaseFactory = new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), dimensionFactory, idGenerator, A.Fake<ICreationMetaDataFactory>());
         _cloneManager = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         _observedData = DomainHelperForSpecs.ObservedData("OBS");
         var outputResiduals = new OutputResiduals(OUTPUT_PATH, _observedData, new[] { new Residual(1f, 0.5f, 1), new Residual(2f, -0.5f, 1) });

         //filled the way the simulation residual presenter does when the chart is displayed
         sut = new SimulationResidualVsTimeChart().WithAxes().WithId("SOURCE_ID");
         var residualsVsTimeChartService = new ResidualsVsTimeChartService(dimensionFactory);
         _scatterRepository = residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(sut, outputResiduals);
         residualsVsTimeChartService.AddCurvesFor(_scatterRepository, (column, curve) => curve.Color = SCATTER_COLOR, sut);
         residualsVsTimeChartService.AddZeroMarkerCurveToChart(sut, 1f, 2f);
      }
   }

   public class When_cloning_a_simulation_residual_vs_time_chart : concern_for_SimulationResidualVsTimeChart
   {
      private SimulationResidualVsTimeChart _clone;

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      private DataRepository clonedRepository() => _clone.DataRepositories.Single();

      [Observation]
      public void should_own_a_copy_of_the_scatter_repository_with_an_id_based_on_the_clone_id()
      {
         clonedRepository().ShouldNotBeEqualTo(_scatterRepository);
         clonedRepository().Id.ShouldBeEqualTo($"{CLONED_ID}-{OUTPUT_PATH}-{_observedData.Id}");
      }

      [Observation]
      public void should_give_new_ids_to_the_copied_columns()
      {
         clonedRepository().Columns.Select(x => x.Id).Intersect(_scatterRepository.Columns.Select(x => x.Id)).ShouldBeEmpty();
      }

      [Observation]
      public void should_not_copy_the_zero_marker_curve()
      {
         _clone.Curves.Any(x => string.Equals(x.yData.Name, ResidualsVsTimeChart.ZERO)).ShouldBeFalse();
      }

      [Observation]
      public void should_plot_the_scatter_curve_from_the_copied_columns_with_the_source_color()
      {
         var clonedCurve = _clone.Curves.Single();
         clonedCurve.xData.ShouldBeEqualTo(clonedRepository().BaseGrid);
         clonedCurve.yData.ShouldBeEqualTo(clonedRepository().FirstDataColumn());
         clonedCurve.Color.ShouldBeEqualTo(SCATTER_COLOR);
      }
   }
}
