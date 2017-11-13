using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface ISingleParameterIdentificationFeedbackPresenter : IPresenter<ISingleParameterIdentificationFeedbackView>, IParameterIdentificationRunFeedbackPresenter
   {
   }

   public class SingleParameterIdentificationFeedbackPresenter : AbstractPresenter<ISingleParameterIdentificationFeedbackView, ISingleParameterIdentificationFeedbackPresenter>, ISingleParameterIdentificationFeedbackPresenter
   {
      private readonly Timer _timer;
      private ParameterIdentificationRunState _lastState;
      private ParameterIdentificationRunState _currentState;

      public SingleParameterIdentificationFeedbackPresenter(ISingleParameterIdentificationFeedbackView view,
         IParameterIdentificationParametersFeedbackPresenter parametersFeedbackPresenter,
         IParameterIdentificationPredictedVsObservedFeedbackPresenter predictedVsObservedFeedbackPresenter,
         IParameterIdentificationTimeProfileFeedbackPresenter timeProfileFeedbackPresenter,
         IParameterIdentificationErrorHistoryFeedbackPresenter errorHistoryFeedbackPresenter) : base(view)
      {
         _view.AddParameterView(parametersFeedbackPresenter.BaseView);
         _view.AddPredictedVsObservedView(predictedVsObservedFeedbackPresenter.BaseView);
         _view.AddTimeProfileView(timeProfileFeedbackPresenter.BaseView);
         _view.AddErrorHistoryView(errorHistoryFeedbackPresenter.BaseView);
         AddSubPresenters(parametersFeedbackPresenter, timeProfileFeedbackPresenter, errorHistoryFeedbackPresenter, predictedVsObservedFeedbackPresenter);

         _timer = new Timer(Constants.FEEDBACK_REFRESH_TIME)
         {
            SynchronizingObject = view as ISynchronizeInvoke
         };
         _timer.Elapsed += (o, e) => updateFeedbackIfRequired();
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         allRunFeedbackPresenters.Each(x => x.EditParameterIdentification(parameterIdentification));
         _timer.Start();
      }

      public void ClearReferences()
      {
         _timer.Stop();
         updateFeedback();
         allRunFeedbackPresenters.Each(x => x.ClearReferences());
      }

      public void ResetFeedback()
      {
         allRunFeedbackPresenters.Each(x => x.ResetFeedback());
         _lastState = null;
         _currentState = null;
      }

      public void UpdateFeedback(ParameterIdentificationRunState runState)
      {
         _currentState = runState;
      }

      private void updateFeedbackIfRequired()
      {
         if (_currentState == null)
            return;

         if (Equals(_lastState, _currentState))
            return;

         _lastState = _currentState;

         updateFeedback();
      }

      private void updateFeedback()
      {
         if (_lastState == null) return;
         allRunFeedbackPresenters.Each(x => x.UpdateFeedback(_lastState));
      }

      private IEnumerable<IParameterIdentificationRunFeedbackPresenter> allRunFeedbackPresenters => AllSubPresenters.OfType<IParameterIdentificationRunFeedbackPresenter>();
   }
}