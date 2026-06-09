using OSPSuite.Core.Import;

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
