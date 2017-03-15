using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Serialization
{
   public class JournalPageEditorSettingsXmlSerializer : ViewSettingsXmlSerializer<JournalPageEditorSettings>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ShowTableGridLines);
      }
   }
}