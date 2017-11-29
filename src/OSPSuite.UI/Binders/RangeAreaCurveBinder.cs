using System.Collections.Generic;
using System.Data;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Binders
{
   internal abstract class RangeAreaCurveBinder : CurveBinder
   {
      private Series _areaSeries;
      private Series _lineSeries;
      protected const string LOW = "Low";
      protected const string HIGH = "High";

      protected override IReadOnlyList<string> YColumnNames { get; } = new[] {LOW, HIGH, Y};

      protected RangeAreaCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView, DataMode dataMode) : base(curve, chartControl, chart, yAxisView, dataMode)
      {
      }

      protected override void CreateSeries()
      {
         _areaSeries = CreateSeries<RangeAreaSeriesView>(Curve.Id, ViewType.RangeArea, new[] {LOW, HIGH}, seriesView =>
         {
            seriesView.Marker1Visibility = DefaultBoolean.False;
            seriesView.Marker2Visibility = DefaultBoolean.False;
            seriesView.FillStyle.FillMode = FillMode.Solid;
         });
         _lineSeries = CreatePointLineSeries();
      }

      protected override void RefreshSeries()
      {
         var rangeAreaSeriesView = _areaSeries.View.DowncastTo<RangeAreaSeriesView>();
         rangeAreaSeriesView.Color = Color.FromArgb(Constants.RANGE_AREA_OPACITY, Curve.Color);

         UpdateRelatedSeriesOf(_areaSeries);
         UpdateLineSeries(_lineSeries);
      }

      protected override void SetRelatedRelativeValuesForRow(DataRow row, float max)
      {
         ScaleValue(row, max, HIGH);
      }

      public override void ShowCurveInLegend(bool showInLegend)
      {
         _areaSeries.ShowInLegend = false;
         _lineSeries.ShowInLegend = showInLegend;
      }
   }
}