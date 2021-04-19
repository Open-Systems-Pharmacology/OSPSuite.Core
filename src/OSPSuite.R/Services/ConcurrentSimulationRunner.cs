using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
      /// Adds a simulation to the list of Simulations
      /// </summary>
      void AddSimulation(IModelCoreSimulation simulation);

      /// <summary>
      /// Adds a SimulationBatch to the list of SimulationBatches
      /// </summary>
      /// <param name="simulationBatch"></param>
      void AddSimulationBatch(SimulationBatch simulationBatch);

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
      private List<SimulationBatch> _simulationBatches = new List<SimulationBatch>();

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }

      public void AddSimulationBatch(SimulationBatch simulationBatch)
      {
         _simulationBatches.Add(simulationBatch);
      }

      public void Clear()
      {
         _simulations.Clear();
         _simulationBatches.Clear();
      }

      public SimulationResults[] RunConcurrently()
      {
         //Currently we only allow for running simulations or simulation batches, but not both
         if (_simulationBatches.Count > 0 && _simulations.Count > 0) 
            //Temporal Exception. We should allow for mixing both use cases but we need to discuss first
            throw new Exception("You already have Simulation and SimulationBatch objects and should not mix, please invoke Clear to start adding objects from a fresh start");

         if (_simulations.Count > 0)
         {
            var results = _concurrentManager.RunAsync(
               NumberOfCores,
               new CancellationToken(false),
               new List<IModelCoreSimulation>(_simulations),
               runSimulation
            ).Result;
            return _simulations.Select(s => results[s]).ToArray();
         }

         if (_simulationBatches.Count > 0)
            return null;

         return Enumerable.Empty<SimulationResults>().ToArray();
      }

      private async Task<SimulationResults> runSimulation(int coreIndex, CancellationToken cancellationToken, IModelCoreSimulation simulation)
      {
         return await Api.GetSimulationRunner().RunAsync(new SimulationRunArgs() { Simulation = simulation, SimulationRunOptions = SimulationRunOptions });
      }
   }


}
