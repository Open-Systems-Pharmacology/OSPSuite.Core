using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentification : ContextSpecification<ParameterIdentification>
   {
      protected ISimulation _simulation;
      protected OutputMapping _outputMapping;
      private WeightedObservedData _observedData;
      private QuantitySelection _quantitySelection;

      protected override void Context()
      {
         sut = new ParameterIdentification();
         _observedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData());
         _simulation = A.Fake<ISimulation>().WithName("Sim").WithId("Id");
         _quantitySelection = new QuantitySelection("Organism|Liver|Cell|Asprin|Concentration", QuantityType.Drug);
         _outputMapping = new OutputMapping { WeightedObservedData = _observedData, OutputSelection = new SimulationQuantitySelection(_simulation, _quantitySelection) };
      }
   }

   public class When_swapping_simulations_in_a_parameter_identification : concern_for_ParameterIdentification
   {
      private ISimulation _newSimulation;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         sut.AddSimulation(_simulation);
         sut.AddOutputMapping(_outputMapping);
         _identificationParameter = new IdentificationParameter();
         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, A.Fake<QuantitySelection>()));
         sut.AddIdentificationParameter(_identificationParameter);
      }

      protected override void Because()
      {
         sut.SwapSimulations(_simulation, _newSimulation);
      }

      [Observation]
      public void identification_parameters_should_reference_the_new_simulation()
      {
         sut.AllIdentificationParameters.All(identificationParameter => identificationParameter.AllLinkedParameters.All(linkedParameter => Equals(linkedParameter.Simulation, _newSimulation))).ShouldBeTrue();
      }

      [Observation]
      public void the_new_simulation_should_be_referenced()
      {
         sut.AllSimulations.ShouldNotContain(_simulation);
         sut.AllSimulations.ShouldContain(_newSimulation);
      }

      [Observation]
      public void output_mapping_references_should_be_updated()
      {
         sut.AllOutputMappings.All(mapping => mapping.UsesSimulation(_newSimulation)).ShouldBeTrue();
         sut.AllOutputMappings.Any(mapping => mapping.UsesSimulation(_simulation)).ShouldBeFalse();
      }
   }

   public class When_removing_a_simulation_from_a_parameter_identification : concern_for_ParameterIdentification
   {
      private IdentificationParameter _identificationParameterReferncingOnlySimulation;
      private IdentificationParameter _identificationParameterReferencingManySimulation;
      private ParameterSelection _parameterSelection1;
      private ParameterSelection _parameterSelection2;
      private ParameterSelection _parameterSelection3;
      private ISimulation _otherSimulation;

      protected override void Context()
      {
         base.Context();
         _identificationParameterReferncingOnlySimulation = new IdentificationParameter();
         _otherSimulation = A.Fake<ISimulation>();
         _parameterSelection1 = new ParameterSelection(_simulation, new QuantitySelection("A", QuantityType.Parameter));
         _parameterSelection2 = new ParameterSelection(_simulation, new QuantitySelection("B", QuantityType.Parameter));
         _parameterSelection3 = new ParameterSelection(_otherSimulation, new QuantitySelection("C", QuantityType.Parameter));

         _identificationParameterReferncingOnlySimulation.AddLinkedParameter(_parameterSelection1);
         _identificationParameterReferencingManySimulation = new IdentificationParameter();
         _identificationParameterReferencingManySimulation.AddLinkedParameter(_parameterSelection2);
         _identificationParameterReferencingManySimulation.AddLinkedParameter(_parameterSelection3);

         sut.AddOutputMapping(_outputMapping);
         sut.AddSimulation(_simulation);
         sut.AddSimulation(_otherSimulation);
         sut.AddIdentificationParameter(_identificationParameterReferncingOnlySimulation);
         sut.AddIdentificationParameter(_identificationParameterReferencingManySimulation);
      }

      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_not_have_the_simulation_anymore()
      {
         sut.UsesSimulation(_simulation).ShouldBeFalse();
      }

      [Observation]
      public void should_remove_the_output_mapping_referencing_an_output_belonging_to_the_simulation()
      {
         sut.AllOutputMappings.ShouldNotContain(_outputMapping);
      }

      [Observation]
      public void should_remove_the_linked_parameter_belonging_to_the_simulation()
      {
         sut.AllIdentificationParameters.ShouldContain(_identificationParameterReferencingManySimulation);
         _identificationParameterReferencingManySimulation.AllLinkedParameters.Count.ShouldBeEqualTo(1);
         _identificationParameterReferencingManySimulation.AllLinkedParameters[0].Simulation.ShouldBeEqualTo(_otherSimulation);
      }

      [Observation]
      public void should_remove_any_identification_parameter_that_is_left_empty()
      {
         sut.AllIdentificationParameters.ShouldNotContain(_identificationParameterReferncingOnlySimulation);
      }
   }

   public class When_checking_if_any_output_of_a_given_simulation_are_mapped_in_the_parameter_identification : concern_for_ParameterIdentification
   {
      private ISimulation _anotherUsedSimulation;
      private ISimulation _aSimulationNotUsed;

      protected override void Context()
      {
         base.Context();
         _anotherUsedSimulation = A.Fake<ISimulation>().WithId("Another");
         _aSimulationNotUsed = A.Fake<ISimulation>().WithId("NotUsed");
         sut.AddOutputMapping(_outputMapping);
         sut.AddSimulation(_simulation);
         sut.AddSimulation(_anotherUsedSimulation);
      }

      [Observation]
      public void should_return_true_if_the_parameter_identification_uses_the_simulation_and_if_at_least_one_output_of_this_simulation_is_mapped()
      {
         sut.AnyOutputOfSimulationMapped(_simulation).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_parameter_identification_uses_the_simulation_but_no_output_of_this_simulation_is_mapped()
      {
         sut.AnyOutputOfSimulationMapped(_anotherUsedSimulation).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_parameter_identification_does_not_use_the_simulation()
      {
         sut.AnyOutputOfSimulationMapped(_aSimulationNotUsed).ShouldBeFalse();
      }
   }

   public class When_updating_the_properties_from_a_source_parameter_identification : concern_for_ParameterIdentification
   {
      private ParameterIdentification _sourceParameterIdentification;
      private ICloneManager _cloneManager;
      private IdentificationParameter _identificationParameter;
      private IdentificationParameter _cloneOfIdentificationParameter;

      protected override void Context()
      {
         base.Context();
         _sourceParameterIdentification = new ParameterIdentification();
         _identificationParameter = new IdentificationParameter();
         _cloneOfIdentificationParameter = new IdentificationParameter();
         _sourceParameterIdentification.AddSimulation(_simulation);
         _sourceParameterIdentification.AddOutputMapping(_outputMapping);
         _sourceParameterIdentification.AddIdentificationParameter(_identificationParameter);

         _cloneManager = A.Fake<ICloneManager>();
         A.CallTo(() => _cloneManager.Clone(_identificationParameter)).Returns(_cloneOfIdentificationParameter);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceParameterIdentification, _cloneManager);
      }

      [Observation]
      public void should_add_the_simulation_references()
      {
         sut.AllSimulations.ShouldContain(_simulation);
      }

      [Observation]
      public void should_add_a_clone_of_all_output_mapping()
      {
         sut.AllOutputMappings.Count.ShouldBeEqualTo(1);
         sut.AllOutputMappings[0].ShouldNotBeEqualTo(_outputMapping);
      }

      [Observation]
      public void should_add_a_clone_of_all_identificaiton_parameters()
      {
         sut.AllIdentificationParameters.Count.ShouldBeEqualTo(1);
         sut.AllIdentificationParameters[0].ShouldBeEqualTo(_cloneOfIdentificationParameter);
      }
   }

   public class When_getting_an_observation_column_for_the_matching_result_column : concern_for_ParameterIdentification
   {
      private DataColumn _simulationColumn;
      private DataColumn _observationColumn1;
      private DataColumn _observationColumn2;

      protected override void Context()
      {
         base.Context();

         
         _simulationColumn = new DataColumn { QuantityInfo = new QuantityInfo("name", new[] { "Sim", "FullPath" }, QuantityType.Undefined) };

         _observationColumn1 = A.Fake<DataColumn>();
         _observationColumn1.DataInfo.Origin = ColumnOrigins.Observation;
         var dataRepository = new DataRepository { _observationColumn1, new BaseGrid("basegrid1", DomainHelperForSpecs.TimeDimensionForSpecs()) };
         var simulationQuantitySelection = new SimulationQuantitySelection(_simulation, new QuantitySelection("FullPath", QuantityType.Undefined));

         var outputMapping = new OutputMapping
         {
            OutputSelection = simulationQuantitySelection,
            WeightedObservedData = new WeightedObservedData(dataRepository)
         };
         sut.AddOutputMapping(outputMapping);

         _observationColumn2 = A.Fake<DataColumn>();
         _observationColumn2.DataInfo.Origin=ColumnOrigins.Observation;
         dataRepository = new DataRepository { _observationColumn2, new BaseGrid("basegrid2", DomainHelperForSpecs.TimeDimensionForSpecs()) };
         outputMapping = new OutputMapping
         {
            OutputSelection = simulationQuantitySelection,
            WeightedObservedData = new WeightedObservedData(dataRepository)
         };
         sut.AddOutputMapping(outputMapping);
      }

      [Observation]
      public void the_observation_columns_should_be_returned()
      {
         sut.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString).ShouldContain(_observationColumn1);
         sut.AllObservationColumnsFor(_simulationColumn.QuantityInfo.PathAsString).ShouldContain(_observationColumn2);
      }
   }
}