using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DistributionTypeXmlSerializer : AttributeMapper<DistributionType?, SerializationContext>
   {
      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         return EnumHelper.ParseValue<DistributionType>(attributeValue);
      }

      public override string Convert(DistributionType? valueToConvert, SerializationContext context)
      {
         return valueToConvert != null ? valueToConvert.ToString() : null;
      }
   }
}