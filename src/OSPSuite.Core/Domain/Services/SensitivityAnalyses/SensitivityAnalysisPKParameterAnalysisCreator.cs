using OSPSuite.Core.Chart.SensitivityAnalyses;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisPKParameterAnalysisCreator : ISimulationAnalysisCreator
   {
      ISimulationAnalysis CreateAnalysisFor(SensitivityAnalysis sensitivityAnalysis);
   }

   public class SensitivityAnalysisPKParameterAnalysisCreator : ParameterAnalysableAnalysisCreator, ISensitivityAnalysisPKParameterAnalysisCreator
   {
      public SensitivityAnalysisPKParameterAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext executionContext, IObjectIdResetter objectIdResetter, IIdGenerator idGenerator)
         : base(containerTask, executionContext, objectIdResetter, idGenerator)
      {
      }

      public ISimulationAnalysis CreateAnalysisFor(SensitivityAnalysis sensitivityAnalysis)
      {
         return AnalysisFor<SensitivityAnalysisPKParameterAnalysis>(sensitivityAnalysis);
      }
   }
}
