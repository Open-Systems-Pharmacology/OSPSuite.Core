using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public abstract class SensitivityAnalysisSimulationSwap
   {
      private readonly ISimulationQuantitySelectionFinder _simulationQuantitySelectionFinder;

      protected SensitivityAnalysisSimulationSwap(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder)
      {
         _simulationQuantitySelectionFinder = simulationQuantitySelectionFinder;
      }

      protected abstract void ActionForMissingSensitivityParameterPaths(SensitivityAnalysis sensitivityAnalysis, ISimulation newSimulation, SensitivityParameter sensitivityParameter);

      protected void FindProblems(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation)
      {
         findMissingSensitivityParamterPaths(sensitivityAnalysis, newSimulation).Each(x => ActionForMissingSensitivityParameterPaths(sensitivityAnalysis, newSimulation, x));
      }

      private IEnumerable<SensitivityParameter> findMissingSensitivityParamterPaths(SensitivityAnalysis sensitivityAnalysis, ISimulation newSimulation)
      {
         return sensitivityAnalysis.AllSensitivityParameters.ToList().Where(x => !_simulationQuantitySelectionFinder.SimulationHasSelection(x.ParameterSelection, newSimulation));
      }
   }
}