using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_IdentificationParameterTask : ContextSpecification<IIdentificationParameterTask>
   {
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         sut = new IdentificationParameterTask(_objectBaseFactory);
      }
   }

   public class When_the_identification_parameter_task_is_told_to_add_range_parameters_to_a_given_identification_parameter_with_negative_start_value : concern_for_IdentificationParameterTask
   {
      private IParameter _parameter;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _identificationParameter = new IdentificationParameter { UseAsFactor = false };
         _parameter = A.Fake<IParameter>();
         _parameter.Value = -1;
         _parameter.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
      }

      protected override void Because()
      {
         sut.AddParameterRangeBasedOn(_identificationParameter, _parameter);
      }

      [Observation]
      public void should_add_a_max_value_parameter_using_the_dimension_of_the_parameter_and_the_default_max_value_equal_to_10_times_the_value()
      {
         validate(_identificationParameter.MaxValueParameter, _parameter.Value / 10);
      }

      [Observation]
      public void should_add_a_min_value_parameter_using_the_dimension_of_the_parameter_and_the_predefined_min_value()
      {
         validate(_identificationParameter.MinValueParameter, _parameter.Value * 10);
      }

      private void validate(IParameter parameter, double value)
      {
         parameter.ShouldNotBeNull();
         parameter.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         parameter.Formula.DowncastTo<ConstantFormula>().Value.ShouldBeEqualTo(value);
      }
   }

   public class When_the_identification_parameter_task_is_told_to_add_range_parameters_to_a_given_identification_parameter : concern_for_IdentificationParameterTask
   {
      private IParameter _parameter;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _identificationParameter = new IdentificationParameter {UseAsFactor = false};
         _parameter = A.Fake<IParameter>();
         _parameter.MinValue = 20;
         _parameter.Value = 50;
         _parameter.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
      }

      protected override void Because()
      {
         sut.AddParameterRangeBasedOn(_identificationParameter, _parameter);
      }

      [Observation]
      public void should_add_a_min_value_parameter_using_the_dimension_of_the_parameter_and_the_predefined_min_value()
      {
         validate(_identificationParameter.MinValueParameter, _parameter.MinValue.Value);
      }

      [Observation]
      public void should_add_a_max_value_parameter_using_the_dimension_of_the_parameter_and_the_default_max_value_equal_to_10_times_the_value()
      {
         validate(_identificationParameter.MaxValueParameter, _parameter.Value * 10);
      }

      [Observation]
      public void should_add_a_start_value_parameter_using_the_dimension_of_the_parameter_and_the_value_of_the_parameter()
      {
         validate(_identificationParameter.StartValueParameter, _parameter.Value);
      }

      private void validate(IParameter parameter, double value)
      {
         parameter.ShouldNotBeNull();
         parameter.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         parameter.Formula.DowncastTo<ConstantFormula>().Value.ShouldBeEqualTo(value);
      }
   }

   public class When_the_identification_parmaeter_task_is_told_to_update_the_range_parameter_of_a_given_identification_parameter : concern_for_IdentificationParameterTask
   {
      private IParameter _parameter;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _identificationParameter = new IdentificationParameter { UseAsFactor = false };
         _parameter = A.Fake<IParameter>();
         _parameter.Value = 50;
         _parameter.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();

         sut.AddParameterRangeBasedOn(_identificationParameter, _parameter);
         var parameterSelection= A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Parameter).Returns(_parameter);
         _identificationParameter.AddLinkedParameter(parameterSelection);
         _identificationParameter.UseAsFactor = true;
      }

      protected override void Because()
      {
         sut.UpdateParameterRange(_identificationParameter);
      }

      [Observation]
      public void should_update_the_dimension_of_the_min_value_parameter_and_the_min_value()
      {
         validate(_identificationParameter.MinValueParameter, 0.1);
      }

      [Observation]
      public void should_update_the_dimension_of_the_max_value_parameter_and_the_max_value()
      {
         validate(_identificationParameter.MaxValueParameter, 10);
      }

      [Observation]
      public void should_update_the_dimension_of_the_start_value_parameter_and_the_start_value()
      {
         validate(_identificationParameter.StartValueParameter, 1);
      }

      private void validate(IParameter parameter, double value)
      {
         parameter.ShouldNotBeNull();
         parameter.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
         parameter.Formula.DowncastTo<ConstantFormula>().Value.ShouldBeEqualTo(value);
      }
   }

   public class When_updating_the_start_values_from_the_simulation : concern_for_IdentificationParameterTask
   {
      private IdentificationParameter _identificationParameter;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _parameter.Value = 50;

         _identificationParameter = new IdentificationParameter {DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.START_VALUE)};

         var parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Parameter).Returns(_parameter);

         _identificationParameter.AddLinkedParameter(parameterSelection);
      }

      protected override void Because()
      {
         sut.UpdateStartValuesFromSimulation(_identificationParameter);
      }

      [Observation]
      public void should_update_the_value_defined_in_the_underlying_first_linked_parameter()
      {
         _identificationParameter.StartValueParameter.Value.ShouldBeEqualTo(_parameter.Value);
      }
   }

   public class When_updating_the_start_values_from_the_simulation_for_a_parameter_that_does_not_exist_in_the_simulation_anymore : concern_for_IdentificationParameterTask
   {
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();

         _identificationParameter = new IdentificationParameter { DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.START_VALUE) };

         var parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Parameter).Returns(null);

         _identificationParameter.AddLinkedParameter(parameterSelection);
      }

      [Observation]
      public void should_throw_an_exception_explaining_that_the_parameter_cannot_be_found()
      {
         The.Action(() => sut.UpdateStartValuesFromSimulation(_identificationParameter)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_updating_the_start_values_from_the_simulation_for_an_identification_parameter_without_linked_parameters : concern_for_IdentificationParameterTask
   {
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();

         _identificationParameter = new IdentificationParameter { DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.START_VALUE) };
      }

      protected override void Because()
      {
         sut.UpdateStartValuesFromSimulation(_identificationParameter);
      }

      [Observation]
      public void should_not_do_anything()
      {
         _identificationParameter.StartValue.ShouldBeEqualTo(10);
      }
   }

   public class When_updating_the_start_values_from_the_simulation_of_a_parameter_using_factor : concern_for_IdentificationParameterTask
   {
      private IdentificationParameter _identificationParameter;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _parameter.Value = 50;

         _identificationParameter = new IdentificationParameter { DomainHelperForSpecs.ConstantParameterWithValue(0.5).WithName(Constants.Parameters.START_VALUE) };
         _identificationParameter.UseAsFactor = true;

         var parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Parameter).Returns(_parameter);

         _identificationParameter.AddLinkedParameter(parameterSelection);
      }

      protected override void Because()
      {
         sut.UpdateStartValuesFromSimulation(_identificationParameter);
      }

      [Observation]
      public void should_not_update_the_value_defined_in_the_underlying_first_linked_parameter()
      {
         _identificationParameter.StartValueParameter.Value.ShouldBeEqualTo(0.5);
      }
   }
}