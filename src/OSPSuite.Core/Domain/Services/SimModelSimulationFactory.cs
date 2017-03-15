using SimModelNET;

namespace OSPSuite.Core.Domain.Services
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