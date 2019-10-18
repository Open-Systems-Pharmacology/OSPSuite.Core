using System;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EventExportSerializer : ContextSpecification<EventExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();  
         sut = (EventExportSerializer) _repository.SerializerFor<EventExport>();
      }
   }
   
   public class When_serializing_a_Event_Export : concern_for_EventExportSerializer
   {
      private XElement _xmlResult;
      private EventExport _eventExport;
      private AssigmentExport _assigmentExport;

      protected override void Context()
      {
         base.Context();
         _eventExport = new EventExport();
         _eventExport.EntityId = "Bla";
         _eventExport.ConditionFormulaId = 1;
         _assigmentExport = new AssigmentExport();
         _assigmentExport.NewFormulaId = 2;
         _assigmentExport.ObjectId = 3;
         _assigmentExport.UseAsValue = true;
         _eventExport.AssignmentList.Add(_assigmentExport);
         _eventExport.Id = 4;
      }
      protected override void Because()
      {
         _xmlResult = sut.Serialize(_eventExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_Create_a_Event_Node()
      {
         _xmlResult.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.Event);
         Convert.ToInt32(_xmlResult.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(4);
         _xmlResult.Attribute("entityId").Value.ShouldBeEqualTo(_eventExport.EntityId);
      }
      [Observation]
      public void should_have_adde_a_assigment_list_to_the_node()
      {
         var xmlResultAssigments = _xmlResult.Element(XName.Get(SimModelSchemaConstants.AssigmentList, SimModelSchemaConstants.Namespace));
         xmlResultAssigments.Descendants().Count().ShouldBeEqualTo(_eventExport.AssignmentList.Count);
      }
   }

   public abstract class concern_for_AssigmentExportSerializer : ContextSpecification<AssignmentExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (AssignmentExportSerializer) _repository.SerializerFor<AssigmentExport>();
      }
   }
   
   public class When_Serializing_a_assigment_export : concern_for_AssigmentExportSerializer
   {
      private AssigmentExport _assigmentExport;
      private XElement _xmlResultElement;

      protected override void Context()
      {
         base.Context();
         _assigmentExport = new AssigmentExport();
         _assigmentExport.NewFormulaId = 2;
         _assigmentExport.ObjectId = 3;
         _assigmentExport.UseAsValue = true;
      }
      protected override void Because()
      {
         _xmlResultElement= sut.Serialize(_assigmentExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_have_created_the_correct_assigment_node()
      {
         _xmlResultElement.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.Assigment);
         Convert.ToInt32(_xmlResultElement.Attribute("objectId").Value).ShouldBeEqualTo(_assigmentExport.ObjectId);
         Convert.ToInt32(_xmlResultElement.Attribute("newFormulaId").Value).ShouldBeEqualTo(_assigmentExport.NewFormulaId);
         Convert.ToBoolean(Convert.ToInt32( _xmlResultElement.Attribute("useAsValue").Value)).ShouldBeTrue();
      }
   }
}