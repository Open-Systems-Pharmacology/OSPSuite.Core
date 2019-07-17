using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ParameterIdentificationSimulationSwapValidator : ContextSpecification<ParameterIdentificationSimulationSwapValidator>
   {
      protected ISimulation _newSimulation;
      protected ISimulation _oldSimulation;
      protected ParameterIdentification _parameterIdentification;
      protected ValidationResult _validationResult;
      protected IReadOnlyList<OutputMapping> _outputMappings;
      protected OutputMapping _outputMapping;
      protected IdentificationParameter _identificationParameter;
      protected ParameterSelection _linkedParameter;

      protected override void Context()
      {
         _newSimulation = A.Fake<ISimulation>();
         _oldSimulation = A.Fake<ISimulation>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _outputMapping = A.Fake<OutputMapping>();
         _outputMappings = new[] { _outputMapping };
         _identificationParameter = new IdentificationParameter();
         _linkedParameter = new ParameterSelection(_oldSimulation, "parameter|path");

         A.CallTo(() => _outputMapping.UsesSimulation(_oldSimulation)).Returns(true);

         sut = new ParameterIdentificationSimulationSwapValidator(new SimulationQuantitySelectionFinder());
      }

      protected override void Because()
      {
         _validationResult = sut.ValidateSwap(_parameterIdentification, _oldSimulation, _newSimulation);
      }
   }

   public class When_validating_a_swap_where_the_new_simulation_does_not_have_a_parameter_matching_a_linked_parameter_from_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapValidator
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.AllIdentificationParameters).Returns(new[] { _identificationParameter });
         _identificationParameter.AddLinkedParameter(_linkedParameter);

      }

      [Observation]
      public void the_validation_result_should_be_valid_with_warning()
      {
         _validationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }

   public class When_validating_a_swap_where_the_new_simulation_does_not_have_the_output_mappings_from_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapValidator
   {
      private OutputSelections _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = new OutputSelections();
         A.CallTo(() => _newSimulation.OutputSelections).Returns(_outputSelections);
         A.CallTo(() => _parameterIdentification.AllOutputMappingsFor(_oldSimulation)).Returns(_outputMappings);
      }

      [Observation]
      public void the_validation_result_should_be_valid_with_warning()
      {
         _validationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }

   public class When_validating_a_swap_where_the_new_simulation_does_not_have_the_observed_data_used_in_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapValidator
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.AllOutputMappingsFor(_oldSimulation)).Returns(_outputMappings);
         A.CallTo(() => _newSimulation.UsesObservedData(A<DataRepository>._)).Returns(false);
      }

      [Observation]
      public void the_validation_result_should_be_valid_with_warning()
      {
         _validationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }
}
