using DevExpress.XtraCharts;

namespace OSPSuite.UI.Extensions
{
   public static class Axis2DExtensions
   {
      /// <summary>
      /// Set all relevant properties concerning side margins with a single function call.
      /// </summary>
      /// <param name="axis"></param>
      /// <param name="sideMarginsEnabled">Enabled or disables auto side margins. If false, sideMarginsValue will always be set to 0.</param>
      public static void SetSideMarginsEnabled(this Axis2D axis, bool sideMarginsEnabled)
      {
         double? margin = null;
         if (axis.Logarithmic)
            margin = 0.7; // in order to show a major tick with label below the min value for a log scaled axis
         
         axis.WholeRange.SetSideMarginsEnabled(sideMarginsEnabled, margin);
         axis.VisualRange.SetSideMarginsEnabled(sideMarginsEnabled, margin);
      }

      public static double? GetMaxForVisualRange(this Axis axis)
      {
         var maxValue = axis.VisualRange.MaxValue;
         return getExtremeForAxis(axis, maxValue);
      }

      public static double? GetMinForVisualRange(this Axis axis)
      {
         var minValue = axis.VisualRange.MinValue;
         return getExtremeForAxis(axis, minValue);
      }

      private static double? getExtremeForAxis(Axis axis, object minValue)
      {
         if (axis.VisualRange.Auto)
            return null;

         double value;

         if (double.TryParse(minValue.ToString(), out value))
            return value;

         return null;
      }

      public static void ResetVisualRange(this Axis axis)
      {
         axis.WholeRange.Auto = true;
         axis.VisualRange.SetMinMaxValues(axis.WholeRange.MinValue, axis.WholeRange.MaxValue);
      }
   }

}
