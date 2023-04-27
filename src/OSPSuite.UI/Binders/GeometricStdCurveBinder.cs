using System.Data;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal class GeometricStdCurveBinder : StdCurveBinder
   {
      public GeometricStdCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView) : base(curve, chartControl, chart, yAxisView, DataMode.GeometricStdDev)
      {
      }

      protected override bool AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue)
      {
         var relatedColumn = yData.GetRelatedColumn(AuxiliaryType.GeometricStdDev);
         var stdDev = relatedColumn.GetValue(baseValue);
         if (!IsValidValue(stdDev) || stdDev == 0)
            stdDev = 1;

         if (!IsValidYValue(y / stdDev))
            return false;

         row[LOW] = y / stdDev;
         row[HIGH] = y * stdDev;
         row[Y2] = y;

         return true;
      }
   }
}