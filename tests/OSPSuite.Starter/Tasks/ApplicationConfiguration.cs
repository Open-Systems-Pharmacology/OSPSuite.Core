using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Starter.Tasks
{
   internal class ApplicationConfiguration : IApplicationConfiguration
   {
      public string ChartLayoutTemplateFolderPath
      {
         get { throw new NotSupportedException(); }
      }

      public string TEXTemplateFolderPath
      {
         get { throw new NotSupportedException(); }
      }

      public string PKParametersFilePath { get; set; }

      public string FullVersion
      {
         get { return "10.0.0"; }
      }

      public string Version
      {
         get { return "10.0"; }
      }

      public string MajorVersion
      {
         get { return "10"; }
      }

      public string BuildVersion
      {
         get { return "0"; }
      }

      public string ProductName
      {
         get { return "OSPSuite.Core"; }
      }

      public Origin Product
      {
         get { return Origins.Other; }
      }

      public string ProductDisplayName
      {
         get { return "OSPSuite.Core"; }
      }

      public string ProductNameWithTrademark
      {
         get { return "OSPSuite.Core"; }
      }

      public ApplicationIcon Icon
      {
         get { return ApplicationIcons.PKSim; }
      }

      public IEnumerable<string> UserApplicationSettingsFilePaths { get; }
      public string LicenseAgreementFilePath { get; }

      public string IssueTrackerUrl => "https://github.com/Open-Systems-Pharmacology/PK-Sim/issues";
      public string OSPSuiteNameWithVersion  =>  $"OSPSuite - {Version}";
   }
}