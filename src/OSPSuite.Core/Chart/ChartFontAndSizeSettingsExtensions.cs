using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public static class ChartFontAndSizeSettingsExtensions
   {
      public static ChartFontAndSizeSettings WithTitleSize(this ChartFontAndSizeSettings chartFontAndSizeSettings, int titleSize)
      {
         chartFontAndSizeSettings.Fonts.TitleSize = titleSize;
         return chartFontAndSizeSettings;
      }

      public static ChartFontAndSizeSettings ForParameterIdentificationFeedback(this ChartFontAndSizeSettings chartFontAndSizeSettings)
      {
         return chartFontAndSizeSettings.WithTitleSize(Constants.ChartFontOptions.DEFAULT_FONT_SIZE_TITLE_FOR_PARAMETER_IDENTIFICATION_FEEDBACK);
      }
   }
}