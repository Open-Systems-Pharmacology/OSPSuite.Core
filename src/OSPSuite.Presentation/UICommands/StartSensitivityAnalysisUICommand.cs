using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class StartSensitivityAnalysisUICommand : ActiveObjectUICommand<SensitivityAnalysis>
   {
      private readonly ISensitivityAnalysisPKParameterAnalysisCreator _sensitivityAnalysisPKParameterAnalysisCreator;

      public StartSensitivityAnalysisUICommand(IActiveSubjectRetriever activeSubjectRetriever, ISensitivityAnalysisPKParameterAnalysisCreator sensitivityAnalysisPKParameterAnalysisCreator) : base(activeSubjectRetriever)
      {
         _sensitivityAnalysisPKParameterAnalysisCreator = sensitivityAnalysisPKParameterAnalysisCreator;
      }

      protected override void PerformExecute()
      {
         _sensitivityAnalysisPKParameterAnalysisCreator.CreateAnalysisFor(Subject);
      }
   }
}