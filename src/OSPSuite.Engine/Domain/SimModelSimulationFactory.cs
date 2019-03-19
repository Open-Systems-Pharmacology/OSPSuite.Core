using SimModelNET;

namespace OSPSuite.Engine.Domain
{
   public interface ISimModelSimulationFactory
   {
      SimModelNET.ISimulation Create();
   }

   public class SimModelSimulationFactory : ISimModelSimulationFactory
   {
      public SimModelNET.ISimulation Create()
      {
         return new Simulation();
      }
   }
}