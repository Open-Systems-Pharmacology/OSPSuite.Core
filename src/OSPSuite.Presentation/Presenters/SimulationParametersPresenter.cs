using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationParametersPresenter : IPresenter<ISimulationParametersView>
   {
      void AddParametersOf(ISimulation simulation);
      void RemoveParametersFrom(ISimulation simulation);
      IReadOnlyList<ParameterSelection> SelectedParameters { get; }
      IReadOnlyList<ParameterSelection> AllParameters { get; }
      void Refresh();

      /// <summary>
      ///    Indicates that the way the parameters are displayed has changed
      /// </summary>
      ParameterGroupingMode ParameterGroupingMode { get; set; }

      IEnumerable<ParameterGroupingMode> AllGroupingModes { get; }

      void EditParameterAnalysable(IParameterAnalysable parameterAnalysable);
   }

   public class SimulationParametersPresenter : AbstractPresenter<ISimulationParametersView, ISimulationParametersPresenter>, ISimulationParametersPresenter
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IQuantityToSimulationParameterSelectionDTOMapper _simulationParameterSelectionDTOMapper;
      private readonly IGroupRepository _groupRepository;
      private readonly IParameterAnalysableParameterSelector _parameterSelector;
      private IParameterAnalysable _parameterAnalysable;

      private readonly List<SimulationParameterSelectionDTO> _allParameterDTOs = new List<SimulationParameterSelectionDTO>();
      private ParameterGroupingMode _parameterGroupingMode;

      public SimulationParametersPresenter(ISimulationParametersView view, IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IQuantityToSimulationParameterSelectionDTOMapper simulationParameterSelectionDTOMapper, IGroupRepository groupRepository,
         IParameterAnalysableParameterSelector parameterSelector) : base(view)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _simulationParameterSelectionDTOMapper = simulationParameterSelectionDTOMapper;
         _groupRepository = groupRepository;
         _parameterSelector = parameterSelector;
         _parameterGroupingMode = _parameterSelector.DefaultParameterSelectionMode;
      }

      public void EditParameterAnalysable(IParameterAnalysable parameterAnalysable)
      {
         _parameterAnalysable = parameterAnalysable;
         _view.BindToModeSelection();
         _view.BindTo(_allParameterDTOs);
         Refresh();
         _view.GroupBy(PathElement.Container, groupIndex: 0);
         _view.GroupBy(PathElement.Name, groupIndex: 1);
      }

      public ParameterGroupingMode ParameterGroupingMode
      {
         get { return _parameterGroupingMode; }
         set
         {
            _parameterGroupingMode = value;
            Refresh();
         }
      }

      public void Refresh()
      {
         _allParameterDTOs.Clear();
         _parameterAnalysable.AllSimulations.Each(addSimulationParameters);

         _view.Rebind();
      }

      public IEnumerable<ParameterGroupingMode> AllGroupingModes
      {
         get
         {
            yield return ParameterGroupingModes.Simple;
            yield return ParameterGroupingModes.Advanced;
         }
      }

      public void AddParametersOf(ISimulation simulation)
      {
         addSimulationParameters(simulation);
         _view.Rebind();
      }

      private void addSimulationParameters(ISimulation simulation)
      {
         _allParameterDTOs.AddRange(_entitiesInSimulationRetriever.ParametersFrom(simulation, parameterCanBeUsedInParameterAnalysable)
            .Select(p => _simulationParameterSelectionDTOMapper.MapFrom(simulation, p)));
      }

      private bool parameterCanBeUsedInParameterAnalysable(IParameter parameter)
      {
         var parameterIsAdvanced = _groupRepository.GroupByName(parameter.GroupName).IsAdvanced;
         var canUseParameter = _parameterSelector.CanUseParameter(parameter);

         if (_parameterGroupingMode == ParameterGroupingModes.Simple)
            canUseParameter = canUseParameter && !parameterIsAdvanced;

         return canUseParameter;
      }

      public void RemoveParametersFrom(ISimulation simulation)
      {
         removeSimulationParameters(simulation);
         _view.Rebind();
      }

      public IReadOnlyList<ParameterSelection> AllParameters => allSelectionParametersFrom(_allParameterDTOs);

      public IReadOnlyList<ParameterSelection> SelectedParameters => allSelectionParametersFrom(View.SelectedParameters);

      private IReadOnlyList<ParameterSelection> allSelectionParametersFrom(IEnumerable<SimulationParameterSelectionDTO> simulationParameterSelectionDTOs)
      {
         return simulationParameterSelectionDTOs.Select(dto => new ParameterSelection(dto.Simulation, dto.QuantitySelectionDTO.ToQuantitySelection())).ToList();
      }

      private void removeSimulationParameters(ISimulation simulation)
      {
         var allParametersToRemove = _allParameterDTOs.Where(x => Equals(x.Simulation, simulation)).ToList();
         allParametersToRemove.Each(x => _allParameterDTOs.Remove(x));
      }
   }
}