using System;
using System.Data;
using System.Threading.Tasks;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Information about the simulation progress
   /// </summary>
   public class PopulationSimulationProgressEventArgs : EventArgs
   {
      /// <summary>
      ///    Actual Progress as a Integer between 0 and 100
      /// </summary>
      public int NumberOfCalculatedSimulation { get; }

      public int NumberOfSimulations { get; }

      public PopulationSimulationProgressEventArgs(int numberOfCalculatedSimulation, int numberOfSimulations)
      {
         NumberOfCalculatedSimulation = numberOfCalculatedSimulation;
         NumberOfSimulations = numberOfSimulations;
      }
   }

   public interface IPopulationRunner
   {
      /// <summary>
      ///    (Maximal) number of cores to be used (1 per default)
      /// </summary>
      int NumberOfCoresToUse { get; set; }

      /// <summary>
      ///    Runs population and returns the results.
      /// </summary>
      /// <param name="simulation"></param>
      /// <param name="populationData">Data table with non-table parameter values for variation</param>
      /// <param name="agingData">Data table with table parameter values for variation</param>
      /// <param name="initialValues">Data table with (molecule) initial values</param>
      /// <returns>
      ///    Simulation results.
      ///    <para></para>
      ///    Only results for successfull individuals are stored.
      ///    <para></para>
      ///    For failed individuals, pairs {IndividualId, ErrorMessage} are stored
      /// </returns>
      Task<PopulationRunResults> RunPopulationAsync(IModelCoreSimulation simulation, DataTable populationData, DataTable agingData = null, DataTable initialValues = null);

      /// <summary>
      ///    Stops SimModelSimulation run
      /// </summary>
      void StopSimulation();

      /// <summary>
      ///    Progress event returns the percent reprensenting the progress of a simulation
      /// </summary>
      event EventHandler<PopulationSimulationProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after nornal termination or cancel)
      /// </summary>
      event EventHandler Terminated;
   }
}