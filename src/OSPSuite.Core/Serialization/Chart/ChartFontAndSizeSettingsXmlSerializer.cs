using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class ChartFontAndSizeSettingsXmlSerializer : OSPSuiteXmlSerializer<ChartFontAndSizeSettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.ChartWidth);
         Map(x => x.ChartHeight);
         Map(x => x.Fonts);
      }
   }
}