using System.Collections.Generic;
using System.Data;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal abstract class StdCurveBinder : CurveBinder
   {
      private Series _financialSeries;
      private Series _lineSeries;
      protected override IReadOnlyList<string> YColumnNames { get; } = new[] {LOW, HIGH, Y, Y2};
      protected const string Y2 = "Y2";
      protected const string LOW = "Low";
      protected const string HIGH = "High";

      protected override DataColumn ActiveYData => Curve.yData;

      protected StdCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView, DataMode dataMode) : base(curve, chartControl, chart, yAxisView, dataMode)
      {
      }

      protected override void CreateSeries()
      {
         _financialSeries= CreateSeries<FinancialSeriesViewBase>(Curve.Id, ViewType.CandleStick, YColumnNames, seriesView =>
         {
            seriesView.ReductionOptions.Visible = false;
            seriesView.LevelLineLength = 0;
            seriesView.LineThickness = 1;
         });

         _lineSeries= CreatePointLineSeries();
         createUpperPointSeries();
         createLowerPointSeries();
      }

      protected override void RefreshSeries()
      {
         var financialSeriesView = _financialSeries.View.DowncastTo<FinancialSeriesViewBase>();
         financialSeriesView.Color = Curve.Color;

         UpdateRelatedSeriesOf(_financialSeries);
         UpdateLineSeries(_lineSeries);
      }

      protected override void SetRelatedRelativeValuesForRow(DataRow row, float max)
      {
         ScaleValue(row, max, Y2);
         ScaleValue(row, max, LOW);
         ScaleValue(row, max, HIGH);
      }

      private void createUpperPointSeries()
      {
         CreateSeries<PointSeriesView>($"{Curve.Id}_Upper", ViewType.Point, HIGH, pointView =>
         {
            pointView.PointMarkerOptions.Kind = MarkerKind.InvertedTriangle;
            pointView.PointMarkerOptions.Size = 4;
         });
      }

      private void createLowerPointSeries()
      {
         CreateSeries<PointSeriesView>($"{Curve.Id}_Lower", ViewType.Point, LOW, pointView =>
         {
            pointView.PointMarkerOptions.Kind = MarkerKind.Triangle;
            pointView.PointMarkerOptions.Size = 4;
         });
      }

      public override void ShowCurveInLegend(bool showInLegend)
      {
         _financialSeries.ShowInLegend = false;
         _lineSeries.ShowInLegend = showInLegend;
      }
   }
}