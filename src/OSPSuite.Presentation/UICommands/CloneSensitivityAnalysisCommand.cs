using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CloneSensitivityAnalysisCommand : ObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CloneSensitivityAnalysisCommand(ISensitivityAnalysisTask sensitivityAnalysisTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      protected override void PerformExecute()
      {
         var clone = _sensitivityAnalysisTask.Clone(Subject);
         if (clone == null)
            return;

         _sensitivityAnalysisTask.AddToProject(clone);
         _singleStartPresenterTask.StartForSubject(clone);
      }
   }
}