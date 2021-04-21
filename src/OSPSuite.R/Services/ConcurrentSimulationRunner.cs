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
   public class SimulationWithBatchOptions
   {
      public IModelCoreSimulation Simulation { get; set; }
      public List<SimulationBatchRunValues> SimulationBatchRunValues { get; } = new List<SimulationBatchRunValues>();
      public SimulationBatchOptions SimulationBatchOptions { get; set; }
      internal List<SimulationBatchRunValues> _pendingForInitialization = new List<SimulationBatchRunValues>();
      internal Dictionary<SimulationBatchRunValues, SimulationBatch> _simulationBatches = new Dictionary<SimulationBatchRunValues, SimulationBatch>();
      public void AddSimulationBatchRunValues(SimulationBatchRunValues simulationBatchRunValues)
      {
         SimulationBatchRunValues.Add(simulationBatchRunValues);
         _pendingForInitialization.Add(simulationBatchRunValues);
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
      public ConcurrentSimulationResults(string simulationId, SimulationResults results, string additionalId = null)
      {
         SimulationId = simulationId;
         SimulationResults = results;
         AdditionalId = additionalId;
      }
      public string SimulationId { get; }
      public string AdditionalId { get; }
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
      void AddSimulationBatchOption(SimulationWithBatchOptions simulationWithBatchOptions);

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
      private readonly List<SimulationWithBatchOptions> _simulationBatches = new List<SimulationWithBatchOptions>();
      private CancellationTokenSource _cancellationTokenSource;

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void AddSimulationBatchOption(SimulationWithBatchOptions simulationWithBatchOptions)
      {
         if (simulationWithBatchOptions.SimulationBatchRunValues.Any(value => string.IsNullOrEmpty(value.Id) || _simulationBatches.Any(simulationBatch => simulationBatch.SimulationBatchRunValues.Any(otherValue => otherValue.Id == value.Id))))
            throw new OSPSuiteException(Error.InvalidSimulationBatchRunValuesId);

         _simulationBatches.Add(simulationWithBatchOptions);
      }

      public void Clear()
      {
         _simulations.Clear();
         _simulationBatches.Clear();
         _cancellationTokenSource?.Cancel();
      }

      private void initializeBatches()
      {
         _concurrencyManager.RunAsync
         (
            NumberOfCores, 
            _cancellationTokenSource.Token, 
            _simulationBatches.SelectMany
            (
               batchOptions => batchOptions._pendingForInitialization.Select
               (
                  pending => new Tuple<SimulationWithBatchOptions, SimulationBatchRunValues>(batchOptions, pending)
               )
            ).ToList(), 
            async (core, ct, simulationBatchRunValuesWithItsOptions) =>
            {
               var options = simulationBatchRunValuesWithItsOptions.Item1;
               var values = simulationBatchRunValuesWithItsOptions.Item2;
               options._simulationBatches.Add(values, Api.GetSimulationBatchFactory().Create(options.Simulation, options.SimulationBatchOptions));
               return true;
            }
         );
         _simulationBatches.ForEach(batchOptions => batchOptions._pendingForInitialization.Clear());
      }

      public ConcurrentSimulationResults[] RunConcurrently()
      {
         //Currently we only allow for running simulations or simulation batches, but not both
         if (_simulationBatches.Count > 0 && _simulations.Count > 0)
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

         if (_simulationBatches.Count > 0)
         {
            initializeBatches();            
            
            return _concurrencyManager.RunAsync(
               NumberOfCores,
               _cancellationTokenSource.Token,
               _simulationBatches.SelectMany(sb => sb.SimulationBatchRunValues.Select(rv => new SimulationBatchRunOptions()
               {
                  Simulation = sb.Simulation,
                  SimulationBatch = sb._simulationBatches[rv],
                  SimulationBatchOptions = sb.SimulationBatchOptions,
                  SimulationBatchRunValues = rv
               })).ToList(),
               runSimulationBatch
            ).Result.Values.ToArray();
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
            simulationBatchWithOptions.Simulation.Id,
            await simulationBatchWithOptions.SimulationBatch.RunAsync(simulationBatchWithOptions.SimulationBatchRunValues),
            simulationBatchWithOptions.SimulationBatchRunValues.Id
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