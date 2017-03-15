using System;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_QuantityExportSerializer : ContextSpecification< QuantityExportSerializer<QuantityExport>>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (QuantityExportSerializer<QuantityExport>) _repository.SerializerFor<QuantityExport>();
      }
   }

   
   public class When_serializing_a_quantity_export : concern_for_QuantityExportSerializer
   {
      private QuantityExport _quantityExport;
      private XElement _resultXML;

      protected override void Context()
      {
         base.Context();
         _quantityExport = new QuantityExport();
         _quantityExport.EntityId = "quantity";
         _quantityExport.Id = 13;
         _quantityExport.FormulaId = 2;
         _quantityExport.Name = "TestValue";
         _quantityExport.Path = @"Test\Plasma\";
         _quantityExport.Unit = "mol";
      }
      protected override void Because()
      {
         _resultXML = sut.Serialize(_quantityExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_create_a_node_with_the_correct_properties()
      {
         _resultXML.ShouldNotBeNull();
         Convert.ToInt32(_resultXML.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(_quantityExport.Id);
         Convert.ToInt32(_resultXML.Attribute(SimModelSchemaConstants.FormulaId).Value).ShouldBeEqualTo(_quantityExport.FormulaId);
         _resultXML.Attribute("name").Value.ShouldBeEqualTo(_quantityExport.Name);
         _resultXML.Attribute("entityId").Value.ShouldBeEqualTo(_quantityExport.EntityId);
         _resultXML.Attribute("unit").Value.ShouldBeEqualTo(_quantityExport.Unit);
         _resultXML.Attribute("path").Value.ShouldBeEqualTo(_quantityExport.Path);
      }
   }
}	