using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class ChartFontsXmlSerializer : OSPSuiteXmlSerializer<ChartFonts>
   {
      public override void PerformMapping()
      {
         Map(x => x.AxisSize);
         Map(x => x.LegendSize);
         Map(x => x.TitleSize);
         Map(x => x.DescriptionSize);
         Map(x => x.FontFamilyName);
         Map(x => x.OriginSize);
         Map(x => x.WatermarkSize);
      }
   }
}