using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class LLOQUsageXmlAttributeMapper<TSerializationContext> : AttributeMapper<RemoveLLOQMode, TSerializationContext>
   {
      public override string Convert(RemoveLLOQMode valueToConvert, TSerializationContext context)
      {
         return valueToConvert?.Name;
      }

      public override object ConvertFrom(string attributeValue, TSerializationContext context)
      {
         return RemoveLLOQModes.ByName(attributeValue);
      }
   }
}