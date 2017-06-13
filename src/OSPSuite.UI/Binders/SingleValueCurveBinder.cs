using System.Collections.Generic;
using System.Data;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.UnitSystem;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal class SingleValueCurveBinder : CurveBinder
   {
      private Series _lineSeries;
      protected override IReadOnlyList<string> YColumnNames { get; } = new[] {Y};

      public SingleValueCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxis) : base(curve, chartControl, chart, yAxis, DataMode.SingleValue)
      {
      }

      protected override void RefreshSeries()
      {
         UpdateLineSeries(_lineSeries);
      }

      protected override bool AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue)
      {
         //no related values here
         return true;
      }

      protected override DataColumn ActiveYData => Curve.yData;

      protected override void SetRelatedRelativeValuesForRow(DataRow row, float max)
      {
         //no related values here
      }

      public override void ShowCurveInLegend(bool showInLegend)
      {
         _lineSeries.ShowInLegend = showInLegend;
      }

      protected override void CreateSeries()
      {
         _lineSeries= CreateScatterLineSeries(Curve.Id);
      }
   }
}