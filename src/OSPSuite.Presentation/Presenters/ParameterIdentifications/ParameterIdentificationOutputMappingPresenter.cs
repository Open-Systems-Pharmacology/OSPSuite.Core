using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
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
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class OutputMappingEventArgs : EventArgs
   {
      public OutputMapping OutputMapping { get; }

      public OutputMappingEventArgs(OutputMapping outputMapping)
      {
         OutputMapping = outputMapping;
      }
   }

   public interface IParameterIdentificationOutputMappingPresenter : IPresenter<IParameterIdentificationOutputMappingView>, IParameterIdentificationPresenter, ILatchable
   {
      void AddOutputMapping();
      IEnumerable<SimulationQuantitySelectionDTO> AllAvailableOutputs { get; }
      IEnumerable<DataRepository> AllObservedDataFor(OutputMappingDTO dto);
      void ObservedDataSelectionChanged(OutputMappingDTO dto, DataRepository newObservedData, DataRepository oldObservedData);
      void RemoveOutputMapping(OutputMappingDTO outputMappingDTO);
      event EventHandler<OutputMappingEventArgs> ObservedDataMapped;
      event EventHandler<OutputMappingEventArgs> ObservedDataUnmapped;
      event EventHandler<OutputMappingEventArgs> ObservedDataSelected;

      /// <summary>
      ///    Ensures that the cached list are updated (useful when a simulation was added to the PI)
      /// </summary>
      void UpdateCache();

      /// <summary>
      ///    Completely rebinds the view to the content of the data source
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
      public event EventHandler<OutputMappingEventArgs> ObservedDataMapped = delegate { };
      public event EventHandler<OutputMappingEventArgs> ObservedDataUnmapped = delegate { };
      public event EventHandler<OutputMappingEventArgs> ObservedDataSelected = delegate { };
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
            updateOutputMappingList();
         });
         _view.BindTo(_allOutputMappingDTOs);
      }

      public void UpdateCache() => _allAvailableOutputs.Clear();

      public void AddOutputMapping()
      {
         var newOutputMapping = new OutputMapping();
         _parameterIdentification.AddOutputMapping(newOutputMapping);
         _allOutputMappingDTOs.Add(mapFrom(newOutputMapping));
         OnStatusChanged();
      }

      private void updateOutputMappingList()
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

      private SimulationQuantitySelectionDTO mapFrom(ISimulation simulation, IQuantity quantity) => _simulationQuantitySelectionDTOMapper.MapFrom(simulation, quantity);

      private OutputMappingDTO mapFrom(OutputMapping outputMapping) => _outputMappingDTOMapper.MapFrom(outputMapping, AllAvailableOutputs);

      public IEnumerable<DataRepository> AllObservedDataFor(OutputMappingDTO outputMappingDTO) => allPossibleObservedDataForOutput(outputMappingDTO.Output);

      private IEnumerable<DataRepository> allPossibleObservedDataForOutput(SimulationQuantitySelectionDTO outputSelectionDTO)
      {
         if (outputSelectionDTO == null)
            return Enumerable.Empty<DataRepository>();

         var usesObservedData = outputSelectionDTO.Simulation as IUsesObservedData;
         return _observedDataRepository.AllObservedDataUsedBy(usesObservedData)
            .Distinct()
            .OrderBy(x => x.Name);
      }

      public void ObservedDataSelectionChanged(OutputMappingDTO dto, DataRepository newObservedData, DataRepository oldObservedData)
      {
         var allOutputsUsingObservedData = _allOutputMappingDTOs.Where(x => Equals(x.ObservedData, newObservedData)).Except(new[] { dto }).ToList();

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

         raiseObservedDataUnmappedFor(dto.Mapping);
         dto.Mapping.WeightedObservedData = weightedObservedData;
         View.CloseEditor();
         raiseObservedDataMappedFor(dto.Mapping);
      }

      private int? nextUniqueIdFor(List<OutputMappingDTO> outputMappings)
      {
         if (!outputMappings.Any())
            return null;

         var allIds = outputMappings.Select(x => x.WeightedObservedData?.Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
         if (!allIds.Any())
            return 1;

         return allIds.Max() + 1;
      }

      private bool observedDataAlreadySelectedForSameOutput(SimulationQuantitySelectionDTO outputDTO, DataRepository observedData) => _allOutputMappingDTOs.Count(x => Equals(x.Output, outputDTO) && Equals(x.ObservedData, observedData)) > 1;

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

      public void Select(OutputMappingDTO outputMappingDTO) => this.DoWithinLatch(() => ObservedDataSelected(this, new OutputMappingEventArgs(outputMappingDTO.Mapping)));

      public void RemoveOutputMapping(OutputMappingDTO outputMappingDTO)
      {
         _allOutputMappingDTOs.Remove(outputMappingDTO);
         _parameterIdentification.RemoveOutputMapping(outputMappingDTO.Mapping);
         raiseObservedDataUnmappedFor(outputMappingDTO.Mapping);

         OnStatusChanged();
      }

      private void raiseObservedDataUnmappedFor(OutputMapping outputMapping)
      {
         if (outputMapping.WeightedObservedData != null)
            ObservedDataUnmapped(this, new OutputMappingEventArgs(outputMapping));
      }

      private void raiseObservedDataMappedFor(OutputMapping weightedObservedData) => ObservedDataMapped(this, new OutputMappingEventArgs(weightedObservedData));
   }
}