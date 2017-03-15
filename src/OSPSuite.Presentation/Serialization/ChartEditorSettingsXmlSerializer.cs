using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Serialization
{
   public class ChartEditorSettingsXmlSerializer : PresentationXmlSerializer<ChartEditorSettings>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.DataBrowserColumnSettings, x => x.AddDataBrowserColumnSetting);
         MapEnumerable(x => x.CurveOptionsColumnSettings, x => x.AddCurveOptionsColumnSetting);
         MapEnumerable(x => x.AxisOptionsColumnSettings, x => x.AddAxisOptionsColumnSetting);
         Map(x => x.DockingLayout);
      }
   }

   public class ChartEditorAndDisplaySettingsXmlSerializer : PresentationXmlSerializer<ChartEditorAndDisplaySettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.EditorSettings);
         Map(x => x.DockingLayout);
      }
   }
}