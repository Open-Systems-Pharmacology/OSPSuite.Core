using System.IO;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

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
      public override string IconName { get; } = ApplicationIcons.PKSim.IconName;
      public override string UserSettingsFileName { get; } = "UserSettings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = "url";

      // Assembly.GetExecutingAssembly() must be called where the constructor is called
      // do not pull up
      public ApplicationConfiguration() : base(Assembly.GetExecutingAssembly())
      {
      }
   }
}