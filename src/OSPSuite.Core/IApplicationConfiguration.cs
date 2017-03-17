using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   /// <summary>
   /// Standard interface defining common properties for all application in the suite
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
      string TEXTemplateFolderPath { get; }

      /// <summary>
      ///    Path of the pk analyses file
      /// </summary>
      string PKParametersFilePath { get; set; }
      
      /// <summary>
      /// Path of the license agreement file
      /// </summary>
      string LicenseAgreementFilePath { get; }

      /// <summary>
      ///    Returns the full version of the software in a format 1.2.3 - Build 123
      /// </summary>
      string FullVersion { get; }

      /// <summary>
      ///    Returns the version of the software in the format 1.2.3
      /// </summary>
      string Version { get; }

      /// <summary>
      ///    Returns the version of the software in the format 1.2
      /// </summary>
      string MajorVersion { get; }

      /// <summary>
      ///    Returns the build version
      /// </summary>
      string BuildVersion { get; }

      /// <summary>
      /// Returns the name of the application
      /// </summary>
      string ProductName { get; }

      /// <summary>
      /// Returns the product corresponding to the current application
      /// </summary>
      Origin Product { get; }

      /// <summary>
      /// Returns the name of the application with trademark
      /// </summary>
      string ProductNameWithTrademark { get; }

      ApplicationIcon Icon { get; }

      /// <summary>
      /// Returns a possible enumeration containg the path of user settings that can be loaded. (Starting from the most recent one down to the first available one)
      /// </summary>
      IEnumerable<string> UserApplicationSettingsFilePaths { get; }

      /// <summary>
      /// Returns the url of the issue tracker
      /// </summary>
      string IssueTrackerUrl { get; }
   }
}