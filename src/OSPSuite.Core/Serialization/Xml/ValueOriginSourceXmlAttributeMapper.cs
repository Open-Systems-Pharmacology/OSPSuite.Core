using OSPSuite.Core.Domain;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginSourceXmlAttributeMapper<TSerializationContext> : AttributeMapper<ValueOriginSource, TSerializationContext>
   {
      public override object ConvertFrom(string attributeValue, TSerializationContext context)
      {
         return ValueOriginSources.ById(EnumHelper.ParseValue<ValueOriginSourceId>(attributeValue));
      }

      public override string Convert(ValueOriginSource valueOriginSource, TSerializationContext context)
      {
         if (valueOriginSource == null)
            return null;

         return valueOriginSource == ValueOriginSources.Undefined ? null : valueOriginSource.Id.ToString();
      }
   }
}