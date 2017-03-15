using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public interface ISimulationParameterOriginIdUpdater
   {
      /// <summary>
      ///    Update the origin.SimulationId in all parameters defined in the simulation
      /// </summary>
      /// <param name="simulation"></param>
      void UpdateSimulationId(ISimulation simulation);
   }

   public class SimulationParameterOriginIdUpdater : ISimulationParameterOriginIdUpdater
   {
      public void UpdateSimulationId(ISimulation simulation)
      {
         simulation.All<IParameter>().Each(p => p.Origin.SimulationId = simulation.Id);
      }
   }
}
