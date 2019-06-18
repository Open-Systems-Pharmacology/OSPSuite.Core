using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class ObservedDataEventArgs : EventArgs
   {
      public WeightedObservedData WeightedObservedData { get; }

      public ObservedDataEventArgs(WeightedObservedData weightedObservedData)
      {
         WeightedObservedData = weightedObservedData;
      }
   }

   public interface IParameterIdentificationOutputMappingPresenter : IPresenter<IParameterIdentificationOutputMappingView>, IParameterIdentificationPresenter, ILatchable
   {
      void AddOutputMapping();
      IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      IEnumerable<DataRepository> AllObservedDataFor(OutputMappingDTO dto);
      void ObservedDataSelectionChanged(OutputMappingDTO dto, DataRepository newObservedData, DataRepository oldObservedData);
      void RemoveOutputMapping(OutputMappingDTO outputMappingDTO);
      event EventHandler<ObservedDataEventArgs> ObservedDataMapped;
      event EventHandler<ObservedDataEventArgs> ObservedDataUnmapped;
      event EventHandler<ObservedDataEventArgs> ObservedDataSelected;

      /// <summary>
      ///    Ensures that the cached list are updated (usuful when a simulation was added to the PI)
      /// </summary>
      void UpdateCache();

      /// <summary>
      ///    Completely rebinds the view to the content of the data sourcee
      /// </summary>
      void Refresh();

      void OutputSelectionChanged(OutputMappingDTO dto, SimulationQuantitySelectionDTO newOutput, SimulationQuantitySelectionDTO oldOutput);
      void Select(OutputMappingDTO outputMappingDTO);
   }

   public class ParameterIdentificationOutputMappingPresenter : AbstractPresenter<IParameterIdentificationOutputMappingView, IParameterIdentificationOutputMappingPresenter>, IParameterIdentificationOutputMappingPresenter
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IOutputMappingToOutputMappingDTOMapper _outputMappingDTOMapper;
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionDTOMapper;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private ParameterIdentification _parameterIdentification;
      private readonly List<SimulationQuantitySelectionDTO> _allAvailableOutputs = new List<SimulationQuantitySelectionDTO>();

      private readonly NotifyList<OutputMappingDTO> _allOutputMappingDTOs;
      public event EventHandler<ObservedDataEventArgs> ObservedDataMapped = delegate { };
      public event EventHandler<ObservedDataEventArgs> ObservedDataUnmapped = delegate { };
      public event EventHandler<ObservedDataEventArgs> ObservedDataSelected = delegate { };
      public bool IsLatched { get; set; }

      public ParameterIdentificationOutputMappingPresenter(
         IParameterIdentificationOutputMappingView view,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         IOutputMappingToOutputMappingDTOMapper outputMappingDTOMapper,
         IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionDTOMapper,
         IParameterIdentificationTask parameterIdentificationTask) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _outputMappingDTOMapper = outputMappingDTOMapper;
         _simulationQuantitySelectionDTOMapper = simulationQuantitySelectionDTOMapper;
         _parameterIdentificationTask = parameterIdentificationTask;
         _allOutputMappingDTOs = new NotifyList<OutputMappingDTO>();
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         Refresh();
      }

      public void Refresh()
      {
         this.DoWithinLatch(() =>
         {
            UpdateCache();
            updateOutputMapppingList();
            _view.BindTo(_allOutputMappingDTOs);
         });
      }

      public void UpdateCache()
      {
         _allAvailableOutputs.Clear();
      }

      public void AddOutputMapping()
      {
         var newOutputMapping = new OutputMapping();
         _parameterIdentification.AddOutputMapping(newOutputMapping);
         _allOutputMappingDTOs.Add(mapFrom(newOutputMapping));
         OnStatusChanged();
      }

      private void updateOutputMapppingList()
      {
         _allOutputMappingDTOs.Clear();
         _parameterIdentification.AllOutputMappings.Each(x => _allOutputMappingDTOs.Add(mapFrom(x)));
      }

      public IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs
      {
         get
         {
            if (!_allAvailableOutputs.Any())
            {
               _parameterIdentification.AllSimulations.Each(sim =>
               {
                  var outputs = _entitiesInSimulationRetriever.OutputsFrom(sim);
                  _allAvailableOutputs.AddRange(outputs.Select(x => mapFrom(sim, x)).OrderBy(x => x.DisplayString));
               });
            }

            return _allAvailableOutputs;
         }
      }

      private SimulationQuantitySelectionDTO mapFrom(ISimulation simulation, IQuantity quantity)
      {
         return _simulationQuantitySelectionDTOMapper.MapFrom(simulation, quantity);
      }

      private OutputMappingDTO mapFrom(OutputMapping outputMapping)
      {
         return _outputMappingDTOMapper.MapFrom(outputMapping, AllAvailableOutputs);
      }

      public IEnumerable<DataRepository> AllObservedDataFor(OutputMappingDTO outputMappingDTO)
      {
         return allPossibleObservedDataForOutput(outputMappingDTO.Output);
      }

      private IEnumerable<DataRepository> allPossibleObservedDataForOutput(SimulationQuantitySelectionDTO outputSelectionDTO)
      {
         if (outputSelectionDTO == null)
            return Enumerable.Empty<DataRepository>();

         return _observedDataRepository.AllObservedDataUsedBy(outputSelectionDTO.Simulation)
            .Distinct()
            .OrderBy(x => x.Name);
      }

      public void ObservedDataSelectionChanged(OutputMappingDTO dto, DataRepository newObservedData, DataRepository oldObservedData)
      {
         var allOutputsUsingObservedData = _allOutputMappingDTOs.Where(x => Equals(x.ObservedData, newObservedData)).Except(new[] {dto}).ToList();

         if (observedDataAlreadySelectedForSameOutput(dto.Output, newObservedData))
         {
            dto.ObservedData = oldObservedData;
            View.CloseEditor();
            throw new CannotSelectTheObservedDataMoreThanOnceException(newObservedData);
         }

         var weightedObservedData = new WeightedObservedData(newObservedData)
         {
            Id = nextUniqueIdFor(allOutputsUsingObservedData)
         };

         raiseObservedDataUnmappedFor(dto.WeightedObservedData);
         dto.Mapping.WeightedObservedData = weightedObservedData;
         View.CloseEditor();
         raiseObservedDataMappedFor(dto.WeightedObservedData);
      }

      public int? nextUniqueIdFor(List<OutputMappingDTO> outputMappings)
      {
         if (!outputMappings.Any())
            return null;

         var allIds = outputMappings.Select(x => x.WeightedObservedData?.Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
         if (!allIds.Any())
            return 1;

         return allIds.Max() + 1;
      }

      private bool observedDataAlreadySelectedForSameOutput(SimulationQuantitySelectionDTO outputDTO, DataRepository observedData)
      {
         return _allOutputMappingDTOs.Count(x => Equals(x.Output, outputDTO) && Equals(x.ObservedData, observedData)) > 1;
      }

      public void OutputSelectionChanged(OutputMappingDTO dto, SimulationQuantitySelectionDTO newOutput, SimulationQuantitySelectionDTO oldOutput)
      {
         if (observedDataAlreadySelectedForSameOutput(newOutput, dto.ObservedData))
         {
            dto.Output = oldOutput;
            View.CloseEditor();
            throw new CannotSelectTheObservedDataMoreThanOnceException(dto.ObservedData);
         }

         dto.Scaling = _parameterIdentificationTask.DefaultScalingFor(newOutput.Quantity);
      }

      public void Select(OutputMappingDTO outputMappingDTO)
      {
         this.DoWithinLatch(() => ObservedDataSelected(this, new ObservedDataEventArgs(outputMappingDTO.WeightedObservedData)));
      }

      public void RemoveOutputMapping(OutputMappingDTO outputMappingDTO)
      {
         _allOutputMappingDTOs.Remove(outputMappingDTO);
         _parameterIdentification.RemoveOutputMapping(outputMappingDTO.Mapping);
         raiseObservedDataUnmappedFor(outputMappingDTO.WeightedObservedData);

         OnStatusChanged();
      }

      private void raiseObservedDataUnmappedFor(WeightedObservedData weightedObservedData)
      {
         if (weightedObservedData != null)
            ObservedDataUnmapped(this, new ObservedDataEventArgs(weightedObservedData));
      }

      private void raiseObservedDataMappedFor(WeightedObservedData weightedObservedData)
      {
         ObservedDataMapped(this, new ObservedDataEventArgs(weightedObservedData));
      }
   }
}