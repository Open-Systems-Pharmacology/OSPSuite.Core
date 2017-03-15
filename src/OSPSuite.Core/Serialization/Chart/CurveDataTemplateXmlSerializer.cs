using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class CurveDataTemplateXmlSerializer : XmlSerializer<CurveDataTemplate, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.Path);
         Map(x => x.QuantityType);
         Map(x => x.RepositoryName);
      }
   }
}