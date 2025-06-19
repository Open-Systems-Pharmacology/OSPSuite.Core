using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class SnapshotContextWithSimulation : SnapshotContext
   {
      public ISimulation Simulation { get; }

      public SnapshotContextWithSimulation(ISimulation simulation, SnapshotContext baseContext) : base(baseContext)
      {
         Simulation = simulation;
      }
   }
}