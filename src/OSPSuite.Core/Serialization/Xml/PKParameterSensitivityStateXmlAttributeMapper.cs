using System.Globalization;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKParameterSensitivityStateXmlAttributeMapper : AttributeMapper<PKParameterSensitivityState, SerializationContext>
   {
      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         var flagValue = int.Parse(attributeValue);
         return (PKParameterSensitivityState)flagValue;
      }
   
      public override string Convert(PKParameterSensitivityState valueToConvert, SerializationContext context)
      {
         return ((int)valueToConvert).ToString(CultureInfo.InvariantCulture);
      }
   }
}