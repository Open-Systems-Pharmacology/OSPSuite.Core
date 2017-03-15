using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class StopSensitivityAnalysisUICommand : ActiveObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisRunner _sensitivityAnalysisRunner;

      public StopSensitivityAnalysisUICommand(ISensitivityAnalysisRunner sensitivityAnalysisRunner, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _sensitivityAnalysisRunner = sensitivityAnalysisRunner;
      }

      protected override void PerformExecute()
      {
         _sensitivityAnalysisRunner.Stop();
      }
   }
}