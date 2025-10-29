using System.Xml.Linq;

namespace OSPSuite.Core.Serialization.Xml
{
   public class StringSerializer : PrimitiveTypeSerializer<string, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override string CreateObject(XElement element, SerializationContext context)
      {
         return element.Value;
      }
   }
}