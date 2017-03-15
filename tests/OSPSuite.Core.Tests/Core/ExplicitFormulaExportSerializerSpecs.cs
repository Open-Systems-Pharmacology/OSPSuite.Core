using System;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_ExplicitFormulaExportSerilalizer : ContextSpecification<ExplicitFormulaExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (ExplicitFormulaExportSerializer) _repository.SerializerFor<ExplicitFormulaExport>();
      }
   }

   public class When_serializing_a_explicit_formula_export : concern_for_ExplicitFormulaExportSerilalizer
   {
      private ExplicitFormulaExport _explicitFormulaExport;
      private XElement _xmlResultElement;

      protected override void Context()
      {
         base.Context();
         _explicitFormulaExport = new ExplicitFormulaExport();
         _explicitFormulaExport.Equation = "PA/VB * 2";
         _explicitFormulaExport.Id = 12;
         _explicitFormulaExport.ReferenceList.Add("VB", 1);
         _explicitFormulaExport.ReferenceList.Add("VC", 1);
         _explicitFormulaExport.ReferenceList.Add("PA", 2);
         _explicitFormulaExport.ReferenceList.Add("PB", 2);
      }

      protected override void Because()
      {
         _xmlResultElement = sut.Serialize(_explicitFormulaExport, new SimModelSerializationContext());
      }

      [Observation]
      public void should_create_the_formula_Node()
      {
         _xmlResultElement.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.ExplicitFormula);
         Convert.ToInt32(_xmlResultElement.Attribute(SimModelSchemaConstants.Id).Value).ShouldBeEqualTo(_explicitFormulaExport.Id);
      }

      [Observation]
      public void should_create_a_equation_node()
      {
         var xmlEquation = _xmlResultElement.Element(XName.Get(SimModelSchemaConstants.Equation, SimModelSchemaConstants.Namespace));
         xmlEquation.ShouldNotBeNull();
         xmlEquation.Value.ShouldBeEqualTo(_explicitFormulaExport.Equation);
      }

      [Observation]
      public void should_have_created_the_reference_list()
      {
         var xmlParameterList = _xmlResultElement.Elements(XName.Get(SimModelSchemaConstants.ReferenceList, SimModelSchemaConstants.Namespace));
         xmlParameterList.ShouldNotBeNull();
         xmlParameterList.DescendantNodes().Count().ShouldBeEqualTo(_explicitFormulaExport.ReferenceList.Count);
      }
   }
}