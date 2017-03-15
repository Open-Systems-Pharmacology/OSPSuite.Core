using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Events
{
   public class SimulationRemovedEvent
   {
      public SimulationRemovedEvent(ISimulation objectBeingRemoved)
      {
         Simulation = objectBeingRemoved;
      }

      public ISimulation Simulation { get; set; }
   }
}
