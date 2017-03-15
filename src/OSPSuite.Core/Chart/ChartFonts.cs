using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;

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
         get { return _axisSize; }
         set
         {
            _axisSize = value;
            OnPropertyChanged(() => AxisSize);
         }
      }

      public int LegendSize
      {
         get { return _legendSize; }
         set
         {
            _legendSize = value;
            OnPropertyChanged(() => LegendSize);
         }
      }

      public int TitleSize
      {
         get { return _titleSize; }
         set
         {
            _titleSize = value;
            OnPropertyChanged(() => TitleSize);
         }
      }

      public int DescriptionSize
      {
         get { return _descriptionSize; }
         set
         {
            _descriptionSize = value;
            OnPropertyChanged(() => DescriptionSize);
         }
      }

      public int OriginSize
      {
         get { return _originSize; }
         set
         {
            _originSize = value;
            OnPropertyChanged(() => OriginSize);
         }
      }

      public string FontFamilyName
      {
         get { return _fontFamilyName; }
         set
         {
            _fontFamilyName = value;
            OnPropertyChanged(() => FontFamilyName);
         }
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