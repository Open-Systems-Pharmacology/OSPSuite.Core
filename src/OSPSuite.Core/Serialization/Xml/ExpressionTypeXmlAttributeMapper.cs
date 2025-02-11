using OSPSuite.Core.Domain.Builder;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ExpressionTypeXmlAttributeMapper : AttributeMapper<ExpressionType, SerializationContext>
   {
      public override string Convert(ExpressionType valueToConvert, SerializationContext context)
      {
         return valueToConvert.Id.ToString();
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         var expressionTypeId = EnumHelper.ParseValue<ExpressionTypesId>(attributeValue);
         return ExpressionTypes.ById(expressionTypeId);
      }
   }
}