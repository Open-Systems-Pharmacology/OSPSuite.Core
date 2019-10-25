using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Serialization
{
   public class ParameterIdentificationFeedbackEditorSettingsXmlSerializer : ViewSettingsXmlSerializer<ParameterIdentificationFeedbackEditorSettings>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.RefreshFeedback);
      }
   }
}