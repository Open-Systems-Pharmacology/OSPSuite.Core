using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presentation
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

   public class When_adding_all_constant_parameter_to_the_sensitivity_analysis : concern_for_SensitivityAnalysisParameterSelectionPresenter
   {
      private ParameterSelection _constantParameterSelection;
      private ParameterSelection _formulaParameterSelection;
      private ParameterSelection _formulaFixedParameterSelection;
      private ISimulation _simulation;
      private IParameter _constantParameter;
      private IParameter _formulaParameter;
      private IParameter _formulaFixedParameter;
      private IReadOnlyList<ParameterSelection> _addedParameters;

      protected override void Context()
      {
         base.Context();
         _simulation= A.Fake<ISimulation>();
         var rootContainer = new Container();
         _simulation.Model.Root = rootContainer;

         _constantParameter = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("ConstantParameter");
         _formulaParameter  = new Parameter().WithFormula(new ExplicitFormula("1+2")).WithName("FormulaParameter");
         _formulaFixedParameter = new Parameter().WithFormula(new ExplicitFormula("1+2")).WithName("FormulaFixedParameter");
         _formulaFixedParameter.Value = 10;

         rootContainer.Add(_constantParameter);
         rootContainer.Add(_formulaParameter);
         rootContainer.Add(_formulaFixedParameter);

         _constantParameterSelection = new ParameterSelection(_simulation, _constantParameter.Name);
         _formulaParameterSelection = new ParameterSelection(_simulation, _formulaParameter.Name);
         _formulaFixedParameterSelection = new ParameterSelection(_simulation, _formulaFixedParameter.Name);


         A.CallTo(() => _simulationParametersPresenter.AllParameters).Returns(new []{_constantParameterSelection, _formulaParameterSelection,_formulaFixedParameterSelection});

         A.CallTo(() => _sensitivityAnalysisParametersPresenter.AddParameters(A<IReadOnlyList<ParameterSelection>>._))
            .Invokes(x => _addedParameters = x.GetArgument<IReadOnlyList<ParameterSelection>>(0));
      }

      protected override void Because()
      {
         sut.AddAllConstantParameters();
      }

      [Observation]
      public void should_ensure_that_all_non_formula_parameters_are_added()
      {
         _addedParameters.ShouldContain(_constantParameterSelection);
      }

      [Observation]
      public void should_ensure_that_all_formula_with_fixed_value_are_added()
      {
         _addedParameters.ShouldContain(_formulaFixedParameterSelection);
      }

      [Observation]
      public void should_not_add_formula_parameters_that_were_not_changed_by_user()
      {
         _addedParameters.ShouldNotContain(_formulaParameterSelection);

      }
   }
}
