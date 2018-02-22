using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisParameterSelectionPresenter : ISensitivityAnalysisItemPresenter,
      IListener<SimulationReplacedInParameterAnalyzableEvent>,
      IListener<SimulationRemovedEvent>
   {
      void AddSelectedParameters();
      void RemoveParameters();
      void ChangeSimulation(ISimulation simulation);
      bool ValidateSimulationChange(ISimulation newSimulation);
      void AddAllConstantParameters();
      int IconFor(ISimulation simulation);
      ISimulation Simulation { get; }
      IEnumerable<ISimulation> AllSimulations { get; }
   }

   public class SensitivityAnalysisParameterSelectionPresenter : AbstractSubPresenter<ISensitivityAnalysisParameterSelectionView, ISensitivityAnalysisParameterSelectionPresenter>, ISensitivityAnalysisParameterSelectionPresenter
   {
      private readonly ISimulationParametersPresenter _simulationParametersPresenter;
      private readonly ISensitivityAnalysisParametersPresenter _sensitivityAnalysisParametersPresenter;
      private readonly ISimulationRepository _simulationRepository;
      private SensitivityAnalysis _sensitivityAnalysis;
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly ISimulationSelector _simulationSelector;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;

      public SensitivityAnalysisParameterSelectionPresenter(ISensitivityAnalysisParameterSelectionView view, ISimulationParametersPresenter simulationParametersPresenter,
          ISensitivityAnalysisParametersPresenter sensitivityAnalysisParametersPresenter, ISimulationRepository simulationRepository, ILazyLoadTask lazyLoadTask, ISimulationSelector simulationSelector, ISensitivityAnalysisTask sensitivityAnalysisTask) : base(view)
      {
         _simulationParametersPresenter = simulationParametersPresenter;
         _sensitivityAnalysisParametersPresenter = sensitivityAnalysisParametersPresenter;
         _simulationRepository = simulationRepository;
         _lazyLoadTask = lazyLoadTask;
         _simulationSelector = simulationSelector;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _subPresenterManager.Add(_simulationParametersPresenter);
         _subPresenterManager.Add(_sensitivityAnalysisParametersPresenter);
         _view.AddSimulationParametersView(_simulationParametersPresenter.BaseView);
         _view.AddSensitivityParametersView(_sensitivityAnalysisParametersPresenter.BaseView);
      }

      public void EditSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis)
      {
         _sensitivityAnalysis = sensitivityAnalysis;
         _sensitivityAnalysisParametersPresenter.EditSensitivityAnalysis(sensitivityAnalysis);
         rebind();
      }

      private void rebind()
      {
         AllSimulations = allProjectSimulationsUsableForSensitivityAnalysis();

         if (_sensitivityAnalysis.Simulation == null)
            _sensitivityAnalysis.Simulation = AllSimulations.FirstOrDefault();

         _view.BindTo(this);
         showParametersFor(_sensitivityAnalysis);
      }

      public IEnumerable<ISimulation> AllSimulations { get; private set; }

      private List<ISimulation> allProjectSimulationsUsableForSensitivityAnalysis()
      {
         return _simulationRepository.All().Where(x => _simulationSelector.SimulationCanBeUsedForSensitivityAnalysis(x)).ToList();
      }

      private void loadSimulation()
      {
         _sensitivityAnalysis.AllSimulations.Each(_lazyLoadTask.Load);
      }

      private void showParametersFor(IParameterAnalysable sensitivityAnalysis)
      {
         loadSimulation();
         _simulationParametersPresenter.EditParameterAnalysable(sensitivityAnalysis);
      }

      public void AddSelectedParameters()
      {
         _sensitivityAnalysisParametersPresenter.AddParameters(_simulationParametersPresenter.SelectedParameters);
      }

      public void RemoveParameters()
      {
         _sensitivityAnalysisParametersPresenter.RemoveSelectedSensitivityParameters();
      }

      public void ChangeSimulation(ISimulation simulation)
      {
         _sensitivityAnalysisTask.SwapSimulations(_sensitivityAnalysis, _sensitivityAnalysis.Simulation, simulation);
         showParametersFor(_sensitivityAnalysis);
      }

      public bool ValidateSimulationChange(ISimulation newSimulation)
      {
         return _sensitivityAnalysisTask.ValidateSwap(_sensitivityAnalysis, _sensitivityAnalysis.Simulation, newSimulation);
      }

      public void AddAllConstantParameters()
      {
         var allConstantParameters = _simulationParametersPresenter.AllParameters.Where(isNonFormulaParameter).ToList();
         _sensitivityAnalysisParametersPresenter.AddParameters(allConstantParameters);
      }

      public int IconFor(ISimulation simulation)
      {
         return ApplicationIcons.Simulation.Index;
      }

      public ISimulation Simulation => _sensitivityAnalysis.Simulation;

      private static bool isNonFormulaParameter(ParameterSelection parameterSelection)
      {
         var parameter = parameterSelection.Parameter;
         return parameter.IsFixedValue || !parameter.Formula.IsExplicit();
      }

      public void Handle(SimulationReplacedInParameterAnalyzableEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            refreshSubPresenters();
      }

      private void refreshSubPresenters()
      {
         _sensitivityAnalysisParametersPresenter.Refresh();
         _simulationParametersPresenter.Refresh();
      }

      private bool canHandle(SimulationReplacedInParameterAnalyzableEvent eventToHandle)
      {
         return Equals(eventToHandle.ParameterAnalysable, _sensitivityAnalysis) && (_sensitivityAnalysis?.Simulation != null);
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         refreshSubPresenters();
         rebind();
      }

      private bool canHandle(SimulationRemovedEvent eventToHandle)
      {
         return eventToHandle.Simulation.IsAnImplementationOf<ISimulation>();
      }
   }
}