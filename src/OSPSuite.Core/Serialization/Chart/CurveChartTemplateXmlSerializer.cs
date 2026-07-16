using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class CurveChartTemplateXmlSerializer : OSPSuiteXmlSerializer<CurveChartTemplate>
   {
      public override void PerformMapping()
      {
         Map(x => x.ChartSettings);
         Map(x => x.IncludeOriginData);
         Map(x => x.FontAndSize);
         Map(x => x.Name);
         Map(x => x.PreviewSettings);
         MapEnumerable(x => x.Axes, x => x.AddAxis);
         MapEnumerable(x => x.Curves, x => x.Curves.Add);
         Map(x => x.IsDefault);
         Map(x => x.ChartType);
      }
   }
}