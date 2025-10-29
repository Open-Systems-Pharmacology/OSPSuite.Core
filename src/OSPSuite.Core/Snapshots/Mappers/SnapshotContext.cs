using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class SnapshotContext
   {
      public Project Project { get; }
      public SnapshotVersion Version { get; }

      public SnapshotContext(SnapshotContext baseContext) : this(baseContext.Project, baseContext.Version)
      {
      }

      public SnapshotContext(Project project, SnapshotVersion version)
      {
         Project = project;
         Version = version;
      }

      /// <summary>
      ///    Returns true if the format is V9 or earlier
      /// </summary>
      public bool IsV9FormatOrEarlier => Version <= SnapshotVersions.V9;

      /// <summary>
      ///    Returns true if the format is V10 or earlier
      /// </summary>
      public bool IsV10FormatOrEarlier => Version <= SnapshotVersions.V10;

      /// <summary>
      ///    Returns true if the format is V11 or earlier
      /// </summary>
      public bool IsV11FormatOrEarlier => Version <= SnapshotVersions.V11;
   }
}