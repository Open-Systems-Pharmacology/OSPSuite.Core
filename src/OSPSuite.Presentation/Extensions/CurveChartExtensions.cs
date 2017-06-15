using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Extensions
{
   public static class CurveChartExtensions
   {
      public static Color SelectNewColor(this CurveChart chart)
      {
         var colors = CommonColors.OrderedForCurves().ToList();

         // select the next unused color (even if the color has been changed manually)
         foreach (var color in colors)
         {
            var colorUsed = chart.Curves.Any(curve => curve.Color == color);
            if (!colorUsed)
               return color;
         }

         // if no unused color can be found, simply return the next color (repeating)
         // colors start with index 0
         int newIndex = (chart.Curves.Count - 1) % colors.Count; 
         return colors[newIndex];
      }

      public static void UpdateCurveColorAndStyle(this CurveChart chart, Curve curve, DataColumn dataColumn, IReadOnlyCollection<DataColumn> dataColumns)
      {
         // Finds color from a related column
         if (dataColumnContainsRelatedColumns(dataColumn))
            curve.Color = getColorFromRelatedColumn(chart, dataColumn);

         // Finds color from a column which dataColumn is related
         else if (otherColumnsContainColumnAsRelated(dataColumns))
            curve.Color = getColorFromColumnRelatedTo(chart, dataColumn, dataColumns);

         else
            curve.Color = chart.SelectNewColor();

         curve.UpdateStyleForObservedData();
      }

      private static bool otherColumnsContainColumnAsRelated(IReadOnlyCollection<DataColumn> dataColumns)
      {
         return dataColumns.Any(x => x.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop)) || dataColumns.Any(x => x.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop));
      }

      private static bool dataColumnContainsRelatedColumns(DataColumn dataColumn)
      {
         return dataColumn.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop) || dataColumn.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop);
      }

      private static Color getColorFromColumnRelatedTo(CurveChart chart, DataColumn relatedColumn, IReadOnlyCollection<DataColumn> dataColumns)
      {
         var column = findColumnsRelatedTo(AuxiliaryType.ArithmeticMeanPop, relatedColumn, dataColumns) ??
                             findColumnsRelatedTo(AuxiliaryType.GeometricMeanPop, relatedColumn, dataColumns);

         return getColorOf(chart, column);
      }

      private static DataColumn findColumnsRelatedTo(AuxiliaryType auxiliaryType, DataColumn relatedColumn, IReadOnlyCollection<DataColumn> dataColumns)
      {
         return dataColumns.Where(x => x.ContainsRelatedColumn(auxiliaryType))
            .FirstOrDefault(x => x.GetRelatedColumn(auxiliaryType) == relatedColumn);
      }

      private static Color getColorFromRelatedColumn(CurveChart chart, DataColumn dataColumn)
      {
         DataColumn relatedColumn = null;
         if (dataColumn.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
            relatedColumn = dataColumn.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);

         if (dataColumn.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
            relatedColumn = dataColumn.GetRelatedColumn(AuxiliaryType.GeometricMeanPop);

         return getColorOf(chart, relatedColumn);
      }

      private static Color getColorOf(CurveChart chart, DataColumn relatedColumn)
      {
         // search curve for relCol
         var relatedCurve = chart.Curves.FirstOrDefault(c => c.yData.Equals(relatedColumn));
         return relatedCurve?.Color ?? chart.SelectNewColor();
      }
   }
}