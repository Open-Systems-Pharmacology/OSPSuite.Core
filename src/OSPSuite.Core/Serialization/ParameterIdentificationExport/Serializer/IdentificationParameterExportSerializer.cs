using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class IdentificationParameterExportSerializer : ParameterIdentificationExportSerializerBase<IdentificationParameter>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.IdentificationParameter;

         Map(x => x.Name);
         Map(x => x.StartValue);
         Map(x => x.MinValue);
         Map(x => x.MaxValue);
         Map(x => x.Dimension);
         Map(x => x.Scaling);
         Map(x => x.UseAsFactor);
         Map(x => x.IsFixed);
         MapEnumerable(x => x.AllLinkedParameters).WithMappingName(ParameterIdentificationExportSchemaConstants.SimulationParameterList);
      }
   }
}