using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.R.Services
{
   public interface ISimulationRunner
   {
      Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
      SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
   }

   public class SimulationRunner : ISimulationRunner
   {
      private readonly ISimModelManager _simModelManager;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;

      public SimulationRunner(ISimModelManager simModelManager, ISimulationResultsCreator simulationResultsCreator, ISimulationPersistableUpdater simulationPersistableUpdater)
      {
         _simModelManager = simModelManager;
         _simulationResultsCreator = simulationResultsCreator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
      }

      public SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, simulationRunOptions).Result;
      }

      public Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return Task.Run(() =>
         {
            _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
            var simulationResults = _simModelManager.RunSimulation(simulation, simulationRunOptions);
            return _simulationResultsCreator.CreateResultsFrom(simulationResults.Results);
         });
      }
   }
}