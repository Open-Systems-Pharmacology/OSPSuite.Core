using OSPSuite.Serializer;
using OSPSuite.Core.Domain.ParameterIdentifications;

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
         MapEnumerable(x => x.AllLinkedParameters).WithMappingName(ParameterIdentificationExportSchemaConstants.SimulationParameterList);
      }
   }


}