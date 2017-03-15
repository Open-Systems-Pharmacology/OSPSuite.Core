using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Serialization
{
   public class ViewSettingsXmlSerializer<TTViewSettings> : PresentationXmlSerializer<TTViewSettings> where TTViewSettings:ViewSettings
   {
      public override void PerformMapping()
      {
         Map(x => x.Size);
         Map(x => x.Location);
      }
   }
}