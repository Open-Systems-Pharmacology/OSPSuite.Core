using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public class ChartFonts : MyNotifier
   {
      private int _axisSize;
      private int _legendSize;
      private int _titleSize;
      private int _descriptionSize;
      private string _fontFamilyName;
      private int _originSize;

      public ChartFonts()
      {
         AxisSize = Constants.ChartFontOptions.DefaultFontSizeAxis;
         LegendSize = Constants.ChartFontOptions.DefaultFontSizeLegend;
         TitleSize = Constants.ChartFontOptions.DefaultFontSizeTitle;
         DescriptionSize = Constants.ChartFontOptions.DefaultFontSizeDescription;
         OriginSize = Constants.ChartFontOptions.DefaultFontSizeOrigin;
         FontFamilyName = Constants.ChartFontOptions.DefaultFontFamilyName;
      }

      public int AxisSize
      {
         get => _axisSize;
         set => SetProperty(ref _axisSize, value, () => AxisSize);
      }

      public int LegendSize
      {
         get => _legendSize;
         set => SetProperty(ref _legendSize, value, () => LegendSize);
      }

      public int TitleSize
      {
         get => _titleSize;
         set => SetProperty(ref _titleSize, value, () => TitleSize);
      }

      public int DescriptionSize
      {
         get => _descriptionSize;
         set => SetProperty(ref _descriptionSize, value, () => DescriptionSize);
      }

      public int OriginSize
      {
         get => _originSize;
         set => SetProperty(ref _originSize, value, () => OriginSize);
      }

      public string FontFamilyName
      {
         get => _fontFamilyName;
         set => SetProperty(ref _fontFamilyName, value, () => FontFamilyName);
      }

      public void UpdateSettingsFrom(ChartFonts newChartFonts)
      {
         AxisSize = newChartFonts.AxisSize;
         LegendSize = newChartFonts.LegendSize;
         TitleSize = newChartFonts.TitleSize;
         DescriptionSize = newChartFonts.DescriptionSize;
         OriginSize = newChartFonts.OriginSize;
         FontFamilyName = newChartFonts.FontFamilyName;
      }
   }
}