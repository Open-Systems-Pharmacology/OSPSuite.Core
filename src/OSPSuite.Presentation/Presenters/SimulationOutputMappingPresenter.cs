﻿using System.Collections.Generic;
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
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationOutputMappingPresenter : IPresenter<ISimulationOutputMappingView>, ILatchable,
      IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>,
      IListener<SimulationOutputSelectionsChangedEvent>
   {
      void EditSimulation(ISimulation simulation);
      IReadOnlyList<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      void RemoveObservedData(SimulationOutputMappingDTO outputMappingDTO);

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
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      private readonly SimulationQuantitySelectionDTO _noneEntry;

      private readonly NotifyList<SimulationOutputMappingDTO> _listOfOutputMappingDTOs;

      private ISimulation _simulation;
      public bool IsLatched { get; set; }

      public SimulationOutputMappingPresenter(
         ISimulationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         ISimulationOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IObservedDataTask observedDataTask) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _noneEntry = new SimulationQuantitySelectionDTO(null, null, Captions.SimulationUI.NoneEditorNullText);
         _outputMappingMatchingTask = new OutputMappingMatchingTask(_entitiesInSimulationRetriever);
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

      private SimulationQuantitySelectionDTO mapFrom(ISimulation simulation, IQuantity quantity) => _simulationQuantitySelectionDTOMapper.MapFrom(simulation, quantity);

      private SimulationOutputMappingDTO mapFrom(OutputMapping outputMapping) => _outputMappingDTOMapper.MapFrom(outputMapping, AllAvailableOutputs);

      private IEnumerable<DataRepository> allAvailableObservedDataUsedBySimulation()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);
      }

      public void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         MarkSimulationAsChanged();

         if (simulationOutputMappingDTO.Output.DisplayString.Equals(Captions.SimulationUI.NoneEditorNullText))
         {
            _simulation.RemoveOutputMappings(simulationOutputMappingDTO.ObservedData);
            //removeOutputMapping(simulationOutputMappingDTO);
            return;
         }

         if (!_simulation.OutputMappings.OutputMappingsUsingDataRepository(simulationOutputMappingDTO.ObservedData).Any())
            _simulation.OutputMappings.Add(simulationOutputMappingDTO.Mapping);

         simulationOutputMappingDTO.Scaling = _outputMappingMatchingTask.DefaultScalingFor(simulationOutputMappingDTO.Output.Quantity);
      }

      //HAVE TO TEST THIS CHANGE OF HANDLING THE DELETION OF OutputMapping
      private void removeOutputMapping(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         _simulation.OutputMappings.Remove(simulationOutputMappingDTO.Mapping);
         _view.RefreshGrid();
      }

      public void MarkSimulationAsChanged()
      {
         _simulation.HasChanged = true;
      }

      public void RemoveObservedData(SimulationOutputMappingDTO outputMappingDTO)
      {
         var usedObservedData = new UsedObservedData
         {
            Id = outputMappingDTO.ObservedData.Id,
            Simulation = _simulation
         };

         _observedDataTask.RemoveUsedObservedDataFromSimulation(new List<UsedObservedData>() {usedObservedData});

         //TEST IF WE NEED THIS
         _view.RefreshGrid();
      }


      public void Handle(ObservedDataAddedToAnalysableEvent eventToHandle)
      {
         Refresh();
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle)
      {
         updateOutputMappingList();
      }

      public void Handle(SimulationOutputSelectionsChangedEvent eventToHandle)
      {
         if (Equals(eventToHandle.Simulation, _simulation))
            Refresh();
      }
   }
}