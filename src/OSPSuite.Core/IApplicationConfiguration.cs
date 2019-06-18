using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   /// <summary>
   ///    Standard interface defining common properties for all application in the suite
   /// </summary>
   public interface IApplicationConfiguration
   {
      /// <summary>
      ///    Folder path where template layouts are being saved (for all users, installed by setup)
      /// </summary>
      string ChartLayoutTemplateFolderPath { get; }

      /// <summary>
      ///    Folder path where report templates are being saved (for all users, installed by setup)
      /// </summary>
      string TeXTemplateFolderPath { get; }

      /// <summary>
      ///    Path of the pk analyses file
      /// </summary>
      string PKParametersFilePath { get; }

      /// <summary>
      ///    Path of the license agreement file
      /// </summary>
      string LicenseAgreementFilePath { get; }

      /// <summary>
      ///    Returns the full version of the software in a format 1.2.3 (Version, Update, Build)
      /// </summary>
      string FullVersion { get; }

      /// <summary>
      ///    Returns the full version of the software in a format 1.2 - Build 123
      /// </summary>
      string FullVersionDisplay { get; }

      /// <summary>
      ///    Returns the version of the software in the format 1.2
      /// </summary>
      string Version { get; }

      /// <summary>
      /// Returns the internal version of the software (for internal use only)
      /// </summary>
      int InternalVersion { get; }

      /// <summary>
      ///    Returns the version of the software (one digit)
      /// </summary>
      int Major { get; }

      /// <summary>
      ///    Returns the version of the software (one digit)
      /// </summary>
      int Minor { get; }

      /// <summary>
      ///    Returns the build version (typically set by CI)
      /// </summary>
      int Build { get; }

      /// <summary>
      /// Release description. Typically empty for a release product. Could be 1.2 alpha, 1.2 beta, 1.2. EAP during development
      /// </summary>
      string ReleaseDescription { get; }

      /// <summary>
      ///    Returns the name of the application
      /// </summary>
      string ProductName { get; }

      /// <summary>
      ///    Returns the product corresponding to the current application
      /// </summary>
      Origin Product { get; }

      /// <summary>
      ///    Returns the name of the application with trademark
      /// </summary>
      string ProductNameWithTrademark { get; }

      /// <summary>
      /// Returns the name of the application with Major Version and release description. Typically used for application title
      /// </summary>
      string ProductDisplayName { get; }

      ApplicationIcon Icon { get; }
      
      /// <summary>
      ///    Paths of the current user specific settings file (current user only)
      /// </summary>
      string UserSettingsFilePath { get; }

      /// <summary>
      ///    Returns a possible enumeration containing the path of user settings that can be loaded. (Starting from the most recent
      ///    one down to the first available one)
      /// </summary>
      IEnumerable<string> UserSettingsFilePaths { get; }
      
      /// <summary>
      ///    Path of the application specific settings file (for all users)
      /// </summary>
      string ApplicationSettingsFilePath { get; }

      /// <summary>
      /// Returns a possible enumeration containing the path of application settings that can be loaded. (Starting from the most recent one down to the first available one)
      /// </summary>
      IEnumerable<string> ApplicationSettingsFilePaths { get; }
      
      /// <summary>
      ///    Returns the url of the issue tracker
      /// </summary>
      string IssueTrackerUrl { get; }

      /// <summary>
      ///    Returns the name of the suite with the Version in format X.Y.Z
      /// </summary>
      string OSPSuiteNameWithVersion { get; }

      /// <summary>
      ///    Returns the application settings folder path (valid for all users)
      /// </summary>
      string AllUsersFolderPath { get; }

      /// <summary>
      ///    Returns the current user settings folder path
      /// </summary>
      string CurrentUserFolderPath { get; }

      /// <summary>
      ///    Path of the schema used to validate our models
      /// </summary>
      string SimModelSchemaFilePath { get; }

      /// <summary>
      ///    Path of the dimension file
      /// </summary>
      string DimensionFilePath { get; }

      /// <summary>
      /// Where can the user change the watermark option in the user interface. Location is application specific
      /// </summary>
      string WatermarkOptionLocation { get; }
   }
}