using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Extensions
{
   public static class UxChartControlExtensions
   {
      private const string WATERMARK_ANNOTATION = "WATERMARK_ANNOTATION";

      /// <summary>
      ///    Copies the chart to clipboard as an image using export settings if defined
      ///    Otherwise uses current visual settings
      /// </summary>
      public static void CopyToClipboard(this UxChartControl chartControl, IChart chart, string watermark)
      {
         using (var cloneOfChartControl = (ChartControl) chartControl.Clone())
         {
            cloneOfChartControl.SetFontAndSizeSettings(chart.FontAndSize, chartControl.Size);
            if (chart.IncludeOriginData)
               AddOriginData(cloneOfChartControl, chart);

            if (!string.IsNullOrEmpty(watermark))
               AddWatermark(cloneOfChartControl, chart, watermark);

            chartControl.CopyChartToClipboard(cloneOfChartControl);
         }
      }

      public static void AddWatermark(this ChartControl chartControl, IChart chart, string watermark)
      {
         var shouldHideWatermark = string.IsNullOrEmpty(watermark);
         var watermarkAnnotation = chartControl.Annotations.OfType<TextAnnotation>().FirstOrDefault(x => Equals(x.Name, WATERMARK_ANNOTATION));
         //we add a watermark but this should be removed
         if (watermarkAnnotation != null && shouldHideWatermark)
            chartControl.Annotations.Remove(watermarkAnnotation);

         if (shouldHideWatermark)
            return;

         if (watermarkAnnotation == null)
            watermarkAnnotation = createWatermarkAnnotation(chartControl);

         watermarkAnnotation.Font = new Font(chart.FontAndSize.Fonts.FontFamilyName, Constants.ChartFontOptions.DEFAULT_FONT_SIZE_WATERMARK);
         watermarkAnnotation.Text = watermark.InBold();
         updateAnnotationPosition(watermarkAnnotation, chartControl);
      }

      private static TextAnnotation createWatermarkAnnotation(ChartControl chartControl)
      {
         var annotation = chartControl.Annotations.AddTextAnnotation(WATERMARK_ANNOTATION);
         annotation.ShapePosition = new FreePosition();
         annotation.Angle = -45;
         annotation.ConnectorStyle = AnnotationConnectorStyle.None;
         annotation.BackColor = Color.Transparent;
         annotation.Border.Visibility = DefaultBoolean.False;
         return annotation;
      }

      private static void updateAnnotationPosition(TextAnnotation watermarkAnnotation, ChartControl chartControl)
      {
         var pos = watermarkAnnotation.ShapePosition.DowncastTo<FreePosition>();
         pos.InnerIndents.Left = (chartControl.Size.Width - watermarkAnnotation.Width) / 2;
         pos.InnerIndents.Top = (chartControl.Size.Height - watermarkAnnotation.Height) / 2;
      }

      public static ChartTitle AddOriginData(this ChartControl chartControl, IChart chart)
      {
         if (string.IsNullOrEmpty(chart.OriginText))
            return null;

         var existingTitle = titleWithEquivalentOriginText(chartControl, chart);

         if (existingTitle != null)
            chartControl.Titles.Remove(existingTitle);

         var originTitle = new ChartTitle
         {
            Text = chart.OriginText,
            Font = new Font(chart.FontAndSize.Fonts.FontFamilyName, chart.FontAndSize.Fonts.OriginSize),
            Alignment = StringAlignment.Near,
            Dock = ChartTitleDockStyle.Bottom,
            WordWrap = true
         };

         chartControl.Titles.Add(originTitle);
         return originTitle;
      }

      private static ChartTitle titleWithEquivalentOriginText(ChartControl chartControl, IChart curveChart)
      {
         return allChartTitlesFor(chartControl).FirstOrDefault(title => string.Equals(title.Text, curveChart.OriginText));
      }

      private static IReadOnlyList<ChartTitle> allChartTitlesFor(ChartControl chartControl)
      {
         return chartControl.Titles.OfType<ChartTitle>().ToArray();
      }
   }
}