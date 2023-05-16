using System.IO;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.R
{
   public class RConfiguration : OSPSuiteConfiguration
   {
      public override string WatermarkOptionLocation { get; } = "Utilities -> Options -> Application";
      public override string ApplicationFolderPathName { get; } = Path.Combine("Open Systems Pharmacology", "OSPSuite-R-API");
      protected override string[] LatestVersionWithOtherMajor { get; } = { };
      public override string ProductName { get; } = "OSPSuite - R API";
      public override int InternalVersion { get; } = 25;
      public override Origin Product { get; } = Origins.R;
      public override string ProductNameWithTrademark { get; } = "OSPSuite";
      public override string IconName { get; } = IconNames.R;
      public override string UserSettingsFileName { get; } = "UserSettings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = "https://github.com/open-systems-pharmacology/ospsuite.core/issues";

      // Assembly.GetExecutingAssembly() must be called where the constructor is called
      // do not pull up
      public RConfiguration() : base(Assembly.GetExecutingAssembly())
      {
      }
   }
}