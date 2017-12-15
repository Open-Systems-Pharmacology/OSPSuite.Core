using OSPSuite.Core.Domain;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginXmlSerializer : OSPSuiteXmlSerializer<ValueOrigin>
   {
      public ValueOriginXmlSerializer() : base(Constants.Serialization.VALUE_ORIGIN)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Type);
         Map(x => x.Description).WithMappingName(Constants.Serialization.Attribute.DESCRIPTION);
      }
   }
}