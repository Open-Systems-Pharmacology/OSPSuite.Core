using System.Drawing;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Extensions
{
   public static class ChartControlExtensions
   {
      public static void SetFontAndSizeSettings(this ChartControl chartControl, ChartFontAndSizeSettings fontAndSizeSettings,Size alternativeSize)
      {
         chartControl.Width = fontAndSizeSettings.ChartWidth.GetValueOrDefault() > 0 ? fontAndSizeSettings.ChartWidth.GetValueOrDefault() : alternativeSize.Width;
         chartControl.Height = fontAndSizeSettings.ChartHeight.GetValueOrDefault() > 0 ? fontAndSizeSettings.ChartHeight.GetValueOrDefault() : alternativeSize.Height;

         var fontTitle = new Font(fontAndSizeSettings.Fonts.FontFamilyName, fontAndSizeSettings.Fonts.TitleSize);
         var fontDescription = new Font(fontAndSizeSettings.Fonts.FontFamilyName, fontAndSizeSettings.Fonts.DescriptionSize);
         var fontAxis = new Font(fontAndSizeSettings.Fonts.FontFamilyName, fontAndSizeSettings.Fonts.AxisSize);
         var fontLegend = new Font(fontAndSizeSettings.Fonts.FontFamilyName, fontAndSizeSettings.Fonts.LegendSize);

         chartControl.Titles[0].Font = fontTitle;
         chartControl.Titles[1].Font = fontDescription;
         chartControl.Legend.Font = fontLegend;

         var xyDiagram = chartControl.Diagram as XYDiagram;
         if (xyDiagram != null)
         {
            xyDiagram.AxisX.Label.Font = fontAxis;
            xyDiagram.AxisX.Title.Font = fontAxis;
            xyDiagram.GetAllAxesY().Each(axis => axis.Label.Font = fontAxis);
            xyDiagram.GetAllAxesY().Each(axis => axis.Title.Font = fontAxis);
         }
      }
   }
}