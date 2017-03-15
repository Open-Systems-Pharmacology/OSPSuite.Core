using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class RunSensitivityAnalysisUICommand : ActiveObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisRunner _sensitivityAnalysisRunner;

      public RunSensitivityAnalysisUICommand(ISensitivityAnalysisRunner sensitivityAnalysisRunner, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _sensitivityAnalysisRunner = sensitivityAnalysisRunner;
      }

      protected override async void PerformExecute()
      {
         await _sensitivityAnalysisRunner.SecureAwait(x => x.Run(Subject));
      }
   }
}