using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.Configuration
{
   public abstract class OSPSuiteConfiguration : IApplicationConfiguration
   {
      protected abstract string[] LatestVersionWithOtherMajor { get; }

      public string ChartLayoutTemplateFolderPath { get; }
      public string TeXTemplateFolderPath { get; }
      public string PKParametersFilePath { get; }
      public string SimModelSchemaFilePath { get; }
      public string DimensionFilePath { get; }
      public abstract string ProductName { get; }
      public abstract Origin Product { get; }
      public abstract string ProductNameWithTrademark { get; }
      public abstract ApplicationIcon Icon { get; }
      public abstract string UserSettingsFileName { get; }
      public abstract string ApplicationSettingsFileName { get; }
      public abstract string IssueTrackerUrl { get; }
      public string ReleaseDescription { get; }
      public string AllUsersFolderPath { get; }
      public string CurrentUserFolderPath { get; }
      public string BuildVersion { get; }
      public string MajorVersion { get; }
      public string ProductDisplayName { get; }
      public string FullVersion { get; }
      public string Version { get; }
      public string UserSettingsFilePath { get; }
      public string ApplicationSettingsFilePath { get; }
      public string OSPSuiteNameWithVersion { get; }
      public string LogConfigurationFile { get; }
      public abstract string WatermarkOptionLocation { get; }
      public abstract string ApplicationFolderPathName { get; }
      public virtual string LicenseAgreementFilePath { get; } = Constants.Files.LICENSE_AGREEMENT_FILE_NAME;

      protected OSPSuiteConfiguration()
      {
         var assemblyVersion = AssemblyVersion;
         MajorVersion = version(assemblyVersion.Minor);
         Version = $"{MajorVersion}.{assemblyVersion.Build}";
         ReleaseDescription = retrieveReleaseDescription();
         FullVersion = fullVersionFrom(assemblyVersion.Revision);
         OSPSuiteNameWithVersion = $"{Constants.SUITE_NAME} - {Version}";
         ProductDisplayName = retrieveProductDisplayName();
         CurrentUserFolderPath = CurrentUserFolderPathFor(MajorVersion);
         AllUsersFolderPath = AllUserFolderPathFor(MajorVersion);
         BuildVersion = AssemblyVersion.Revision.ToString(CultureInfo.InvariantCulture);
         PKParametersFilePath = AllUsersOrLocalPathForFile(Constants.Files.PK_PARAMETERS_FILE_NAME);
         SimModelSchemaFilePath = LocalPathFor(Constants.Files.SIM_MODEL_SCHEMA_FILE_NAME);
         TeXTemplateFolderPath = AllUsersOrLocalPathForFolder(Constants.Files.TEX_TEMPLATE_FOLDER_NAME);
         ChartLayoutTemplateFolderPath = AllUsersOrLocalPathForFolder(Constants.Files.CHART_LAYOUT_FOLDER_NAME);
         DimensionFilePath = AllUsersOrLocalPathForFile(Constants.Files.DIMENSIONS_FILE_NAME);
         LogConfigurationFile = AllUsersOrLocalPathForFile(Constants.Files.LOG_4_NET_CONFIG_FILE);
         UserSettingsFilePath = userSettingsFilePathFor(MajorVersion);
         ApplicationSettingsFilePath = AllUsersFile(ApplicationSettingsFileName);
      }

      private string retrieveProductDisplayName()
      {
         var productDisplayName = $"{ProductNameWithTrademark} {MajorVersion}";
         if (string.IsNullOrEmpty(ReleaseDescription))
            return productDisplayName;

         return $"{productDisplayName} - {ReleaseDescription}";
      }

      private string fullVersionFrom(int revision)
      {
         if (string.IsNullOrEmpty(ReleaseDescription))
            return $"{Version} - Build {revision}";

         return $"{Version}.{revision} - {ReleaseDescription}";
      }

      private string retrieveReleaseDescription()
      {
         var informationalVersionAttribute = Assembly.GetEntryAssembly()
            ?.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), inherit: false)
            .OfType<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault();

         return informationalVersionAttribute?.InformationalVersion ?? string.Empty;
      }

      private string version(int minor) => $"{AssemblyVersion.Major}.{minor}";

      public IEnumerable<string> UserSettingsFilePaths => SettingsFilePaths(UserSettingsFilePath, userSettingsFilePathFor);

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

      private string userSettingsFilePathFor(string version) => Path.Combine(CurrentUserFolderPathFor(version), UserSettingsFileName);

      protected string ApplicationFolderPathWithRevision(string revision) => Path.Combine(ApplicationFolderPathName, revision);

      protected string AllUsersFile(string fileName) => Path.Combine(AllUsersFolderPath, fileName);

      protected string CurrentUserFile(string fileName) => Path.Combine(CurrentUserFolderPath, fileName);

      /// <summary>
      ///    Returns a local full path for the file with name <paramref name="fileName" />
      /// </summary>
      protected string LocalPathFor(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

      protected string AllUserFolderPathFor(string version) => Path.Combine(EnvironmentHelper.ApplicationDataFolder(), ApplicationFolderPathWithRevision(version));

      protected string CurrentUserFolderPathFor(string version) => Path.Combine(EnvironmentHelper.UserApplicationDataFolder(), ApplicationFolderPathWithRevision(version));

      private string createApplicationDataOrLocalPathFor(string appDataName, string localName, Func<string, bool> existsFunc)
      {
         var applicationDataOrLocal = AllUsersFile(appDataName);
         if (existsFunc(applicationDataOrLocal))
            return applicationDataOrLocal;

         //try local if id does not exist
         var localPath = LocalPathFor(localName);
         if (existsFunc(localPath))
            return localPath;

         //neither app data nor local exist, return app data
         return applicationDataOrLocal;
      }

      protected string AllUsersOrLocalPathForFile(string fileName) => createApplicationDataOrLocalPathFor(fileName, fileName, FileHelper.FileExists);

      protected string AllUsersOrLocalPathForFolder(string folderName) => AllUsersOrLocalPathForFolder(folderName, folderName);

      protected string AllUsersOrLocalPathForFolder(string folderNameAppData, string folderNameLocal) => createApplicationDataOrLocalPathFor(folderNameAppData, folderNameLocal, DirectoryHelper.DirectoryExists);

      private string applicationSettingsFolderPathFor(string version) => Path.Combine(EnvironmentHelper.ApplicationDataFolder(), ApplicationFolderPathWithRevision(version));

      public IEnumerable<string> ApplicationSettingsFilePaths => SettingsFilePaths(ApplicationSettingsFilePath, applicationSettingsFolderPathFor);
   }
}