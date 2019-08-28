using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using CorePersistor = OSPSuite.Core.Serialization.Exchange.ISimulationPersistor;

namespace OSPSuite.R.Services
{
   public interface ISimulationPersister
   {
      IModelCoreSimulation LoadSimulation(string fileName);
      void SaveSimulation(IModelCoreSimulation simulation, string fileName);
   }

   public class SimulationPersister : ISimulationPersister
   {
      private readonly CorePersistor _simulationPersistor;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public SimulationPersister(
         CorePersistor simulationPersistor,
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

      public void SaveSimulation(IModelCoreSimulation simulation, string fileName)
      {
         var simulationTransfer = new SimulationTransfer {Simulation = simulation};
         _simulationPersistor.Save(simulationTransfer, fileName);
      }
   }
}