using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ObjectUICommand<IParameterIdentificationFeedbackPresenter>
   {
      private readonly IParameterIdentificationFeedbackPresenter _feedbackPresenter;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(IParameterIdentificationFeedbackPresenter feedbackPresenter)
      {
         _feedbackPresenter = feedbackPresenter;
      }

      protected override void PerformExecute()
      {
         _feedbackPresenter.Display();
      }
   }
}