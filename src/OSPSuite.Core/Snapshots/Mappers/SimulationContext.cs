namespace OSPSuite.Core.Snapshots.Mappers
{
   public class SimulationContext : SnapshotContext
   {
      public bool Run { get; }

      public SimulationContext(bool run, SnapshotContext baseContext) : base(baseContext)
      {
         Run = run;
      }

      public int NumberOfSimulationsToLoad { get; set; } = 1;
      public int NumberOfSimulationsLoaded { get; set; } = 1;
   }
}