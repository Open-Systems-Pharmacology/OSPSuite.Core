using System;
using System.IO;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_OSPSuiteConfiguration : ContextSpecification<IApplicationConfiguration>
   {
      protected Version _version;

      protected override void Context()
      {
         sut = new OSPSuiteConfigurationForSpecs(_version);
      }
   }

   public class When_resolving_the_version_for_a_release_app_with_a_minor_set : concern_for_OSPSuiteConfiguration
   {
      protected override void Context()
      {
         _version = new Version(8, 2, 5, 0);
         base.Context();
      }

      [Observation]
      public void should_return_the_expected_major()
      {
         sut.Major.ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_return_the_expected_minor()
      {
         sut.Minor.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_the_expected_version_in_x_y_format()
      {
         sut.Version.ShouldBeEqualTo("8.2");
      }

      [Observation]
      public void should_return_the_expected_full_version_in_the_x_y_z_format()
      {
         sut.FullVersion.ShouldBeEqualTo("8.2.5");
      }

      [Observation]
      public void should_return_the_expected_display_version()
      {
         sut.FullVersionDisplay.ShouldBeEqualTo("8.2 - Build 5");
      }

      [Observation]
      public void should_return_the_expected_product_display_name()
      {
         sut.ProductDisplayName.ShouldBeEqualTo("MySuperTool TM 8 Update 2");
      }
   }

   public class When_resolving_the_version_for_a_release_app_with_a_minor__not_set : concern_for_OSPSuiteConfiguration
   {
      protected override void Context()
      {
         _version = new Version(8, 0, 5, 0);
         base.Context();
      }

      [Observation]
      public void should_return_the_expected_major()
      {
         sut.Major.ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_return_the_expected_minor()
      {
         sut.Minor.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_the_expected_version_in_x_y_format()
      {
         sut.Version.ShouldBeEqualTo("8.0");
      }

      [Observation]
      public void should_return_the_expected_full_version_in_the_x_y_z_format()
      {
         sut.FullVersion.ShouldBeEqualTo("8.0.5");
      }

      [Observation]
      public void should_return_the_expected_display_version()
      {
         sut.FullVersionDisplay.ShouldBeEqualTo("8 - Build 5");
      }

      [Observation]
      public void should_return_the_expected_product_display_name()
      {
         sut.ProductDisplayName.ShouldBeEqualTo("MySuperTool TM 8");
      }
   }

   internal class OSPSuiteConfigurationForSpecs : OSPSuiteConfiguration
   {
      public OSPSuiteConfigurationForSpecs(Version assemblyVersion) : base(assemblyVersion)
      {
      }

      public override string WatermarkOptionLocation { get; } = "Utilities -> Options -> Application";
      public override string ApplicationFolderPathName { get; } = Path.Combine("Open Systems Pharmacology", "OSPSuite.Starter");
      protected override string[] LatestVersionWithOtherMajor { get; } = { };
      public override string ProductName { get; } = "MySuperTool";
      public override int InternalVersion { get; } = 25;
      public override Origin Product { get; } = Origins.PKSim;
      public override string ProductNameWithTrademark { get; } = "MySuperTool TM";
      public override ApplicationIcon Icon { get; } = ApplicationIcons.PKSim;
      public override string UserSettingsFileName { get; } = "UserSettings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = "url";
   }
}