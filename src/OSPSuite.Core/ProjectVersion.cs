using System.Globalization;

namespace OSPSuite.Core
{
   public class ProjectVersion
   {
      /// <summary>
      ///    The version as number
      /// </summary>
      public int Version { get; private set; }

      /// <summary>
      ///    The version as display string (e.g. 5.1.2)
      /// </summary>
      public string VersionDisplay { get; private set; }

      public ProjectVersion(int version, string versionDisplay)
      {
         Version = version;
         VersionDisplay = versionDisplay;
      }

      /// <summary>
      ///    The version number converted to string (e.g. "55")
      /// </summary>
      public string VersionAsString => Version.ToString(CultureInfo.InvariantCulture);

      public override string ToString()
      {
         return VersionDisplay;
      }

      public static implicit operator int(ProjectVersion projectVersion)
      {
         return projectVersion.Version;
      }
   }
}