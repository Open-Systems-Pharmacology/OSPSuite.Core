using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Starter.Tasks
{
   internal class ApplicationConfiguration : IApplicationConfiguration
   {
      public string ChartLayoutTemplateFolderPath => "./AFolderThatShouldntExist";

      public string TEXTemplateFolderPath => throw new NotSupportedException();

      public string PKParametersFilePath { get; set; } = "OSPSuite.PKParameters.xml";

      public string FullVersion => "10.0.0";

      public string Version => "10.0";

      public string MajorVersion => "10";

      public string BuildVersion => "0";

      public string ProductName => "OSPSuite.Core";

      public Origin Product => Origins.Other;

      public string ProductDisplayName => "OSPSuite.Core";

      public string ProductNameWithTrademark => "OSPSuite.Core";

      public ApplicationIcon Icon => ApplicationIcons.PKSim;

      public IEnumerable<string> UserApplicationSettingsFilePaths { get; }
      public string LicenseAgreementFilePath { get; }

      public string IssueTrackerUrl => "https://github.com/Open-Systems-Pharmacology/PK-Sim/issues";
      public string OSPSuiteNameWithVersion => $"OSPSuite - {Version}";
   }
}