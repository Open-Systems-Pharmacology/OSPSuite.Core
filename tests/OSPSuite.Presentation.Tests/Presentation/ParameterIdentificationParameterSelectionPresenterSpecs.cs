using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationParameterSelectionPresenter : ContextSpecification<IParameterIdentificationParameterSelectionPresenter>
   {
      protected IParameterIdentificationIdentificationParametersPresenter _identificationParameterPresenter;
      protected IParameterIdentificationParameterSelectionView _view;
      protected ISimulationParametersPresenter _simulationParametersPresenter;
      protected ParameterIdentification _parameterIdentification;
      protected IParameterIdentificationLinkedParametersPresenter _linkedParametersPresenter;

      protected override void Context()
      {
         _identificationParameterPresenter = A.Fake<IParameterIdentificationIdentificationParametersPresenter>();
         _view = A.Fake<IParameterIdentificationParameterSelectionView>();
         _simulationParametersPresenter = A.Fake<ISimulationParametersPresenter>();
         _linkedParametersPresenter = A.Fake<IParameterIdentificationLinkedParametersPresenter>();
         sut = new ParameterIdentificationParameterSelectionPresenter(_view, _simulationParametersPresenter, _identificationParameterPresenter, _linkedParametersPresenter);

         _parameterIdentification = new ParameterIdentification();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_editing_a_parameter_identification : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_let_the_identification_parameters_presenter_edit_the_parameter_identification()
      {
         A.CallTo(() => _identificationParameterPresenter.EditParameterIdentification(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_let_the_simulation_parameters_presenter_edit_the_parameter_identification()
      {
         A.CallTo(() => _simulationParametersPresenter.EditParameterAnalysable(_parameterIdentification)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_simulation_was_added_to_a_parameter_identification : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.SimulationAdded(_simulation);
      }

      [Observation]
      public void should_tell_the_simulation_parameter_presenter_to_add_the_parameters_from_this_simulation()
      {
         A.CallTo(() => _simulationParametersPresenter.AddParametersOf(_simulation)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_simulation_was_remove_from_a_parameter_identification : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.SimulationRemoved(_simulation);
      }

      [Observation]
      public void should_tell_the_simulation_parameter_presenter_to_remove_the_parameters_from_this_simulation()
      {
         A.CallTo(() => _simulationParametersPresenter.RemoveParametersFrom(_simulation)).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_add_some_parameters_as_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private List<ParameterSelection> _selectedParameters;

      protected override void Context()
      {
         base.Context();
         _selectedParameters = new List<ParameterSelection>();
         A.CallTo(() => _simulationParametersPresenter.SelectedParameters).Returns(_selectedParameters);
      }

      protected override void Because()
      {
         sut.AddIdentificationParameter();
      }

      [Observation]
      public void should_add_the_selected_parameters_to_the_identification_parameter_presenter()
      {
         A.CallTo(() => _identificationParameterPresenter.AddParameters(_selectedParameters)).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_add_some_parameters_as_linked_parameter_to_an_existing_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private List<ParameterSelection> _selectedParameters;

      protected override void Context()
      {
         base.Context();
         _selectedParameters = new List<ParameterSelection>();
         A.CallTo(() => _simulationParametersPresenter.SelectedParameters).Returns(_selectedParameters);
      }

      protected override void Because()
      {
         sut.AddLinkedParameter();
      }

      [Observation]
      public void should_add_the_selected_parameters_to_the_identification_parameter_presenter()
      {
         A.CallTo(() => _linkedParametersPresenter.AddLinkedParameters(_selectedParameters)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_simulation_was_replaced : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationReplacedInParameterAnalyzableEvent(_parameterIdentification, A.Fake<ISimulation>(), A.Fake<ISimulation>()));
      }

      [Observation]
      public void the_simulation_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _simulationParametersPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void the_identification_parameter_presenter_must_be_refreshed()
      {
         A.CallTo(() => _identificationParameterPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void the_linked_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _linkedParametersPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_simulation_was_removed : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private ISimulation _simulation;
      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRemovedEvent(_simulation));
      }

      [Observation]
      public void the_simulation_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _simulationParametersPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void the_identification_parameter_presenter_must_be_refreshed()
      {
         A.CallTo(() => _identificationParameterPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void the_linked_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _linkedParametersPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_simulation_was_renamed : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private ISimulation _simulation;
      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _parameterIdentification.AddSimulation(_simulation);
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.Handle(new RenamedEvent(_simulation));
      }

      [Observation]
      public void the_simulation_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _simulationParametersPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void the_linked_parameters_presenter_must_be_refreshed()
      {
         A.CallTo(() => _linkedParametersPresenter.Refresh()).MustHaveHappened();   
      }
   }

   public class When_the_user_selects_a_given_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _identificationParameter = new IdentificationParameter();
      }

      protected override void Because()
      {
         _identificationParameterPresenter.IdentificationParameterSelected += Raise.With(new IdentificationParameterEventArgs(_identificationParameter));
      }

      [Observation]
      public void should_update_the_list_of_linked_parameters_for_this_identification_parameter()
      {
         A.CallTo(() => _linkedParametersPresenter.Edit(_identificationParameter)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_no_identification_parameter_is_selected : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      protected override void Because()
      {
         _identificationParameterPresenter.NoIdentificationParameterSelected += Raise.WithEmpty();
      }

      [Observation]
      public void should_clear_the_list_of_displayed_parameters_in_the_linked_parameters_presenter()
      {
         A.CallTo(() => _linkedParametersPresenter.ClearSelection()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_parameter_was_removed_from_an_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private IdentificationParameter _identificationParameter;
      private ParameterSelection _linkedParameter;

      protected override void Context()
      {
         base.Context();
         _linkedParameter = A.Fake<ParameterSelection>();
         _identificationParameter = new IdentificationParameter();
      }

      protected override void Because()
      {
         _linkedParametersPresenter.ParameterRemovedFromIdentificationParameter += Raise.With(new ParameterInIdentificationParameterEventArgs(_identificationParameter, _linkedParameter));
      }

      [Observation]
      public void should_simply_refresh_the_identification_parameter_presenter()
      {
         A.CallTo(() => _identificationParameterPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_parameter_was_unlinked_from_an_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private IdentificationParameter _identificationParameter;
      private ParameterSelection _linkedParameter;

      protected override void Context()
      {
         base.Context();
         _linkedParameter = A.Fake<ParameterSelection>();
         _identificationParameter = new IdentificationParameter();
      }

      protected override void Because()
      {
         _linkedParametersPresenter.ParameterUnlinkedFromIdentificationParameter += Raise.With(new ParameterInIdentificationParameterEventArgs(_identificationParameter, _linkedParameter));
      }

      [Observation]
      public void should_add_the_unlinked_parameter_as_new_linked_parameter()
      {
         A.CallTo(() => _identificationParameterPresenter.AddParameter(_linkedParameter)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_parameter_selection_presenter_is_notified_that_a_parameter_was_linked_to_an_identification_parameter : concern_for_ParameterIdentificationParameterSelectionPresenter
   {
      private IdentificationParameter _identificationParameter;
      private ParameterSelection _linkedParameter;

      protected override void Context()
      {
         base.Context();
         _linkedParameter = A.Fake<ParameterSelection>();
         _identificationParameter = new IdentificationParameter();
      }

      protected override void Because()
      {
         _linkedParametersPresenter.ParameterLinkedToIdentificationParameter += Raise.With(new ParameterInIdentificationParameterEventArgs(_identificationParameter, _linkedParameter));
      }

      [Observation]
      public void should_refresh_the_identification_parameter_presenter()
      {
         A.CallTo(() => _identificationParameterPresenter.Refresh()).MustHaveHappened();
      }
   }
}