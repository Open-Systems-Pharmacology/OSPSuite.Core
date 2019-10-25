using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Serialization
{
   public abstract class DefaultPresentationSettingsXmlSerializer<TPresenterSettings> : PresentationXmlSerializer<TPresenterSettings> where TPresenterSettings : DefaultPresentationSettings
   {
      public override void PerformMapping()
      {
         Map(x => x.PresenterPropertyCache);
      }
   }

   public class DefaultPresentationSettingsXmlSerializer : DefaultPresentationSettingsXmlSerializer<DefaultPresentationSettings>
   {
   }
}