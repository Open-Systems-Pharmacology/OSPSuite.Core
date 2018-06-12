using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationTask : ContextSpecification<IParameterIdentificationTask>
   {
      protected IParameterIdentificationFactory _parameterIdentificationFactory;
      protected IWithIdRepository _withIdRepository;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected IObservedDataRepository _observedDataRepository;
      protected IEntityPathResolver _entityPathResolver;
      protected IIdentificationParameterFactory _identificationParameterFactory;
      protected IOSPSuiteExecutionContext _executionContext;
      protected IFavoriteRepository _favoriteRepository;
      protected IParameterIdentificationSimulationSwapValidator _simulationSwapValidator;
      protected IApplicationController _applicationController;
      protected IParameterIdentificationSimulationSwapCorrector _simulationSwapCorrector;
      protected IDialogCreator _dialogCreator;
      private ISimulationSelector _simulationSelector;
      private IHeavyWorkManager _heavyWorkManager;
      protected IParameterAnalysableParameterSelector _parameterAnalysableParameterSelector;

      protected override void Context()
      {
         _parameterIdentificationFactory = A.Fake<IParameterIdentificationFactory>();
         _withIdRepository = A.Fake<IWithIdRepository>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _identificationParameterFactory = A.Fake<IIdentificationParameterFactory>();
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _simulationSwapValidator = A.Fake<IParameterIdentificationSimulationSwapValidator>();
         _applicationController = A.Fake<IApplicationController>();
         _simulationSwapCorrector = A.Fake<IParameterIdentificationSimulationSwapCorrector>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _simulationSelector = A.Fake<ISimulationSelector>();
         _parameterAnalysableParameterSelector = A.Fake<IParameterAnalysableParameterSelector>();

         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         sut = new ParameterIdentificationTask(_parameterIdentificationFactory, _withIdRepository, _entitiesInSimulationRetriever, _observedDataRepository,
            _entityPathResolver, _identificationParameterFactory, _executionContext, _favoriteRepository, _simulationSwapValidator, _applicationController,
            _simulationSwapCorrector, _dialogCreator, _simulationSelector, _heavyWorkManager, _parameterAnalysableParameterSelector);
      }
   }

   public class When_finding_parameter_identifications_that_use_an_observed_data_in_output_mapping : concern_for_ParameterIdentificationTask
   {
      private DataRepository _dataRepository;
      private IReadOnlyCollection<ParameterIdentification> _parameterIdentifications;
      private ParameterIdentification _parameterIdentification;
      private IEnumerable<ParameterIdentification> _result;

      protected override void Context()
      {
         base.Context();
         _dataRepository = A.Fake<DataRepository>();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentifications = new List<ParameterIdentification> {_parameterIdentification, new ParameterIdentification()};
         A.CallTo(() => _executionContext.Project.AllParameterIdentifications).Returns(_parameterIdentifications);

         _parameterIdentification.AddOutputMapping(new OutputMapping {WeightedObservedData = new WeightedObservedData(_dataRepository)});
      }

      protected override void Because()
      {
         _result = sut.ParameterIdentificationsUsingObservedData(_dataRepository);
      }

      [Observation]
      public void the_task_returns_the_list_of_parameter_identifications_that_include_the_observed_data()
      {
         _result.ShouldOnlyContain(_parameterIdentification);
      }
   }

   public class When_replacing_simulations_and_the_validation_messages_presenter_is_dismissed_with_cancel : concern_for_ParameterIdentificationTask
   {
      private ISimulation _newSimulation;
      private ISimulation _oldSimulation;
      private IValidationMessagesPresenter _validationMessagesPresenter;
      private ParameterIdentification _parameterIdentification;
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         _oldSimulation = A.Fake<ISimulation>();
         _validationMessagesPresenter = A.Fake<IValidationMessagesPresenter>();
         _parameterIdentification = A.Fake<ParameterIdentification>();

         A.CallTo(() => _applicationController.Start<IValidationMessagesPresenter>()).Returns(_validationMessagesPresenter);
         _validationResult = new ValidationResult(new[] {new ValidationMessage(NotificationType.Warning, "", _parameterIdentification, null)});
         A.CallTo(() => _simulationSwapValidator.ValidateSwap(_parameterIdentification, _oldSimulation, _newSimulation)).Returns(_validationResult);
         A.CallTo(() => _validationMessagesPresenter.Display(_validationResult)).Returns(false);

         _newSimulation.IsLoaded = false;
      }

      protected override void Because()
      {
         sut.SwapSimulations(_parameterIdentification, _oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_swap_must_not_have_been_corrected()
      {
         A.CallTo(() => _simulationSwapCorrector.CorrectParameterIdentification(_parameterIdentification, _oldSimulation, _newSimulation)).MustNotHaveHappened();
      }

      [Observation]
      public void the_swap_must_have_been_validated()
      {
         A.CallTo(() => _simulationSwapValidator.ValidateSwap(_parameterIdentification, _oldSimulation, _newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void the_validation_message_presenter_should_display_the_results_of_validation_if_any()
      {
         A.CallTo(() => _validationMessagesPresenter.Display(_validationResult)).MustHaveHappened();
      }

      [Observation]
      public void the_swap_event_must_not_be_published_using_the_execution_context()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SimulationReplacedInParameterAnalyzableEvent>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_parameter_identification_object_must_not_swap_simulations()
      {
         A.CallTo(() => _parameterIdentification.SwapSimulations(_oldSimulation, _newSimulation)).MustNotHaveHappened();
      }
   }

   public class When_replacing_simulations_in_a_parameter_identification : concern_for_ParameterIdentificationTask
   {
      private ISimulation _newSimulation;
      private ISimulation _oldSimulation;
      private ParameterIdentification _parameterIdentification;
      private IValidationMessagesPresenter _validationMessagesPresenter;
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         _oldSimulation = A.Fake<ISimulation>();
         _validationMessagesPresenter = A.Fake<IValidationMessagesPresenter>();
         _parameterIdentification = A.Fake<ParameterIdentification>();

         A.CallTo(() => _applicationController.Start<IValidationMessagesPresenter>()).Returns(_validationMessagesPresenter);
         _validationResult = new ValidationResult(new[] {new ValidationMessage(NotificationType.Warning, "", _parameterIdentification, null)});
         A.CallTo(() => _simulationSwapValidator.ValidateSwap(_parameterIdentification, _oldSimulation, _newSimulation)).Returns(_validationResult);
         A.CallTo(() => _validationMessagesPresenter.Display(_validationResult)).Returns(true);

         _newSimulation.IsLoaded = false;
      }

      protected override void Because()
      {
         sut.SwapSimulations(_parameterIdentification, _oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_swap_must_have_been_corrected()
      {
         A.CallTo(() => _simulationSwapCorrector.CorrectParameterIdentification(_parameterIdentification, _oldSimulation, _newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void the_validation_swap_must_have_been_validated()
      {
         A.CallTo(() => _simulationSwapValidator.ValidateSwap(_parameterIdentification, _oldSimulation, _newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void the_validation_message_presenter_should_display_the_results_of_validation_if_any()
      {
         A.CallTo(() => _validationMessagesPresenter.Display(_validationResult)).MustHaveHappened();
      }

      [Observation]
      public void the_simulation_must_first_be_loaded()
      {
         A.CallTo(() => _executionContext.Load(_newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void the_swap_event_must_be_published_using_the_execution_context()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SimulationReplacedInParameterAnalyzableEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void the_parameter_identification_object_must_swap_simulations()
      {
         A.CallTo(() => _parameterIdentification.SwapSimulations(_oldSimulation, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_task_is_retrieving_the_default_scaling_for_a_given_output : concern_for_ParameterIdentificationTask
   {
      private IQuantity _quantity;

      protected override void Context()
      {
         base.Context();
         _quantity = A.Fake<IQuantity>();
      }

      [Observation]
      public void should_return_lin_for_a_fraction_quantity()
      {
         _quantity.Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
         sut.DefaultScalingFor(_quantity).ShouldBeEqualTo(Scalings.Linear);
      }

      [Observation]
      public void should_return_log_otherwise()
      {
         sut.DefaultScalingFor(_quantity).ShouldBeEqualTo(Scalings.Log);
      }
   }

   public class When_the_parameter_identification_task_is_adding_a_simulation_to_an_existing_parameter_identification_which_already_uses_the_simulation : concern_for_ParameterIdentificationTask
   {
      private ISimulation _simulation;
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         A.CallTo(() => _parameterIdentification.UsesSimulation(_simulation)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddSimulationTo(_parameterIdentification, _simulation);
      }

      [Observation]
      public void should_not_add_the_simulation_twice()
      {
         A.CallTo(() => _parameterIdentification.AddSimulation(_simulation)).MustNotHaveHappened();
      }
   }

   public class When_the_parameter_identification_task_is_adding_a_simulation_to_an_existing_parameter_identification_which_does_not_already_use_the_simulation : concern_for_ParameterIdentificationTask
   {
      private ISimulation _simulation;
      private ParameterIdentification _parameterIdentification;
      private IQuantity _output1;
      private DataRepository _observedData1;
      private DataRepository _observedData2;
      private string _outputPath;
      private DataRepository _observedData3;

      protected override void Context()
      {
         base.Context();
         _outputPath = "Liver|Cell|Drug";
         _simulation = A.Fake<ISimulation>().WithId("Sim");
         _parameterIdentification = new ParameterIdentification();

         _output1 = A.Fake<IQuantity>();

         var allOutputs = new PathCacheForSpecs<IQuantity> {{_outputPath, _output1}};
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(_simulation)).Returns(allOutputs);

         _observedData1 = DomainHelperForSpecs.ObservedData("OBS1");
         _observedData2 = DomainHelperForSpecs.ObservedData("OBS2");
         _observedData3 = DomainHelperForSpecs.ObservedData("OBS3");

         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(_simulation)).Returns(new[] {_observedData1, _observedData2});

         var simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         A.CallTo(() => simulationQuantitySelection.Path).Returns(_outputPath);
         _parameterIdentification.AddOutputMapping(new OutputMapping {WeightedObservedData = new WeightedObservedData(_observedData2), OutputSelection = simulationQuantitySelection});

         _observedData1.ExtendedProperties.Add(new ExtendedProperty<string> {Name = ObservedData.MOLECULE, Value = "Drug"});
         _observedData1.ExtendedProperties.Add(new ExtendedProperty<string> {Name = ObservedData.ORGAN, Value = "Liver"});
         _observedData1.ExtendedProperties.Add(new ExtendedProperty<string> {Name = ObservedData.COMPARTMENT, Value = "Cell"});

         _observedData3.ExtendedProperties.Add(new ExtendedProperty<string> {Name = ObservedData.ORGAN, Value = "Kidney"});
      }

      protected override void Because()
      {
         sut.AddSimulationTo(_parameterIdentification, _simulation);
      }

      [Observation]
      public void should_add_the_simulation_to_the_parameter_identification()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeTrue();
      }

      [Observation]
      public void should_automatically_mapped_observed_data_not_already_in_use_and_that_matches_simulation_output()
      {
         _parameterIdentification.UsesObservedData(_observedData1).ShouldBeTrue();
         _parameterIdentification.UsesObservedData(_observedData3).ShouldBeFalse();
      }
   }

   public class When_the_parameter_identification_task_is_adding_a_parameter_to_an_existing_parameter_identification_which_does_not_contain_an_indentified_parameter_for_the_parameter_path : concern_for_ParameterIdentificationTask
   {
      private IParameter _parameter;
      private ParameterIdentification _parameterIdentification;
      private ISimulation _simulation;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _parameterIdentification = new ParameterIdentification();
         _parameter = A.Fake<IParameter>();
         _parameter.Origin.SimulationId = "Sim";
         A.CallTo(() => _withIdRepository.Get<ISimulation>("Sim")).Returns(_simulation);
         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns("Path");
         _identificationParameter = new IdentificationParameter();
         A.CallTo(_identificationParameterFactory).WithReturnType<IdentificationParameter>().Returns(_identificationParameter);
         A.CallTo(() => _parameterAnalysableParameterSelector.CanUseParameter(_parameter)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddParameterTo(_parameterIdentification, _parameter);
      }

      [Observation]
      public void should_add_a_new_identification_parameter()
      {
         _parameterIdentification.AllIdentificationParameters.ShouldContain(_identificationParameter);
      }

      [Observation]
      public void should_add_the_simulation_containing_the_parameter()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeTrue();
      }
   }

   public class When_the_parameter_identification_task_is_adding_a_parameter_to_an_existing_parameter_identification_which_already_contains_an_indentified_parameter_for_the_parameter_path : concern_for_ParameterIdentificationTask
   {
      private IParameter _parameter;
      private ParameterIdentification _parameterIdentification;
      private ISimulation _simulation;
      private IdentificationParameter _identificationParameter;
      private ISimulation _simulation2;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>().WithName("Sim");
         _simulation2 = A.Fake<ISimulation>().WithName("Sim2");
         _parameterIdentification = new ParameterIdentification();
         _parameter = A.Fake<IParameter>();
         _parameter.Origin.SimulationId = "Sim";
         A.CallTo(() => _withIdRepository.Get<ISimulation>("Sim")).Returns(_simulation);
         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns("Path");
         _identificationParameter = new IdentificationParameter();
         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation2, "Path"));
         _parameterIdentification.AddIdentificationParameter(_identificationParameter);

         A.CallTo(() => _parameterAnalysableParameterSelector.CanUseParameter(_parameter)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddParameterTo(_parameterIdentification, _parameter);
      }

      [Observation]
      public void should_add_the_parameter_as_linked_parameter()
      {
         _identificationParameter.AllLinkedParameters.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_add_the_simulation_containing_the_parameter()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeTrue();
      }
   }

   public class When_the_parameter_identification_task_is_creating_a_parameter_identification_based_on_a_given_parameter_that_can_be_used_in_parameter_analysis : concern_for_ParameterIdentificationTask
   {
      private IParameter _parameter;
      private ISimulation _simulation;
      private ParameterIdentification _result;
      private IProject _project;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _simulation = A.Fake<ISimulation>().WithName("Sim");
         _parameter.Origin.SimulationId = "Sim";
         _project = A.Fake<IProject>();
         A.CallTo(() => _withIdRepository.Get<ISimulation>("Sim")).Returns(_simulation);
         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns("Path");
         A.CallTo(() => _parameterIdentificationFactory.Create()).Returns(new ParameterIdentification());
         A.CallTo(() => _parameterAnalysableParameterSelector.CanUseParameter(_parameter)).Returns(true);
         A.CallTo(() => _executionContext.Project).Returns(_project);
      }

      protected override void Because()
      {
         _result = sut.CreateParameterIdentificationBasedOn(new[] {_parameter});
      }

      [Observation]
      public void the_new_parameter_identification_should_be_added_to_the_project()
      {
         A.CallTo(() => _project.AddParameterIdentification(_result)).MustHaveHappened();
      }

      [Observation]
      public void the_new_parameter_identification_must_be_registered()
      {
         A.CallTo(() => _executionContext.Register(_result)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_paramter_to_the_parameter_identification()
      {
         _result.AllIdentificationParameters.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_the_parameter_identification_task_is_creating_a_parameter_identification_based_on_simulations : concern_for_ParameterIdentificationTask
   {
      private ISimulation _simulation;
      private ParameterIdentification _result;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private readonly string _parameterPath1 = "Organ|Liver|P1";
      private readonly string _parameterPath2 = "Organ|Liver|P2";
      private IdentificationParameter _parameter1IdentificationParameter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>().WithName("S");
         _parameter1 = A.Fake<IParameter>();
         _parameter2 = A.Fake<IParameter>();
         _parameter1.Origin.SimulationId = "S";
         _parameter2.Origin.SimulationId = "S";

         A.CallTo(() => _withIdRepository.Get<ISimulation>(_parameter1.Origin.SimulationId)).Returns(_simulation);
         A.CallTo(() => _withIdRepository.Get<ISimulation>(_parameter2.Origin.SimulationId)).Returns(_simulation);
         A.CallTo(() => _parameterIdentificationFactory.Create()).Returns(new ParameterIdentification());
         A.CallTo(() => _executionContext.Project).Returns(A.Fake<IProject>());

         var parameterCache = new PathCacheForSpecs<IParameter> {{_parameterPath1, _parameter1}, {_parameterPath2, _parameter2}};
         A.CallTo(() => _entitiesInSimulationRetriever.ParametersFrom(_simulation)).Returns(parameterCache);
         A.CallTo(() => _favoriteRepository.All()).Returns(new[] {_parameterPath1, _parameterPath2,});
         A.CallTo(() => _entityPathResolver.PathFor(_parameter1)).Returns(_parameterPath1);
         A.CallTo(() => _entityPathResolver.PathFor(_parameter2)).Returns(_parameterPath2);
         A.CallTo(() => _parameterAnalysableParameterSelector.CanUseParameter(_parameter1)).Returns(true);
         A.CallTo(() => _parameterAnalysableParameterSelector.CanUseParameter(_parameter2)).Returns(false);

         _parameter1IdentificationParameter = new IdentificationParameter();
         A.CallTo(() => _identificationParameterFactory.CreateFor(A<ParameterSelection>.That.Matches(x => x.Path == _parameterPath1), A<ParameterIdentification>._)).Returns(_parameter1IdentificationParameter);
      }

      protected override void Because()
      {
         _result = sut.CreateParameterIdentificationBasedOn(new[] {_simulation});
      }

      [Observation]
      public void should_load_the_simulation_to_add()
      {
         A.CallTo(() => _executionContext.Load(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_simulations_to_the_created_parameter_identification()
      {
         _result.UsesSimulation(_simulation).ShouldBeTrue();
      }

      [Observation]
      public void should_add_only_favorite_parameters_to_the_pi_that_can_be_used_in_parameter_identification()
      {
         _result.AllIdentificationParameters.ShouldOnlyContain(_parameter1IdentificationParameter);
      }
   }

   public class When_executin_the_delete_parameter_identification_ui_command_and_the_user_accepts : concern_for_ParameterIdentificationTask
   {
      private ParameterIdentification _parameterIdentification;
      private TestProject _project;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _project = new TestProject();
         _project.AddParameterIdentification(_parameterIdentification);
         A.CallTo(() => _executionContext.Project).Returns(_project);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Delete(new[] {_parameterIdentification});
      }

      [Observation]
      public void should_close_the_view_associated_with_the_parameter_identification()
      {
         A.CallTo(() => _applicationController.Close(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_parameter_identification_from_the_project()
      {
         _project.AllParameterIdentifications.ShouldNotContain(_parameterIdentification);
      }

      [Observation]
      public void should_unregister_the_parameter_identification()
      {
         A.CallTo(() => _executionContext.Unregister(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_the_parameter_identification_deleted_event()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<ParameterIdentificationDeletedEvent>._)).MustHaveHappened();
      }
   }

   public class When_executin_the_delete_parameter_identification_ui_command_and_the_user_cancels : concern_for_ParameterIdentificationTask
   {
      private ParameterIdentification _parameterIdentification;
      private TestProject _project;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _project = new TestProject();
         _project.AddParameterIdentification(_parameterIdentification);
         A.CallTo(() => _executionContext.Project).Returns(_project);
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.Delete(new[] {_parameterIdentification});
      }

      [Observation]
      public void should_not_delete_the_parameter_identification()
      {
         _project.AllParameterIdentifications.ShouldContain(_parameterIdentification);
      }
   }

   public class When_cloning_a_parameter_identification_from_task : concern_for_ParameterIdentificationTask
   {
      private ParameterIdentification _parameterIdentification;
      private ICloneObjectBasePresenter<ParameterIdentification> _cloneObjectBasePresenter;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification {IsLoaded = false};
         _cloneObjectBasePresenter = A.Fake<ICloneObjectBasePresenter<ParameterIdentification>>();
         A.CallTo(_applicationController).WithReturnType<ICloneObjectBasePresenter<ParameterIdentification>>().Returns(_cloneObjectBasePresenter);
      }

      protected override void Because()
      {
         sut.Clone(_parameterIdentification);
      }

      [Observation]
      public void the_lazy_load_task_must_be_called()
      {
         A.CallTo(() => _executionContext.Load(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void The_application_controller_must_start_the_rename_presenter()
      {
         A.CallTo(() => _cloneObjectBasePresenter.CreateCloneFor(_parameterIdentification)).MustHaveHappened();
      }
   }
}