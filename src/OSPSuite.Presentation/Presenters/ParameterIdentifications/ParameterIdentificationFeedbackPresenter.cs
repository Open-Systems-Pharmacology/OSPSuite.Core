using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationRunFeedbackPresenter : IParameterIdentificationPresenter
   {
      /// <summary>
      ///    Removes all references to parameter identification objects but do not clear the output
      /// </summary>
      void ClearReferences();

      /// <summary>
      ///    Clears the output
      /// </summary>
      void ResetFeedback();

      void UpdateFeedback(ParameterIdentificationRunState runState);
   }

   public interface IParameterIdentificationFeedbackPresenter : IPresenter<IParameterIdentificationFeedbackView>,
      IToogleablePresenter,
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>,
      IListener<ParameterIdentificationIntermediateResultsUpdatedEvent>,
      IListener<ProjectClosedEvent>
   {
      bool ShouldRefreshFeedback { get; set; }
   }

   public class ParameterIdentificationFeedbackPresenter : AbstractToggleablePresenter<IParameterIdentificationFeedbackView, IParameterIdentificationFeedbackPresenter>, IParameterIdentificationFeedbackPresenter
   {
      private readonly IPresentationUserSettings _presenterUserSettings;
      private readonly ISingleParameterIdentificationFeedbackPresenter _singleFeedbackPresenter;
      private readonly IMultipleParameterIdentificationFeedbackPresenter _multipleFeedbackPresenter;
      private ParameterIdentification _parameterIdentification;
      private IParameterIdentificationRunFeedbackPresenter _activeFeedbackPresenter;
      public bool ShouldRefreshFeedback { get; set; }
      private ParameterIdentificationFeedbackEditorSettings feedbackEditorSettings => _presenterUserSettings.ParameterIdentificationFeedbackEditorSettings;

      public ParameterIdentificationFeedbackPresenter(IParameterIdentificationFeedbackView view, IPresentationUserSettings presenterUserSettings,
         ISingleParameterIdentificationFeedbackPresenter singleFeedbackPresenter, IMultipleParameterIdentificationFeedbackPresenter multipleFeedbackPresenter) : base(view)
      {
         _presenterUserSettings = presenterUserSettings;
         _singleFeedbackPresenter = singleFeedbackPresenter;
         _multipleFeedbackPresenter = multipleFeedbackPresenter;

         AddSubPresenters(_singleFeedbackPresenter, _multipleFeedbackPresenter);
         ShouldRefreshFeedback = feedbackEditorSettings.RefreshFeedback;
         _view.BindToProperties();
         _view.NoFeedbackAvailable();
         _activeFeedbackPresenter = null;
      }

      public override void Display()
      {
         DisplayViewAt(feedbackEditorSettings.Location, feedbackEditorSettings.Size);
      }

      protected override void SaveFormLayout(Point location, Size size)
      {
         feedbackEditorSettings.Location = location;
         feedbackEditorSettings.Size = size;
      }

      public void Handle(ParameterIdentificationStartedEvent eventToHandle)
      {
         _parameterIdentification = eventToHandle.ParameterIdentification;
         _view.Caption = Captions.ParameterIdentification.FeedbackViewFor(_parameterIdentification.Name);
         showParameterIdentificationFeedback();
      }

      private void showParameterIdentificationFeedback()
      {
         if (_parameterIdentification == null)
            return;

         if (_parameterIdentification.IsSingleRun)
            showSingleRunFeedback();
         else
            showMultipleRunFeedback();
      }

      private void showMultipleRunFeedback()
      {
         showRunFeedback(_multipleFeedbackPresenter);
      }

      private void showSingleRunFeedback()
      {
         showRunFeedback(_singleFeedbackPresenter);
      }

      private void showRunFeedback(IParameterIdentificationRunFeedbackPresenter feedbackPresenterToShow)
      {
         resetFeedback();
         _activeFeedbackPresenter = feedbackPresenterToShow;
         feedbackPresenterToShow.EditParameterIdentification(_parameterIdentification);
         _view.ShowFeedbackView(feedbackPresenterToShow.BaseView);
      }

      public void Handle(ParameterIdentificationTerminatedEvent eventToHandle)
      {
         _parameterIdentification = null;
         clearFeedbackReferences();
      }

      private void clearFeedbackReferences()
      {
         _singleFeedbackPresenter.ClearReferences();
         _multipleFeedbackPresenter.ClearReferences();
      }

      private void resetFeedback()
      {
         _singleFeedbackPresenter.ResetFeedback();
         _multipleFeedbackPresenter.ResetFeedback();
         _activeFeedbackPresenter = null;
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         clearFeedbackReferences();
         resetFeedback();
         _view.NoFeedbackAvailable();
      }

      public void Handle(ParameterIdentificationIntermediateResultsUpdatedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         if (!canRefresh)
            return;

         updateFeedback(eventToHandle.State);
      }

      private bool canHandle(ParameterIdentificationEvent eventToHandle)
      {
         return Equals(eventToHandle.ParameterIdentification, _parameterIdentification);
      }

      private void updateFeedback(ParameterIdentificationRunState state)
      {
         _activeFeedbackPresenter.UpdateFeedback(state);
      }

      private bool canRefresh => ShouldRefreshFeedback && _view.Visible;
   }
}