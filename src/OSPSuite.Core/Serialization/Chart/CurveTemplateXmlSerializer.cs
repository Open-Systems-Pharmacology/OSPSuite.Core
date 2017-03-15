using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class CurveTemplateXmlSerializer : XmlSerializer<CurveTemplate, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.xData);
         Map(x => x.yData);
         Map(x => x.CurveOptions);
      }
   }
}