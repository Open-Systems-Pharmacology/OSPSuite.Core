using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface ISingleParameterIdentificationFeedbackPresenter : IPresenter<ISingleParameterIdentificationFeedbackView>, IParameterIdentificationRunFeedbackPresenter
   {
   }

   public class SingleParameterIdentificationFeedbackPresenter : AbstractPresenter<ISingleParameterIdentificationFeedbackView, ISingleParameterIdentificationFeedbackPresenter>, ISingleParameterIdentificationFeedbackPresenter
   {
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
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         allRunFeedbackPresenters.Each(x => x.EditParameterIdentification(parameterIdentification));
      }

      public void ClearReferences()
      {
         allRunFeedbackPresenters.Each(x => x.ClearReferences());
      }

      public void ResetFeedback()
      {
         allRunFeedbackPresenters.Each(x => x.ResetFeedback());
      }

      public void UpdateFeedback(ParameterIdentificationRunState runState)
      {
         allRunFeedbackPresenters.Each(x => x.UpdateFeedback(runState));
      }

      private IEnumerable<IParameterIdentificationRunFeedbackPresenter> allRunFeedbackPresenters => AllSubPresenters.OfType<IParameterIdentificationRunFeedbackPresenter>();
   }
}