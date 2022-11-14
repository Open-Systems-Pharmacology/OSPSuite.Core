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
      private readonly ISimulationOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      protected readonly IDialogCreator _dialogCreator;
      private readonly IOSPSuiteExecutionContext _context;


      private readonly NotifyList<SimulationOutputMappingDTO> _listOfOutputMappingDTOs;

      private ISimulation _simulation;
      public bool IsLatched { get; set; }

      public SimulationOutputMappingPresenter(
         ISimulationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         ISimulationOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IDialogCreator dialogCreator,
         IOSPSuiteExecutionContext context) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _outputMappingMatchingTask = new OutputMappingMatchingTask(_entitiesInSimulationRetriever);
         _listOfOutputMappingDTOs = new NotifyList<SimulationOutputMappingDTO>();
         _dialogCreator = dialogCreator;
         _context = context;
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
         var test = new SimulationQuantitySelectionDTO(_simulation, null, Captions.SimulationUI.NoneEditorNullText);
         _allAvailableOutputs.Add(test);

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
            removeOutputMapping(simulationOutputMappingDTO);
            return;
         }

         if (!_simulation.OutputMappings.OutputMappingsUsingDataRepository(simulationOutputMappingDTO.ObservedData).Any())
            _simulation.OutputMappings.Add(simulationOutputMappingDTO.Mapping);

         simulationOutputMappingDTO.Scaling = _outputMappingMatchingTask.DefaultScalingFor(simulationOutputMappingDTO.Output.Quantity);
      }

      private void removeOutputMapping(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         simulationOutputMappingDTO.Output = null;
         _simulation.OutputMappings.Remove(simulationOutputMappingDTO.Mapping);
         _view.RefreshGrid();
      }

      public void MarkSimulationAsChanged()
      {
         _simulation.HasChanged = true;
      }

      public void RemoveObservedData(SimulationOutputMappingDTO outputMappingDTO)
      {
         var parameterIdentifications = findParameterIdentificationsUsing(outputMappingDTO.ObservedData).ToList();
         if (parameterIdentifications.Any())
         {
            _dialogCreator.MessageBoxInfo(
               Captions.ParameterIdentification.CannotRemoveObservedDataBeingUsedByParameterIdentification(outputMappingDTO.ObservedData.Name,
                  parameterIdentifications.AllNames().ToList()));

            return;
         }

         var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ReallyRemoveObservedDataFromSimulation);
         if (viewResult == ViewResult.No)
            return;

         _simulation.RemoveUsedObservedData(outputMappingDTO.ObservedData);
         _simulation.OutputMappings.Remove(outputMappingDTO.Mapping);

         _context.PublishEvent(new ObservedDataRemovedFromAnalysableEvent(_simulation, outputMappingDTO.ObservedData));
         _context.PublishEvent(new SimulationStatusChangedEvent(_simulation));

         _view.RefreshGrid();
      }

      private IEnumerable<ParameterIdentification> findParameterIdentificationsUsing(DataRepository dataRepository)
      {
         return from parameterIdentification in _context.Project.AllParameterIdentifications
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

      public void Handle(SimulationOutputSelectionsChangedEvent eventToHandle)
      {
         if (Equals(eventToHandle.Simulation, _simulation))
            Refresh();
      }
   }
}