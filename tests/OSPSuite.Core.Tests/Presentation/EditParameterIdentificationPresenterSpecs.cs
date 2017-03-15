using OSPSuite.BDDHelper;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_EditParameterIdentificationPresenter : ContextSpecification<IEditParameterIdentificationPresenter>
   {
      protected ISubPresenterItemManager<IParameterIdentificationItemPresenter> _subPresenterItemManager;
      protected IEditParameterIdentificationView _view;
      protected IParameterIdentificationDataSelectionPresenter _parameterIdentificationDataSelectionPresenter;
      protected IParameterIdentificationParameterSelectionPresenter _parameterIdentificationParameterSelectionPresenter;
      protected IParameterIdentificationConfigurationPresenter _parameterIdentificationConfigurationPresenter;
      protected ParameterIdentification _parameterIdentification;
      protected IOSPSuiteExecutionContext _executionContext;
      protected ISimulation _simulation;
      private ISimulationAnalysisPresenterFactory _simulationAnalysisPresenterFactory;
      private ISimulationAnalysisPresenterContextMenuFactory _contextMenuFactory;
      private IPresentationSettingsTask _presentationSettingsTask;
      private IParameterIdentificationAnalysisCreator _simulationAnalysisCreator;

      protected override void Context()
      {
         _view = A.Fake<IEditParameterIdentificationView>();
         _subPresenterItemManager = SubPresenterHelper.Create<IParameterIdentificationItemPresenter>();
         _parameterIdentificationDataSelectionPresenter = _subPresenterItemManager.CreateFake(ParameterIdentificationItems.Data);
         _parameterIdentificationParameterSelectionPresenter = _subPresenterItemManager.CreateFake(ParameterIdentificationItems.Parameters);
         _parameterIdentificationConfigurationPresenter = _subPresenterItemManager.CreateFake(ParameterIdentificationItems.Configuration);

         _simulationAnalysisPresenterFactory= A.Fake<ISimulationAnalysisPresenterFactory>();
         _contextMenuFactory= A.Fake<ISimulationAnalysisPresenterContextMenuFactory>();
         _presentationSettingsTask= A.Fake<IPresentationSettingsTask>();
         _simulationAnalysisCreator= A.Fake<IParameterIdentificationAnalysisCreator>();
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _simulation = A.Fake<ISimulation>();

         sut = new EditParameterIdentificationPresenter(_view, _subPresenterItemManager, _executionContext,_simulationAnalysisPresenterFactory,_contextMenuFactory,_presentationSettingsTask,_simulationAnalysisCreator);

         sut.InitializeWith(A.Fake<ICommandCollector>());
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.AddSimulation(_simulation);
      }
   }

   public class When_editing_a_parameter_identification : concern_for_EditParameterIdentificationPresenter
   {
      protected override void Because()
      {
         sut.Edit(_parameterIdentification);
      }

      [Observation]
      public void should_edit_all_sub_presenters_with_the_given_parameter_identification()
      {
         _subPresenterItemManager.AllSubPresenters.Each(p => A.CallTo(() => p.EditParameterIdentification(_parameterIdentification)).MustHaveHappened());
      }

      [Observation]
      public void should_load_all_simulations_defined_in_the_parameter_identification()
      {
         A.CallTo(() => _executionContext.Load(_simulation)).MustHaveHappened();
      }
   }

   public class When_the_edit_parameter_identification_presenter_is_notififed_that_a_simulation_was_added_to_the_parameter_identification : concern_for_EditParameterIdentificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameterIdentification);
      }

      protected override void Because()
      {
         _parameterIdentificationDataSelectionPresenter.SimulationAdded += Raise.With(new SimulationEventArgs(_simulation));
      }

      [Observation]
      public void should_notify_the_parameter_presenter_that_a_simulation_was_added()
      {
         A.CallTo(() => _parameterIdentificationParameterSelectionPresenter.SimulationAdded(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_notify_the_parameter_configuration_presenter_that_a_simulation_was_added()
      {
         A.CallTo(() => _parameterIdentificationConfigurationPresenter.SimulationAdded(_simulation)).MustHaveHappened();
      }
   }

   public class When_the_edit_parameter_identification_presenter_is_notififed_that_a_simulation_was_removed_from_the_parameter_identification : concern_for_EditParameterIdentificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameterIdentification);
      }

      protected override void Because()
      {
         _parameterIdentificationDataSelectionPresenter.SimulationRemoved += Raise.With(new SimulationEventArgs(_simulation));
      }

      [Observation]
      public void should_notify_the_parameter_presenter_that_a_simulation_was_removed()
      {
         A.CallTo(() => _parameterIdentificationParameterSelectionPresenter.SimulationRemoved(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_notify_the_parameter_configuration_presenter_that_a_simulation_was_removed()
      {
         A.CallTo(() => _parameterIdentificationConfigurationPresenter.SimulationRemoved(_simulation)).MustHaveHappened();
      }
   }
}