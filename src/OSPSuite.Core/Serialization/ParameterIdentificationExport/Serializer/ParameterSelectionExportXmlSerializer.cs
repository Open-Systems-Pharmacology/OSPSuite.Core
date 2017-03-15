using OSPSuite.Serializer;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class ParameterSelectionExportXmlSerializer : ParameterIdentificationExportSerializerBase<ParameterSelection>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.SimulationParameter;
         Map(x => x.FullQuantityPath).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.Path);
      }
   }
}