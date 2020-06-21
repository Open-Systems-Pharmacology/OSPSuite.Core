using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CloneSensitivityAnalysisCommand : ObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;
      private readonly IApplicationController _applicationController;

      public CloneSensitivityAnalysisCommand(
         ISensitivityAnalysisTask sensitivityAnalysisTask,
         ISingleStartPresenterTask singleStartPresenterTask,
         IApplicationController applicationController)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _singleStartPresenterTask = singleStartPresenterTask;
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var clonePresenter = _applicationController.Start<ICloneObjectBasePresenter<SensitivityAnalysis>>())
         {
            var clone = clonePresenter.CreateCloneFor(Subject);
            if (clone == null)
               return;

            _sensitivityAnalysisTask.AddToProject(clone);
            _singleStartPresenterTask.StartForSubject(clone);
         }
      }
   }
}