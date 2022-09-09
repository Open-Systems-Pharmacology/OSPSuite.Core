using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationOutputMappingPresenter : IPresenter<ISimulationOutputMappingView>, ILatchable, IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>
   {
      void SetSimulation(ISimulation simulation);
      IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      void RemoveOutputMapping(SimulationOutputMappingDTO outputMappingDTO);

      /// <summary>
      ///    Ensures that the cached list are updated
      /// </summary>
      void UpdateCache();

      /// <summary>
      ///    Completely rebinds the view to the content of the data source
      /// </summary>
      void Refresh();

      void InitializeSimulation(ISimulation simulation);
      void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO);
   }

   public class SimulationOutputMappingPresenter : AbstractSubPresenter<ISimulationOutputMappingView, ISimulationOutputMappingPresenter>,
      ISimulationOutputMappingPresenter
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly ISimulationOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();

      private readonly NotifyList<SimulationOutputMappingDTO> _listOfOutputMappingDTOs;

      private ISimulation _simulation;
      public bool IsLatched { get; set; }

      public SimulationOutputMappingPresenter(
         ISimulationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         ISimulationOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _listOfOutputMappingDTOs = new NotifyList<SimulationOutputMappingDTO>();
      }

      public void Refresh()
      {
         this.DoWithinLatch(() =>
         {
            UpdateCache();
            updateOutputMappingList();
            _view.BindTo(_listOfOutputMappingDTOs);
         });
      }

      public void UpdateCache()
      {
         _allAvailableOutputs.Clear();
      }

      public void SetSimulation(ISimulation simulation)
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
         foreach (var observedData in getAllAvailableObservedData())
         {
            if (_listOfOutputMappingDTOs.Any(x => x.ObservedData.Equals(observedData))) continue;

            var newOutputMapping = new OutputMapping();
            mapMatchingOutput(observedData, newOutputMapping);

            var newOutputMappingDTO = mapFrom(newOutputMapping);

            if (newOutputMapping.Output != null) 
               _simulation.OutputMappings.Add(newOutputMapping);

            _listOfOutputMappingDTOs.Add(newOutputMappingDTO);
         }
      }

      private void mapMatchingOutput(DataRepository observedData, OutputMapping newOutputMapping)
      {
         var pathCache = _entitiesInSimulationRetriever.OutputsFrom(_simulation);
         var matchingOutputPath = pathCache.Keys.FirstOrDefault(x => observedDataMatchesOutput(observedData, x));

         if (matchingOutputPath == null)
         {
            newOutputMapping.WeightedObservedData = new WeightedObservedData(observedData);
            return;
         }

         var matchingOutput = pathCache[matchingOutputPath];

         newOutputMapping.OutputSelection =
            new SimulationQuantitySelection(_simulation, new QuantitySelection(matchingOutputPath, matchingOutput.QuantityType));
         newOutputMapping.WeightedObservedData = new WeightedObservedData(observedData);
         newOutputMapping.Scaling = DefaultScalingFor(matchingOutput);
      }

      public IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs
      {
         get
         {
            var outputs = _entitiesInSimulationRetriever.OutputsFrom(_simulation);
            _allAvailableOutputs.Clear();
            _allAvailableOutputs.AddRange(outputs.Select(x => mapFrom(_simulation, x)).OrderBy(x => x.DisplayString));
            return _allAvailableOutputs;
         }
      }

      private SimulationQuantitySelectionDTO mapFrom(ISimulation simulation, IQuantity quantity)
      {
         return _simulationQuantitySelectionDTOMapper.MapFrom(simulation, quantity);
      }

      private SimulationOutputMappingDTO mapFrom(OutputMapping outputMapping)
      {
         return _outputMappingDTOMapper.MapFrom(outputMapping, AllAvailableOutputs);
      }

      private IEnumerable<DataRepository> getAllAvailableObservedData()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);
      }

      public void InitializeSimulation(ISimulation simulation)
      {
         _simulation = simulation;
      }

      public void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         if(!_simulation.OutputMappings.OutputMappingsUsingDataRepository(simulationOutputMappingDTO.ObservedData).Any())
            _simulation.OutputMappings.Add(simulationOutputMappingDTO.Mapping);
      }

      public void RemoveOutputMapping(SimulationOutputMappingDTO outputMappingDTO)
      {
         outputMappingDTO.Output = null;
         _simulation.OutputMappings.Remove(outputMappingDTO.Mapping);
         _view.RefreshGrid();
      }

      public void Handle(ObservedDataAddedToAnalysableEvent eventToHandle)
      {
         Refresh();
      }

      public Scalings DefaultScalingFor(IQuantity output)
      {
         return output.IsFraction() ? Scalings.Linear : Scalings.Log;
      }

      private bool observedDataMatchesOutput(DataRepository observedData, string outputPath)
      {
         var organ = observedData.ExtendedPropertyValueFor(Constants.ObservedData.ORGAN);
         var compartment = observedData.ExtendedPropertyValueFor(Constants.ObservedData.COMPARTMENT);
         var molecule = observedData.ExtendedPropertyValueFor(Constants.ObservedData.MOLECULE);

         if (organ == null || compartment == null || molecule == null)
            return false;

         return outputPath.Contains(organ) && outputPath.Contains(compartment) && outputPath.Contains(molecule);
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle)
      {
         foreach (var removedObservedData in eventToHandle.ObservedData)
         {
            var outputsMatchingDeletedObservedData = _simulation.OutputMappings.OutputMappingsUsingDataRepository(removedObservedData).ToList();
            foreach (var outputMapping in outputsMatchingDeletedObservedData)
            {
               _simulation.OutputMappings.Remove(outputMapping);
            }
         }

         updateOutputMappingList();
      }
   }
}