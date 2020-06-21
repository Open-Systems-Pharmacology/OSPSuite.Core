using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SensitivityParameter : ContextSpecification<SensitivityParameter>
   {
      protected IParameter _parameter;

      protected override void Context()
      {
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(10);
         var parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Parameter).Returns(_parameter);
         sut = new SensitivityParameter {ParameterSelection = parameterSelection};
         sut.Add(DomainHelperForSpecs.ConstantParameterWithValue(0.1).WithName(Constants.Parameters.VARIATION_RANGE));
         sut.Add(DomainHelperForSpecs.ConstantParameterWithValue(2).WithName(Constants.Parameters.NUMBER_OF_STEPS));
      }
   }

   public class When_generating_variation_values_for_an_unconstrained_parameter_whose_default_value_is_not_zero : concern_for_SensitivityParameter
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         sut.VariationValues().ShouldOnlyContain(_parameter.Value * (1 + sut.VariationRangeValue / 2), _parameter.Value * (1 + sut.VariationRangeValue), _parameter.Value / (1 + sut.VariationRangeValue / 2), _parameter.Value / (1 + sut.VariationRangeValue));
      }
   }

   public class When_generating_variation_values_for_an_unconstrained_parameter_whose_defaalt_value_is_zero : concern_for_SensitivityParameter
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Value = 0;
      }

      [Observation]
      public void should_return_the_expected_values()
      {
         sut.VariationValues().ShouldBeEmpty();
      }
   }

   public class When_generating_variation_values_for_constrained_parameter_whose_default_value_is_zero : concern_for_SensitivityParameter
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Value = 0;
         _parameter.MinValue = 0;
         _parameter.MinIsAllowed = false;
      }

      [Observation]
      public void should_return_the_expected_values()
      {
         sut.VariationValues().ShouldBeEmpty();
      }
   }

   public class When_generating_variation_values_for_constrained_parameter_whose_default_value_not_zero : concern_for_SensitivityParameter
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Value = 10;
         _parameter.MinValue = 10;
         _parameter.MinIsAllowed = true;
      }

      [Observation]
      public void should_return_the_expected_values()
      {
         sut.VariationValues().ShouldOnlyContain(_parameter.Value * (1 + sut.VariationRangeValue / 2), _parameter.Value * (1 + sut.VariationRangeValue));
      }
   }

   public class When_updating_the_properties_form_an_source_sensitivity_parameter : concern_for_SensitivityParameter
   {
      private SensitivityParameter _sourceParameterSelection;
      private ParameterSelection _parameterSelection;

      protected override void Context()
      {
         base.Context();
         _sourceParameterSelection = A.Fake<SensitivityParameter>();
         _parameterSelection= A.Fake<ParameterSelection>();
         _sourceParameterSelection.ParameterSelection = _parameterSelection;
      }
      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceParameterSelection, A.Fake<ICloneManager>());
      }

      [Observation]
      public void should_also_clone_the_parameter_selection()
      {
         sut.ParameterSelection.ShouldBeEqualTo(_parameterSelection.Clone());
      }
   }
}