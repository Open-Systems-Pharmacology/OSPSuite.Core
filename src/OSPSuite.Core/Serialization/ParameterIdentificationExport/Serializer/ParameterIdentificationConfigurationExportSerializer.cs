using OSPSuite.Serializer;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class ParameterIdentificationConfigurationExportSerializer : ParameterIdentificationExportSerializerBase<ParameterIdentificationConfiguration>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.ParameterIdentificationConfiguration;
         Map(x => x.LLOQMode).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.LLOQMode);
         Map(x => x.RemoveLLOQMode).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.LLOQUsage);
      }
   }
}