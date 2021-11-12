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
using OSPSuite.Utility.Extensions;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   internal class SimulationBatchRunOptions : IWithId
   {
      public IModelCoreSimulation Simulation { get; set; }
      public SimulationBatch SimulationBatch { get; set; }
      public SimulationBatchRunValues SimulationBatchRunValues { get; set; }
      public SimulationBatchOptions SimulationBatchOptions { get; set; }
      public string Id 
      { 
         get => SimulationBatchRunValues.Id; 
         set => SimulationBatchRunValues.Id = value; 
      }
   }

   public interface IConcurrentSimulationRunner : IDisposable
   {
      /// <summary>
      ///    General simulation options
      /// </summary>
      SimulationRunOptions SimulationRunOptions { get; set; }

      /// <summary>
      ///    Adds a simulation to the list of Simulations
      /// </summary>
      void AddSimulation(IModelCoreSimulation simulation);

      /// <summary>
      ///    Adds a SimulationBatch to the list of SimulationBatches
      /// </summary>
      /// <param name="simulationBatch">the concurrent batch to add</param>
      void AddSimulationBatch(ConcurrentRunSimulationBatch simulationBatch);

      /// <summary>
      ///    Clear all data for freshly start
      /// </summary>
      void Clear();

      /// <summary>
      ///    Runs all preset settings concurrently
      /// </summary>
      /// <returns></returns>
      ConcurrencyManagerResult<SimulationResults>[] RunConcurrently();

      /// <summary>
      ///    After initialization phase, run all simulations or simulationBatches Async
      /// </summary>
      /// <returns></returns>
      Task<IEnumerable<ConcurrencyManagerResult<SimulationResults>>> RunConcurrentlyAsync();
   }

   public class ConcurrentSimulationRunner : IConcurrentSimulationRunner
   {
      private readonly IConcurrencyManager _concurrencyManager;
      public ConcurrentSimulationRunner(IConcurrencyManager concurrencyManager)
      {
         _concurrencyManager = concurrencyManager;
      }

      public SimulationRunOptions SimulationRunOptions { get; set; }

      private readonly List<IModelCoreSimulation> _simulations = new List<IModelCoreSimulation>();
      private readonly List<ConcurrentRunSimulationBatch> _listOfConcurrentRunSimulationBatch = new List<ConcurrentRunSimulationBatch>();
      private CancellationTokenSource _cancellationTokenSource;

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void AddSimulationBatch(ConcurrentRunSimulationBatch simulationBatch)
      {
         _listOfConcurrentRunSimulationBatch.Add(simulationBatch);
      }

      public void Clear()
      {
         _simulations.Clear();
         _listOfConcurrentRunSimulationBatch.Clear();
         _cancellationTokenSource?.Cancel();
      }

      private Task initializeBatches()
      {
         return _concurrencyManager.RunAsync
         (
            //The batch creation is expensive so we store the created batches from one RunConcurrently call
            //to the next one. It might happen though that the later call needs more batches than the former
            //so for each needed batch, we create a new one.
            //The iteration occurs on the list of _listOfSettingsForConcurrentRunSimulationBatch (over different
            //simulation objects), taking for each settings (or simulation) a list with the missing batches 
            //(one for each MissingBatchesCount) to add a new batch on such a settings object
            _listOfConcurrentRunSimulationBatch.SelectMany
            (
               settings => Enumerable.Range(0, settings.MissingBatchesCount).Select(_ => settings)
            ).ToList(),
            (settings, ct) => settings.AddNewBatch(), 
            _cancellationTokenSource.Token);
      }

      public async Task<IEnumerable<ConcurrencyManagerResult<SimulationResults>>> RunConcurrentlyAsync()
      {
         //Currently we only allow for running simulations or simulation batches, but not both
         if (_listOfConcurrentRunSimulationBatch.Count > 0 && _simulations.Count > 0)
            throw new OSPSuiteException(Error.InvalidMixOfSimulationAndSimulationBatch);

         _cancellationTokenSource = new CancellationTokenSource();

         if (_simulations.Any())
         {
            return (await _concurrencyManager.RunAsync(
               _simulations,
               runSimulation, 
               _cancellationTokenSource.Token)).Values;
         }

         if (_listOfConcurrentRunSimulationBatch.Count > 0)
         {
            await initializeBatches();

            var results = await _concurrencyManager.RunAsync(
               _listOfConcurrentRunSimulationBatch.SelectMany(sb => sb.SimulationBatchRunValues.Select((rv, i) => new SimulationBatchRunOptions()
               {
                  Simulation = sb.Simulation,
                  SimulationBatch = sb.SimulationBatches.ElementAt(i),
                  SimulationBatchOptions = sb.SimulationBatchOptions,
                  SimulationBatchRunValues = rv
               })).ToList(),
               runSimulationBatch, 
               _cancellationTokenSource.Token);

            //After one RunConcurrently call, we need to forget the SimulationBatchRunValues and expect the new set of values. So the caller has to
            //specify new SimulationBatchRunValues before each RunConcurrently call.
            _listOfConcurrentRunSimulationBatch.Each(x => x.ClearRunValues());
            return results.Values;
         }

         return Enumerable.Empty<ConcurrencyManagerResult<SimulationResults>>();
      }

      public ConcurrencyManagerResult<SimulationResults>[] RunConcurrently() => RunConcurrentlyAsync().Result.ToArray();

      private SimulationResults runSimulation(IModelCoreSimulation simulation, CancellationToken cancellationToken)
      {
         //We want a new instance every time that's why we are not injecting SimulationRunner in constructor
         return Api.GetSimulationRunner().Run(new SimulationRunArgs {Simulation = simulation, SimulationRunOptions = SimulationRunOptions});
      }

      private SimulationResults runSimulationBatch(SimulationBatchRunOptions simulationBatchWithOptions, CancellationToken cancellationToken)
      {
         return simulationBatchWithOptions.SimulationBatch.Run(simulationBatchWithOptions.SimulationBatchRunValues);
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