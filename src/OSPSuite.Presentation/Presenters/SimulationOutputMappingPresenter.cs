using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
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
   public interface ISimulationOutputMappingPresenter : IPresenter<ISimulationOutputMappingView>, ILatchable
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
   }

   public class SimulationOutputMappingPresenter : AbstractSubPresenter<ISimulationOutputMappingView, ISimulationOutputMappingPresenter>, ISimulationOutputMappingPresenter, IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>

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
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IParameterIdentificationTask parameterIdentificationTask) : base(view)
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
         var newOutputMapping = new OutputMapping();

         //get all available observed data, and create non-mapped DTOs for each one
         foreach (var observedData in getAllAvailableObservedData())
         {
            if (!_listOfOutputMappingDTOs.Any(x =>
               x.ObservedData.Id.Equals(observedData.Id))) //possibly a better way (or already existing way) to write this exists. 
            {

               //`==========================================

               var outputPaths = _entitiesInSimulationRetriever.OutputsFrom(_simulation).Keys;
               var matchingOutputPath = outputPaths.FirstOrDefault(x => observedDataMatchesOutput(observedData, x));

               if (matchingOutputPath != null)
               {
                  var matchingOutput = _entitiesInSimulationRetriever.OutputsFrom(_simulation)[matchingOutputPath];

                  newOutputMapping.OutputSelection =
                     new SimulationQuantitySelection(_simulation, new QuantitySelection(matchingOutputPath, matchingOutput.QuantityType));
                  newOutputMapping.WeightedObservedData = new WeightedObservedData(observedData);
                  newOutputMapping.Scaling = DefaultScalingFor(matchingOutput);
               }

               //`==========================================
               
               var newOutputMappingDTO = mapFrom(newOutputMapping);
               newOutputMappingDTO.ObservedData = observedData;

               _simulation.OutputMappings.Add(newOutputMapping);
               _listOfOutputMappingDTOs.Add(newOutputMappingDTO);
            }
         }
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

      public void RemoveOutputMapping(SimulationOutputMappingDTO outputMappingDTO)
      {
         outputMappingDTO.Output = null;
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
         //remove deleted observed data
         var outputsMatchingDeletedObservedData = _simulation.OutputMappings.All.Where(x => !getAllAvailableObservedData().Contains(x.WeightedObservedData.ObservedData)).ToList();

         foreach (var outputMapping in outputsMatchingDeletedObservedData)
         {
            _simulation.OutputMappings.Remove(outputMapping);
         }

         updateOutputMappingList();
      }
   }
}