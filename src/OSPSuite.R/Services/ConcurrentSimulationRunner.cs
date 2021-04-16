using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public interface IConcurrentSimulationRunner
   {
      /// <summary>
      /// General simulation options
      /// </summary>
      SimulationRunOptions SimulationRunOptions { get; set; }

      /// <summary>
      /// Number of cores to use concurrently. Use 0 or a negative value for using the maximum available.
      /// </summary>
      int NumberOfCores { get; set; }

      /// <summary>
      /// List of Simulations
      /// </summary>
      void AddSimulation(IModelCoreSimulation simulation);

      void Clear();

      /// <summary>
      /// Runs all preset settings concurrently
      /// </summary>
      /// <returns></returns>
      SimulationResults[] RunConcurrently();
   }

   public class ConcurrentSimulationRunner : IConcurrentSimulationRunner
   {
      private IConcurrencyManager _concurrentManager;

      public ConcurrentSimulationRunner(IConcurrencyManager concurrentManager)
      {
         _concurrentManager = concurrentManager;
      }

      public SimulationRunOptions SimulationRunOptions { get; set; }

      public int NumberOfCores { get; set; }

      private List<IModelCoreSimulation> _simulations = new List<IModelCoreSimulation>();

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void Clear()
      {
         _simulations.Clear();
      }

      public SimulationResults[] RunConcurrently()
      {
         var results = _concurrentManager.RunAsync<IModelCoreSimulation, SimulationResults>(
            NumberOfCores,
            new System.Threading.CancellationToken(false),
            new ConcurrentQueue<IModelCoreSimulation>(_simulations),
            async (coreIndex, cancellationToken, simulation) =>
               await Api.GetSimulationRunner().RunAsync(new SimulationRunArgs() { Simulation = simulation, SimulationRunOptions = SimulationRunOptions })
         ).Result;
         return _simulations.Select(s => results[s]).ToArray();
      }
   }
}
