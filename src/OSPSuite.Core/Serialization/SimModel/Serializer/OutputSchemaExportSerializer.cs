using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class OutputSchemaExportSerializer : SimModelSerializerBase<OutputSchemaExport>
   {
      public override void PerformMapping()
      {
         ElementName = SimModelSchemaConstants.OutputSchema;
         MapEnumerable(x => x.OutputIntervals).WithMappingName(SimModelSchemaConstants.OutputIntervalList);
         MapEnumerable(x => x.OutputTimes).WithMappingName(SimModelSchemaConstants.OutputTimeList).WithChildMappingName(SimModelSchemaConstants.OutputTime);
      }
   }

   internal class OutputIntervalExportSerializer : SimModelSerializerBase<OutputIntervalExport>
   {
      public override void PerformMapping()
      {
         Map(x => x.StartTime).AsNode();
         Map(x => x.EndTime).AsNode();
         Map(x => x.NumberOfTimePoints).AsNode();
         ElementName = SimModelSchemaConstants.OutputInterval;
      }

      protected override XElement TypedSerialize(OutputIntervalExport objectToSerialize, SimModelSerializationContext serializationContext)
      {
         var element = base.TypedSerialize(objectToSerialize, serializationContext);
         element.AddAttribute("distribution", "Uniform");
         return element;
      }
   }
}