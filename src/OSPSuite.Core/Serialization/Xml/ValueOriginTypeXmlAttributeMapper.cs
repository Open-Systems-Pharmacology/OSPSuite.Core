using OSPSuite.Core.Domain;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginTypeXmlAttributeMapper<TSerializationContext> : AttributeMapper<ValueOriginType, TSerializationContext>
   {
      public override object ConvertFrom(string attributeValue, TSerializationContext context)
      {
         return ValueOriginTypes.ById(EnumHelper.ParseValue<ValueOriginTypeId>(attributeValue));
      }

      public override string Convert(ValueOriginType valueOriginType, TSerializationContext context)
      {
         return valueOriginType?.Id.ToString();
      }
   }
}