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

      public static readonly SnapshotVersion V7_1_0 = addVersion(71, "7.1.0");
      public static readonly SnapshotVersion V7_2_0 = addVersion(72, "7.2.0");
      public static readonly SnapshotVersion V7_2_1 = addVersion(73, "7.2.1");
      public static readonly SnapshotVersion V7_3_0 = addVersion(74, "7.3.0");
      public static readonly SnapshotVersion V7_4_0 = addVersion(75, "7.4.0");
      public static readonly SnapshotVersion V8 = addVersion(76, "8");
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

      public static bool CanLoadVersion(int projectVersion)
      {
         return (projectVersion <= Current.Version) && _knownVersions.Contains(projectVersion);
      }

      public static ProjectVersion OldestSupportedVersion => _knownVersions.OrderBy(x => x.Version).First();

      public static SnapshotVersion FindBy(int version)
      {
         return _knownVersions[version];
      }
   }
}