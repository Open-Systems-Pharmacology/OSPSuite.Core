using System;
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
   public class SimulationBatchRunConcurrentlyOptions
   {
      private List<SimulationBatchRunValues> _simulationBatchRunValues = new List<SimulationBatchRunValues>();
      public IReadOnlyList<SimulationBatchRunValues> SimulationBatchRunValues { get => _simulationBatchRunValues; }

      public void AddSimulationBatchRunValues(SimulationBatchRunValues runValues)
      {
         _simulationBatchRunValues.Add(runValues);
      }
   }

   class SimulationBatchWithOptions
   {
      public SimulationBatch SimulationBatch { get; set; }
      public SimulationBatchRunConcurrentlyOptions SimulationBatchRunConcurrentlyOptions { get; set; }
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
      /// <param name="simulationBatch">the batch</param>
      /// <param name="simulationBatchRunConcurrentlyOptions">the options to run the batch</param>
      void AddSimulationBatchOption(SimulationBatch simulationBatch, SimulationBatchRunConcurrentlyOptions simulationBatchRunConcurrentlyOptions);

      void Clear();

      /// <summary>
      ///    Runs all preset settings concurrently
      /// </summary>
      /// <returns></returns>
      SimulationResults[] RunConcurrently();
   }

   public class ConcurrentSimulationRunner : IConcurrentSimulationRunner
   {
      private readonly IConcurrencyManager _concurrentManager;

      public ConcurrentSimulationRunner(IConcurrencyManager concurrentManager)
      {
         _concurrentManager = concurrentManager;
      }

      public SimulationRunOptions SimulationRunOptions { get; set; }

      public int NumberOfCores { get; set; }

      private readonly List<IModelCoreSimulation> _simulations = new List<IModelCoreSimulation>();
      private readonly Dictionary<SimulationBatch, List<SimulationBatchRunConcurrentlyOptions>> _simulationBatches = new Dictionary<SimulationBatch, List<SimulationBatchRunConcurrentlyOptions>>();
      private CancellationTokenSource _cancellationTokenSource;

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void AddSimulationBatchOption(SimulationBatch simulationBatch, SimulationBatchRunConcurrentlyOptions simulationBatchRunConcurrentlyOptions)
      {
         if (_simulationBatches.ContainsKey(simulationBatch))
            _simulationBatches[simulationBatch].Add(simulationBatchRunConcurrentlyOptions);
         else
            _simulationBatches.Add(simulationBatch, new List<SimulationBatchRunConcurrentlyOptions>() { simulationBatchRunConcurrentlyOptions });
      }

      public void Clear()
      {
         _simulations.Clear();
         _simulationBatches.Clear();
         if (_cancellationTokenSource != null)
            _cancellationTokenSource.Cancel();
      }

      public SimulationResults[] RunConcurrently()
      {
         //Currently we only allow for running simulations or simulation batches, but not both
         if (_simulationBatches.Count > 0 && _simulations.Count > 0)
            throw new OSPSuiteException(Error.InvalidMixOfSimulationAndSimulationBatch);

         _cancellationTokenSource = new CancellationTokenSource();
         if (_simulations.Count > 0)
         { 
            var results = _concurrentManager.RunAsync(
               NumberOfCores,
               _cancellationTokenSource.Token,
               _simulations,
               runSimulation
            ).Result;
            return _simulations.Select(s => results[s]).ToArray();
         }

         if (_simulationBatches.Count > 0)
         {
            var results = _concurrentManager.RunAsync(
               NumberOfCores,
               _cancellationTokenSource.Token,
               _simulationBatches.SelectMany(kv => ),
               runSimulation
            ).Result;
            return _simulations.Select(s => results[s]).ToArray();
         }

         return Array.Empty<SimulationResults>();
      }

      private Task<SimulationResults> runSimulation(int coreIndex, CancellationToken cancellationToken, IModelCoreSimulation simulation)
      {
         //We want a new instance every time that's why we are not injecting SimulationRunner in constructor
         return Api.GetSimulationRunner().RunAsync(new SimulationRunArgs {Simulation = simulation, SimulationRunOptions = SimulationRunOptions});
      }

      private Task<SimulationResults> runSimulationBatch(int coreIndex, CancellationToken cancellationToken, )

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