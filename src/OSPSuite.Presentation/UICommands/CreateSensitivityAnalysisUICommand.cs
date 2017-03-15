using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CreateSensitivityAnalysisUICommand : IUICommand
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CreateSensitivityAnalysisUICommand(ISensitivityAnalysisTask sensitivityAnalysisTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      public void Execute()
      {
         _singleStartPresenterTask.StartForSubject(_sensitivityAnalysisTask.CreateSensitivityAnalysis());
      }
   }
}