using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ParameterFlagAttributeMapper : ContextSpecification<ParameterFlagAttributeMapper>
   {
      protected SerializationContext _serializationContext;

      protected override void Context()
      {
         sut = new ParameterFlagAttributeMapper();
         _serializationContext = SerializationTransaction.Create();
      }
   }


   public class When_converting_a_parameter_flag_to_a_string_using_the_parameter_flag_attribute_mapper : concern_for_ParameterFlagAttributeMapper
   {
      private ParameterFlag _onlyVariable;
      private ParameterFlag _variableAndMinIsAllowedAndMaxIsAllowed;
      private string _result1;
      private string _result2;

      protected override void Context()
      {
         base.Context();
         _onlyVariable = ParameterFlag.CanBeVaried;
         _variableAndMinIsAllowedAndMaxIsAllowed = ParameterFlag.CanBeVaried;
         _variableAndMinIsAllowedAndMaxIsAllowed ^= ParameterFlag.MinIsAllowed;
         _variableAndMinIsAllowedAndMaxIsAllowed ^= ParameterFlag.MaxIsAllowed;
      }

      protected override void Because()
      {
         _result1 = sut.Convert(_variableAndMinIsAllowedAndMaxIsAllowed, _serializationContext);
         _result2 = sut.Convert(_onlyVariable, _serializationContext);
      }

      [Observation]
      public void should_return_a_value_representing_the_actual_flag()
      {
         _result1.ShouldNotBeNull();
         _result2.ShouldNotBeNull();
      }

      [Observation]
      public void should_be_able_to_restore_the_flag_from_the_converted_value()
      {
         sut.ConvertFrom(_result1, _serializationContext).ShouldBeEqualTo(_variableAndMinIsAllowedAndMaxIsAllowed);
         sut.ConvertFrom(_result2, _serializationContext).ShouldBeEqualTo(_onlyVariable);
      }
   }
}