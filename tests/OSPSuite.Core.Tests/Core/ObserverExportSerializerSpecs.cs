using System;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObserverExportSerializer : ContextSpecification<ObserverExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (ObserverExportSerializer) _repository.SerializerFor<QuantityExport>();
      }
   }

   public class When_serializing_an_observer_export : concern_for_ObserverExportSerializer
   {
      private QuantityExport _observerExport;
      private XElement _xmlResultNode;

      protected override void Context()
      {
         base.Context();
         _observerExport = new QuantityExport();
         _observerExport.EntityId = "Bla";
         _observerExport.Id = 3;
         _observerExport.FormulaId = 2;
         _observerExport.Name = "look";
         _observerExport.Path =@"Hier\gibt\es\was\zu\sehen";
         _observerExport.Unit = "mm";
      }

      protected override void Because()
      {
         _xmlResultNode=sut.Serialize(_observerExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_return_a_observer_export_node()
      {
         _xmlResultNode.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.Observer);
         _xmlResultNode.Attribute("entityId").Value.ShouldBeEqualTo(_observerExport.EntityId);
         Convert.ToInt32(_xmlResultNode.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(_observerExport.Id);
         Convert.ToInt32(_xmlResultNode.Attribute(SimModelSchemaConstants.FormulaId).Value).ShouldBeEqualTo(_observerExport.FormulaId);
         _xmlResultNode.Attribute("name").Value.ShouldBeEqualTo(_observerExport.Name);
         _xmlResultNode.Attribute("path").Value.ShouldBeEqualTo(_observerExport.Path);
         _xmlResultNode.Attribute("unit").Value.ShouldBeEqualTo(_observerExport.Unit);
      }
   }
}	