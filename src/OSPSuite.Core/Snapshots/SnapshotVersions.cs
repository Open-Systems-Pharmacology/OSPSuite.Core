using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Snapshots
{
   /// <summary>
   ///    Originally based on PK-Sim project versions, so the first few entries must coincide with PK-Sim project versions
   /// </summary>
   public static class SnapshotVersions
   {
      private static readonly Cache<int, SnapshotVersion> _knownVersions = new Cache<int, SnapshotVersion>(x => x.Version, x => null);
      public static readonly SnapshotVersion V9 = addVersion(77, "9");
      public static readonly SnapshotVersion V10 = addVersion(78, "10");
      public static readonly SnapshotVersion V11 = addVersion(79, "11");
      public static readonly SnapshotVersion V12 = addVersion(80, "12");
      public static readonly SnapshotVersion Current = V12;

      private static SnapshotVersion addVersion(int versionNumber, string versionDisplay)
      {
         var projectVersion = new SnapshotVersion(versionNumber, versionDisplay);
         _knownVersions.Add(projectVersion);
         return projectVersion;
      }

      public static string CurrentAsString => Current.VersionAsString;

      public static bool CanLoadVersion(int snapshotVersion)
      {
         return snapshotVersion <= Current.Version;
      }

      public static SnapshotVersion FindByPKSimProjectVersion(int projectVersion)
      {
         // All snapshots up to V9 are compatible.
         if (projectVersion < V9)
            return V9;

         // if project version increases, return the current snapshot version
         if (projectVersion > Current)
            return Current;
         
         return _knownVersions[projectVersion];
      }

      public static SnapshotVersion FindByMoBiProjectVersion(int projectVersion)
      {
         return Current;
      }
   }
}