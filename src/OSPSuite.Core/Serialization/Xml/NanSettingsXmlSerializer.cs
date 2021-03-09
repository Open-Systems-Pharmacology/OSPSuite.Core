using OSPSuite.Core.Import;
using System;
using System.Xml.Linq;
using static OSPSuite.Core.Import.NanSettings;

namespace OSPSuite.Core.Serialization.Xml
{
   public class NanSettingsXmlSerializer : OSPSuiteXmlSerializer<NanSettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.Indicator);
         Map(x => x.Action);
      }
   }
}
