using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Configuration
{
   public abstract class OSPSuiteConfiguration : IApplicationConfiguration
   {
      protected abstract string[] LatestVersionWithOtherMajor { get; }

      public abstract string ChartLayoutTemplateFolderPath { get; }
      public abstract string TEXTemplateFolderPath { get; }
      public string PKParametersFilePath { get; set; }
      public abstract string ProductName { get; }
      public abstract Origin Product { get; }
      public abstract string ProductNameWithTrademark { get; }
      public abstract ApplicationIcon Icon { get; }
      public abstract string UserSettingsFileName { get; }

      public string FullVersion => $"{Version} - Build {AssemblyVersion.Revision}";

      public string Version
      {
         get
         {
            var version = AssemblyVersion;
            return $"{MajorVersion}.{version.Build}";
         }
      }

      public string MajorVersion => version(AssemblyVersion.Minor);

      private string version(int minor)
      {
         var version = AssemblyVersion;
         return $"{version.Major}.{minor}";
      }

      public string BuildVersion => AssemblyVersion.Revision.ToString(CultureInfo.InvariantCulture);

      public IEnumerable<string> UserApplicationSettingsFilePaths => SettingsFilePaths(UserApplicationSettingsFilePath, userApplicationSettingsFilePath);

      protected IEnumerable<string> SettingsFilePaths(string newerSettingFile, Func<string, string> olderSettingFileFunc)
      {
         //starts with the latest one 
         yield return newerSettingFile;
         int minor = AssemblyVersion.Minor;
         //add revision with the same major until reaching 0
         while (--minor >= 0)
         {
            yield return olderSettingFileFunc(version(minor));
         }

         foreach (var previousMajorVersion in LatestVersionWithOtherMajor)
         {
            yield return olderSettingFileFunc(previousMajorVersion);
         }
      }

      public Version AssemblyVersion => Assembly.GetAssembly(GetType()).GetName().Version;

      public string UserApplicationSettingsFilePath => userApplicationSettingsFilePath(MajorVersion);

      private string userApplicationSettingsFilePath(string revision)
      {
         return Path.Combine(EnvironmentHelper.UserApplicationDataFolder(), ApplicationFolderPathWithRevision(revision), UserSettingsFileName);
      }

      protected abstract string ApplicationFolderPathWithRevision(string revision);
      public virtual string LicenseAgreementFilePath { get; } = Constants.LICENSE_AGREEMENT_FILE_NAME;
   }
}