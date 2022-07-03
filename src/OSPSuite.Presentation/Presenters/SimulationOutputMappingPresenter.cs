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
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
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
      void AddOutputMapping();
      IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      IEnumerable<DataRepository> AllObservedDataFor(SimulationOutputMappingDTO dto);
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

   public class SimulationOutputMappingPresenter : AbstractPresenter<ISimulationOutputMappingView, ISimulationOutputMappingPresenter>, ISimulationOutputMappingPresenter, IListener<ObservedDataAddedToAnalysableEvent>,
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

      public void AddOutputMapping()
      {
         var newOutputMapping = new OutputMapping();
         //_allOutputMappingDTOs.Add(mapFrom(newOutputMapping));  
         OnStatusChanged();
      }

      private void updateOutputMappingList()
      {
         _listOfOutputMappingDTOs.Clear();

         //KEEP SHIT SIMPLE: just add the observed data as mappings to the DTO list (hence also the simulation.OutputMappings)
//and for the observed data, if you do not find it, just add it. - then we simply have to handle this latter on when using those

         //addOutputMappingsFor(_simulation);
         //first map all the existing OutputMappings in the Simulation
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
                  newOutputMapping = new OutputMapping //  probably not this way, just initialize if we need
                  {
                     OutputSelection = new SimulationQuantitySelection(_simulation, new QuantitySelection(matchingOutputPath, matchingOutput.QuantityType)),
                     WeightedObservedData = new WeightedObservedData(observedData),
                     Scaling = DefaultScalingFor(matchingOutput)
                  };
               }

               //`==========================================
               
               var newOutputMappingDTO = mapFrom(newOutputMapping);
               newOutputMappingDTO.ObservedData = observedData;

               var weightedObservedData = new WeightedObservedData(observedData);
               newOutputMapping.WeightedObservedData = weightedObservedData;
               newOutputMappingDTO.ObservedData = observedData;
               //var newOutputMappingDTO = mapFrom(newOutputMapping);
               _simulation.OutputMappings.Add(newOutputMapping);
               _listOfOutputMappingDTOs.Add(newOutputMappingDTO);


            }
         }

         //IMPORTANT: WHAT IF THIS HERE IS CALLED WHEN OBSERVED DATA GETS UPDATED??? WE ARE SUPPOSED TO TRY TO MAP IT AUTOMATICALLY
         //SHOULD WE KEEP A LIST OF WHAT GOT ADDED? LETS HANDLE THIS SEPARATELY, ITS OK FOR NOW
      }

      public IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs
      {
         get
         { 
            var outputs = _entitiesInSimulationRetriever.OutputsFrom(_simulation);
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


      //probably will have to delete
      public virtual IEnumerable<DataRepository> AllObservedDataFor(SimulationOutputMappingDTO outputMappingDTO)
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);

         //return new List<DataRepository>(); //we are returning an empty list here, the implementations in PK-Sim and MoBi will provide the actual data
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

      private void addOutputMappingsFor(ISimulation simulation)
      {
         foreach (var keyValue in _entitiesInSimulationRetriever.OutputsFrom(simulation).KeyValues)
         {
            var output = keyValue.Value;
            var outputPath = keyValue.Key;
            observedDataInSimulationMatchingOutputs(outputPath, simulation).Each(obs => addOutputMapping(output, outputPath, obs, simulation));
         }
      }

      private void addOutputMapping(IQuantity output, string outputPath, DataRepository observedData, ISimulation simulation)
      {
         _simulation.OutputMappings.Add(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(simulation, new QuantitySelection(outputPath, output.QuantityType)),
            WeightedObservedData = new WeightedObservedData(observedData),
            Scaling = DefaultScalingFor(output)
         });
      }
      public Scalings DefaultScalingFor(IQuantity output)
      {
         return output.IsFraction() ? Scalings.Linear : Scalings.Log;
      }
      private IEnumerable<DataRepository> observedDataInSimulationMatchingOutputs(string outputPath, ISimulation simulation)
      {
         var allObservedDataUsedByTheSimulation = _observedDataRepository.AllObservedDataUsedBy(simulation);
         return allObservedDataUsedByTheSimulation.Where(obs => observedDataMatchesOutput(obs, outputPath));
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

      //So this is a better question let's say. What happens actually when observed data has been removed?
      //I dont think this is specified. Even I do not think this was thought of: probably we will have to remove
      //the corresponding mapping I would say. This is also an extra reason why we would need to keep the tree nodes
      //under the Simulation (at least in PK-sIM): Is this even doable in MoBi? 
      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle) 
      {
         throw new System.NotImplementedException();
      }
   }
}