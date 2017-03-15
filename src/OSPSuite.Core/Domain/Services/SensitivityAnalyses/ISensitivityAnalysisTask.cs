using System.Collections.Generic;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisTask
   {
      /// <summary>
      ///    Creates an empty <see cref="SensitivityAnalysis" />
      /// </summary>
      SensitivityAnalysis CreateSensitivityAnalysis();

      void AddToProject(SensitivityAnalysis sensitivityAnalysis);
      /// <summary>
      /// Asks the user if he really wants to delete the <paramref name="sensitivityAnalyses"/>. Returns <c>true</c> if the <paramref name="sensitivityAnalyses"/>
      /// are deleted from the current project otherwise <c>false</c>
      /// </summary>
      bool Delete(IReadOnlyList<SensitivityAnalysis> sensitivityAnalyses);

      void SwapSimulations(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation);
      bool ValidateSwap(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation);
      SensitivityAnalysis CreateSensitivityAnalysisFor(ISimulation simulation);
      SensitivityAnalysis Clone(SensitivityAnalysis sensitivityAnalysis);
   }
}