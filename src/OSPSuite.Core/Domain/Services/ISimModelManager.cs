using System;

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
      ///    Simulates the active simulation using default run options
      /// </summary>
      SimulationRunResults RunSimulation(IModelCoreSimulation simulation);

      /// <summary>
      ///    Run simulation using given options
      /// </summary>
      SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions);

      /// <summary>
      ///    Stops SimModelSimulation run
      /// </summary>
      void StopSimulation();

      /// <summary>
      ///    Progress event returns the percent reprensenting the progress of a simulation
      /// </summary>
      event EventHandler<SimulationProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after nornal termination or cancel)
      /// </summary>
      event EventHandler Terminated;
   }
}