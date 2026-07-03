using System;
using System.Collections.Generic;
using System.Linq;
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
using OSPSuite.Utility.Extensions;
using RSimulation = OSPSuite.R.Domain.Simulation;
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
      ConcurrencyManagerResult<SimulationResults>[] RunSimulations(SimulationRunOptions options, params RSimulation[] simulations);
   }

   public class SimulationRunner : ISimulationRunner
   {
      private readonly ISimModelManager _simModelManager;
      private readonly IPopulationRunner _populationRunner;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IPopulationTask _populationTask;
      private readonly IProgressManager _progressManager;
      private readonly IConcurrencyManager _concurrencyManager;
      private readonly Func<ISimulationRunner> _simulationRunnerFactory;

      public SimulationRunner(
         ISimModelManager simModelManager,
         IPopulationRunner populationRunner,
         ISimulationResultsCreator simulationResultsCreator,
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IPopulationTask populationTask,
         IProgressManager progressManager,
         IConcurrencyManager concurrencyManager,
         Func<ISimulationRunner> simulationRunnerFactory)
      {
         _simModelManager = simModelManager;
         _populationRunner = populationRunner;
         _simulationResultsCreator = simulationResultsCreator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _populationTask = populationTask;
         _progressManager = progressManager;
         _concurrencyManager = concurrencyManager;
         _simulationRunnerFactory = simulationRunnerFactory;
      }

      private async Task<SimulationResults> runPopulationAsync(
         IModelCoreSimulation simulation,
         IndividualValuesCache population,
         AgingData agingData,
         SimulationRunOptions simulationRunOptions)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();

         //progress state is local so population runs do not share mutable state on this instance
         var progressUpdater = options.ShowProgress ? _progressManager.Create() : new NoneProgressUpdater();

         void onSimulationProgress(object sender, MultipleSimulationsProgressEventArgs e) =>
            progressUpdater.ReportProgress(e.NumberOfCalculatedSimulation, e.NumberOfSimulations,
               Messages.CalculationPopulationSimulation(e.NumberOfCalculatedSimulation, e.NumberOfSimulations));

         _populationRunner.SimulationProgress += onSimulationProgress;
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
            _populationRunner.SimulationProgress -= onSimulationProgress;
            progressUpdater.Dispose();
         }
      }

      private async Task<SimulationResults> runIndividualAsync(IModelCoreSimulation simulation)
      {
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
         var simulationResults = await _simModelManager.RunSimulationAsync(
            simulation,
            CancellationToken.None,
            new Core.Domain.SimulationRunOptions { SimModelExportMode = SimModelExportMode.Optimized });
         return _simulationResultsCreator.CreateResultsFrom(simulationResults.Results);
      }

      public Task<SimulationResults> RunAsync(SimulationRunArgs simulationRunArgs)
      {
         var (simulation, population, agingData, simulationRunOptions) = simulationRunArgs;
         return population == null ?
            runIndividualAsync(simulation) :
            runPopulationAsync(simulation, population, agingData, simulationRunOptions);
      }

      public SimulationResults Run(SimulationRunArgs simulationRunArgs)
      {
         //Not really async without a task
         return RunAsync(simulationRunArgs).Result;
      }

      public ConcurrencyManagerResult<SimulationResults>[] RunSimulations(SimulationRunOptions options, params RSimulation[] simulations)
      {
         options ??= new SimulationRunOptions();

         var individualSimulations = simulations.Where(x => !x.IsPopulation).ToList();
         var populationSimulations = simulations.Where(x => x.IsPopulation).ToList();

         return runIndividuals(individualSimulations, options)
            .Concat(runPopulations(populationSimulations, options))
            .ToArray();
      }

      private IEnumerable<ConcurrencyManagerResult<SimulationResults>> runIndividuals(IReadOnlyList<RSimulation> individualSimulations, SimulationRunOptions options)
      {
         if (!individualSimulations.Any())
            return Enumerable.Empty<ConcurrencyManagerResult<SimulationResults>>();

         var coreSimulations = individualSimulations.Select(x => x.CoreSimulation).ToList();

         //a fresh runner per item gives each parallel individual its own (stateful) SimModelManager
         var resultsByData = _concurrencyManager.RunAsync(
            coreSimulations,
            (simulation, ct) => _simulationRunnerFactory().Run(new SimulationRunArgs
            {
               Simulation = simulation,
               SimulationRunOptions = options
            }),
            CancellationToken.None,
            options.NumberOfCoresToUse
         ).Result;

         return resultsByData.Values;
      }

      private IEnumerable<ConcurrencyManagerResult<SimulationResults>> runPopulations(IReadOnlyList<RSimulation> populationSimulations, SimulationRunOptions options) =>
         populationSimulations.Select(x => runPopulation(x, options));

      private ConcurrencyManagerResult<SimulationResults> runPopulation(RSimulation simulation, SimulationRunOptions options)
      {
         try
         {
            var results = runPopulationAsync(
               simulation.CoreSimulation,
               simulation.IndividualValuesCache,
               simulation.AgingData,
               options).Result;

            return new ConcurrencyManagerResult<SimulationResults>(simulation.Id, results);
         }
         catch (Exception e)
         {
            return new ConcurrencyManagerResult<SimulationResults>(simulation.Id, e.FullMessage());
         }
      }
   }
}