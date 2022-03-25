using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public class ConcurrentRunSimulationBatch : IDisposable, IWithId
   {
      public string Id { get; set; }
      public IModelCoreSimulation Simulation { get; }
      private readonly List<SimulationBatchRunValues> _simulationBatchRunValues = new List<SimulationBatchRunValues>();

      public IReadOnlyList<SimulationBatchRunValues> SimulationBatchRunValues => _simulationBatchRunValues;

      public SimulationBatchOptions SimulationBatchOptions { get; }

      public string[] RunValuesIds => SimulationBatchRunValues.Select(srv => srv.Id).ToArray();

      private readonly ConcurrentQueue<SimulationBatch> _simulationBatches = new ConcurrentQueue<SimulationBatch>();

      internal int MissingBatchesCount => Math.Max(0, SimulationBatchRunValues.Count - _simulationBatches.Count);

      public ConcurrentRunSimulationBatch(IModelCoreSimulation simulation, SimulationBatchOptions simulationBatchOptions)
      {
         Simulation = simulation;
         SimulationBatchOptions = simulationBatchOptions;
         Id = generateId();
      }

      internal SimulationBatch AddNewBatch()
      {
         var batch = Api.GetSimulationBatchFactory().Create(Simulation, SimulationBatchOptions);
         _simulationBatches.Enqueue(batch);
         return batch;
      }

      public void ClearRunValues() => _simulationBatchRunValues.Clear();

      public IReadOnlyCollection<SimulationBatch> SimulationBatches => _simulationBatches;

      public void AddSimulationBatchRunValues(SimulationBatchRunValues simulationBatchRunValues)
      {
         _simulationBatchRunValues.Add(simulationBatchRunValues);
      }

      private string generateId() => Guid.NewGuid().ToString();

      protected virtual void Cleanup()
      {
         _simulationBatches.Each(x => x.Dispose());
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

      ~ConcurrentRunSimulationBatch()
      {
         Cleanup();
      }

      #endregion
   }
}