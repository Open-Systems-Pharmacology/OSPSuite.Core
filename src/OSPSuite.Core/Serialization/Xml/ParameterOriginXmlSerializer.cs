using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterOriginXmlSerializer : OSPSuiteXmlSerializer<ParameterOrigin>
   {
      public override void PerformMapping()
      {
         Map(x => x.BuilingBlockId).WithMappingName(Constants.Serialization.Attribute.BUILDING_BLOCK_ID);
         Map(x => x.ParameterId).WithMappingName(Constants.Serialization.Attribute.PARAMETER_ID);
      }

      protected override XElement TypedSerialize(ParameterOrigin parameterOrigin, SerializationContext context)
      {
         return isUndefined(parameterOrigin) ? null : base.TypedSerialize(parameterOrigin, context);
      }

      private static bool isUndefined(ParameterOrigin parameterOrigin)
      {
         return parameterOrigin == null ||
                (string.IsNullOrEmpty(parameterOrigin.BuilingBlockId) && string.IsNullOrEmpty(parameterOrigin.ParameterId));
      }
   }
}