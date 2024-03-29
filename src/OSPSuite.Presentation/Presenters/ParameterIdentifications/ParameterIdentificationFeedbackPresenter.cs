﻿using System;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Events;

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
      IListener<ParameterIdentificationIntermediateResultsUpdatedEvent>,
      IListener<ProjectClosedEvent>,
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>,
      ISingleStartPresenter<ParameterIdentificationFeedback>
   {
      bool ShouldRefreshFeedback { get; set; }
   }

   public class ParameterIdentificationFeedbackPresenter : AbstractToggleablePresenter<IParameterIdentificationFeedbackView, IParameterIdentificationFeedbackPresenter>, IParameterIdentificationFeedbackPresenter
   {
      private readonly IPresentationUserSettings _presenterUserSettings;
      private readonly ISingleParameterIdentificationFeedbackPresenter _singleFeedbackPresenter;
      private readonly IMultipleParameterIdentificationFeedbackPresenter _multipleFeedbackPresenter;
      private ParameterIdentification _parameterIdentification;
      private ParameterIdentificationFeedback _parameterIdentificationFeedback;
      private IParameterIdentificationRunFeedbackPresenter _activeFeedbackPresenter;

      public event EventHandler Closing = delegate { };

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
         if (!Equals(_parameterIdentificationFeedback.ParameterIdentification, eventToHandle.ParameterIdentification))
            return;

         setParameterIdentificationToStarted(eventToHandle.ParameterIdentification);
      }

      private void setParameterIdentificationToStarted(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
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
         if (!Equals(_parameterIdentification, eventToHandle.ParameterIdentification))
            return;
         setParameterIdentificationToTerminated();
      }

      private void setParameterIdentificationToTerminated()
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
         closeAndClearReferences();
      }

      private void closeAndClearReferences()
      {
         clearFeedbackReferences();
         resetFeedback();
         _view.NoFeedbackAvailable();
         _view.Hide();
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
         _activeFeedbackPresenter?.UpdateFeedback(state);
      }

      public void OnFormClosed()
      {
         //Nothing to do here
      }

      public void Close()
      {
         closeAndClearReferences();
      }

      public IPresentationSettings GetSettings()
      {
         //Nothing to do here
         return null;
      }

      public void RestoreSettings(IPresentationSettings settings)
      {
         //Nothing to do here
      }

      public void SaveChanges()
      {
         //Nothing to do here
      }

      public void Activated()
      {
         //Nothing to do here
      }

      public void Handle(RenamedEvent eventToHandle)
      {
         //Nothing to do here
      }

      public void Edit(ParameterIdentificationFeedback parameterIdentificationFeedback)
      {
         _parameterIdentificationFeedback = parameterIdentificationFeedback;

         switch (parameterIdentificationFeedback.RunStatus)
         {
            case RunStatusId.Running:
               setParameterIdentificationToStarted(parameterIdentificationFeedback.ParameterIdentification);
               break;
            case RunStatusId.Canceled:
               // Presenter was opened after the events were triggered but actually a terminated
               // event is triggered after a started event was triggered and the feedback view
               // should evolve through both of them. If terminated is invoked directly, then the
               // view would should its initial state asking the user to start the parameter identification
               // instead of showing the terminated parameter identification and its last state.
               setParameterIdentificationToStarted(parameterIdentificationFeedback.ParameterIdentification);
               setParameterIdentificationToTerminated();
               break;
         }

         _view.Display();
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit as ParameterIdentificationFeedback);
      }

      private bool canRefresh => ShouldRefreshFeedback && _view.Visible;

      //Used as an id to check if the subject already has a presenter associated
      public object Subject => _parameterIdentificationFeedback;
   }
}