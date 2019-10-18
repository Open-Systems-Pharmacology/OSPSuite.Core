using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationOutputMappingPresenter : ContextSpecification<IParameterIdentificationOutputMappingPresenter>
   {
      private IParameterIdentificationOutputMappingView _view;
      private IObservedDataRepository _observedDataRepository;
      private IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private IOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation1;
      private UsedObservedData _usedObservedData1;
      private UsedObservedData _usedObservedData2;
      protected ISimulation _simulation2;
      protected DataRepository _observedData1;
      protected WeightedObservedData _weightedObservedData1;
      protected DataRepository _observedData2;
      protected WeightedObservedData _weightedObservedData2;
      protected SimulationQuantitySelectionDTO _output1;
      protected SimulationQuantitySelectionDTO _output2;
      protected IQuantity _quantity1;
      private IQuantity _quantity2;
      protected IEnumerable<OutputMappingDTO> _allOutputMappingDTOs;
      protected OutputMapping _outputMapping1;
      protected OutputMapping _outputMapping2;
      protected OutputMappingDTO _outputMappingDTO1;
      protected OutputMappingDTO _outputMappingDTO2;
      private IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      protected IParameterIdentificationTask _parameterIdentificationTask;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationOutputMappingView>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _outputMappingDTOMapper = A.Fake<IOutputMappingToOutputMappingDTOMapper>();
         _simulationQuantitySelectionDTOMapper = A.Fake<IQuantityToSimulationQuantitySelectionDTOMapper>();
         _parameterIdentificationTask= A.Fake<IParameterIdentificationTask>();

         sut = new ParameterIdentificationOutputMappingPresenter(_view, _entitiesInSimulationRetriever, _observedDataRepository, _outputMappingDTOMapper,
            _simulationQuantitySelectionDTOMapper, _parameterIdentificationTask);


         _observedData1 = DomainHelperForSpecs.ObservedData("Obs1").WithName("Obs1");
         _weightedObservedData1 = new WeightedObservedData(_observedData1);
         _observedData2 = DomainHelperForSpecs.ObservedData("Obs2").WithName("Obs2");
         _weightedObservedData2 = new WeightedObservedData(_observedData2);
         _parameterIdentification = new ParameterIdentification();
         _simulation1 = A.Fake<ISimulation>().WithId("Id1");
         _simulation2 = A.Fake<ISimulation>().WithId("Id2");
         _usedObservedData1 = new UsedObservedData {Id = "Obs1"};
         _usedObservedData2 = new UsedObservedData {Id = "Obs2"};
         A.CallTo(() => _observedDataRepository.All()).Returns(new[] {_observedData1, _observedData2});

         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(_simulation1)).Returns(new[] { _observedData2 });
         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(_simulation2)).Returns(new[] { _observedData1, _observedData2 });

         _parameterIdentification.AddSimulation(_simulation1);
         _parameterIdentification.AddSimulation(_simulation2);

         _quantity1 = A.Fake<IQuantity>();
         _quantity2 = A.Fake<IQuantity>();
         _output1 = A.Fake<SimulationQuantitySelectionDTO>();
         A.CallTo(() => _output1.Simulation).Returns(_simulation1);
         _output2 = A.Fake<SimulationQuantitySelectionDTO>();
         A.CallTo(() => _output2.Simulation).Returns(_simulation2);
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(_simulation1)).Returns(new PathCache<IQuantity>(new EntityPathResolverForSpecs()) {{"AA", _quantity1}});
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(_simulation2)).Returns(new PathCache<IQuantity>(new EntityPathResolverForSpecs()) {{"BB", _quantity2}});
         A.CallTo(() => _simulationQuantitySelectionDTOMapper.MapFrom(_simulation1, _quantity1)).Returns(_output1);
         A.CallTo(() => _simulationQuantitySelectionDTOMapper.MapFrom(_simulation2, _quantity2)).Returns(_output2);

         A.CallTo(() => _view.BindTo(A<IEnumerable<OutputMappingDTO>>._))
            .Invokes(x => _allOutputMappingDTOs = x.GetArgument<IEnumerable<OutputMappingDTO>>(0));


         sut.EditParameterIdentification(_parameterIdentification);

         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();

         _outputMappingDTO1 = new OutputMappingDTO(_outputMapping1) {Output = _output1};
         _outputMappingDTO2 = new OutputMappingDTO(_outputMapping2) {Output = _output2};

         A.CallTo(() => _outputMapping1.Simulation).Returns(_simulation1);
         A.CallTo(() => _outputMapping2.Simulation).Returns(_simulation2);

         A.CallTo(() => _outputMappingDTOMapper.MapFrom(_outputMapping1, A<IEnumerable<SimulationQuantitySelectionDTO>>._)).Returns(_outputMappingDTO1);
         A.CallTo(() => _outputMappingDTOMapper.MapFrom(_outputMapping2, A<IEnumerable<SimulationQuantitySelectionDTO>>._)).Returns(_outputMappingDTO2);
      }
   }

   public class When_retrieving_the_list_of_all_available_observed_data_from_a_parameter_identification : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      [Observation]
      public void should_return_the_distinct_list_of_observed_data_used_accross_all_simulations_used_in_the_PI()
      {
         sut.AllObservedDataFor(_outputMappingDTO1).ShouldOnlyContainInOrder(_observedData2);
         sut.AllObservedDataFor(_outputMappingDTO2).ShouldOnlyContainInOrder(_observedData1,_observedData2);
      }
   }

   public class When_retrieving_the_list_of_all_available_outputs_from_a_parameter_identification : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      [Observation]
      public void should_return_the_distinct_list_of_all_outputs_accross_all_simulations_used_in_the_PI()
      {
         sut.AllAvailableOutputs.ShouldOnlyContainInOrder(_output1, _output2);
      }
   }

   public class When_adding_a_new_output_mapping_to_the_editied_parameter_identification : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      protected override void Because()
      {
         sut.AddOutputMapping();
      }

      [Observation]
      public void should_add_a_new_output_mapping()
      {
         _parameterIdentification.AllOutputMappings.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_update_the_view()
      {
         _allOutputMappingDTOs.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_loading_a_parameter_identification_with_existing_output_mapping : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();

         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_show_the_existing_mapping_to_the_user()
      {
         _allOutputMappingDTOs.Count().ShouldBeEqualTo(2);
         _allOutputMappingDTOs.ElementAt(0).ShouldBeEqualTo(_outputMappingDTO1);
         _allOutputMappingDTOs.ElementAt(1).ShouldBeEqualTo(_outputMappingDTO2);
      }
   }

   public class When_the_user_is_mapping_an_output_not_mapped_with_any_previous_data_with_observed_data_not_used_yet : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      private WeightedObservedData _eventObservedData;
      private bool _unmappedRaised;

      protected override void Context()
      {
         base.Context();
         _outputMapping1.WeightedObservedData = null;
         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);
         sut.EditParameterIdentification(_parameterIdentification);

         sut.ObservedDataMapped += (o, e) => { _eventObservedData = e.WeightedObservedData; };
         sut.ObservedDataUnmapped += (o, e) => { _unmappedRaised = true; };
      }

      protected override void Because()
      {
         sut.ObservedDataSelectionChanged(_outputMappingDTO1, _weightedObservedData1, null);
      }

      [Observation]
      public void should_set_the_observed_data_in_the_output()
      {
         _outputMapping1.WeightedObservedData.ObservedData.ShouldBeEqualTo(_observedData1);
      }

      [Observation]
      public void should_notify_the_observed_data_mapped_event()
      {
         _eventObservedData.ObservedData.ShouldBeEqualTo(_observedData1);
      }

      [Observation]
      public void should_not_notify_the_observed_data_unmapped_event()
      {
         _unmappedRaised.ShouldBeFalse();
      }
   }

   public class When_the_user_is_mapping_an_output_mapped_with_previous_data_with_observed_data_not_used_yet : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      private DataRepository _eventObservedData;
      private bool _unmappedRaised;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);
         sut.EditParameterIdentification(_parameterIdentification);
         sut.ObservedDataMapped += (o, e) => { _eventObservedData = e.WeightedObservedData; };
         sut.ObservedDataUnmapped += (o, e) => { _unmappedRaised = true; };
      }

      protected override void Because()
      {
         sut.ObservedDataSelectionChanged(_outputMappingDTO1, _observedData1, _observedData2);
      }

      [Observation]
      public void should_set_the_observed_data_in_the_output()
      {
         _outputMapping1.WeightedObservedData.ObservedData.ShouldBeEqualTo(_observedData1);
      }

      [Observation]
      public void should_notify_the_observed_data_mapped_event()
      {
         _eventObservedData.ShouldBeEqualTo(_observedData1);
      }

      [Observation]
      public void should_notify_the_observed_data_unmapped_event()
      {
         _unmappedRaised.ShouldBeTrue();
      }
   }

   public class When_the_user_is_mapping_an_output_representing_a_fraction : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentificationTask.DefaultScalingFor(_output2.Quantity)).Returns(Scalings.Linear);
      }

      protected override void Because()
      {
         sut.OutputSelectionChanged(_outputMappingDTO1, _output2, _output1);
      }

      [Observation]
      public void should_set_the_scaling_to_linear()
      {
         _outputMappingDTO1.Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_the_user_is_mapping_an_output_representing_a_concentration : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentificationTask.DefaultScalingFor(_output2.Quantity)).Returns(Scalings.Log);
      }

      protected override void Because()
      {
         sut.OutputSelectionChanged(_outputMappingDTO1, _output2, _output1);
      }

      [Observation]
      public void should_set_the_scaling_to_log()
      {
         _outputMappingDTO1.Scaling.ShouldBeEqualTo(Scalings.Log);
      }
   }

   public class When_the_user_is_mapping_an_output_with_observed_data_already_used_by_another_output : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      private DataRepository _eventObservedData;
      private bool _unmappedRaised;

      protected override void Context()
      {
         base.Context();
         _outputMapping1.WeightedObservedData = _weightedObservedData1;
         _outputMapping2.WeightedObservedData = _weightedObservedData2;
         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);
         sut.EditParameterIdentification(_parameterIdentification);

         //mimic binding behavior. Object is set and then method is called
         _outputMappingDTO1.ObservedData = _observedData1;
         _outputMappingDTO2.ObservedData = _observedData1;

         sut.ObservedDataMapped += (o, e) => { _eventObservedData = e.WeightedObservedData; };
         sut.ObservedDataUnmapped += (o, e) => { _unmappedRaised = true; };
      }

      protected override void Because()
      {
         sut.ObservedDataSelectionChanged(_outputMappingDTO2, _observedData1, _weightedObservedData2.ObservedData);
      }

      [Observation]
      public void should_have_updated_the_observed_data()
      {
         _outputMappingDTO2.ObservedData.ShouldBeEqualTo(_observedData1);
         _eventObservedData.ShouldNotBeNull();
         _unmappedRaised.ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_a_unique_mapping_id_to_ensure_display_unicity()
      {
         _outputMappingDTO2.WeightedObservedData.Id.ShouldBeEqualTo(1);
      }
   }

   public class When_the_user_is_mapping_an_output_with_observed_data_already_used_by_the_same_output : concern_for_ParameterIdentificationOutputMappingPresenter
   {
      private DataRepository _eventObservedData;
      private bool _unmappedRaised;

      protected override void Context()
      {
         base.Context();
         _outputMapping1.WeightedObservedData = _weightedObservedData1;
         _outputMapping2.WeightedObservedData = _weightedObservedData2;
         _parameterIdentification.AddOutputMapping(_outputMapping1);
         _parameterIdentification.AddOutputMapping(_outputMapping2);
         sut.EditParameterIdentification(_parameterIdentification);

         _outputMappingDTO1.Output = _output1;
         _outputMappingDTO2.Output = _output1;
         //mimic binding behavior. Object is set and then method is called
         _outputMappingDTO1.ObservedData = _observedData1;
         _outputMappingDTO2.ObservedData = _observedData1;

         sut.ObservedDataMapped += (o, e) => { _eventObservedData = e.WeightedObservedData; };
         sut.ObservedDataUnmapped += (o, e) => { _unmappedRaised = true; };
      }

      [Observation]
      public void should_reset_the_observed_data_in_the_output_and_not_raise_any_event()
      {
         The.Action(() => sut.ObservedDataSelectionChanged(_outputMappingDTO2, _observedData1, _observedData2)).ShouldThrowAn<CannotSelectTheObservedDataMoreThanOnceException>();
         _outputMappingDTO2.ObservedData.ShouldBeEqualTo(_observedData2);
         _outputMapping2.WeightedObservedData.ShouldBeEqualTo(_weightedObservedData2);
         _eventObservedData.ShouldBeNull();
         _unmappedRaised.ShouldBeFalse();
      }
   }
}