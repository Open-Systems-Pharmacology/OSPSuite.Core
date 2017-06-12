using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Extensions
{
   public static class CurveExtensions
   {
      /// <summary>
      /// If the <paramref name="curve"/> plots <paramref name="column"/> on the Y axis <returns>true</returns>
      /// otherwise <returns>false</returns>
      /// </summary>
      public static bool PlotsColumn(this Curve curve, DataColumn column)
      {
         return column == curve.yData;
      }

      /// <summary>
      /// Sets the default <see cref="Symbols"/> and <see cref="LineStyles"/> for the <paramref name="curve"/> if it represents an oberved data curve.
      /// </summary>
      /// <param name="curve"></param>
      public static void UpdateStyleForObservedData(this Curve curve)
      {
         if (curve.yData == null || !curve.yData.IsObservedData())
            return;

         curve.Symbol = Symbols.Circle;
         curve.LineStyle = LineStyles.None;
      }

      public static void UpdateMarkerCurve(this Curve curve, string curveName)
      {
         curve.VisibleInLegend = false;
         curve.LineStyle = LineStyles.Dash;
         curve.Color = Color.Black;
         curve.Name = curveName;
         curve.LineThickness = 2;
         curve.Name = curveName;
      }
   }
}
