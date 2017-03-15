using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   //we do not use the standard EnumAttributeMapper here since we want to optimize the output size and only generate a value
   //if not local
   public class ParameterBuildModeAttributeMapper : AttributeMapper<ParameterBuildMode, SerializationContext>
   {
      public override string Convert(ParameterBuildMode parameterBuildMode, SerializationContext context)
      {
         //default value in parameter
         if (parameterBuildMode == ParameterBuildMode.Local)
            return string.Empty;

         return parameterBuildMode.ToString();
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         if (string.IsNullOrEmpty(attributeValue))
            return ParameterBuildMode.Local;

         return EnumHelper.ParseValue<ParameterBuildMode>(attributeValue);
      }
   }
}