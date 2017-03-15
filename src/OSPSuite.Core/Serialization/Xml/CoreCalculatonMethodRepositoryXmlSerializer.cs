using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CoreCalculationMethodRepositoryXmlSerializer : XmlSerializer<CoreCalculationMethodRepository, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All(), x => x.AddCalculationMethod);
      }
   }
}