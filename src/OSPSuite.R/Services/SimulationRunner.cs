using System;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public class SimulationRunArgs
   {
      public IModelCoreSimulation Simulation { get; set; }
      public SimulationRunOptions SimulationRunOptions { get; set; }
      public IndividualValuesCache Population { get; set; }
      public AgingData AgingData { get; set; }

      public void Deconstruct(out IModelCoreSimulation simulation, out IndividualValuesCache population, out AgingData agingData,
         out SimulationRunOptions simulationRunOptions)
      {
         simulation = Simulation;
         simulationRunOptions = SimulationRunOptions;
         population = Population;
         agingData = AgingData;
      }
   }

   public interface ISimulationRunner
   {
      Task<SimulationResults> RunAsync(SimulationRunArgs simulationRunArgs);
      SimulationResults Run(SimulationRunArgs simulationRunArgs);
   }

   public class SimulationRunner : ISimulationRunner
   {
      private readonly ISimModelManager _simModelManager;
      private readonly IPopulationRunner _populationRunner;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IPopulationTask _populationTask;
      private readonly IProgressManager _progressManager;
      private IProgressUpdater _progressUpdater;

      public SimulationRunner(
         ISimModelManager simModelManager,
         IPopulationRunner populationRunner,
         ISimulationResultsCreator simulationResultsCreator,
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IPopulationTask populationTask,
         IProgressManager progressManager)
      {
         _simModelManager = simModelManager;
         _populationRunner = populationRunner;
         _simulationResultsCreator = simulationResultsCreator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _populationTask = populationTask;
         _progressManager = progressManager;
      }

      private void simulationProgress(object sender, MultipleSimulationsProgressEventArgs e)
      {
         _progressUpdater.ReportProgress(e.NumberOfCalculatedSimulation, e.NumberOfSimulations,
            Messages.CalculationPopulationSimulation(e.NumberOfCalculatedSimulation, e.NumberOfSimulations));
      }

      private void simulationTerminated()
      {
         terminated(this, EventArgs.Empty);
      }

      private void terminated(object sender, EventArgs e)
      {
         _progressUpdater?.Dispose();
         _populationRunner.Terminated -= terminated;
         _populationRunner.SimulationProgress -= simulationProgress;
      }

      private async Task<SimulationResults> runAsync(
         IModelCoreSimulation simulation, 
         IndividualValuesCache population, 
         AgingData agingData = null,
         SimulationRunOptions simulationRunOptions = null)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();
         initializeProgress(options);
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
         try
         {
            var populationRunResults = await _populationRunner.RunPopulationAsync(
               simulation,
               options,
               populationData: _populationTask.PopulationTableFrom(population, simulation),
               agingData: agingData?.ToDataTable()
            );
            return populationRunResults.Results;
         }
         finally
         {
            simulationTerminated();
         }
      }

      private void initializeProgress(SimulationRunOptions options)
      {
         _populationRunner.Terminated += terminated;
         _populationRunner.SimulationProgress += simulationProgress;
         _progressUpdater = options.ShowProgress ? _progressManager.Create() : new NoneProgressUpdater();
      }

      private async Task<SimulationResults> runAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions)
      {
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
         var simulationResults = await _simModelManager.RunSimulationAsync(simulation, CancellationToken.None, coreSimulationRunOptionsFrom(simulationRunOptions));
         return _simulationResultsCreator.CreateResultsFrom(simulationResults.Results);
      }

      private Core.Domain.SimulationRunOptions coreSimulationRunOptionsFrom(SimulationRunOptions simulationRunOptions)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();
         return new Core.Domain.SimulationRunOptions
         {
            SimModelExportMode = SimModelExportMode.Optimized,
         };
      }

      public Task<SimulationResults> RunAsync(SimulationRunArgs simulationRunArgs)
      {
         var (simulation, population, agingData, simulationRunOptions) = simulationRunArgs;
         return population == null ? 
            runAsync(simulation, simulationRunOptions) : 
            runAsync(simulation, population, agingData, simulationRunOptions);
      }

      public SimulationResults Run(SimulationRunArgs simulationRunArgs)
      {
         var (simulation, population, agingData, simulationRunOptions) = simulationRunArgs;
         if (population != null)
            return runAsync(simulation, population, agingData, simulationRunOptions).Result; //Not really without a task
         return runAsync(simulation, simulationRunOptions).Result;
      }
   }
}