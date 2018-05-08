using System.IO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;

namespace OSPSuite.Starter.Tasks
{
   internal class ApplicationConfiguration : OSPSuiteConfiguration
   {
      public override string WatermarkOptionLocation { get; } = "Utilities -> Options -> Application";
      public override string ApplicationFolderPathName { get; } = Path.Combine("Open Systems Pharmacology", "OSPSuite.Starter");
      protected override string[] LatestVersionWithOtherMajor { get; } = { };
      public override string ProductName { get; } = "OSPSuite";
      public override int InternalVersion { get; } = 25;
      public override Origin Product { get; } = Origins.PKSim;
      public override string ProductNameWithTrademark { get; } = "OSPSuite";
      public override ApplicationIcon Icon { get; } = ApplicationIcons.PKSim;
      public override string UserSettingsFileName { get; } = "UserSettings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = "url";
   }
}