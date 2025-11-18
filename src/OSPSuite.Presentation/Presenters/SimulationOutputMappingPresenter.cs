using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationOutputMappingPresenter : IPresenter<ISimulationOutputMappingView>, ILatchable,
      IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>,
      IListener<SimulationOutputSelectionsChangedEvent>
   {
      void EditSimulation(ISimulation simulation);
      IReadOnlyList<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      void RemoveObservedData(IReadOnlyList<SimulationOutputMappingDTO> outputMappingDTOs);
      /// <summary>
      ///    Completely rebinds the view to the content of the data source
      /// </summary>
      void Refresh();

      void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO);
      void MarkSimulationAsChanged();
   }

   public class SimulationOutputMappingPresenter : AbstractSubPresenter<ISimulationOutputMappingView, ISimulationOutputMappingPresenter>,
      ISimulationOutputMappingPresenter
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IObservedDataTask _observedDataTask;
      private readonly ISimulationOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      private readonly IEventPublisher _eventPublisher;
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly SimulationQuantitySelectionDTO _noneEntry;

      private readonly NotifyList<SimulationOutputMappingDTO> _listOfOutputMappingDTOs;

      private ISimulation _simulation;
      public bool IsLatched { get; set; }

      public SimulationOutputMappingPresenter(ISimulationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         ISimulationOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IObservedDataTask observedDataTask,
         IEventPublisher eventPublisher, 
         IOutputMappingMatchingTask outputMappingMatchingTask, 
         IOSPSuiteExecutionContext executionContext) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _noneEntry = new SimulationQuantitySelectionDTO(null, null, Captions.SimulationUI.NoneEditorNullText);
         _eventPublisher = eventPublisher;
         _outputMappingMatchingTask = outputMappingMatchingTask;
         _executionContext = executionContext;
         _listOfOutputMappingDTOs = new NotifyList<SimulationOutputMappingDTO>();
         _observedDataTask = observedDataTask;
      }

      public void Refresh()
      {
         this.DoWithinLatch(() =>
         {
            updateOutputLists();
            updateOutputMappingList();
            _view.BindTo(_listOfOutputMappingDTOs);
         });
      }

      private void updateOutputLists()
      {
         _allAvailableOutputs.Clear();
         var outputs = _entitiesInSimulationRetriever.OutputsFrom(_simulation);
         _allAvailableOutputs.AddRange(outputs.Select(x => mapFrom(_simulation, x)).OrderBy(x => x.DisplayString));

         //adding a none DTO output, in order to be able to select it and delete the outputMapping
         _allAvailableOutputs.Add(_noneEntry);
      }

      public void EditSimulation(ISimulation simulation)
      {
         _simulation = simulation;
         Refresh();
      }


      private void removeFromOutputMappingList(IReadOnlyList<DataRepository> itemsToRemove)
      {
         var idsToRemove = new HashSet<string>(
             (itemsToRemove ?? Enumerable.Empty<DataRepository>())
             .Where(item => item != null)
             .Select(item => item.Id)
         );

         for (int i = _listOfOutputMappingDTOs.Count - 1; i >= 0; i--)
         {
            var dto = _listOfOutputMappingDTOs[i];
            var observedData = dto.ObservedData;

            if (observedData == null)
               continue;

            if (idsToRemove.Contains(observedData.Id))
               _listOfOutputMappingDTOs.RemoveAt(i);
         }

         var mappedIds = new HashSet<string>(
             _listOfOutputMappingDTOs
                 .Where(dto => dto.ObservedData != null)
                 .Select(dto => dto.ObservedData.Id)
         );

         foreach (var observedData in allAvailableObservedDataUsedBySimulation())
         {
            if (!mappedIds.Contains(observedData.Id))
            {
               var newOutputMapping = new OutputMapping
               {
                  WeightedObservedData = new WeightedObservedData(observedData)
               };
               var newOutputMappingDTO = mapFrom(newOutputMapping);
               _listOfOutputMappingDTOs.Add(newOutputMappingDTO);
            }
         }
      }

      private void updateOutputMappingList()
      {
         _listOfOutputMappingDTOs.Clear();

         //first add all the existing OutputMappings in the Simulation
         _simulation.OutputMappings.All.Each(x => _listOfOutputMappingDTOs.Add(mapFrom(x)));

         //get all available observed data, and create new mappings for the unmapped
         foreach (var observedData in allAvailableObservedDataUsedBySimulation())
         {
            if (_listOfOutputMappingDTOs.Any(x => x.ObservedData.Equals(observedData)))
               continue;

            //add only a DTO with the observed data with Output == null
            var newOutputMapping = new OutputMapping
            {
               WeightedObservedData = new WeightedObservedData(observedData)
            };
            var newOutputMappingDTO = mapFrom(newOutputMapping);

            _listOfOutputMappingDTOs.Add(newOutputMappingDTO);
         }
      }

      public IReadOnlyList<SimulationQuantitySelectionDTO> AllAvailableOutputs => _allAvailableOutputs;

      private SimulationQuantitySelectionDTO mapFrom(ISimulation simulation, IQuantity quantity) =>
         _simulationQuantitySelectionDTOMapper.MapFrom(simulation, quantity);

      private SimulationOutputMappingDTO mapFrom(OutputMapping outputMapping) => _outputMappingDTOMapper.MapFrom(outputMapping, AllAvailableOutputs);

      private IEnumerable<DataRepository> allAvailableObservedDataUsedBySimulation()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);
      }

      public void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         if (Equals(simulationOutputMappingDTO.Output, _noneEntry))
         {
            _simulation.RemoveOutputMappings(simulationOutputMappingDTO.ObservedData);
            _eventPublisher.PublishEvent(new SimulationOutputMappingsChangedEvent(_simulation));
            return;
         }

         if (!_simulation.OutputMappings.OutputMappingsUsingDataRepository(simulationOutputMappingDTO.ObservedData).Any())
            _simulation.OutputMappings.Add(simulationOutputMappingDTO.Mapping);

         simulationOutputMappingDTO.Scaling = _outputMappingMatchingTask.DefaultScalingFor(simulationOutputMappingDTO.Output.Quantity);
         MarkSimulationAsChanged();
      }

      public void MarkSimulationAsChanged()
      {
         _simulation.HasChanged = true;
         _executionContext.ProjectChanged();
         _eventPublisher.PublishEvent(new SimulationOutputMappingsChangedEvent(_simulation));
      }

      public void Handle(ObservedDataAddedToAnalysableEvent eventToHandle)
      {
         Refresh();
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle)
      {
         removeFromOutputMappingList(eventToHandle.ObservedData);
      }

      public void Handle(SimulationOutputSelectionsChangedEvent eventToHandle)
      {
         if (Equals(eventToHandle.Simulation, _simulation))
            Refresh();
      }

      public void RemoveObservedData(IReadOnlyList<SimulationOutputMappingDTO> outputMappingDTOs)
      {
         var usedObservedDataList = outputMappingDTOs
            .Select(outputMappingDTO => new UsedObservedData
            {
               Id = outputMappingDTO.ObservedData.Id,
               Simulation = _simulation
            })
            .ToList();


         _observedDataTask.RemoveUsedObservedDataFromSimulation(usedObservedDataList);
         MarkSimulationAsChanged();
      }
   }
}