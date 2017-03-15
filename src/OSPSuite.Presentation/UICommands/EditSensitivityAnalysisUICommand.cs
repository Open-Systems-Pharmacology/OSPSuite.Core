using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class EditSensitivityAnalysisUICommand : ObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public EditSensitivityAnalysisUICommand(ISingleStartPresenterTask singleStartPresenterTask)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(Subject);
      }
   }
}