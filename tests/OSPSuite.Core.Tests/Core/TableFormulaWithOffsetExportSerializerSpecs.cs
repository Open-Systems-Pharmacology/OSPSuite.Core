using System;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{

   class TableFormulaWithOffsetExportSerializerSpecs
   {
   }
   public abstract class concern_for_TableFormulaWithOffsetExportSerializer : ContextSpecification<TableFormulaWithOffsetExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (TableFormulaWithOffsetExportSerializer)_repository.SerializerFor<TableFormulaWithOffsetExport>();
      }
   }

   public class When_serializing_a_table_formula_with_offset_export : concern_for_TableFormulaWithOffsetExportSerializer
   {
      private TableFormulaWithOffsetExport _tableFormulaWithOffsetExport;
      private XElement _xmlResultElement;

      protected override void Context()
      {
         base.Context();
         _tableFormulaWithOffsetExport = new TableFormulaWithOffsetExport();
         _tableFormulaWithOffsetExport.TableObjectId = 123;
         _tableFormulaWithOffsetExport.OffsetObjectId = 456;
         _tableFormulaWithOffsetExport.Id = 789;

      }
      protected override void Because()
      {
         _xmlResultElement = sut.Serialize(_tableFormulaWithOffsetExport, new SimModelSerializationContext() );
      }
      [Observation]
      public void should_create_the_formula_Node()
      {
         _xmlResultElement.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.TableFormulaWithOffset);
         Convert.ToInt32(_xmlResultElement.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(_tableFormulaWithOffsetExport.Id);
      }

      [Observation]
      public void should_create_table_node()
      {
         var tableNode = _xmlResultElement.Element(XName.Get(SimModelSchemaConstants.TableObject, SimModelSchemaConstants.Namespace));
         tableNode.ShouldNotBeNull();

         var idAttr = tableNode.Attribute(XName.Get(SimModelSchemaConstants.Id));
         idAttr.ShouldNotBeNull();
         Convert.ToInt32(idAttr.Value).ShouldBeEqualTo(_tableFormulaWithOffsetExport.TableObjectId);
      }

      [Observation]
      public void should_create_offset_node()
      {
         var offsetNode = _xmlResultElement.Element(XName.Get(SimModelSchemaConstants.OffsetObject, SimModelSchemaConstants.Namespace));
         offsetNode.ShouldNotBeNull();

         var idAttr = offsetNode.Attribute(XName.Get(SimModelSchemaConstants.Id));
         idAttr.ShouldNotBeNull();
         Convert.ToInt32(idAttr.Value).ShouldBeEqualTo(_tableFormulaWithOffsetExport.OffsetObjectId);
      }

   }

}
