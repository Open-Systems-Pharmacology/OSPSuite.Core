using System.Globalization;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKSimBuildingBlockTypeXmlAttributeMapper : AttributeMapper<PKSimBuildingBlockType, SerializationContext>
   {
      public override string Convert(PKSimBuildingBlockType valueToConvert, SerializationContext context)
      {
         return ((int) valueToConvert).ToString(CultureInfo.InvariantCulture);
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         var flagValue = int.Parse(attributeValue);
         return (PKSimBuildingBlockType) flagValue;
      }
   }
}