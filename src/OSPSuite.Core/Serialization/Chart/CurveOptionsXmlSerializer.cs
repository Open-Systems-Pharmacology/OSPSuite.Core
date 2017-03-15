using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class CurveOptionsXmlSerializer : OSPSuiteXmlSerializer<CurveOptions>
   {
      public override void PerformMapping()
      {
         Map(x => x.yAxisType);
         Map(x => x.InterpolationMode);
         Map(x => x.Visible);
         Map(x => x.Color);
         Map(x => x.LineStyle);
         Map(x => x.Symbol);
         Map(x => x.LineThickness);
         Map(x => x.VisibleInLegend);
         Map(x => x.LegendIndex);
         Map(x => x.ShouldShowLLOQ);
      }
   }
}