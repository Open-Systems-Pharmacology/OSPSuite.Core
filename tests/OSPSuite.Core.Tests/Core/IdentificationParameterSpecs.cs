using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Format;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core
{
   public abstract class concern_for_IdentificationParameter : ContextSpecification<IdentificationParameter>
   {
      protected ParameterSelection _parameterSelection;

      protected override void Context()
      {
         sut = new IdentificationParameter();

         _parameterSelection = A.Fake<ParameterSelection>();
      }
   }

   public class When_swapping_a_simulation_reference_in_an_identification_parameter_and_the_old_simulation_is_not_being_replaced : concern_for_IdentificationParameter
   {
      private ISimulation _oldSimulation;
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _oldSimulation = A.Fake<ISimulation>();
         _newSimulation = A.Fake<ISimulation>();
         sut.AddLinkedParameter(new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>()));
         sut.AddLinkedParameter(new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>()));
      }

      protected override void Because()
      {
         sut.SwapSimulation(A.Fake<ISimulation>(), _newSimulation);
      }

      [Observation]
      public void the_references_to_the_simulation_should_not_be_swapped()
      {
         sut.AllLinkedParameters.All(parameter => Equals(parameter.Simulation, _newSimulation)).ShouldBeFalse();
      }
   }

   public class When_swapping_a_simulation_reference_in_an_identification_parameter : concern_for_IdentificationParameter
   {
      private ISimulation _oldSimulation;
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _oldSimulation = A.Fake<ISimulation>();
         _newSimulation = A.Fake<ISimulation>();
         sut.AddLinkedParameter(new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>()));
         sut.AddLinkedParameter(new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>()));
      }

      protected override void Because()
      {
         sut.SwapSimulation(_oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_references_to_the_simulation_should_be_swapped()
      {
         sut.AllLinkedParameters.All(parameter => Equals(parameter.Simulation, _newSimulation)).ShouldBeTrue();
      }
   }

   public class When_adding_a_linked_parameter_to_an_identification_parameter_that_does_not_have_a_dimension_defined : concern_for_IdentificationParameter
   {
      protected override void Because()
      {
         sut.AddLinkedParameter(_parameterSelection);
      }

      [Observation]
      public void should_add_the_parameter_as_linked_presenter()
      {
         sut.AllLinkedParameters.First().Parameter.ShouldBeEqualTo(_parameterSelection.Parameter);
      }

      [Observation]
      public void the_dimension_of_the_identification_parameter_should_be_the_one_of_the_first_linked_parameter()
      {
         sut.Dimension.ShouldBeEqualTo(_parameterSelection.Dimension);
      }
   }

   public class When_adding_a_linked_parameter_to_an_identification_parameter_that_does_match_the_dimension : concern_for_IdentificationParameter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddLinkedParameter(_parameterSelection);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AddLinkedParameter(A.Fake<ParameterSelection>())).ShouldThrowAn<DimensionMismatchException>();
      }
   }

   public class When_retrieving_the_linked_path_for_an_identification_parameter_linking_two_parameters_with_the_same_path : concern_for_IdentificationParameter
   {
      private ParameterSelection _anotherLinkedParameter;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterSelection.QuantitySelection.Path).Returns("A|B");
         _anotherLinkedParameter = A.Fake<ParameterSelection>();
         A.CallTo(() => _anotherLinkedParameter.QuantitySelection.Path).Returns("A|B");
         A.CallTo(() => _anotherLinkedParameter.Dimension).Returns(_parameterSelection.Dimension);
         sut.AddLinkedParameter(_parameterSelection);
         sut.AddLinkedParameter(_anotherLinkedParameter);
      }

      [Observation]
      public void should_return_their_path()
      {
         sut.LinkedPath.ShouldBeEqualTo(new[] {"A", "B"}.ToPathString());
      }
   }

   public class When_retrieving_the_linked_path_for_an_identification_parameter_linking_two_parameters_with_different_path : concern_for_IdentificationParameter
   {
      private ParameterSelection _anotherLinkedParameter;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterSelection.QuantitySelection.Path).Returns("A|B");
         _anotherLinkedParameter = A.Fake<ParameterSelection>();
         A.CallTo(() => _anotherLinkedParameter.QuantitySelection.Path).Returns("B|C");
         A.CallTo(() => _anotherLinkedParameter.Dimension).Returns(_parameterSelection.Dimension);
         sut.AddLinkedParameter(_parameterSelection);
         sut.AddLinkedParameter(_anotherLinkedParameter);
      }

      [Observation]
      public void should_return_null()
      {
         sut.LinkedPath.ShouldBeNull();
      }
   }

   public class When_updating_an_identification_parameter_from_a_source_identification_parameter : concern_for_IdentificationParameter
   {
      private ICloneManager _cloneManager;
      private IdentificationParameter _sourceParameterIdentification;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _sourceParameterIdentification = new IdentificationParameter {Name = "TOTO", IsFixed = true, UseAsFactor = false};
         _sourceParameterIdentification.AddLinkedParameter(_parameterSelection);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceParameterIdentification, _cloneManager);
      }

      [Observation]
      public void should_update_the_base_properties()
      {
         sut.Name.ShouldBeEqualTo("TOTO");
         sut.IsFixed.ShouldBeEqualTo(_sourceParameterIdentification.IsFixed);
         sut.UseAsFactor.ShouldBeEqualTo(_sourceParameterIdentification.UseAsFactor);
      }

      [Observation]
      public void should_add_a_clone_of_all_linked_parameters()
      {
         sut.AllLinkedParameters.Count.ShouldBeEqualTo(1);
         sut.AllLinkedParameters[0].ShouldNotBeEqualTo(_parameterSelection);
      }
   }

   public class When_checking_if_an_identification_parameter_is_linking_a_parameter_selection_with_an_undefined_simulation : concern_for_IdentificationParameter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddLinkedParameter(new ParameterSelection(null, "A|B"));
      }

      [Observation]
      public void should_return_false_even_if_another_linked_parameter_existis_with_the_same_path()
      {
         sut.LinksParameter(new ParameterSelection(null, "A|B")).ShouldBeFalse();
      }
   }

   public class When_validating_identification_parameters_for_consistency_with_linked_parameters : concern_for_IdentificationParameter
   {
      protected Parameter _parameter;

      protected override void Context()
      {
         sut = DomainHelperForSpecs.IdentificationParameter(min: 0, max: 1, startValue: 0.5);
         _parameterSelection = A.Fake<ParameterSelection>();
         _parameter = new Parameter {Dimension = DomainHelperForSpecs.TimeDimensionForSpecs()};
         _parameter.DisplayUnit = _parameter.Dimension.Unit("h");
         A.CallTo(() => _parameterSelection.Parameter).Returns(_parameter);
         A.CallTo(() => _parameterSelection.IsValid).Returns(true);

         sut.AddLinkedParameter(_parameterSelection);
      }
   }

   public class When_validating_an_identification_parameter_where_the_linked_parameters_have_inconsistent_maximum_and_are_used_as_factor : concern_for_IdentificationParameter
   {
      private Parameter _parameter;

      protected override void Context()
      {
         sut = DomainHelperForSpecs.IdentificationParameter(min: 0.1, max: 10, startValue: 1.5);
         _parameterSelection = A.Fake<ParameterSelection>();
         _parameter = new Parameter();
         A.CallTo(() => _parameterSelection.Parameter).Returns(_parameter);
         A.CallTo(() => _parameterSelection.IsValid).Returns(true);
         sut.UseAsFactor = true;

         sut.AddLinkedParameter(_parameterSelection);
         _parameter.Value = 100;
         _parameter.MinValue = 1;
         _parameter.MaxValue = 1000;
         _parameter.MaxIsAllowed = true;
      }

      [Observation]
      public void the_value_is_inconsistent_but_as_factor_is_ok()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_an_identification_parameter_where_the_linked_parameters_have_inconsistent_maximum : When_validating_identification_parameters_for_consistency_with_linked_parameters
   {
      protected override void Context()
      {
         base.Context();
         _parameter.MaxValue = 1;
      }

      [Observation]
      public void should_not_validate_because_maximum_should_be_smaller_than_parameter_maximum()
      {
         _parameter.MaxIsAllowed = false;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_validate_because_maximum_is_smaller_than_or_equal_to_parameter_maximum()
      {
         _parameter.MaxIsAllowed = true;
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_an_identification_parameter_where_the_linked_parameters_have_inconsistent_minimum : When_validating_identification_parameters_for_consistency_with_linked_parameters
   {
      protected override void Context()
      {
         base.Context();
         _parameter.MinValue = 0;
      }

      [Observation]
      public void should_not_validate_because_minimum_should_be_greater_than()
      {
         _parameter.MinIsAllowed = false;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_validate_because_minimum_should_be_greater_than_or_equal_to()
      {
         _parameter.MinIsAllowed = true;
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_an_identification_parameter : concern_for_IdentificationParameter
   {
      protected override void Context()
      {
         base.Context();
         sut = DomainHelperForSpecs.IdentificationParameter(min: 20, max: 120, startValue: 60);
      }

      [Observation]
      public void should_return_valid_if_value_is_between_min_and_max()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_return_invalid_if_value_is_bigger_than_max()
      {
         sut.StartValueParameter.Value = 200;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_return_invalid_if_value_is_smaller_than_min()
      {
         sut.StartValueParameter.Value = 1;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_return_invalid_if_value_of_min_is_bigger_or_equal_to_max()
      {
         sut.MinValueParameter.Value = 200;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_return_invalid_if_min_is_smaller_or_equal_to_zero_and_scaling_is_log()
      {
         sut.Scaling = Scalings.Log;

         sut.MinValueParameter.Value = 0;
         sut.IsValid().ShouldBeFalse();

         sut.MinValueParameter.Value = -5;
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_return_valid_if_the_only_underlying_linked_parameter_is_null()
      {
         sut.AddLinkedParameter(new ParameterSelection(null, "A|B|C"));
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_an_identification_parameter_with_a_parameter_selection_max_set_to_a_value_higher_than_the_underlying_parameter_max : concern_for_IdentificationParameter
   {
      public NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      protected override void Context()
      {
         base.Context();
         var parameter = new Parameter
         {
            Dimension = DomainHelperForSpecs.TimeDimensionForSpecs(),
            Value = 80,
            MinValue = 20,
            MinIsAllowed = true,
            MaxValue = 120,
            MaxIsAllowed = true
         };

         parameter.DisplayUnit = parameter.Dimension.Unit("h");
         A.CallTo(() => _parameterSelection.Parameter).Returns(parameter);
         A.CallTo(() => _parameterSelection.IsValid).Returns(true);
         sut = DomainHelperForSpecs.IdentificationParameter(min: 20, max: 120, startValue: 60);
         sut.AddLinkedParameter(_parameterSelection);

         sut.MaxValueParameter.Value = 200;
      }

      [Observation]
      public void should_return_invalid()
      {
         sut.IsValid().ShouldBeFalse();

         var message = sut.Validate().Message;
         message.Contains($"{_numericFormatter.Format(2)} h").ShouldBeTrue();
      }
   }

   public class When_returning_the_optimized_parameter_value_for_a_linked_parameter : concern_for_IdentificationParameter
   {
      [Observation]
      public void should_return_the_optimized_parameter_value_itself_if_the_flag_use_as_factor_is_disabled()
      {
         sut.UseAsFactor = false;
         sut.OptimizedParameterValueFor(new OptimizedParameterValue("P", 10, 20), _parameterSelection).ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_return_the_optimized_parameter_value_multiply_by_the_parameter_start_value_otherwise()
      {
         sut.UseAsFactor = true;
         _parameterSelection.Parameter.Value = 50;
         sut.OptimizedParameterValueFor(new OptimizedParameterValue("P", 10, 20), _parameterSelection).ShouldBeEqualTo(500);
      }
   }
}