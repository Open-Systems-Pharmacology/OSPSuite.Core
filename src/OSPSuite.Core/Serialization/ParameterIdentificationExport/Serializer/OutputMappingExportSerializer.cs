using OSPSuite.Serializer;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class OutputMappingExportSerializer : ParameterIdentificationExportSerializerBase<OutputMapping>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.OutputMapping;
         Map(x => x.OutputSelection).WithMappingName(ParameterIdentificationExportSchemaConstants.Output);
         Map(x => x.Scaling);
         Map(x => x.Weight).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.Weight);
         Map(x => x.WeightedObservedData).WithMappingName(ParameterIdentificationExportSchemaConstants.ObservedData);
      }
   }
}