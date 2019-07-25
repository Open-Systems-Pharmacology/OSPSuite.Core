using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public interface ISimulationLoader
   {
      IModelCoreSimulation LoadSimulation(string fileName);
   }

   public class SimulationLoader : ISimulationLoader
   {
      private readonly ISimulationPersistor _simulationPersistor;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public SimulationLoader() : this(
         IoC.Resolve<ISimulationPersistor>(),
         IoC.Resolve<IDimensionFactory>(),
         IoC.Resolve<IObjectBaseFactory>(),
         IoC.Resolve<ICloneManagerForModel>()
      )
      {
      }

      internal SimulationLoader(
         ISimulationPersistor simulationPersistor,
         IDimensionFactory dimensionFactory,
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel)
      {
         _simulationPersistor = simulationPersistor;
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IModelCoreSimulation LoadSimulation(string fileName)
      {
         var simulationTransfer = _simulationPersistor.Load(fileName, _dimensionFactory, _objectBaseFactory, new WithIdRepository(), _cloneManagerForModel);
         return simulationTransfer.Simulation;
      }
   }
}