using System.Data;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal class ArithmeticStdCurveBinder : StdCurveBinder
   {
      public ArithmeticStdCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView) : base(curve, chartControl, chart, yAxisView, DataMode.ArithmeticStdDev)
      {
      }

      protected override bool AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue)
      {
         var relatedColumn = yData.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);
         var stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, relatedColumn.GetValue(baseValue));
         if (!IsValidValue(stdDev))
            stdDev = 0;

         row[HIGH] = y + stdDev;
         row[LOW] = y - stdDev;
         row[Y2] = y;

         if (!IsValidYValue(y - stdDev))
            row[LOW] = y;

         return true;
      }
   }
}