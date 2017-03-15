using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisSimulationSwapCorrector
   {
      void CorrectSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation);
   }

   public class SensitivityAnalysisSimulationSwapCorrector : SensitivityAnalysisSimulationSwap, ISensitivityAnalysisSimulationSwapCorrector
   {
      public SensitivityAnalysisSimulationSwapCorrector(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder) : base(simulationQuantitySelectionFinder)
      {
      }

      public void CorrectSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation)
      {
         FindProblems(sensitivityAnalysis, oldSimulation, newSimulation);
      }

      protected override void ActionForMissingSensitivityParameterPaths(SensitivityAnalysis sensitivityAnalysis, ISimulation newSimulation, SensitivityParameter sensitivityParameter)
      {
         sensitivityAnalysis.RemoveSensitivityParameter(sensitivityParameter);
      }
   }
}