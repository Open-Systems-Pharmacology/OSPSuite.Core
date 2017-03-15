using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public static class SimulationResultsExtensions
   {
      public static bool IsNull(this SimulationResults simulationResults)
      {
         return simulationResults == null || simulationResults.IsAnImplementationOf<NullSimulationResults>();
      }
   }
}