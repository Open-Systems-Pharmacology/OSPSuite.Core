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
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_PredictedVsObservedChartService : ContextSpecification<PredictedVsObservedChartService>
   {
      protected ParameterIdentificationPredictedVsObservedChart _predictedVsObservedChart;
      protected DataColumn _concentrationObservationColumn;
      protected DataColumn _simulationColumn;
      protected ParameterIdentification _identification;
      private IDimensionFactory _dimensionFactory;
      protected DataRepository _simulationData;
      protected DataRepository _observedConcentrationData;
      protected DataColumn _fractionObservationColumn;
      private DataRepository _fractionObservedData;
      private IDisplayUnitRetriever _displayUnitRetriever;

      protected override void Context()
      {
         _identification = A.Fake<ParameterIdentification>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();

         A.CallTo(_dimensionFactory).WithReturnType<IDimension>().ReturnsLazily(x => x.Arguments.Get<IWithDimension>(0).Dimension);

         _predictedVsObservedChart = new ParameterIdentificationPredictedVsObservedChart().WithAxes();

         _observedConcentrationData = DomainHelperForSpecs.ObservedData();
         _concentrationObservationColumn = _observedConcentrationData.FirstDataColumn();

         _simulationData = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Simulation");
         _simulationColumn = _simulationData.FirstDataColumn();

         _fractionObservedData = DomainHelperForSpecs.ObservedData();
         _fractionObservationColumn = _fractionObservedData.FirstDataColumn();
         _fractionObservationColumn.Dimension = DomainHelperForSpecs.NoDimension();

         sut = new PredictedVsObservedChartService(_dimensionFactory, _displayUnitRetriever);
      }
   }

   public class When_the_observation_column_has_only_0_values : concern_for_PredictedVsObservedChartService
   {
      protected override void Context()
      {
         base.Context();
         _concentrationObservationColumn.Values = Enumerable.Repeat(0f, _concentrationObservationColumn.Values.Count).ToArray();
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _predictedVsObservedChart.AxisBy(AxisTypes.Y).Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
      }

      [Observation]
      public void the_identity_repository_should_be_null()
      {
         sut.AddIdentityCurves(_observedConcentrationData.ObservationColumns(), _predictedVsObservedChart).ShouldBeEmpty();
      }
   }

   public class When_observed_data_has_multiple_dimensions : concern_for_PredictedVsObservedChartService
   {
      protected override void Context()
      {
         base.Context();
         _simulationColumn.Dimension = _fractionObservationColumn.Dimension;
         A.CallTo(() => _identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString)).Returns(new List<DataColumn> {_fractionObservationColumn});
         sut.AddCurvesFor(_identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString), _simulationColumn, _predictedVsObservedChart);
      }

      protected override void Because()
      {
         sut.AddIdentityCurves(new[] {_concentrationObservationColumn, _fractionObservationColumn}, _predictedVsObservedChart);
      }

      [Observation]
      public void the_dimension_selected_to_plot_identity_curve_should_be_fraction()
      {
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension.ShouldBeEqualTo(DomainHelperForSpecs.NoDimension());
      }
   }

   public class When_observed_data_has_only_a_single_data_point : concern_for_PredictedVsObservedChartService
   {
      private IList<DataColumn> _allObservations;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString)).Returns(new List<DataColumn> {_concentrationObservationColumn});
         _baseGrid = new BaseGrid("basegrid", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {0f}};
         _allObservations = new List<DataColumn> {new DataColumn("name", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)};
         _allObservations.First().Values = new[] {1f};
         sut.AddCurvesFor(_identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString), _simulationColumn, _predictedVsObservedChart);
      }

      protected override void Because()
      {
         sut.AddIdentityCurves(_allObservations, _predictedVsObservedChart);
      }

      [Observation]
      public void the_resulting_repository_for_identity_contains_only_one_data_point()
      {
         _predictedVsObservedChart.Curves.Single(x => Equals(x.Name, "Identity")).xData.Values.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_charting_multiple_dimensions : concern_for_PredictedVsObservedChartService
   {
      private List<DataColumn> _allObservations;
      private BaseGrid _concentrationBaseGrid;
      private BaseGrid _fractionBaseGrid;
      private BaseGrid _secondFractionBaseGrid;
      private readonly IDimension _fractionDimensionForSpecs = DomainHelperForSpecs.FractionDimensionForSpecs();
      private readonly IDimension _concentrationDimensionForSpecs = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
      private DataRepository _concentrationRepository;
      private DataRepository _fractionRepository;
      private DataRepository _secondFractionRepository;

      protected override void Context()
      {
         base.Context();
         _concentrationBaseGrid = new BaseGrid("basegrid", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {0f}};
         _fractionBaseGrid = new BaseGrid("basegrid", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {0f}};
         _secondFractionBaseGrid = new BaseGrid("basegrid", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {0f}};

         var concentrationColumn = new DataColumn("name", _concentrationDimensionForSpecs, _concentrationBaseGrid) {Values = new[] {1f}};
         _concentrationRepository = new DataRepository {concentrationColumn};

         var fractionColumn = new DataColumn("name", _fractionDimensionForSpecs, _fractionBaseGrid) {Values = new[] {1f}};
         _fractionRepository = new DataRepository {fractionColumn};

         var secondFractionColumn = new DataColumn("name", _fractionDimensionForSpecs, _secondFractionBaseGrid) {Values = new[] {1f}};
         _secondFractionRepository = new DataRepository {secondFractionColumn};

         _allObservations = new List<DataColumn> {concentrationColumn, fractionColumn, secondFractionColumn};

         sut.AddCurvesFor(_allObservations, concentrationColumn, _predictedVsObservedChart);
      }

      protected override void Because()
      {
         sut.AddIdentityCurves(_allObservations, _predictedVsObservedChart);
         sut.SetXAxisDimension(_allObservations, _predictedVsObservedChart);
      }

      [Observation]
      public void the_chart_x_axis_should_be_set_to_the_correct_dimension()
      {
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension.DisplayName.ShouldBeEqualTo(_fractionDimensionForSpecs.DisplayName);
      }

      [Observation]
      public void the_chart_should_contain_identity_lines_for_each_dimension()
      {
         _predictedVsObservedChart.Curves.Count(curve => Equals(curve.Name, "Identity") && Equals(curve.xDimension.DisplayName, _fractionDimensionForSpecs.DisplayName)).ShouldBeEqualTo(1);
         _predictedVsObservedChart.Curves.Count(curve => Equals(curve.Name, "Identity") && Equals(curve.xDimension.DisplayName, _concentrationDimensionForSpecs.DisplayName)).ShouldBeEqualTo(1);
      }
   }

   public abstract class updating_x_axis_dimension : concern_for_PredictedVsObservedChartService
   {
      protected DataColumn _secondColumnForSimulation;
      protected DataColumn _secondColumnForObservations;

      protected override void Context()
      {
         base.Context();
         _secondColumnForObservations = DomainHelperForSpecs.ConcentrationColumnForObservedData(_observedConcentrationData.BaseGrid);
         _secondColumnForObservations.Dimension = SecondColumnDimension();
         _observedConcentrationData.Add(_secondColumnForObservations);
         _secondColumnForSimulation = DomainHelperForSpecs.ConcentrationColumnForSimulation("Simulation", _simulationColumn.BaseGrid);
         _secondColumnForSimulation.Dimension = SecondColumnDimension();
         _simulationData.Add(_secondColumnForSimulation);

         A.CallTo(() => _identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString)).Returns(new List<DataColumn> {_concentrationObservationColumn});
         sut.AddCurvesFor(_identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString), _simulationColumn, _predictedVsObservedChart);
         sut.AddIdentityCurves(new[] {_concentrationObservationColumn, _secondColumnForObservations}, _predictedVsObservedChart);
      }

      protected abstract IDimension SecondColumnDimension();

      protected override void Because()
      {
         sut.SetXAxisDimension(new[] {_concentrationObservationColumn, _secondColumnForObservations}, _predictedVsObservedChart);
      }
   }

   public class When_setting_the_x_dimension_of_the_chart_with_preferred_and_non_preferred_dimensions : updating_x_axis_dimension
   {
      protected override IDimension SecondColumnDimension()
      {
         return DomainHelperForSpecs.LengthDimensionForSpecs();
      }

      [Observation]
      public void only_preferred_axes_are_visible()
      {
         _predictedVsObservedChart.Axes.Count.ShouldBeEqualTo(3);
         _predictedVsObservedChart.Axes.Count(axis => axis.Visible && axis.Dimension == DomainHelperForSpecs.ConcentrationDimensionForSpecs()).ShouldBeEqualTo(2);
         _predictedVsObservedChart.Axes.Count(axis => !axis.Visible && axis.Dimension != DomainHelperForSpecs.ConcentrationDimensionForSpecs()).ShouldBeEqualTo(1);
      }
   }

   public class When_setting_the_x_dimension_of_the_chart_with_no_preferred_dimensions : updating_x_axis_dimension
   {
      protected override void Context()
      {
         base.Context();
         _observedConcentrationData.FirstDataColumn().Dimension = SecondColumnDimension();
         _simulationData.FirstDataColumn().Dimension = SecondColumnDimension();
      }

      protected override IDimension SecondColumnDimension()
      {
         return DomainHelperForSpecs.LengthDimensionForSpecs();
      }

      [Observation]
      public void only_non_preferred_are_visible()
      {
         _predictedVsObservedChart.Axes.Count(axis => axis.Visible && axis.Dimension == DomainHelperForSpecs.LengthDimensionForSpecs()).ShouldBeEqualTo(2);
         _predictedVsObservedChart.Axes.Count(axis => !axis.Visible && axis.Dimension != DomainHelperForSpecs.LengthDimensionForSpecs()).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_x_axis_of_the_chart_should_be_updated()
      {
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y2).Dimension);
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Scaling.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y2).Scaling);
         _predictedVsObservedChart.AxisBy(AxisTypes.X).UnitName.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y2).UnitName);
      }
   }

   public class When_setting_the_x_dimension_of_the_chart_with_only_preferred_dimensions : updating_x_axis_dimension
   {
      protected override IDimension SecondColumnDimension()
      {
         return DomainHelperForSpecs.ConcentrationDimensionForSpecs();
      }

      [Observation]
      public void all_axes_are_visible()
      {
         _predictedVsObservedChart.Axes.Count.ShouldBeEqualTo(2);
         _predictedVsObservedChart.Axes.Each(axis => axis.Visible.ShouldBeTrue());
      }

      [Observation]
      public void the_x_axis_of_the_chart_should_be_updated()
      {
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y).Dimension);
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Scaling.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y).Scaling);
         _predictedVsObservedChart.AxisBy(AxisTypes.X).UnitName.ShouldBeEqualTo(_predictedVsObservedChart.AxisBy(AxisTypes.Y).UnitName);
      }
   }

   public class When_adding_curves_to_a_chart : concern_for_PredictedVsObservedChartService
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString)).Returns(new List<DataColumn> {_concentrationObservationColumn});
      }

      protected override void Because()
      {
         sut.AddCurvesFor(_identification.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString), _simulationColumn, _predictedVsObservedChart);
         sut.AddIdentityCurves(new[] {_concentrationObservationColumn}, _predictedVsObservedChart);
      }

      [Observation]
      public void The_columns_should_come_from_observed_and_simulated_data_value_column()
      {
         var curve = _predictedVsObservedChart.Curves.First();
         curve.xData.ShouldBeEqualTo(_concentrationObservationColumn);
         curve.yData.ShouldBeEqualTo(_simulationColumn);
      }

      [Observation]
      public void the_curves_should_have_the_same_x_and_y_dimension()
      {
         _predictedVsObservedChart.Curves.All(curve => Equals(curve.xDimension, curve.yDimension)).ShouldBeTrue();
      }

      [Observation]
      public void the_curve_x_axis_should_have_the_merged_dimension()
      {
         _predictedVsObservedChart.AxisBy(AxisTypes.X).Dimension.ShouldBeEqualTo(_concentrationObservationColumn.Dimension);
      }

      [Observation]
      public void the_chart_should_have_two_curves()
      {
         _predictedVsObservedChart.Curves.Count.ShouldBeEqualTo(2);
      }
   }
}