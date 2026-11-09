using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serializers
{
   public class ParameterValueWithInitialStateSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      private const string INITIAL_FORMULA_ID = "F1";

      private static IndividualParameter parameterWithInitialState()
      {
         return new IndividualParameter
         {
            ContainerPath = new ObjectPath("A", "B"),
            Name = "P1",
            Value = 10,
            InitialValue = 5,
            InitialFormulaId = INITIAL_FORMULA_ID
         };
      }

      [Observation]
      public void should_be_able_to_serialize_and_deserialize_an_individual_with_a_parameter_keeping_its_initial_state()
      {
         var initialFormula = new ExplicitFormula().WithFormulaString("Height * 2").WithName("F").WithId(INITIAL_FORMULA_ID);
         var x1 = new IndividualBuildingBlock {parameterWithInitialState()}.WithId("test");
         x1.AddFormula(initialFormula);

         var x2 = SerializeAndDeserialize(x1);

         var parameter = x2.First();
         parameter.InitialValue.ShouldBeEqualTo(5);
         parameter.InitialFormulaId.ShouldBeEqualTo(INITIAL_FORMULA_ID);
         x2.FormulaCache[parameter.InitialFormulaId].Name.ShouldBeEqualTo("F");
      }

      [Observation]
      public void should_write_the_initial_formula_id_into_the_attribute_used_by_existing_files()
      {
         var parameter = parameterWithInitialState();
         using (var serializationContext = SerializationTransaction.Create(IoC.Container))
         {
            var element = SerializerRepository.SerializerFor(parameter).Serialize(parameter, serializationContext);
            element.Attribute("initialFormulaId").Value.ShouldBeEqualTo(INITIAL_FORMULA_ID);
         }
      }

      [Observation]
      public void should_write_the_reference_marker_of_the_serialization_context_in_front_of_the_initial_formula_id()
      {
         var parameter = parameterWithInitialState();
         using (var serializationContext = SerializationTransaction.Create(IoC.Container))
         {
            serializationContext.ReferenceMarker = "ref:";
            var element = SerializerRepository.SerializerFor(parameter).Serialize(parameter, serializationContext);
            element.Attribute(Constants.Serialization.Attribute.INITIAL_FORMULA_ID).Value.ShouldBeEqualTo("ref:" + INITIAL_FORMULA_ID);
         }
      }

      [Observation]
      public void should_not_write_the_attribute_for_a_parameter_without_initial_formula()
      {
         var parameter = parameterWithInitialState();
         parameter.InitialFormulaId = null;
         using (var serializationContext = SerializationTransaction.Create(IoC.Container))
         {
            var element = SerializerRepository.SerializerFor(parameter).Serialize(parameter, serializationContext);
            element.Attribute(Constants.Serialization.Attribute.INITIAL_FORMULA_ID).ShouldBeNull();
         }
      }
   }
}
