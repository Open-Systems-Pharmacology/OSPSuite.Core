using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;

namespace OSPSuite.R.Services
{
   public interface ISimulationRunner
   {
      Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
      SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);

      SimulationResults RunSimulation(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null);
      Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null);
   }

   public class SimulationRunner : ISimulationRunner
   {
      private readonly ISimModelManager _simModelManager;
      private readonly IPopulationRunner _populationRunner;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IPopulationTask _populationTask;

      public SimulationRunner(
         ISimModelManager simModelManager,
         IPopulationRunner populationRunner,
         ISimulationResultsCreator simulationResultsCreator,
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IPopulationTask populationTask)
      {
         _simModelManager = simModelManager;
         _populationRunner = populationRunner;
         _simulationResultsCreator = simulationResultsCreator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _populationTask = populationTask;
      }

      public SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, simulationRunOptions).Result;
      }

      public SimulationResults RunSimulation(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, population, simulationRunOptions).Result;
      }

      public async Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();
         _populationRunner.NumberOfCoresToUse = options.NumberOfCoresToUse;
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
         var populationRunResults = await _populationRunner.RunPopulationAsync(simulation, _populationTask.PopulationTableFrom(population));
         return populationRunResults.Results;
      }

      public Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return Task.Run(() =>
         {
            _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
            var simulationResults = _simModelManager.RunSimulation(simulation, coreSimulationRunOptionsFrom(simulationRunOptions));
            return _simulationResultsCreator.CreateResultsFrom(simulationResults.Results);
         });
      }

      private Core.Domain.SimulationRunOptions coreSimulationRunOptionsFrom(SimulationRunOptions simulationRunOptions)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();
         return new Core.Domain.SimulationRunOptions
         {
            CheckForNegativeValues = options.CheckForNegativeValues,
            SimModelExportMode = SimModelExportMode.Optimized
         };
      }
   }
}