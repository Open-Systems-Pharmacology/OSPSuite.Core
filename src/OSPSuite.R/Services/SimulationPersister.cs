using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.R.Domain;
using CorePersistor = OSPSuite.Core.Serialization.Exchange.ISimulationPersistor;

namespace OSPSuite.R.Services
{
   public interface ISimulationPersister
   {
      Simulation LoadSimulation(string fileName, bool resetIds = true);
      void SaveSimulation(Simulation simulation, string fileName);
   }

   public class SimulationPersister : ISimulationPersister
   {
      private readonly CorePersistor _simulationPersistor;
      private readonly IObjectIdResetter _objectIdResetter;

      public SimulationPersister(
         CorePersistor simulationPersistor,
         IDimensionFactory dimensionFactory,
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel,
         IObjectIdResetter objectIdResetter)
      {
         _simulationPersistor = simulationPersistor;
         _objectIdResetter = objectIdResetter;
      }

      public Simulation LoadSimulation(string fileName, bool resetIds = true)
      {
         var simulationTransfer = _simulationPersistor.Load(fileName);
         var simulation = simulationTransfer.Simulation;
         if(resetIds)
            _objectIdResetter.ResetIdFor(simulation);

         return new Simulation(simulation);
      }

      public void SaveSimulation(Simulation simulation, string fileName)
      {
         var simulationTransfer = new SimulationTransfer {Simulation = simulation.CoreSimulation};
         _simulationPersistor.Save(simulationTransfer, fileName);
      }
   }
}