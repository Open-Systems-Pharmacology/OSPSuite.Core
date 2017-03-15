using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Tasks
{
   public class SimulationSelector : ISimulationSelector
   {
      public bool SimulationCanBeUsedForIdentification(ISimulation simulation)
      {
         return true;
      }

      public bool SimulationCanBeUsedForSensitivityAnalysis(ISimulation simulation)
      {
         return true;
      }
   }
}
