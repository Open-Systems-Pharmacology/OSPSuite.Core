using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serializers
{
   public abstract class concern_for_UnitXmlSerializer : ContextSpecification<IXmlSerializer<SerializationContext>>
   {
      private UnitSystemXmlSerializerRepository _unitSystemXmlRepository;
      protected SerializationContext _serializationContext;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _unitSystemXmlRepository = new UnitSystemXmlSerializerRepository();
         _serializationContext = SerializationTransaction.Create();
      }

      protected override void Context()
      {
         sut = _unitSystemXmlRepository.SerializerFor<Unit>();
      }
   }

   
   public class When_serializing_a_unit_defined_with_a_factor_as_a_double : concern_for_UnitXmlSerializer
   {
      private Unit _unit;
      private Unit _deserializedUnit;

      protected override void Context()
      {
         base.Context();
         _unit = new Unit("unit",2,-1);
      }

      protected override void Because()
      {
         var serializationString = sut.Serialize(_unit, _serializationContext);
         _deserializedUnit = sut.Deserialize<Unit>(serializationString, _serializationContext);
      }

      [Observation]
      public void should_be_able_to_deserialize_it_and_retrieve_the_accurate_factor()
      {
         _deserializedUnit.Factor.ShouldBeEqualTo(_unit.Factor);
         _deserializedUnit.FactorFormula.ShouldBeEqualTo(_unit.FactorFormula);
      }
   }

   
   public class When_serializing_a_unit_defined_with_a_factor_as_a_string : concern_for_UnitXmlSerializer
   {
      private Unit _unit;
      private Unit _deserializedUnit;

      protected override void Context()
      {
         base.Context();
         _unit = new Unit("unit", 1.0/60, -1);
         _unit.FactorFormula = "1/60";
      }

      protected override void Because()
      {
         var serializationString = sut.Serialize(_unit, _serializationContext);
         _deserializedUnit = sut.Deserialize<Unit>(serializationString, _serializationContext);

      }
      [Observation]
      public void should_be_able_to_deserialize_it_and_retrieve_the_accurate_factor()
      {
         _deserializedUnit.Factor.ShouldBeEqualTo(_unit.Factor);
         _deserializedUnit.FactorFormula.ShouldBeEqualTo(_unit.FactorFormula);
      }
   }
}	