using DevExpress.XtraCharts;

namespace OSPSuite.UI.Extensions
{
   public static class HotTrackEventArgsExtensions
   {
      /// <summary>
      /// returns a serie object if the series is defined, and has at least one pont
      /// </summary>
      public static Series Series(this HotTrackEventArgs e)
      {
         var series = e.Object as Series;
         return series == null || series.Points.Count == 0 || e.HitInfo.InLegend ? null : series;
      }
   }
}