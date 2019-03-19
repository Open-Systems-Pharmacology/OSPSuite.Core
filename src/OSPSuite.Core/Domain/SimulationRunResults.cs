using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    All Information about the Simulation Run
   /// </summary>
   public class SimulationRunResults
   {
      /// <summary>
      ///    The Property indicates if the simulation run succeeds
      /// </summary>
      public bool Success { get; }

      /// <summary>
      ///    Contains a List of messages describing error or/and warnings from the solver
      /// </summary>
      public IEnumerable<SolverWarning> Warnings { get; }

      /// <summary>
      ///    Actual results of the simulation
      /// </summary>
      public DataRepository Results { get; }

      /// <summary>
      ///    Possible external errors that may have occured
      /// </summary>
      public string Error { get; }

      public SimulationRunResults(bool success, IEnumerable<SolverWarning> warnings, DataRepository results, string error = null)
      {
         Success = success;
         Warnings = warnings;
         Results = results;
         Error = error;
      }
   }
}