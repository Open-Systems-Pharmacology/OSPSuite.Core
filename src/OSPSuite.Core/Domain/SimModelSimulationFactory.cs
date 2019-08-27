using OSPSuite.SimModel;

namespace OSPSuite.Core.Domain
{
   public interface ISimModelSimulationFactory
   {
      Simulation Create();
   }

   public class SimModelSimulationFactory : ISimModelSimulationFactory
   {
      public Simulation Create()
      {
         return new Simulation();
      }
   }
}