using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core
{
   public abstract class concern_for_BaseDimensionRepresentationXmlSerializer : ContextSpecification<IXmlSerializer<SerializationContext>>
   {
      private UnitSystemXmlSerializerRepository _unitSystemXmlRepository;
      protected SerializationContext _serializationContext;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _unitSystemXmlRepository = new UnitSystemXmlSerializerRepository();
         _unitSystemXmlRepository.PerformMapping();
         _serializationContext = SerializationTransaction.Create();
      }

      protected override void Context()
      {
         sut = _unitSystemXmlRepository.SerializerFor<BaseDimensionRepresentation>();
      }
   }

   public class When_serializing_a_base_dimension_representation_with_a_prefix_set_as_a_double : concern_for_BaseDimensionRepresentationXmlSerializer
   {
      private BaseDimensionRepresentation _baseDimensionRepresentation;
      private BaseDimensionRepresentation _deserializedBaseDimensionRepresentation;

      protected override void Context()
      {
         base.Context();
         _baseDimensionRepresentation = new BaseDimensionRepresentation { TimeExponent = 2};
      }

      protected override void Because()
      {
         var serializationString = sut.Serialize(_baseDimensionRepresentation, _serializationContext);
         _deserializedBaseDimensionRepresentation = sut.Deserialize<BaseDimensionRepresentation>(serializationString, _serializationContext);
      }

      [Observation]
      public void should_be_able_to_deserialize_it_and_retrieve_the_accurate_time_exponent()
      {
         _deserializedBaseDimensionRepresentation.TimeExponent.ShouldBeEqualTo(_baseDimensionRepresentation.TimeExponent);
      }
   }

}	