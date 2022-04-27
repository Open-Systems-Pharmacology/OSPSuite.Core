using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
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
      private const string WATERMARK_ANNOTATION = "WATERMARK_ANNOTATION";

      public static void SetFontAndSizeSettings(this ChartControl chartControl, ChartFontAndSizeSettings fontAndSizeSettings, Size alternativeSize)
      {
         var width = fontAndSizeSettings.ChartWidth.GetValueOrDefault(alternativeSize.Width);
         var height = fontAndSizeSettings.ChartHeight.GetValueOrDefault(alternativeSize.Height);
         var fontTitle = fontAndSizeSettings.FontFor(x => x.TitleSize);
         var fontDescription = fontAndSizeSettings.FontFor(x => x.DescriptionSize);
         var fontAxis = fontAndSizeSettings.FontFor(x => x.AxisSize);
         var fontLegend = fontAndSizeSettings.FontFor(x => x.LegendSize);

         setFontAndSizeSettings(chartControl, width, height, fontTitle, fontDescription, fontAxis, fontLegend);
      }

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

            prepareChartForCopying(cloneOfChartControl);

            if (!string.IsNullOrEmpty(watermark))
            {
               //Bug in devexpress that does not keep the color in bar charts
               AddWatermark(cloneOfChartControl, watermark);
            }

            cloneOfChartControl.CopyChartToClipboard();
         }
      }

      public static void CopyChartToClipboard(this ChartControl chartControl)
      {
         using (var ms = new MemoryStream())
         {
            chartControl.ExportToImage(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            using (var mf = new Bitmap(ms))
               Clipboard.SetImage(mf);
         }
      }

      //returns whether the color was changed or not
      private static bool prepareChartForCopying(ChartControl cloneOfChartControl)
      {
         //remove the outer border from the copy
         cloneOfChartControl.BorderOptions.Visibility = DefaultBoolean.False;

         //if the color is not set (transparent), assign white for the copy
         if (cloneOfChartControl.BackColor == Color.Transparent)
         {
            cloneOfChartControl.BackColor = Color.White;
            return true;
         }

         return false;
      }

      private static void setFontAndSizeSettings(ChartControl chartControl, int width, int height, Font fontTitle, Font fontDescription,
         Font fontAxis, Font fontLegend)
      {
         chartControl.Width = width;
         chartControl.Height = height;

         if (chartControl.Titles.Count > 0 && fontTitle != null)
            chartControl.Titles[0].Font = fontTitle;

         if (chartControl.Titles.Count > 1 && fontDescription != null)
            chartControl.Titles[1].Font = fontDescription;

         if (fontLegend != null)
            chartControl.Legend.Font = fontLegend;

         var xyDiagram = chartControl.Diagram as XYDiagram;
         if (xyDiagram == null || fontAxis == null) return;

         xyDiagram.AxisX.Label.Font = fontAxis;
         xyDiagram.AxisX.Title.Font = fontAxis;
         xyDiagram.GetAllAxesY().Each(axis => axis.Label.Font = fontAxis);
         xyDiagram.GetAllAxesY().Each(axis => axis.Title.Font = fontAxis);
      }

      public static void CopyToClipboard(this UxChartControl chartControl, string watermark)
      {
         var watermarkAdded = false;
            var backColorChanged = prepareChartForCopying(chartControl);

            if (!string.IsNullOrEmpty(watermark))
            {
               //Bug in devexpress that does not keep the color in bar charts
               AddWatermark(chartControl, watermark);
               watermarkAdded = true;
            }

            chartControl.Annotations.AddTextAnnotation("GAMW");
            chartControl.CopyChartToClipboard();

            resetChartAfterCopying(chartControl, backColorChanged, watermarkAdded);
      }

      private static void resetChartAfterCopying(UxChartControl chartControl, bool backColorChanged, bool watermarkAdded)
      {
         chartControl.BorderOptions.Visibility = DefaultBoolean.False;

         if (backColorChanged)
            chartControl.BackColor = Color.Transparent;

         if (watermarkAdded)
            chartControl.RemoveWatermark();
      }

      public static void AddWatermark(this ChartControl chartControl, string watermark, IChart chart)
      {
         AddWatermark(chartControl, watermark, chart.FontAndSize.FontFor(x => x.WatermarkSize));
      }

      public static void RemoveWatermark(this ChartControl chartControl)
      {
         var watermarkAnnotation = chartControl.Annotations.OfType<TextAnnotation>().FirstOrDefault(x => Equals(x.Name, WATERMARK_ANNOTATION));

         if (watermarkAnnotation != null)
            chartControl.Annotations.Remove(watermarkAnnotation);
      }

      public static void AddWatermark(this ChartControl chartControl, string watermark, Font watermarkFont = null)
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

         watermarkAnnotation.Font =
            watermarkFont ?? new Font(watermarkAnnotation.Font.FontFamily, Constants.ChartFontOptions.DEFAULT_FONT_SIZE_WATERMARK);
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
            Font = chart.FontAndSize.FontFor(x => x.OriginSize),
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

      public static Font FontFor(this ChartFontAndSizeSettings chartFontAndSizeSettings, Func<ChartFonts, int> fontSizeFunc) =>
         FontFor(chartFontAndSizeSettings, fontSizeFunc(chartFontAndSizeSettings.Fonts));

      public static Font FontFor(this ChartFontAndSizeSettings chartFontAndSizeSettings, int fontSize) =>
         new Font(chartFontAndSizeSettings.Fonts.FontFamilyName, fontSize);
   }
}