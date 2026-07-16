using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ProjectContext : SnapshotContext
   {
      public ProjectContext(Project project, bool runSimulations) : base(project, SnapshotVersions.Current)
      {
         RunSimulations = runSimulations;
      }

      public bool RunSimulations { get; }
   }
}