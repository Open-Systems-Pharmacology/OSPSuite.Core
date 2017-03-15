using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputSchemaExportSerialiter : ContextSpecification<OutputSchemaExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (OutputSchemaExportSerializer) _repository.SerializerFor<OutputSchemaExport>();
      }
   }

   public class When_serializing_the_output_schema_export : concern_for_OutputSchemaExportSerialiter
   {
      private OutputSchemaExport _outputSchemaExport;
      private XElement _xmlResultNode;

      protected override void Context()
      {
         base.Context();
         _outputSchemaExport = new OutputSchemaExport();
         _outputSchemaExport.OutputIntervals.Add(new OutputIntervalExport());
         _outputSchemaExport.OutputTimes.Add(0);
         _outputSchemaExport.OutputTimes.Add(100);
      }

      protected override void Because()
      {
         _xmlResultNode = sut.Serialize(_outputSchemaExport, new SimModelSerializationContext());
      }

      [Observation]
      public void should_create_the_output_schema_node()
      {
         _xmlResultNode.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.OutputSchema);
      }

      [Observation]
      public void should_add_the_Output_intervalls_list_node()
      {
         var xmlOutputIntervals = _xmlResultNode.Element(XName.Get(SimModelSchemaConstants.OutputIntervalList, SimModelSchemaConstants.Namespace));
         xmlOutputIntervals.ShouldNotBeNull();
         xmlOutputIntervals.Descendants(XName.Get(SimModelSchemaConstants.OutputInterval, SimModelSchemaConstants.Namespace)).Count().ShouldBeEqualTo(_outputSchemaExport.OutputIntervals.Count);
      }

      [Observation]
      public void should_add_the_Output_times_list_node()
      {
         var xmlOutputTimesList = _xmlResultNode.Element(XName.Get(SimModelSchemaConstants.OutputTimeList, SimModelSchemaConstants.Namespace));
         xmlOutputTimesList.ShouldNotBeNull();
         xmlOutputTimesList.Descendants().Count().ShouldBeEqualTo(_outputSchemaExport.OutputTimes.Count);
      }
   }
}