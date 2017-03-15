using System.Drawing;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisFeedbackPresenter : IPresenter<ISensitivityAnalysisFeedbackView>, IToogleablePresenter,
      IListener<SensitivityAnalysisStartedEvent>,
      IListener<SensitivityAnalysisTerminatedEvent>,
      IListener<SensitivityAnalysisProgressEvent>

   {
   }

   public class SensitivityAnalysisFeedbackPresenter : AbstractToggleablePresenter<ISensitivityAnalysisFeedbackView, ISensitivityAnalysisFeedbackPresenter>, ISensitivityAnalysisFeedbackPresenter
   {
      private readonly IPresentationUserSettings _presentationUserSettings;

      public SensitivityAnalysisFeedbackPresenter(ISensitivityAnalysisFeedbackView view, IPresentationUserSettings presentationUserSettings) : base(view)
      {
         _presentationUserSettings = presentationUserSettings;
      }

      public override void Display()
      {
         DisplayViewAt(feedbackEditorSettings.Location, feedbackEditorSettings.Size);
      }

      private SensitivityAnalysisFeedbackEditorSettings feedbackEditorSettings => _presentationUserSettings.SensitivityAnalysisFeedbackEditorSettings;

      protected override void SaveFormLayout(Point location, Size size)
      {
         feedbackEditorSettings.Location = location;
         feedbackEditorSettings.Size = size;
      }

      public void Handle(SensitivityAnalysisStartedEvent eventToHandle)
      {
         _view.ResetFeedback();
         _view.ShowFeedback();
      }

      public void Handle(SensitivityAnalysisTerminatedEvent eventToHandle)
      {
         _view.ResetFeedback();
      }

      public void Handle(SensitivityAnalysisProgressEvent eventToHandle)
      {
         _view.UpdateProgress(eventToHandle.NumberOfCalculatedSimulation, eventToHandle.NumberOfSimulations);
      }
   }
}
