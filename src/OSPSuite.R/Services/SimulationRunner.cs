using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public interface ISimulationRunner
   {
      Task<SimulationResults> RunAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
      SimulationResults Run(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);

      SimulationResults Run(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null);
      Task<SimulationResults> RunAsync(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null);

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
         _progressUpdater.ReportProgress(e.NumberOfCalculatedSimulation, e.NumberOfSimulations, Messages.CalculationPopulationSimulation(e.NumberOfCalculatedSimulation, e.NumberOfSimulations));
      }

      private void simulationTerminated()
      {
         terminated(this, new EventArgs());
      }

      private void terminated(object sender, EventArgs e)
      {
         _progressUpdater?.Dispose();
         _populationRunner.Terminated -= terminated;
         _populationRunner.SimulationProgress -= simulationProgress;
      }

      public SimulationResults Run(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunAsync(simulation, simulationRunOptions).Result;
      }

      public SimulationResults Run(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null)
      {
         return RunAsync(simulation, population, simulationRunOptions).Result;
      }

      public async Task<SimulationResults> RunAsync(IModelCoreSimulation simulation, IndividualValuesCache population, SimulationRunOptions simulationRunOptions = null)
      {
         var options = simulationRunOptions ?? new SimulationRunOptions();
         initializeProgress(options);
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
         try
         {
            var populationRunResults = await _populationRunner.RunPopulationAsync(simulation, options, _populationTask.PopulationTableFrom(population, simulation));
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

      public Task<SimulationResults> RunAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
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