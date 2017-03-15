using OSPSuite.Serializer;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterOriginXmlSerializer : OSPSuiteXmlSerializer<ParameterOrigin>
   {
      public override void PerformMapping()
      {
         Map(x => x.BuilingBlockId).WithMappingName(Constants.Serialization.Attribute.BUILDING_BLOCK_ID);
         Map(x => x.ParameterId).WithMappingName(Constants.Serialization.Attribute.PARAMETER_ID);
      }
   }
}