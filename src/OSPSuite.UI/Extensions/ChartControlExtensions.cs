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
   public static class ChartControlExtensions
   {
      public static void SetFontAndSizeSettings(this ChartControl chartControl, ChartFontAndSizeSettings fontAndSizeSettings, Size alternativeSize)
      {
         chartControl.Width = fontAndSizeSettings.ChartWidth.GetValueOrDefault(alternativeSize.Width);
         chartControl.Height = fontAndSizeSettings.ChartHeight.GetValueOrDefault(alternativeSize.Height);

         var fontTitle = fontAndSizeSettings.FontFor(x => x.TitleSize);
         var fontDescription = fontAndSizeSettings.FontFor(x => x.DescriptionSize);
         var fontAxis = fontAndSizeSettings.FontFor(x => x.AxisSize);
         var fontLegend = fontAndSizeSettings.FontFor(x => x.LegendSize);

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

      private const string WATERMARK_ANNOTATION = "WATERMARK_ANNOTATION";

      /// <summary>
      ///    Copies the chart to clipboard as an image using export settings if defined
      ///    Otherwise uses current visual settings
      /// </summary>
      public static void CopyToClipboard(this UxChartControl chartControl, IChart chart, string watermark)
      {
         using (var cloneOfChartControl = (ChartControl)chartControl.Clone())
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

         watermarkAnnotation.Font = chart.FontAndSize.FontFor(x => x.WatermkarkSize);
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
         annotation.TextColor = Constants.ChartFontOptions.DEFAULT_FONT_COLOR_WATERMARK;
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
            Font = chart.FontAndSize.FontFor(x=>x.OriginSize),
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