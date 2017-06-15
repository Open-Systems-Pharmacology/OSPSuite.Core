using System.Data;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal class ArithmeticMeanAreaCurveBinder : RangeAreaCurveBinder
   {
      public ArithmeticMeanAreaCurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView) : base(curve, chartControl, chart, yAxisView, DataMode.ArithmeticMeanArea)
      {
      }

      protected override bool AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue)
      {
         var stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, Curve.yData.GetValue(baseValue));
         if (!IsValidValue(stdDev))
            stdDev = 0;

         if (!IsValidYValue(y - stdDev))
            return false;

         row[LOW] = y - stdDev;
         row[HIGH] = y + stdDev;

         return true;
      }

      protected override DataColumn ActiveYData => Curve.yData.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);

      protected override float GetRelatedMax(string columnName, DataRow maxRow, float max)
      {
         return ((float) maxRow[LOW] + (float) maxRow[HIGH]) / 2;
      }
   }
}