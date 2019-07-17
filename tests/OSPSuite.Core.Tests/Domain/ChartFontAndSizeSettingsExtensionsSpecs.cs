using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ChartFontAndSizeSettingsExtensions : StaticContextSpecification
   {
      protected ChartFontAndSizeSettings _chartFondAndSizeSettings;

      protected override void Context()
      {
         _chartFondAndSizeSettings = new ChartFontAndSizeSettings();
      }
   }

   public class When_initializing_a_chart_font_and_size_settings_for_parameter_identification_feedback : concern_for_ChartFontAndSizeSettingsExtensions
   {
      protected override void Because()
      {
         _chartFondAndSizeSettings.ForParameterIdentificationFeedback();
      }

      [Observation]
      public void should_set_the_title_size_as_expected()
      {
         _chartFondAndSizeSettings.Fonts.TitleSize.ShouldBeEqualTo(Constants.ChartFontOptions.DEFAULT_FONT_SIZE_TITLE_FOR_PARAMETER_IDENTIFICATION_FEEDBACK);
      }
   }
}	