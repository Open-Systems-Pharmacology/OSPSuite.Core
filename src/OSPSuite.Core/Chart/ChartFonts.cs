using System.Drawing;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public class ChartFonts : Notifier
   {
      private int _axisSize;
      private int _legendSize;
      private int _titleSize;
      private int _descriptionSize;
      private string _fontFamilyName;
      private int _originSize;
      private int _watermarkSize;

      public ChartFonts()
      {
         AxisSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_AXIS;
         LegendSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_LEGEND;
         TitleSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_TITLE;
         DescriptionSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_DESCRIPTION;
         OriginSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_ORIGIN;
         FontFamilyName = Constants.ChartFontOptions.DEFAULT_FONT_FAMILY_NAME;
         WatermarkSize = Constants.ChartFontOptions.DEFAULT_FONT_SIZE_WATERMARK;
      }

      public int AxisSize
      {
         get => _axisSize;
         set => SetProperty(ref _axisSize, value);
      }

      public int LegendSize
      {
         get => _legendSize;
         set => SetProperty(ref _legendSize, value);
      }

      public int TitleSize
      {
         get => _titleSize;
         set => SetProperty(ref _titleSize, value);
      }

      public int DescriptionSize
      {
         get => _descriptionSize;
         set => SetProperty(ref _descriptionSize, value);
      }

      public int OriginSize
      {
         get => _originSize;
         set => SetProperty(ref _originSize, value);
      }

      public string FontFamilyName
      {
         get => _fontFamilyName;
         set => SetProperty(ref _fontFamilyName, value);
      }

      public virtual int WatermarkSize
      {
         get => _watermarkSize;
         set => SetProperty(ref _watermarkSize, value);
      }

      public void UpdateSettingsFrom(ChartFonts newChartFonts)
      {
         AxisSize = newChartFonts.AxisSize;
         LegendSize = newChartFonts.LegendSize;
         TitleSize = newChartFonts.TitleSize;
         DescriptionSize = newChartFonts.DescriptionSize;
         OriginSize = newChartFonts.OriginSize;
         FontFamilyName = newChartFonts.FontFamilyName;
         WatermarkSize = newChartFonts.WatermarkSize;
      }
   }
}