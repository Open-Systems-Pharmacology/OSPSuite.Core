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
      /// </summary>
      SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);

      /// <summary>
      ///    Stops SimModelSimulation run
      /// </summary>
      void StopSimulation();

      /// <summary>
      ///    Progress event returns the percent representing the progress of a simulation
      /// </summary>
      event EventHandler<SimulationProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after normal termination or cancel)
      /// </summary>
      event EventHandler Terminated;

      Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, CancellationTokenSource cts, SimulationRunOptions simulationRunOptions = null);
   }
}