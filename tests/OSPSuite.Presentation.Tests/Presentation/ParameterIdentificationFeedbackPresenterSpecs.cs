using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationFeedbackPresenter : ContextSpecification<IParameterIdentificationFeedbackPresenter>
   {
      protected IParameterIdentificationFeedbackView _view;
      protected IPresentationUserSettings _presenterUserSettings;
      protected ISingleParameterIdentificationFeedbackPresenter _singleFeedbackPresenter;
      protected IMultipleParameterIdentificationFeedbackPresenter _multipleFeedbackPresenter;

      protected ParameterIdentification _paramterIdentification;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationFeedbackView>();
         _presenterUserSettings = A.Fake<IPresentationUserSettings>();
         _singleFeedbackPresenter = A.Fake<ISingleParameterIdentificationFeedbackPresenter>();
         _multipleFeedbackPresenter = A.Fake<IMultipleParameterIdentificationFeedbackPresenter>();

         sut = new ParameterIdentificationFeedbackPresenter(_view, _presenterUserSettings, _singleFeedbackPresenter, _multipleFeedbackPresenter);

         _paramterIdentification = A.Fake<ParameterIdentification>();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_a_single_run_optimization_is_starting : concern_for_ParameterIdentificationFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _paramterIdentification.IsSingleRun).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationStartedEvent(_paramterIdentification));
      }

      [Observation]
      public void should_show_the_single_run_view()
      {
         A.CallTo(() => _singleFeedbackPresenter.EditParameterIdentification(_paramterIdentification)).MustHaveHappened();
         A.CallTo(() => _view.ShowFeedbackView(_singleFeedbackPresenter.BaseView)).MustHaveHappened();
      }

      [Observation]
      public void should_reset_the_single_run_and_multiple_view()
      {
         A.CallTo(() => _singleFeedbackPresenter.ResetFeedback()).MustHaveHappened();
         A.CallTo(() => _multipleFeedbackPresenter.ResetFeedback()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_a_multiple_run_optimization_is_starting : concern_for_ParameterIdentificationFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _paramterIdentification.IsSingleRun).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationStartedEvent(_paramterIdentification));
      }

      [Observation]
      public void should_reset_the_single_run_and_multiple_view()
      {
         A.CallTo(() => _singleFeedbackPresenter.ResetFeedback()).MustHaveHappened();
         A.CallTo(() =>_multipleFeedbackPresenter.ResetFeedback()).MustHaveHappened();
      }

      [Observation]
      public void should_show_the_multiple_run_view()
      {
         A.CallTo(() => _multipleFeedbackPresenter.EditParameterIdentification(_paramterIdentification)).MustHaveHappened();
         A.CallTo(() => _view.ShowFeedbackView(_multipleFeedbackPresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_a_run_optimization_is_terminated : concern_for_ParameterIdentificationFeedbackPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationTerminatedEvent(_paramterIdentification));
      }

      [Observation]
      public void should_clear_the_single_run_view()
      {
         A.CallTo(() => _singleFeedbackPresenter.ClearReferences()).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_multiple_run_view()
      {
         A.CallTo(() => _multipleFeedbackPresenter.ClearReferences()).MustHaveHappened();
      }

      [Observation]
      public void should_show_the_no_run_warning()
      {
         A.CallTo(() => _view.NoFeedbackAvailable()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_intermediated_results_were_updated_and_the_feedback_is_switched_off : concern_for_ParameterIdentificationFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;

      protected override void Context()
      {
         base.Context();
         _runState = A.Fake<ParameterIdentificationRunState>();
         sut.ShouldRefreshFeedback = false;
         A.CallTo(() => _paramterIdentification.IsSingleRun).Returns(true);
         sut.Handle(new ParameterIdentificationStartedEvent(_paramterIdentification));
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationIntermediateResultsUpdatedEvent(_paramterIdentification, _runState));
      }

      [Observation]
      public void should_not_update_the_active_feedback_presenter()
      {
         A.CallTo(() => _singleFeedbackPresenter.UpdateFeedback(_runState)).MustNotHaveHappened();
      }
   }


   public class When_the_parameter_identification_feedback_presenter_is_notified_that_intermediated_results_were_updated_and_the_vuew_is_hidden : concern_for_ParameterIdentificationFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;

      protected override void Context()
      {
         base.Context();
         _runState = A.Fake<ParameterIdentificationRunState>();
         sut.ShouldRefreshFeedback = true;
         A.CallTo(() => _view.Visible).Returns(false);
         A.CallTo(() => _paramterIdentification.IsSingleRun).Returns(true);
         sut.Handle(new ParameterIdentificationStartedEvent(_paramterIdentification));
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationIntermediateResultsUpdatedEvent(_paramterIdentification, _runState));
      }

      [Observation]
      public void should_not_update_the_active_feedback_presenter()
      {
         A.CallTo(() => _singleFeedbackPresenter.UpdateFeedback(_runState)).MustNotHaveHappened();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_intermediated_results_were_updated_and_the_feedback_is_switched_on_and_the_view_is_visible : concern_for_ParameterIdentificationFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;

      protected override void Context()
      {
         base.Context();
         _runState = A.Fake<ParameterIdentificationRunState>();
         sut.ShouldRefreshFeedback = true;
         A.CallTo(() => _view.Visible).Returns(true);
         A.CallTo(() => _paramterIdentification.IsSingleRun).Returns(true);
         sut.Handle(new ParameterIdentificationStartedEvent(_paramterIdentification));
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationIntermediateResultsUpdatedEvent(_paramterIdentification, _runState));
      }

      [Observation]
      public void should_update_the_active_feedback_presenter()
      {
         A.CallTo(() => _singleFeedbackPresenter.UpdateFeedback(_runState)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_feedback_presenter_is_notified_that_the_current_project_was_closed : concern_for_ParameterIdentificationFeedbackPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ProjectClosedEvent());
      }

      [Observation]
      public void should_clear_and_reset_the_feedback_presenters()
      {
         A.CallTo(() => _multipleFeedbackPresenter.ClearReferences()).MustHaveHappened();
         A.CallTo(() => _singleFeedbackPresenter.ClearReferences()).MustHaveHappened();

         A.CallTo(() => _multipleFeedbackPresenter.ResetFeedback()).MustHaveHappened();
         A.CallTo(() => _singleFeedbackPresenter.ResetFeedback()).MustHaveHappened();   
      }
   }
}