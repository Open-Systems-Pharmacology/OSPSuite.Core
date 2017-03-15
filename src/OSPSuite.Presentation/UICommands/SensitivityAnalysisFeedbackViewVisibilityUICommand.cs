using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.UICommands
{
   public class SensitivityAnalysisFeedbackViewVisibilityUICommand : ObjectUICommand<ISensitivityAnalysisFeedbackPresenter>
   {
      private readonly ISensitivityAnalysisFeedbackPresenter _feedbackPresenter;

      public SensitivityAnalysisFeedbackViewVisibilityUICommand(ISensitivityAnalysisFeedbackPresenter feedbackPresenter)
      {
         _feedbackPresenter = feedbackPresenter;
      }

      protected override void PerformExecute()
      {
         _feedbackPresenter.Display();
      }
   }
}