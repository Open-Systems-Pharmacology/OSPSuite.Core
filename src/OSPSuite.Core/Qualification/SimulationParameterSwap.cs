namespace OSPSuite.Core.Qualification
{
   public class SimulationParameterSwap
   {
      public string SnapshotFile { get; set; }
      public string Simulation { get; set; }
      public string Path { get; set; }
      public string[] TargetSimulations { get; set; }

      public void Deconstruct(out string path, out string simulation, out string snapshotFile)
      {
         snapshotFile = SnapshotFile;
         simulation = Simulation;
         path = Path;
      }
   }
}