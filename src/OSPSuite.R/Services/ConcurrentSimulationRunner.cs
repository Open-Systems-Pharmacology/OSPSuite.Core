using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Exceptions;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public class SettingsForConcurrentltyRunSimulationBatch
   {
      public IModelCoreSimulation Simulation { get; set; }
      public List<SimulationBatchRunValues> SimulationBatchRunValues { get; } = new List<SimulationBatchRunValues>();
      public SimulationBatchOptions SimulationBatchOptions { get; set; }
      internal ConcurrentQueue<SimulationBatch> _simulationBatches = new ConcurrentQueue<SimulationBatch>();
      public string AddSimulationBatchRunValues(SimulationBatchRunValues simulationBatchRunValues)
      {
         var id = Guid.NewGuid().ToString();
         simulationBatchRunValues.Id = id;
         SimulationBatchRunValues.Add(simulationBatchRunValues);
         return id;
      }
   }

   class SimulationBatchRunOptions
   {
      public IModelCoreSimulation Simulation { get; set; }
      public SimulationBatch SimulationBatch { get; set; }
      public SimulationBatchRunValues SimulationBatchRunValues { get; set; }
      public SimulationBatchOptions SimulationBatchOptions { get; set; }
   }

   public class ConcurrentSimulationResults
   {
      public ConcurrentSimulationResults(string id, SimulationResults results)
      {
         Id = id;
         SimulationResults = results;
      }
      public string Id { get; }
      public SimulationResults SimulationResults { get; }
   }

   public interface IConcurrentSimulationRunner : IDisposable
   {
      /// <summary>
      ///    General simulation options
      /// </summary>
      SimulationRunOptions SimulationRunOptions { get; set; }

      /// <summary>
      ///    Number of cores to use concurrently. Use 0 or a negative value for using the maximum available.
      /// </summary>
      int NumberOfCores { get; set; }

      /// <summary>
      ///    Adds a simulation to the list of Simulations
      /// </summary>
      void AddSimulation(IModelCoreSimulation simulation);

      /// <summary>
      ///    Adds a SimulationBatch to the list of SimulationBatches
      /// </summary>
      /// <param name="simulationWithBatchOptions">the options to run the batch</param>
      void AddSimulationBatchOption(SettingsForConcurrentltyRunSimulationBatch simulationWithBatchOptions);

      void Clear();

      /// <summary>
      ///    Runs all preset settings concurrently
      /// </summary>
      /// <returns></returns>
      ConcurrentSimulationResults[] RunConcurrently();
   }

   public class ConcurrentSimulationRunner : IConcurrentSimulationRunner
   {
      private readonly IConcurrencyManager _concurrencyManager;

      public ConcurrentSimulationRunner(IConcurrencyManager concurrentManager)
      {
         _concurrencyManager = concurrentManager;
      }

      public SimulationRunOptions SimulationRunOptions { get; set; }

      public int NumberOfCores { get; set; }

      private readonly List<IModelCoreSimulation> _simulations = new List<IModelCoreSimulation>();
      private readonly List<SettingsForConcurrentltyRunSimulationBatch> _listOfSettingsForConcurrentltyRunSimulationBatch = new List<SettingsForConcurrentltyRunSimulationBatch>();
      private CancellationTokenSource _cancellationTokenSource;

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void AddSimulationBatchOption(SettingsForConcurrentltyRunSimulationBatch settings)
      {
         _listOfSettingsForConcurrentltyRunSimulationBatch.Add(settings);
      }

      public void Clear()
      {
         _simulations.Clear();
         _listOfSettingsForConcurrentltyRunSimulationBatch.Clear();
         _cancellationTokenSource?.Cancel();
      }

      private async void initializeBatches()
      {
         await _concurrencyManager.RunAsync
         (
            NumberOfCores,
            _cancellationTokenSource.Token,
            _listOfSettingsForConcurrentltyRunSimulationBatch.SelectMany
            (
               settings => Enumerable.Range(0, settings.SimulationBatchRunValues.Count - settings._simulationBatches.Count).Select(_ => settings)
            ).ToList(),
            (core, ct, settings) =>
            {
               settings._simulationBatches.Enqueue(Api.GetSimulationBatchFactory().Create(settings.Simulation, settings.SimulationBatchOptions));
               var completedTask = new TaskCompletionSource<Boolean>();
               completedTask.SetResult(true);
               return completedTask.Task;
            }
         );
      }

      public ConcurrentSimulationResults[] RunConcurrently()
      {
         //Currently we only allow for running simulations or simulation batches, but not both
         if (_listOfSettingsForConcurrentltyRunSimulationBatch.Count > 0 && _simulations.Count > 0)
            throw new OSPSuiteException(Error.InvalidMixOfSimulationAndSimulationBatch);

         _cancellationTokenSource = new CancellationTokenSource();
         if (_simulations.Count > 0)
         {
            return _concurrencyManager.RunAsync(
               NumberOfCores,
               _cancellationTokenSource.Token,
               _simulations,
               runSimulation
            ).Result.Values.ToArray();
         }

         if (_listOfSettingsForConcurrentltyRunSimulationBatch.Count > 0)
         {
            initializeBatches();
            var results = _concurrencyManager.RunAsync(
               NumberOfCores,
               _cancellationTokenSource.Token,
               _listOfSettingsForConcurrentltyRunSimulationBatch.SelectMany(sb => sb.SimulationBatchRunValues.Select((rv, i) => new SimulationBatchRunOptions()
               {
                  Simulation = sb.Simulation,
                  SimulationBatch = sb._simulationBatches.ElementAt(i),
                  SimulationBatchOptions = sb.SimulationBatchOptions,
                  SimulationBatchRunValues = rv
               })).ToList(),
               runSimulationBatch
            ).Result.Values.ToArray();
            _listOfSettingsForConcurrentltyRunSimulationBatch.ForEach(settings => settings.SimulationBatchRunValues.Clear());
            return results;
         }

         return Array.Empty<ConcurrentSimulationResults>();
      }

      private async Task<ConcurrentSimulationResults> runSimulation(int coreIndex, CancellationToken cancellationToken, IModelCoreSimulation simulation)
      {
         //We want a new instance every time that's why we are not injecting SimulationRunner in constructor
         return new ConcurrentSimulationResults(
            simulation.Id,
            await Api.GetSimulationRunner().RunAsync(new SimulationRunArgs { Simulation = simulation, SimulationRunOptions = SimulationRunOptions })
         );
      }

      private async Task<ConcurrentSimulationResults> runSimulationBatch(int coreIndex, CancellationToken cancellationToken, SimulationBatchRunOptions simulationBatchWithOptions)
      {
         return new ConcurrentSimulationResults(
            simulationBatchWithOptions.SimulationBatchRunValues.Id,
            await simulationBatchWithOptions.SimulationBatch.RunAsync(simulationBatchWithOptions.SimulationBatchRunValues)
         );
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~ConcurrentSimulationRunner()
      {
         Cleanup();
      }

      protected virtual void Cleanup()
      {
         Clear();
      }

      #endregion
   }
}