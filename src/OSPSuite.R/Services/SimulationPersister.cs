using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
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
      private readonly ISimulationToSimulationTransferMapper _simulationTransferMapper;

      public SimulationPersister(CorePersistor simulationPersistor, IObjectIdResetter objectIdResetter, ISimulationToSimulationTransferMapper simulationTransferMapper)
      {
         _simulationPersistor = simulationPersistor;
         _objectIdResetter = objectIdResetter;
         _simulationTransferMapper = simulationTransferMapper;
      }

      public Simulation LoadSimulation(string fileName, bool resetIds = true)
      {
         var simulationTransfer = _simulationPersistor.Load(fileName);
         var simulation = simulationTransfer.Simulation;
         if (resetIds)
            _objectIdResetter.ResetIdFor(simulation);

         return new Simulation(simulation)
         {
            Changes = new SimulationChanges(simulation.Model.Root, simulationTransfer.OriginalQuantityValues, simulationTransfer.HasUntraceableChanges)
         };
      }

      public void SaveSimulation(Simulation simulation, string fileName)
      {
         _simulationPersistor.Save(_simulationTransferMapper.MapFrom(simulation), fileName);
      }
   }
}
