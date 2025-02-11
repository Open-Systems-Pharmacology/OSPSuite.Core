using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimulationOutputMappingPresenter : ContextSpecification<ISimulationOutputMappingPresenter>
   {
      protected ISimulationOutputMappingView _view;
      private IObservedDataRepository _observedDataRepository;
      private IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private ISimulationOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      protected ISimulation _simulation1;
      protected DataRepository _observedData1;
      protected WeightedObservedData _weightedObservedData1;
      protected DataRepository _observedData2;
      protected WeightedObservedData _weightedObservedData2;
      protected SimulationQuantitySelectionDTO _output1;
      protected SimulationQuantitySelectionDTO _output2;
      protected IQuantity _quantity1;
      protected IEnumerable<SimulationOutputMappingDTO> _allOutputMappingDTOs;
      protected OutputMapping _outputMapping1;
      protected OutputMapping _outputMapping2;
      protected SimulationOutputMappingDTO _outputMappingDTO1;
      protected SimulationOutputMappingDTO _outputMappingDTO2;
      protected SimulationOutputMappingDTO _newOutputMappingDTO;
      protected IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      protected IObservedDataTask _observedDataTask;
      protected IEventPublisher _eventPublisher;
      protected IOutputMappingMatchingTask _outputMappingMatchingTask;
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _outputMappingMatchingTask = new OutputMappingMatchingTask(_entitiesInSimulationRetriever, _eventPublisher);
         _view = A.Fake<ISimulationOutputMappingView>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _outputMappingDTOMapper = A.Fake<ISimulationOutputMappingToOutputMappingDTOMapper>();
         _simulationQuantitySelectionDTOMapper = A.Fake<IQuantityToSimulationQuantitySelectionDTOMapper>();
         _observedDataTask = A.Fake<IObservedDataTask>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _executionContext= A.Fake<IOSPSuiteExecutionContext>();

         sut = new SimulationOutputMappingPresenter(_view, _entitiesInSimulationRetriever, _observedDataRepository, _outputMappingDTOMapper,
            _simulationQuantitySelectionDTOMapper, _observedDataTask, _eventPublisher, _outputMappingMatchingTask, _executionContext);


         _observedData1 = DomainHelperForSpecs.ObservedData("Obs1").WithName("Obs1");
         _weightedObservedData1 = new WeightedObservedData(_observedData1);
         _observedData2 = DomainHelperForSpecs.ObservedData("Obs2").WithName("Obs2");
         _weightedObservedData2 = new WeightedObservedData(_observedData2);
         _simulation1 = A.Fake<ISimulation>().WithId("Id1");

         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(_simulation1)).Returns(new[] { _observedData2 });

         _quantity1 = A.Fake<IQuantity>();
         _output1 = A.Fake<SimulationQuantitySelectionDTO>();
         A.CallTo(() => _output1.Simulation).Returns(_simulation1);
         _output2 = A.Fake<SimulationQuantitySelectionDTO>();
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(_simulation1)).Returns(new PathCache<IQuantity>(new EntityPathResolverForSpecs())
            { { "Liver_AA", _quantity1 } });

         A.CallTo(() => _simulationQuantitySelectionDTOMapper.MapFrom(_simulation1, _quantity1)).Returns(_output1);

         A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationOutputMappingDTO>>._))
            .Invokes(x => _allOutputMappingDTOs = x.GetArgument<IEnumerable<SimulationOutputMappingDTO>>(0));


         _outputMapping1 = A.Fake<OutputMapping>();
         _outputMapping2 = A.Fake<OutputMapping>();


         _outputMappingDTO1 = new SimulationOutputMappingDTO(_outputMapping1) { Output = _output1, ObservedData = _observedData1 };
         _outputMappingDTO2 = new SimulationOutputMappingDTO(_outputMapping2) { Output = _output2, ObservedData = _observedData2 };
         _newOutputMappingDTO = new SimulationOutputMappingDTO(new OutputMapping());

         A.CallTo(() => _simulation1.OutputMappings.OutputMappingsUsingDataRepository(_observedData1))
            .Returns(new List<OutputMapping>() { _outputMapping1 });
         A.CallTo(() => _simulation1.OutputMappings.OutputMappingsUsingDataRepository(_observedData2))
            .Returns(new List<OutputMapping>() { _outputMapping2 });
         A.CallTo(() => _outputMappingDTOMapper.MapFrom(_outputMapping1, A<IReadOnlyList<SimulationQuantitySelectionDTO>>._))
            .Returns(_outputMappingDTO1);
         A.CallTo(() => _outputMappingDTOMapper.MapFrom(_outputMapping2, A<IReadOnlyList<SimulationQuantitySelectionDTO>>._))
            .Returns(_outputMappingDTO2);
         A.CallTo(() => _outputMappingDTOMapper.MapFrom(new OutputMapping(), A<IReadOnlyList<SimulationQuantitySelectionDTO>>._))
            .Returns(_newOutputMappingDTO);
      }
   }

   public class When_retrieving_the_list_of_all_available_outputs_from_a_simulation : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Because()
      {
         sut.EditSimulation(_simulation1);
      }

      [Observation]
      public void should_return_the_distinct_list_of_all_outputs_in_the_simulation()
      {
         sut.AllAvailableOutputs.ShouldContain(_output1);
      }

      [Observation]
      public void should_contain_an_empty_output_for_none_()
      {
         sut.AllAvailableOutputs.Count(output => output.DisplayString.Equals(Captions.SimulationUI.NoneEditorNullText)).ShouldBeEqualTo(1);
      }
   }

   public class When_loading_a_simulation_with_existing_output_mapping : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping2);
      }

      protected override void Because()
      {
         sut.EditSimulation(_simulation1);
      }

      [Observation]
      public void should_show_the_existing_mapping_to_the_user()
      {
         _allOutputMappingDTOs.Count().ShouldBeEqualTo(1);
         _allOutputMappingDTOs.ElementAt(0).ShouldBeEqualTo(_outputMappingDTO2);
      }
   }

   public class When_loading_a_simulation_with_existing_output_mapping_and_unmapped_observed_data_without_matching_output :
      concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping1);
      }

      protected override void Because()
      {
         sut.EditSimulation(_simulation1);
      }

      [Observation]
      public void should_show_the_existing_mapping_and_also_add_a_new_OutputMappingDTO()
      {
         _allOutputMappingDTOs.Count().ShouldBeEqualTo(2);
         _allOutputMappingDTOs.ElementAt(0).ShouldBeEqualTo(_outputMappingDTO1);
         _simulation1.OutputMappings.All.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_simulation_output_selections_have_changed : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSimulation(_simulation1);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationOutputSelectionsChangedEvent(_simulation1));
      }

      [Observation]
      public void should_have_removed_the_corresponding_output_mapping()
      {
         A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationOutputMappingDTO>>._)).MustHaveHappened(2, Times.Exactly);
      }
   }

   public class When_simulation_observed_data_gets_deleted : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping1);
         _simulation1.OutputMappings.Add(_outputMapping2);
         sut.EditSimulation(_simulation1);
         _simulation1.OutputMappings.Remove(_outputMapping1);
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataRemovedFromAnalysableEvent(_simulation1, _observedData1));
      }

      [Observation]
      public void should_have_removed_the_corresponding_output_mapping()
      {
         _allOutputMappingDTOs.Count().ShouldBeEqualTo(1);
         _allOutputMappingDTOs.ElementAt(0).ShouldBeEqualTo(_outputMappingDTO2);
      }
   }

   public class When_new_output_gets_mapped : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping1);
         _simulation1.OutputMappings.All[0].Scaling = Scalings.Linear;
         sut.EditSimulation(_simulation1);
      }

      protected override void Because()
      {
         sut.UpdateSimulationOutputMappings(_outputMappingDTO1);
      }

      [Observation]
      public void should_have_removed_the_corresponding_output_mapping()
      {
         _simulation1.OutputMappings.All[0].Scaling.ShouldBeEqualTo(Scalings.Log);
      }

      [Observation]
      public void the_event_for_output_mapping_changes_must_have_been_published()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationOutputMappingsChangedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_mark_the_project_as_changed()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_removing_observed_data : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping1);
         _simulation1.OutputMappings.All[0].Scaling = Scalings.Linear;
         sut.EditSimulation(_simulation1);
         sut.RemoveObservedData(_outputMappingDTO1);
      }

      [Observation]
      public void should_have_called_the_service_to_remove_the_corresponding_observed_data()
      {
         A.CallTo(() => _observedDataTask.RemoveUsedObservedDataFromSimulation(A<IReadOnlyList<UsedObservedData>>.That.Matches(args => args.Any(x => x.Id.Equals(_outputMappingDTO1.ObservedData.Id))))).MustHaveHappened();
      }
   }

   public class When_output_gets_removed : concern_for_SimulationOutputMappingPresenter
   {
      protected override void Context()
      {
         base.Context();
         _simulation1.OutputMappings.Add(_outputMapping1);
         _simulation1.OutputMappings.All[0].Scaling = Scalings.Linear;
         sut.EditSimulation(_simulation1);
         //we know we add the none entry at the end of the list by construction
         //Setting the no output selection
         _outputMappingDTO1.Output = sut.AllAvailableOutputs.Last();
      }

      protected override void Because()
      {
         sut.UpdateSimulationOutputMappings(_outputMappingDTO1);
      }

      [Observation]
      public void should_have_removed_the_corresponding_output_mapping()
      {
         A.CallTo(() => _simulation1.RemoveOutputMappings(_outputMappingDTO1.ObservedData)).MustHaveHappened();
      }
   }
}