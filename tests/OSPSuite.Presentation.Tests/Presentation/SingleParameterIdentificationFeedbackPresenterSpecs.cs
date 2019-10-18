using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SingleParameterIdentificationFeedbackPresenter : ContextSpecification<ISingleParameterIdentificationFeedbackPresenter>
   {
      protected IParameterIdentificationParametersFeedbackPresenter _parametersFeedbackPresenter;
      protected ISingleParameterIdentificationFeedbackView _view;
      protected IParameterIdentificationPredictedVsObservedFeedbackPresenter _predictedVsObservedFeedbackPresenter;
      protected IParameterIdentificationTimeProfileFeedbackPresenter _timeProfileFeedbackPresenter;

      protected ParameterIdentificationRunState _runState;
      protected ParameterIdentification _parameterIdentification;
      protected IParameterIdentificationErrorHistoryFeedbackPresenter _errroHistorFeedbackPresenter;

       protected override void Context()
      {
         _view = A.Fake<ISingleParameterIdentificationFeedbackView>();
         _parametersFeedbackPresenter = A.Fake<IParameterIdentificationParametersFeedbackPresenter>();
         _predictedVsObservedFeedbackPresenter = A.Fake<IParameterIdentificationPredictedVsObservedFeedbackPresenter>();
         _timeProfileFeedbackPresenter = A.Fake<IParameterIdentificationTimeProfileFeedbackPresenter>();
         _errroHistorFeedbackPresenter= A.Fake<IParameterIdentificationErrorHistoryFeedbackPresenter>();    

         sut = new SingleParameterIdentificationFeedbackPresenter(_view, _parametersFeedbackPresenter,
             _predictedVsObservedFeedbackPresenter, _timeProfileFeedbackPresenter, _errroHistorFeedbackPresenter);

         _runState = A.Fake<ParameterIdentificationRunState>();
         _parameterIdentification = new ParameterIdentification();
      }
   }

   public class When_the_single_parameter_identification_parameter_feedback_presenter_is_editing_a_parameter_identification : concern_for_SingleParameterIdentificationFeedbackPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_add_the_parameter_view_to_the_view()
      {
         A.CallTo(() => _view.AddParameterView(_parametersFeedbackPresenter.BaseView)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_the_parameter_identification_in_the_parameters_feedback_presenter()
      {
         A.CallTo(() => _parametersFeedbackPresenter.EditParameterIdentification(_parameterIdentification)).MustHaveHappened();
      }
   }

   public class When_the_single_parameter_identification_parameter_feedback_presenter_is_told_to_update_the_feedback : concern_for_SingleParameterIdentificationFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
         //a little more than the default refresh time to ensure that the call is being triggered
         Thread.Sleep(Constants.FEEDBACK_REFRESH_TIME + 1000);
      }

      [Observation]
      public void should_tell_the_paramters_feedback_presenter_to_update_the_parameters()
      {
         A.CallTo(() => _parametersFeedbackPresenter.UpdateFeedback(_runState)).MustHaveHappened();
      }
   }

   public class When_the_single_parameter_identification_parameter_feedback_presenter_is_told_to_update_the_feedback_twice : concern_for_SingleParameterIdentificationFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
         sut.UpdateFeedback(_runState);
         Thread.Sleep(Constants.FEEDBACK_REFRESH_TIME + 500);
      }

      [Observation]
      public void should_tell_the_paramters_feedback_presenter_to_update_the_parameters_only_once()
      {
         A.CallTo(() => _parametersFeedbackPresenter.UpdateFeedback(_runState)).MustHaveHappenedOnceExactly();
      }
   }

   public class When_the_single_parameter_identification_feedback_presenter_is_resetting_the_feedback : concern_for_SingleParameterIdentificationFeedbackPresenter
   {
      protected override void Because()
      {
         sut.ResetFeedback();
      }

      [Observation]
      public void should_reset_feedback_in_all_sub_presenters()
      {
         A.CallTo(() => _parametersFeedbackPresenter.ResetFeedback()).MustHaveHappened();
         A.CallTo(() => _timeProfileFeedbackPresenter.ResetFeedback()).MustHaveHappened();
         A.CallTo(() => _errroHistorFeedbackPresenter.ResetFeedback()).MustHaveHappened();
      }
   }
}