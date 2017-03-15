using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Extensions
{
   public static class WithEditableChartPropertiesExtensions
   {
      public static void CopyToClipboard(this IChart curveChart, UxChartControl chartControl)
      {
         using (var cloneOfChartControl = (ChartControl)chartControl.Clone())
         {
            cloneOfChartControl.SetFontAndSizeSettings(curveChart.FontAndSize, chartControl.Size);
            if (curveChart.IncludeOriginData)
               curveChart.AddOriginData(cloneOfChartControl);

            chartControl.CopyChartToClipboard(cloneOfChartControl);
         }
      }

      public static ChartTitle AddOriginData(this IChart curveChart, ChartControl chartControl)
      {
         if (string.IsNullOrEmpty(curveChart.OriginText))
            return null;

         var existingTitle = titleWithEquivalentOriginText(chartControl, curveChart);

         if (existingTitle != null)
            chartControl.Titles.Remove(existingTitle);

         var originTitle = new ChartTitle
         {
            Text = string.Empty,
            Font = new Font(curveChart.FontAndSize.Fonts.FontFamilyName, curveChart.FontAndSize.Fonts.OriginSize),
            Alignment = StringAlignment.Near,
            Dock = ChartTitleDockStyle.Bottom,
            WordWrap = true
         };
         originTitle.Text = curveChart.OriginText;
         chartControl.Titles.Add(originTitle);
         return originTitle;
      }

      private static ChartTitle titleWithEquivalentOriginText(ChartControl chartControl, IChart curveChart)
      {
         return allChartTitlesFor(chartControl).FirstOrDefault(title => string.Equals(title.Text, curveChart.OriginText));
      }

      private static IEnumerable<ChartTitle> allChartTitlesFor(ChartControl chartControl)
      {
         return chartControl.Titles.ToArray().OfType<ChartTitle>();
      }
   }
}
