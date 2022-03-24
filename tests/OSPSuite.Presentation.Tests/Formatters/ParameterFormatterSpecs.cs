using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presentation;

namespace a
{
   public abstract class concern_for_ParameterFormatter : ContextSpecification<ParameterFormatter>
   {
      protected IParameter _parameter;
      protected IParameterDTO _parameterDTO;

      protected override void Context()
      {
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(double.NaN);
         _parameterDTO = A.Fake<IParameterDTO>();
         A.CallTo(() => _parameterDTO.Parameter).Returns(_parameter);

      }
   }

   public class When_formatting_a_parameter_dto_with_a_NaN_value_and_editable_is_supported : concern_for_ParameterFormatter
   {
      protected override void Context()
      {
         base.Context();
         sut = new ParameterFormatter(_parameterDTO, checkForEditable:true);

      }
      [Observation]
      public void should_return_the_expected_caption()
      {
         sut.Format(_parameter.Value).ShouldBeEqualTo(Captions.EnterAValue);       
      }
   }


   public class When_formatting_a_parameter_dto_with_a_NaN_value_and_editable_is_not_supported : concern_for_ParameterFormatter
   {
      protected override void Context()
      {
         base.Context();
         sut = new ParameterFormatter(_parameterDTO, checkForEditable: false);

      }
      [Observation]
      public void should_return_the_expected_caption()
      {
         sut.Format(_parameter.Value).ShouldBeEqualTo(Captions.NaN);
      }
   }
}