using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class SimulationQuantitySelectionSerializer : ParameterIdentificationExportSerializerBase<SimulationQuantitySelection>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.SimulationQuantitySelection;
         Map(x => x.FullQuantityPath).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.Path);
         Map(x => x.Dimension).WithMappingName(ParameterIdentificationExportSchemaConstants.Attributes.Dimension);
      }

      protected override XElement TypedSerialize(SimulationQuantitySelection x, ParameterIdentificationExportSerializationContext context)
      {
         var node = base.TypedSerialize(x, context);
         node.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.QuantityType, x.QuantitySelection.QuantityType.ToString());
         return node;
      }
   }
}