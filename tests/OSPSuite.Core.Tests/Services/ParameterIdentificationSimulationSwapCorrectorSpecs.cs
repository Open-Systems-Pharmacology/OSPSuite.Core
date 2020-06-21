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
   public abstract class concern_for_ParameterIdentificationSimulationSwapCorrector : ContextSpecification<ParameterIdentificationSimulationSwapCorrector>
   {
      protected ISimulation _newSimulation;
      protected ISimulation _oldSimulation;
      protected ParameterIdentification _parameterIdentification;
      protected IReadOnlyList<OutputMapping> _outputMappings;
      protected OutputMapping _outputMapping;
      protected IdentificationParameter _identificationParameter;
      protected ParameterSelection _linkedParameter;
      private ISimulation _anotherSimulation;
      protected ParameterSelection _anotherParameter;

      protected override void Context()
      {
         _newSimulation = A.Fake<ISimulation>();
         _oldSimulation = A.Fake<ISimulation>();
         _anotherSimulation = A.Fake<ISimulation>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _outputMapping = A.Fake<OutputMapping>();
         _outputMappings = new[] { _outputMapping };
         _identificationParameter = new IdentificationParameter();
         _linkedParameter = new ParameterSelection(_oldSimulation, "parameter|path");
         _anotherParameter = new ParameterSelection(_anotherSimulation, "parameter|path");

         A.CallTo(() => _parameterIdentification.AllOutputMappingsFor(_oldSimulation)).Returns(_outputMappings);

         var simulationQuantitySelectionFinder = new SimulationQuantitySelectionFinder();

         sut = new ParameterIdentificationSimulationSwapCorrector(simulationQuantitySelectionFinder);
      }

      protected override void Because()
      {
         sut.CorrectParameterIdentification(_parameterIdentification, _oldSimulation, _newSimulation);
      }
   }

   public class When_correcting_a_swap_where_the_new_simulation_does_not_have_a_parameter_matching_a_linked_parameter_from_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapCorrector
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.AllIdentificationParameters).Returns(new[] { _identificationParameter });
         _identificationParameter.AddLinkedParameter(_linkedParameter);
         _identificationParameter.AddLinkedParameter(_anotherParameter);

      }

      [Observation]
      public void the_identification_parameter_should_not_contain_the_linked_parameter()
      {
         _identificationParameter.AllLinkedParameters.ShouldContain(_anotherParameter);
         _identificationParameter.AllLinkedParameters.ShouldNotContain(_linkedParameter);
      }
   }

   public class When_correcting_a_swap_where_the_new_simulation_does_not_have_the_output_mappings_from_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapCorrector
   {
      private OutputSelections _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = new OutputSelections();
         A.CallTo(() => _newSimulation.OutputSelections).Returns(_outputSelections);
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_outputMappings);
      }

      [Observation]
      public void the_output_mapping_should_have_been_removed_from_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentification.RemoveOutputMapping(_outputMapping)).MustHaveHappened();
      }
   }

   public class When_correcting_a_swap_where_the_new_simulation_does_not_have_the_observed_data_used_in_the_old_simulation : concern_for_ParameterIdentificationSimulationSwapCorrector
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_outputMappings);
         A.CallTo(() => _newSimulation.UsesObservedData(A<DataRepository>._)).Returns(false);
      }

      [Observation]
      public void the_output_mapping_should_have_been_removed_from_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentification.RemoveOutputMapping(_outputMapping)).MustHaveHappened();
      }
   }
}
