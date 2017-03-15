using DevExpress.XtraCharts;

namespace OSPSuite.UI.Extensions
{
   /// <summary>
   /// This is an extension for the range object of the devexpress charts.
   /// </summary>
   public static class RangeExtensions
   {
      /// <summary>
      /// Set all relevant properties concerning side margins with a single function call.
      /// </summary>
      /// <param name="range"></param>
      /// <param name="sideMarginsEnabled">Enabled or disables auto side margins. If false, sideMarginsValue will always be set to 0.</param>
      /// <param name="sideMarginsValue">Specifies the size of the side margin in axis unit. (optional)</param>
      public static void SetSideMarginsEnabled(this Range range, bool sideMarginsEnabled, double? sideMarginsValue = null)
      {
         // If turning off autoside margins, then the SideMarginsValue should be 0
         if (!sideMarginsEnabled)
         {
            setSideMarginValue(range, 0);
            return;
         }

         // If sideMarginsValue has a value, it means sideMarginsEnabled is not applicable.
         if (sideMarginsValue != null)
         {
            setSideMarginValue(range, sideMarginsValue.Value);
            return;
         }

         range.AutoSideMargins = true;
      }

      private static void setSideMarginValue(Range range, double value)
      {
         range.AutoSideMargins = false;
         range.SideMarginsValue = value;
      }
   }
}
