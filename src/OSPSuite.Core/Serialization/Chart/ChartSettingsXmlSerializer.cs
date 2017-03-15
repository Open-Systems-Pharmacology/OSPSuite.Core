using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class ChartSettingsXmlSerializer : OSPSuiteXmlSerializer<ChartSettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.SideMarginsEnabled);
         Map(x => x.LegendPosition);
         Map(x => x.BackColor);
         Map(x => x.DiagramBackColor);
      }
   }
}