using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
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
      void MarkSimulationAsChanged();
   }

   public class SimulationOutputMappingPresenter : AbstractSubPresenter<ISimulationOutputMappingView, ISimulationOutputMappingPresenter>,
      ISimulationOutputMappingPresenter
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly ISimulationOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      private readonly IEventPublisher _eventPublisher;
      protected readonly IDialogCreator _dialogCreator;
      private readonly IOSPSuiteExecutionContext _executionContext;


      private readonly NotifyList<SimulationOutputMappingDTO> _listOfOutputMappingDTOs;

      private ISimulation _simulation;
      public bool IsLatched { get; set; }

      public SimulationOutputMappingPresenter(
         ISimulationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         ISimulationOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IEventPublisher eventPublisher,
         IDialogCreator dialogCreator,
         IOSPSuiteExecutionContext executionContext) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _eventPublisher = eventPublisher;
         _outputMappingMatchingTask = new OutputMappingMatchingTask(_entitiesInSimulationRetriever);
         _listOfOutputMappingDTOs = new NotifyList<SimulationOutputMappingDTO>();
         _dialogCreator = dialogCreator;
         _executionContext = executionContext;
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

            //add only a DTO with the observed data with Output == null
            var newOutputMapping = new OutputMapping
            {
               WeightedObservedData = new WeightedObservedData(observedData)
            };
            var newOutputMappingDTO = mapFrom(newOutputMapping);

            _listOfOutputMappingDTOs.Add(newOutputMappingDTO);
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

      public void UpdateSimulationOutputMappings(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         MarkSimulationAsChanged();
         if (!_simulation.OutputMappings.OutputMappingsUsingDataRepository(simulationOutputMappingDTO.ObservedData).Any())
            _simulation.OutputMappings.Add(simulationOutputMappingDTO.Mapping);

         simulationOutputMappingDTO.Scaling = _outputMappingMatchingTask.DefaultScalingFor(simulationOutputMappingDTO.Output.Quantity);
      }

      public void MarkSimulationAsChanged()
      {
         _simulation.HasChanged = true;
      }

      public void RemoveOutputMapping(SimulationOutputMappingDTO outputMappingDTO)
      {
         var parameterIdentifications = findParameterIdentificationsUsing(outputMappingDTO.ObservedData).ToList();
         if (parameterIdentifications.Any())
         {
            _dialogCreator.MessageBoxInfo(
               Captions.ParameterIdentification.CannotRemoveObservedDataBeingUsedByParameterIdentification(outputMappingDTO.ObservedData.Name,
                  parameterIdentifications.AllNames().ToList()));


            var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ReallyRemoveObservedDataFromSimulation);
            if (viewResult == ViewResult.No)
               return;
         }


         _simulation.RemoveUsedObservedData(outputMappingDTO.ObservedData);
         _simulation.OutputMappings.Remove(outputMappingDTO.Mapping);

         _eventPublisher.PublishEvent(new ObservedDataRemovedFromAnalysableEvent(_simulation, outputMappingDTO.ObservedData));
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(_simulation));

         _view.RefreshGrid();
      }

      private IEnumerable<ParameterIdentification> findParameterIdentificationsUsing(DataRepository dataRepository)
      {
         return from parameterIdentification in _executionContext.Project.AllParameterIdentifications
            let outputMappings = parameterIdentification.AllOutputMappingsFor(_simulation)
            where outputMappings.Any(x => x.UsesObservedData(dataRepository))
            select parameterIdentification;
      }

      public void Handle(ObservedDataAddedToAnalysableEvent eventToHandle)
      {
         Refresh();
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle)
      {
         updateOutputMappingList();
      }
   }
}