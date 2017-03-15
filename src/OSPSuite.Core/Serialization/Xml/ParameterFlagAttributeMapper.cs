using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterFlagAttributeMapper : AttributeMapper<ParameterFlag, SerializationContext>
   {
      public override string Convert(ParameterFlag valueToConvert, SerializationContext context)
      {
         return ((int) valueToConvert).ToString();
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         var flagValue = int.Parse(attributeValue);
         return (ParameterFlag) flagValue;
      }
   }
}