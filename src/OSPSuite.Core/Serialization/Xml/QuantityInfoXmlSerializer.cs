using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class QuantityInfoXmlSerializer : OSPSuiteXmlSerializer<QuantityInfo>
   {
      public QuantityInfoXmlSerializer(): base(Constants.Serialization.QUANTITY_INFO)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Type);
         Map(x => x.OrderIndex);
         Map(x => x.Path).WithMappingName("Path").AsAttribute(); //or PathAsString
      }
   }
}