using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class LLOQModeXmlAttributeMapper<TSerializationContext> : AttributeMapper<LLOQMode, TSerializationContext>
   {
      public override string Convert(LLOQMode valueToConvert, TSerializationContext context)
      {
         return valueToConvert?.Name;
      }

      public override object ConvertFrom(string attributeValue, TSerializationContext context)
      {
         return LLOQModes.ByName(attributeValue);
      }
   }
}