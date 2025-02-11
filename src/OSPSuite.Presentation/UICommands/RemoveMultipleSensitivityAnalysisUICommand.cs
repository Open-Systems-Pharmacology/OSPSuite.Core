using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using System.Collections.Generic;

namespace OSPSuite.Presentation.UICommands
{
   public class RemoveMultipleSensitivityAnalysisUICommand : ObjectUICommand<IReadOnlyList<SensitivityAnalysis>>
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;

      public RemoveMultipleSensitivityAnalysisUICommand(ISensitivityAnalysisTask sensitivityAnalysisTask)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
      }

      protected override void PerformExecute()
      {
         _sensitivityAnalysisTask.Delete(Subject);
      }
   }
}