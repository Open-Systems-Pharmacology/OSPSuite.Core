using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

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
      public abstract int InternalVersion { get; }
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
      public string FullVersionDisplay { get; }
      public string Version { get; }
      public string UserSettingsFilePath { get; }
      public string ApplicationSettingsFilePath { get; }
      public string OSPSuiteNameWithVersion { get; }
      public abstract string WatermarkOptionLocation { get; }
      public abstract string ApplicationFolderPathName { get; }
      public virtual string LicenseAgreementFilePath { get; } = Constants.Files.LICENSE_AGREEMENT_FILE_NAME;
      private readonly bool _isReleasedVersion;

      protected OSPSuiteConfiguration()
      {
         var assemblyVersion = AssemblyVersion;
         MajorVersion = version(assemblyVersion.Minor);
         Version = combineVersions(MajorVersion, assemblyVersion.Build);
         BuildVersion = assemblyVersion.Revision.ToString();
         FullVersion = combineVersions(Version, BuildVersion);

         ReleaseDescription = retrieveReleaseDescription();
         _isReleasedVersion = string.Equals(FullVersion, ReleaseDescription);

         FullVersionDisplay = retrieveFullVersionDisplay();
         OSPSuiteNameWithVersion = $"{Constants.SUITE_NAME} - {Version}";
         ProductDisplayName = retrieveProductDisplayName();
         CurrentUserFolderPath = CurrentUserFolderPathFor(MajorVersion);
         AllUsersFolderPath = AllUserFolderPathFor(MajorVersion);
         PKParametersFilePath = LocalOrAllUsersPathForFile(Constants.Files.PK_PARAMETERS_FILE_NAME);
         SimModelSchemaFilePath = LocalPathFor(Constants.Files.SIM_MODEL_SCHEMA_FILE_NAME);
         TeXTemplateFolderPath = LocalOrAllUsersPathForFolder(Constants.Files.TEX_TEMPLATE_FOLDER_NAME);
         ChartLayoutTemplateFolderPath = LocalOrAllUsersPathForFolder(Constants.Files.CHART_LAYOUT_FOLDER_NAME);
         DimensionFilePath = LocalOrAllUsersPathForFile(Constants.Files.DIMENSIONS_FILE_NAME);
         UserSettingsFilePath = CurrentUserFile(UserSettingsFileName);
         ApplicationSettingsFilePath = AllUsersFile(ApplicationSettingsFileName);
      }

      private string retrieveProductDisplayName()
      {
         return _isReleasedVersion ? $"{ProductNameWithTrademark} {MajorVersion}" : $"{ProductNameWithTrademark} {ReleaseDescription}";
      }

      private string retrieveFullVersionDisplay()
      {
         return _isReleasedVersion ? $"{Version} - Build {BuildVersion}" : ReleaseDescription;
      }

      private string retrieveReleaseDescription()
      {
         var informationalVersionAttribute = Assembly.GetEntryAssembly()
            ?.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), inherit: false)
            .OfType<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault();

         return string.IsNullOrEmpty(informationalVersionAttribute?.InformationalVersion) ? FullVersion : informationalVersionAttribute.InformationalVersion;
      }

      private string version(int minor) => combineVersions(AssemblyVersion.Major, minor);

      private string combineVersions(params object[] items) => items.ToString(".");

      public IEnumerable<string> UserSettingsFilePaths => SettingsFilePaths(UserSettingsFilePath, userSettingsFilePathFor);

      public IEnumerable<string> ApplicationSettingsFilePaths => SettingsFilePaths(ApplicationSettingsFilePath, applicationSettingsFilePathFor);

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

      protected string ApplicationFolderPathWithRevision(string revision) => Path.Combine(ApplicationFolderPathName, revision);

      private string userSettingsFilePathFor(string version) => Path.Combine(CurrentUserFolderPathFor(version), UserSettingsFileName);

      private string applicationSettingsFilePathFor(string version) => Path.Combine(AllUserFolderPathFor(version), ApplicationSettingsFileName);

      protected string AllUsersFile(string fileName) => Path.Combine(AllUsersFolderPath, fileName);

      protected string CurrentUserFile(string fileName) => Path.Combine(CurrentUserFolderPath, fileName);

      protected string AllUserFolderPathFor(string version) => Path.Combine(EnvironmentHelper.ApplicationDataFolder(), ApplicationFolderPathWithRevision(version));

      protected string CurrentUserFolderPathFor(string version) => Path.Combine(EnvironmentHelper.UserApplicationDataFolder(), ApplicationFolderPathWithRevision(version));

      /// <summary>
      ///    Returns a local full path for the file with name <paramref name="fileName" />
      /// </summary>
      protected string LocalPathFor(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

      private string getLocalPathOrAllUsersPathFor(string appDataName, string localName, Func<string, bool> existsFunc)
      {
         //local first. 
         var localPath = LocalPathFor(localName);
         if (existsFunc(localPath))
            return localPath;

         //local does not exist, try global
         var applicationDataPath = AllUsersFile(appDataName);
         if (existsFunc(applicationDataPath))
            return applicationDataPath;

         //neither app data nor local exist. This is probably a portable installation and suggest local path;
         return localPath;
      }

      protected string LocalOrAllUsersPathForFile(string fileName) => getLocalPathOrAllUsersPathFor(fileName, fileName, FileHelper.FileExists);

      protected string LocalOrAllUsersPathForFolder(string folderName) => LocalOrAllUsersPathForFolder(folderName, folderName);

      protected string LocalOrAllUsersPathForFolder(string folderNameAppData, string folderNameLocal) => getLocalPathOrAllUsersPathFor(folderNameAppData, folderNameLocal, DirectoryHelper.DirectoryExists);
   }
}