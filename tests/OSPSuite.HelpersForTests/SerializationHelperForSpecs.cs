using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;

namespace OSPSuite.Helpers
{
   public static class SerializationHelperForSpecs
   {
      public static SimulationTransfer Load(string fileName)
      {
         var simulationPersister = IoC.Resolve<ISimulationPersistor>();
         return simulationPersister.Load(fileName, IoC.Resolve<IDimensionFactory>(), IoC.Resolve<IObjectBaseFactory>(), IoC.Resolve<IWithIdRepository>(), IoC.Resolve<ICloneManagerForModel>());
      }
   }
}