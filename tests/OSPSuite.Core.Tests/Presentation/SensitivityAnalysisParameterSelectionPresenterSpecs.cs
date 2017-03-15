using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityAnalysisParameterSelectionPresenter : ContextSpecification<SensitivityAnalysisParameterSelectionPresenter>
   {
      protected ISensitivityAnalysisParameterSelectionView _view;
      protected ISimulationParametersPresenter _simulationParametersPresenter;
      protected ISensitivityAnalysisParametersPresenter _sensitivityAnalysisParametersPresenter;
      protected ISimulationRepository _simulationRepository;
      protected ILazyLoadTask _lazyLoadTask;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected IReadOnlyCollection<ISimulation> _simulations;
      private ISimulationSelector _simulationSelector;
      protected ISensitivityAnalysisTask _sensitivityAnalysisTask;

      protected override void Context()
      {
         _view = A.Fake<ISensitivityAnalysisParameterSelectionView>();
         _simulationParametersPresenter = A.Fake<ISimulationParametersPresenter>();
         _sensitivityAnalysisParametersPresenter = A.Fake<ISensitivityAnalysisParametersPresenter>();
         _simulationRepository = A.Fake<ISimulationRepository>();
         _lazyLoadTask = A.Fake<ILazyLoadTask>();
         _simulationSelector = A.Fake<ISimulationSelector>();
         _sensitivityAnalysisTask = A.Fake<ISensitivityAnalysisTask>();

         sut = new SensitivityAnalysisParameterSelectionPresenter(_view, _simulationParametersPresenter, _sensitivityAnalysisParametersPresenter, _simulationRepository, _lazyLoadTask, _simulationSelector, _sensitivityAnalysisTask);

         _sensitivityAnalysis = new SensitivityAnalysis { Simulation = A.Fake<ISimulation>() };
         _simulations = new[] { _sensitivityAnalysis.Simulation };
         A.CallTo(() => _simulationRepository.All()).Returns(_simulations);
         A.CallTo(() => _simulationSelector.SimulationCanBeUsedForSensitivityAnalysis(_sensitivityAnalysis.Simulation)).Returns(true);
      }
   }

   public class When_changing_a_simulation_in_a_sensitivity_analysis : concern_for_SensitivityAnalysisParameterSelectionPresenter
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         sut.ChangeSimulation(_newSimulation);
      }

      [Observation]
      public void the_simulation_parameter_presenter_should_be_refreshed()
      {
         A.CallTo(() => _simulationParametersPresenter.EditParameterAnalysable(_sensitivityAnalysis)).MustHaveHappened();
      }

      [Observation]
      public void the_new_simulation_should_be_loaded()
      {
         A.CallTo(() => _lazyLoadTask.Load(A<ISimulation>._)).MustHaveHappened();
      }

      [Observation]
      public void the_sensitivity_analysis_task_is_used_to_swap_the_simulation()
      {
         A.CallTo(() => _sensitivityAnalysisTask.SwapSimulations(_sensitivityAnalysis, _sensitivityAnalysis.Simulation, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_validating_a_simulation_change_in_a_sensitivity_analysis : concern_for_SensitivityAnalysisParameterSelectionPresenter
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         sut.ValidateSimulationChange(_newSimulation);
      }

      [Observation]
      public void the_simulation_swap_validation_should_be_called()
      {
         A.CallTo(() => _sensitivityAnalysisTask.ValidateSwap(_sensitivityAnalysis, _sensitivityAnalysis.Simulation, _newSimulation)).MustHaveHappened();
      }
   }
   public class When_editing_a_sensitivity_analysis : concern_for_SensitivityAnalysisParameterSelectionPresenter
   {
      protected override void Because()
      {
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      [Observation]
      public void the_sub_presenters_must_edit_the_analysis()
      {
         A.CallTo(() => _sensitivityAnalysisParametersPresenter.EditSensitivityAnalysis(_sensitivityAnalysis)).MustHaveHappened();
      }
   }
}
