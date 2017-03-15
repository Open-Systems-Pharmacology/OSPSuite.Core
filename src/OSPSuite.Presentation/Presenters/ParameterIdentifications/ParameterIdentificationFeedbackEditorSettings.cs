using OSPSuite.Assets;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class ParameterIdentificationFeedbackEditorSettings : ViewSettings
   {
      public bool RefreshFeedback { get; set; }

      public ParameterIdentificationFeedbackEditorSettings()
      {
         Location = SizeAndLocation.ParameterIdentificationFeedbackEditorLocation;
         Size = SizeAndLocation.ParameterIdentificationFeedbackEditorSize;
         RefreshFeedback = true;
      }
   }
}