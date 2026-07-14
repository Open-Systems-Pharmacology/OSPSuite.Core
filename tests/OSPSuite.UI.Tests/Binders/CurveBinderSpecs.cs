using System;
using System.Data;
using System.Linq;
using DevExpress.XtraCharts;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using Axis = OSPSuite.Core.Chart.Axis;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   public abstract class concern_for_CurveBinder : ContextSpecification<ICurveBinder>
   {
      protected UxChartControl _chartControl;
      protected CurveChart _chart;
      protected Curve _curve;
      protected IDimensionFactory _dimensionFactory;
      protected BaseGrid _baseGrid;
      protected DataColumn _dataColumn;
      protected Axis _xAxis;
      protected Axis _yAxis;

      private const string X = "X";
      private const string Y = "Y";
      private const string INDEX_OF_VALUE_IN_CURVE = "INDEX_OF_VALUE_IN_CURVE";

      protected override void Context()
      {
         base.Context();
         _chartControl = new UxChartControl();
         //an initial series is required so that the diagram of the chart control is available
         _chartControl.Series.Add(new Series("dummySeries", ViewType.ScatterLine));

         _dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(A<DataColumn>._)).ReturnsLazily(x => x.GetArgument<DataColumn>(0).Dimension);

         _baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = TimeValues};
         _dataColumn = new DataColumn("Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            Values = ColumnValues,
            DataInfo = {Origin = ColumnOrigins.Observation}
         };

         _xAxis = new Axis(AxisTypes.X) {Dimension = _baseGrid.Dimension, UnitName = _baseGrid.Dimension.BaseUnit.Name, Scaling = XScaling};
         _yAxis = new Axis(AxisTypes.Y) {Dimension = _dataColumn.Dimension, UnitName = _dataColumn.Dimension.BaseUnit.Name, Scaling = YScaling};

         _chart = new CurveChart();
         _chart.AddAxis(_xAxis);
         _chart.AddAxis(_yAxis);

         _curve = new Curve();
         _curve.SetxData(_baseGrid, _dimensionFactory);
         _curve.SetyData(_dataColumn, _dimensionFactory);

         var yAxisBinder = new AxisBinder(_yAxis, _chartControl, new NumericFormatterOptions());
         sut = new CurveBinderFactory(new CurveToDataModeMapper()).CreateFor(_curve, _chartControl, _chart, yAxisBinder);
      }

      protected virtual float[] TimeValues { get; } = {1f, 2f, 3f, 4f};
      protected abstract float[] ColumnValues { get; }
      protected virtual Scalings XScaling { get; } = Scalings.Linear;
      protected virtual Scalings YScaling { get; } = Scalings.Linear;

      protected Series CurveSeries => _chartControl.Series.Cast<Series>().First(series => string.Equals(series.Name, _curve.Id));

      protected DataTable DataTableForCurveSeries => CurveSeries.DataSource.DowncastTo<DataTable>();

      protected object YValueAt(int rowIndex) => DataTableForCurveSeries.Rows[rowIndex][Y];

      protected float XValueAt(int rowIndex) => (float) DataTableForCurveSeries.Rows[rowIndex][X];

      protected int OriginalIndexAt(int rowIndex) => (int) DataTableForCurveSeries.Rows[rowIndex][INDEX_OF_VALUE_IN_CURVE];
   }

   public class When_binding_a_curve_containing_zero_values_to_a_logarithmic_y_axis : concern_for_CurveBinder
   {
      protected override float[] ColumnValues { get; } = {10f, 0f, 0f, 30f};
      protected override Scalings YScaling { get; } = Scalings.Log;

      [Observation]
      public void should_add_a_row_for_each_point()
      {
         DataTableForCurveSeries.Rows.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_leave_the_y_value_empty_for_values_that_cannot_be_plotted_so_that_the_curve_shows_a_gap()
      {
         YValueAt(0).ShouldBeEqualTo(10f);
         YValueAt(1).ShouldBeEqualTo(DBNull.Value);
         YValueAt(2).ShouldBeEqualTo(DBNull.Value);
         YValueAt(3).ShouldBeEqualTo(30f);
      }

      [Observation]
      public void should_keep_the_x_value_and_the_original_curve_index_for_empty_points()
      {
         XValueAt(1).ShouldBeEqualTo(2f);
         OriginalIndexAt(1).ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_keep_the_default_empty_point_handling_which_renders_empty_points_as_breaks_in_the_line()
      {
         //Despite its name, the default InsertZero mode renders empty points of a line series as breaks.
         //Ignore or Interpolate would connect the neighboring points across the gap.
         CurveSeries.View.DowncastTo<XYDiagramSeriesViewBase>().EmptyPointOptions.ProcessPoints.ShouldBeEqualTo(ProcessEmptyPointsMode.InsertZero);
      }

      [Observation]
      public void should_return_the_underlying_y_value_for_empty_rows_as_well_as_for_regular_rows()
      {
         sut.YValueForRow(DataTableForCurveSeries.Rows[0]).ShouldBeEqualTo(10.0);
         sut.YValueForRow(DataTableForCurveSeries.Rows[1]).ShouldBeEqualTo(0.0);
      }
   }

   public class When_binding_a_curve_containing_zero_values_to_a_linear_y_axis : concern_for_CurveBinder
   {
      protected override float[] ColumnValues { get; } = {10f, 0f, 0f, 30f};

      [Observation]
      public void should_plot_all_values_including_zero()
      {
         DataTableForCurveSeries.Rows.Count.ShouldBeEqualTo(4);
         YValueAt(0).ShouldBeEqualTo(10f);
         YValueAt(1).ShouldBeEqualTo(0f);
         YValueAt(2).ShouldBeEqualTo(0f);
         YValueAt(3).ShouldBeEqualTo(30f);
      }
   }

   public class When_binding_a_curve_containing_nan_values : concern_for_CurveBinder
   {
      protected override float[] ColumnValues { get; } = {10f, float.NaN, 30f, 40f};

      [Observation]
      public void should_leave_the_y_value_empty_for_the_nan_value_so_that_the_curve_shows_a_gap()
      {
         DataTableForCurveSeries.Rows.Count.ShouldBeEqualTo(4);
         YValueAt(0).ShouldBeEqualTo(10f);
         YValueAt(1).ShouldBeEqualTo(DBNull.Value);
         YValueAt(2).ShouldBeEqualTo(30f);
         YValueAt(3).ShouldBeEqualTo(40f);
      }
   }

   public class When_binding_a_curve_with_x_values_that_cannot_be_plotted_on_a_logarithmic_x_axis : concern_for_CurveBinder
   {
      protected override float[] TimeValues { get; } = {0f, 2f, 3f, 4f};
      protected override float[] ColumnValues { get; } = {10f, 20f, 30f, 40f};
      protected override Scalings XScaling { get; } = Scalings.Log;

      [Observation]
      public void should_not_add_a_row_for_points_without_a_valid_x_value()
      {
         DataTableForCurveSeries.Rows.Count.ShouldBeEqualTo(3);
         OriginalIndexAt(0).ShouldBeEqualTo(1);
      }
   }
}
