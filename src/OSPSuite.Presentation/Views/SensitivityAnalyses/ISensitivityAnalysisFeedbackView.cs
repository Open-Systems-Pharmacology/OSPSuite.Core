using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisFeedbackView : IView<ISensitivityAnalysisFeedbackPresenter>, IToggleableView
   {
      void ShowFeedback();
      void UpdateProgress(int progressAmount, int totalAmount);
      void ResetFeedback();
   }
}