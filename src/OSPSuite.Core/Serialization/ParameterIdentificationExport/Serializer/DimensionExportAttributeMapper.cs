using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class DimensionExportAttributeMapper : AttributeMapper<IDimension, ParameterIdentificationExportSerializationContext>
   {
      public override object ConvertFrom(string attributeValue, ParameterIdentificationExportSerializationContext context)
      {
         //nothing to do here
         return null;
      }

      public override string Convert(IDimension dimension, ParameterIdentificationExportSerializationContext context)
      {
         return dimension.Name;
      }
   }
}