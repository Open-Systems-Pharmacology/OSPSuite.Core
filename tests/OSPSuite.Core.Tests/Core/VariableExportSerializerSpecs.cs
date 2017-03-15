using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_VariableExportSerializer : ContextSpecification<VariableExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (VariableExportSerializer) _repository.SerializerFor<VariableExport>();
      }
   }

   
   public class When_serializing_a_variable_export : concern_for_VariableExportSerializer
   {
      private VariableExport _variableExport;
      private XElement _resultXML;

      protected override void Context()
      {
         base.Context();
         _variableExport = new VariableExport();

         _variableExport.EntityId = "quantity";
         _variableExport.Id = 13;
         _variableExport.FormulaId = 2;
         _variableExport.Name = "TestValue";
         _variableExport.Path = @"Test\Plasma\";
         _variableExport.Unit = "mol";
         _variableExport.ScaleFactor = 1;
         _variableExport.RHSIds = new List<int> {1, 3};
      }

      protected override void Because()
      {
         _resultXML = sut.Serialize(_variableExport, new SimModelSerializationContext());
      }

      [Observation]
      public void should_create_a_node_with_the_correct_properties()
      {
         _resultXML.ShouldNotBeNull();
         _resultXML.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.Variable);
         Convert.ToInt32(_resultXML.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(_variableExport.Id);
         Convert.ToInt32(_resultXML.Attribute("initialValueFormulaId").Value).ShouldBeEqualTo(_variableExport.FormulaId);
         _resultXML.Attribute("name").Value.ShouldBeEqualTo(_variableExport.Name);
         _resultXML.Attribute("entityId").Value.ShouldBeEqualTo(_variableExport.EntityId);
         _resultXML.Attribute("unit").Value.ShouldBeEqualTo(_variableExport.Unit);
         _resultXML.Attribute("path").Value.ShouldBeEqualTo(_variableExport.Path);
      }

      [Observation]
      public void should_create_the_rhs_sides_node()
      {
         var resultRhsNode = _resultXML.Element(XName.Get(SimModelSchemaConstants.RhsFormulaList, SimModelSchemaConstants.Namespace));
         resultRhsNode.ShouldNotBeNull();
         resultRhsNode.Descendants().Count().ShouldBeEqualTo(_variableExport.RHSIds.Count);
      }

      [Observation]
      public void should_create_the_scale_factor_node()
      {
         var scaleFactorNode = _resultXML.Element(XName.Get(SimModelSchemaConstants.ScaleFactor, SimModelSchemaConstants.Namespace));
         scaleFactorNode.ShouldNotBeNull();
         Convert.ToDouble(scaleFactorNode.Value).ShouldBeEqualTo(_variableExport.ScaleFactor);
      }
   }
}