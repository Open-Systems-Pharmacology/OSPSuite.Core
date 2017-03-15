using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface ISingleParameterIdentificationFeedbackView : IView<ISingleParameterIdentificationFeedbackPresenter>
   {
      void AddParameterView(IView view);
      void AddTimeProfileView(IView view);
      void AddErrorHistoryView(IView view);
      void AddPredictedVsObservedView(IView view);
   }
}