using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CreateSensisitivityAnalysisBasedOnSimulationUICommand : ObjectUICommand<ISimulation>
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CreateSensisitivityAnalysisBasedOnSimulationUICommand(ISensitivityAnalysisTask sensitivityAnalysisTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_sensitivityAnalysisTask.CreateSensitivityAnalysisFor(Subject));
      }
   }
}