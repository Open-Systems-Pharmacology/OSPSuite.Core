using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Information about the simulation progress
   /// </summary>
   public class SimulationProgressEventArgs : EventArgs
   {
      /// <summary>
      ///    Actual Progress as a Integer between 0 and 100
      /// </summary>
      public int Progress { get; private set; }

      public SimulationProgressEventArgs(int progress)
      {
         Progress = progress;
      }
   }

   public interface ISimModelManager
   {
      /// <summary>
      ///    Run simulation using given options or the default if not specified
      ///    Once triggered, the simulation can`t be stopped, so if a stop is needed please use the RunSimulationAsync method
      /// </summary>
      SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);

      /// <summary>
      ///    Progress event returns the percent representing the progress of a simulation
      /// </summary>
      event EventHandler<SimulationProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after normal termination or cancel)
      /// </summary>
      event EventHandler Terminated;

      /// <summary>
      /// Runs Simulation Async.
      /// Executes the simulation asynchronously with the provided options.
      /// </summary>
      /// <param name="simulation">The simulation model to be executed.</param>
      /// <param name="cancellationToken">
      /// Token used to cancel the simulation run if needed. 
      /// If not provided (defaults to <see cref="CancellationToken.None"/>), the simulation will run to completion unless an internal failure occurs.
      /// </param>
      /// <param name="simulationRunOptions">Optional parameters to customize the simulation run behavior.</param>
      /// <returns>Returns a <see cref="SimulationRunResults"/> object containing the results of the simulation.</returns>
      Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, CancellationToken cancellationToken = default, SimulationRunOptions simulationRunOptions = null);

   }
}