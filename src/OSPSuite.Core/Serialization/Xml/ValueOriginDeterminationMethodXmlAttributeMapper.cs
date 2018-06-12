using OSPSuite.Core.Domain;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginDeterminationMethodXmlAttributeMapper<TSerializationContext> : AttributeMapper<ValueOriginDeterminationMethod, TSerializationContext>
   {
      public override object ConvertFrom(string attributeValue, TSerializationContext context)
      {
         return ValueOriginDeterminationMethods.ById(EnumHelper.ParseValue<ValueOriginDeterminationMethodId>(attributeValue));
      }

      public override string Convert(ValueOriginDeterminationMethod valueOriginDeterminationMethod, TSerializationContext context)
      {
         if (valueOriginDeterminationMethod == null)
            return null;

         return valueOriginDeterminationMethod == ValueOriginDeterminationMethods.Undefined ? null : valueOriginDeterminationMethod.Id.ToString();
      }
   }
}