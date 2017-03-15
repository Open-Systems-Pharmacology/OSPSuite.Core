namespace OSPSuite.Core.Domain
{
   public class PopulationSimulationProgressEventArgs
   {
      /// <summary>
      /// Number of simultion already calculated in the population simulation run
      /// </summary>
      public int NumberOfCalculatedSimulation { get; set; }

      /// <summary>
      /// Number of simulations to perform
      /// </summary>
      public int NumberOfSimulations{ get; set; }

   }
}