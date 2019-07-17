using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SensitivityAnalysisTask : ContextSpecification<SensitivityAnalysisTask>
   {
      protected ISensitivityAnalysisSimulationSwapValidator _sensitivityAnalysisSwapValidator;
      protected ISensitivityAnalysisSimulationSwapCorrector _sensitivityAnalysisSwapCorrector;
      protected IApplicationController _applicationController;
      protected IOSPSuiteExecutionContext _executionContext;
      protected ISensitivityAnalysisFactory _sensitivityAnalysisFactory;
      protected IValidationMessagesPresenter _validationMessagesPresenter;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _sensitivityAnalysisFactory = A.Fake<ISensitivityAnalysisFactory>();
         _applicationController = A.Fake<IApplicationController>();
         _sensitivityAnalysisSwapCorrector = A.Fake<ISensitivityAnalysisSimulationSwapCorrector>();
         _sensitivityAnalysisSwapValidator = A.Fake<ISensitivityAnalysisSimulationSwapValidator>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new SensitivityAnalysisTask(_sensitivityAnalysisFactory, _executionContext, _applicationController, _sensitivityAnalysisSwapCorrector, _sensitivityAnalysisSwapValidator, _dialogCreator);

         _validationMessagesPresenter = A.Fake<IValidationMessagesPresenter>();
         A.CallTo(() => _applicationController.Start<IValidationMessagesPresenter>()).Returns(_validationMessagesPresenter);
      }
   }

   public class When_validating_a_simulation_swap_and_the_old_and_new_simulations_are_the_same : concern_for_SensitivityAnalysisTask
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private ISimulation _oldSimulation;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _oldSimulation = A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.ValidateSwap(_sensitivityAnalysis, _oldSimulation, _oldSimulation);
      }

      [Observation]
      public void the_sensitivity_validator_must_not_be_called()
      {
         A.CallTo(() => _sensitivityAnalysisSwapValidator.ValidateSwap(A<SensitivityAnalysis>._, A<ISimulation>._, A<ISimulation>._)).MustNotHaveHappened();
      }
   }

   public class When_validating_a_simulation_swap : concern_for_SensitivityAnalysisTask
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private ISimulation _oldSimulation;
      private ISimulation _newSimulation;
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         _validationResult = new ValidationResult();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _oldSimulation = A.Fake<ISimulation>();
         _newSimulation = A.Fake<ISimulation>();
         _validationResult.AddMessage(NotificationType.Warning, _sensitivityAnalysis, "this is text");

         A.CallTo(() => _sensitivityAnalysisSwapValidator.ValidateSwap(_sensitivityAnalysis, _oldSimulation, _newSimulation)).Returns(_validationResult);
      }

      protected override void Because()
      {
         sut.ValidateSwap(_sensitivityAnalysis, _oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_validation_message_presenter_should_display_any_validation_messages()
      {
         A.CallTo(() => _validationMessagesPresenter.Display(_validationResult)).MustHaveHappened();
      }

      [Observation]
      public void the_execution_context_must_be_used_to_load_the_simulation_before_validation()
      {
         A.CallTo(() => _executionContext.Load(_newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void the_sensitivity_validator_must_be_called()
      {
         A.CallTo(() => _sensitivityAnalysisSwapValidator.ValidateSwap(_sensitivityAnalysis, _oldSimulation, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_swapping_a_simulation_in_a_sensitivity_analysis : concern_for_SensitivityAnalysisTask
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private ISimulation _oldSimulation;
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _oldSimulation = A.Fake<ISimulation>();
         _newSimulation = A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.SwapSimulations(_sensitivityAnalysis, _oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_sensitivity_analysis_corrector_must_be_called()
      {
         A.CallTo(() => _sensitivityAnalysisSwapCorrector.CorrectSensitivityAnalysis(_sensitivityAnalysis, _oldSimulation, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_cloning_a_sensitivity_analysis : concern_for_SensitivityAnalysisTask
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private SensitivityAnalysis _result;
      private SensitivityAnalysis _cloneSensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis().WithId("Id1");
         _cloneSensitivityAnalysis = new SensitivityAnalysis().WithId("Id2");
         var clonePresenter = A.Fake<ICloneObjectBasePresenter<SensitivityAnalysis>>();
         A.CallTo(() => clonePresenter.CreateCloneFor(_sensitivityAnalysis)).Returns(_cloneSensitivityAnalysis);
         A.CallTo(() => _applicationController.Start<ICloneObjectBasePresenter<SensitivityAnalysis>>()).Returns(clonePresenter);
      }

      protected override void Because()
      {
         _result = sut.Clone(_sensitivityAnalysis);
      }

      [Observation]
      public void should_load_the_sensitivity_analysis_to_be_cloned()
      {
         A.CallTo(() => _executionContext.Load(_sensitivityAnalysis)).MustHaveHappened();
      }

      [Observation]
      public void should_leverage_the_clone_object_presenter_to_clone_the_sensitivity_and_return_the_clone()
      {
         _result.ShouldBeEqualTo(_cloneSensitivityAnalysis);
      }
   }

   public class When_updating_the_name_of_a_sensitivity_parameter : concern_for_SensitivityAnalysisTask
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private PKParameterSensitivity _parameterSensitivity1;
      private PKParameterSensitivity _parameterSensitivity2;
      private SensitivityParameter _sensitivityParameter;
      private readonly string _newName = "NEW_NAME";

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _sensitivityParameter = new SensitivityParameter().WithName("OLD_NAME");
         _parameterSensitivity1 = new PKParameterSensitivity {ParameterName = _sensitivityParameter.Name};
         _parameterSensitivity2 = new PKParameterSensitivity {ParameterName = "Another name"};
         _sensitivityAnalysis.Results = new SensitivityAnalysisRunResult();
         _sensitivityAnalysis.Results.AddPKParameterSensitivity(_parameterSensitivity1);
         _sensitivityAnalysis.Results.AddPKParameterSensitivity(_parameterSensitivity2);
      }

      protected override void Because()
      {
         sut.UpdateSensitivityParameterName(_sensitivityAnalysis, _sensitivityParameter, _newName);
      }

      [Observation]
      public void should_rename_the_name_of_the_sensitivity_parameter()
      {
         _sensitivityParameter.Name.ShouldBeEqualTo(_newName);
      }

      [Observation]
      public void should_update_the_name_of_the_parameter_in_the_underlying_results()
      {  
         _parameterSensitivity1.ParameterName.ShouldBeEqualTo(_newName);
      }

      [Observation]
      public void should_not_rename_the_name_of_unrelated_parameter_in_underlying_results()
      {
         _parameterSensitivity2.ParameterName.ShouldNotBeEqualTo(_newName);
      }

      [Observation]
      public void should_notify_a_result_updated_event()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SensitivityAnalysisResultsUpdatedEvent>._)).MustHaveHappened();
      }
   }
}