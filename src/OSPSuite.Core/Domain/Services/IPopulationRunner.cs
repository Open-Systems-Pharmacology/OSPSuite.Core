using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Information about the simulation progress
   /// </summary>
   public class MultipleSimulationsProgressEventArgs : EventArgs
   {
      /// <summary>
      ///    Actual Progress as a Integer between 0 and 100
      /// </summary>
      public int NumberOfCalculatedSimulation { get; }

      public int NumberOfSimulations { get; }

      public MultipleSimulationsProgressEventArgs(int numberOfCalculatedSimulation, int numberOfSimulations)
      {
         NumberOfCalculatedSimulation = numberOfCalculatedSimulation;
         NumberOfSimulations = numberOfSimulations;
      }
   }

   public interface IPopulationRunner
   {
      /// <summary>
      ///    Runs population and returns the results.
      /// </summary>
      /// <param name="simulation">simulation being run</param>
      /// <param name="runOptions">Options for the run</param>
      /// <param name="populationData">Data table with non-table parameter values for variation</param>
      /// <param name="agingData">Data table with table parameter values for variation</param>
      /// <param name="initialValues">Data table with (molecule) initial values</param>
      /// <param name="cancellationToken">cancellation token to cancel the running process</param>
      /// <returns>
      ///    Simulation results.
      ///    <para></para>
      ///    Only results for successful individuals are stored.
      ///    <para></para>
      ///    For failed individuals, pairs {IndividualId, ErrorMessage} are stored
      /// </returns>
      Task<PopulationRunResults> RunPopulationAsync(IModelCoreSimulation simulation, RunOptions runOptions, DataTable populationData, DataTable agingData = null, DataTable initialValues = null, CancellationToken cancellationToken = default);

      /// <summary>
      ///    Progress event returns the percent representing the progress of a simulation
      /// </summary>
      event EventHandler<MultipleSimulationsProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after normal termination or cancel)
      /// </summary>
      event EventHandler Terminated;
   }
}