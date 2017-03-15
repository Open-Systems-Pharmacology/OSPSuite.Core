using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;

namespace OSPSuite.Presentation.UICommands
{
   public class DeleteSensitivityAnalysisUICommand : ObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;

      public DeleteSensitivityAnalysisUICommand(ISensitivityAnalysisTask sensitivityAnalysisTask)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
      }

      protected override void PerformExecute()
      {
         _sensitivityAnalysisTask.Delete(new[] {Subject });
      }
   }
}