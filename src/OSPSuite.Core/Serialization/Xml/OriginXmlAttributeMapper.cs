using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OriginXmlAttributeMapper : AttributeMapper<Origin, SerializationContext>
   {
      public override string Convert(Origin valueToConvert, SerializationContext context)
      {
         return valueToConvert.Id.ToString();
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         var originId = EnumHelper.ParseValue<OriginId>(attributeValue);
         return Origins.ById(originId);
      }
   }
}