using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public abstract class CurveChartXmlSerializer<TCurveChart> : OSPSuiteXmlSerializer<TCurveChart> where TCurveChart: CurveChart
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.IncludeOriginData);
         Map(x => x.OriginText);
         Map(x => x.ChartSettings);
         Map(x => x.FontAndSize);
         Map(x => x.Name);
         Map(x => x.Title);
         Map(x => x.Description);
         MapEnumerable(x => x.Axes, x => x.AddAxis);
         MapEnumerable(x => x.Curves, x => x.AddCurveIfConsistent);
      }
   }

   public class CurveChartXmlSerializer : CurveChartXmlSerializer<CurveChart>
   {
      
   }
}