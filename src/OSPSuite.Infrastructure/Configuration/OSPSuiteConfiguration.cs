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

      public string ChartLayoutTemplateFolderPath { get; }
      public string TEXTemplateFolderPath { get; }
      public string PKParametersFilePath { get;  }
      public string SimModelSchemaPath { get;  }
      public abstract string ProductName { get; }
      public abstract Origin Product { get; }
      public abstract string ProductNameWithTrademark { get; }
      public abstract ApplicationIcon Icon { get; }
      public abstract string UserSettingsFileName { get; }
      public abstract string IssueTrackerUrl { get; }
      public string AllUsersFolderPath { get; }
      public string CurrentUserFolderPath { get; }
      public string BuildVersion { get; }
      public string MajorVersion { get; }
      public string FullVersion { get; }
      public string Version { get; }
      public string OSPSuiteNameWithVersion { get; }
      public virtual string LicenseAgreementFilePath { get; } = Constants.Files.LICENSE_AGREEMENT_FILE_NAME;


      protected OSPSuiteConfiguration()
      {
         var assemblyVersion = AssemblyVersion;
         MajorVersion = version(assemblyVersion.Minor);
         Version = $"{MajorVersion}.{assemblyVersion.Build}";
         FullVersion = $"{Version} - Build {assemblyVersion.Revision}";
         OSPSuiteNameWithVersion = $"{Constants.SUITE_NAME} - {Version}";
         CurrentUserFolderPath = dataFolderFor(EnvironmentHelper.UserApplicationDataFolder());
         AllUsersFolderPath = dataFolderFor(EnvironmentHelper.ApplicationDataFolder());
         BuildVersion = AssemblyVersion.Revision.ToString(CultureInfo.InvariantCulture);
         PKParametersFilePath = AllUsersOrLocalPathForFile(Constants.Files.PK_PARAMETERS_FILE_NAME);
         SimModelSchemaPath = LocalPath(Constants.Files.SIM_MODEL_SCHEMA_FILE_NAME);
         TEXTemplateFolderPath = AllUsersOrLocalPathForFolder(Constants.Files.APP_DATA_TEX_TEMPLATE_FOLDER_NAME, Constants.Files.LOCAL_TEX_TEMPLATE_FOLDER_NAME);
         ChartLayoutTemplateFolderPath = AllUsersOrLocalPathForFolder(Constants.Files.CHART_LAYOUT_FOLDER_NAME, Constants.Files.CHART_LAYOUT_FOLDER_NAME);

      }
      private string version(int minor) => $"{AssemblyVersion.Major}.{minor}";

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

      protected string AllUsersFile(string fileName) => Path.Combine(AllUsersFolderPath, fileName);

      /// <summary>
      /// Returns a local full path for the file with name <paramref name="fileName"/>
      /// </summary>
      protected string LocalPath(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

      private string dataFolderFor(string rootPath) => Path.Combine(rootPath, ApplicationFolderPathWithRevision(MajorVersion));

      private string createApplicationDataOrLocalPathFor(string appDataName, string localName, Func<string, bool> existsFunc)
      {
         var applicationDataOrLocal = AllUsersFile(appDataName);
         if (existsFunc(applicationDataOrLocal))
            return applicationDataOrLocal;

         //try local if id does not exist
         var localPath = LocalPath(localName);
         if (existsFunc(localPath))
            return localPath;

         //neither app data nor local exist, return app data
         return applicationDataOrLocal;
      }

      protected string AllUsersOrLocalPathForFile(string fileName) => createApplicationDataOrLocalPathFor(fileName, fileName, FileHelper.FileExists);

      protected string AllUsersOrLocalPathForFolder(string folderNameAppData, string folderNameLocal) => createApplicationDataOrLocalPathFor(folderNameAppData, folderNameLocal, DirectoryHelper.DirectoryExists);


   }
}