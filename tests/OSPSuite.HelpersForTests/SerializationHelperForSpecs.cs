using OSPSuite.Utility.Container;
using OSPSuite.Core.Serialization.Exchange;

namespace OSPSuite.Helpers
{
   public static class SerializationHelperForSpecs
   {
      public static SimulationTransfer Load(string fileName)
      {
         var simulationPersister = IoC.Resolve<ISimulationPersistor>();
         return simulationPersister.Load(fileName);
      }
   }
}