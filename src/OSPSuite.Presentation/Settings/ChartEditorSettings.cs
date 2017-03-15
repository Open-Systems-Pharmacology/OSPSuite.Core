using System.Collections.Generic;

namespace OSPSuite.Presentation.Settings
{
   public class ChartEditorSettings
   {
      private readonly IList<GridColumnSettings> _dataBrowserColumnSettings;
      private readonly IList<GridColumnSettings> _curveOptionsColumnSettings;
      private readonly IList<GridColumnSettings> _axisOptionsColumnSettings;
      public string DockingLayout { get; set; }

      public ChartEditorSettings()
      {
         _dataBrowserColumnSettings = new List<GridColumnSettings>();
         _curveOptionsColumnSettings = new List<GridColumnSettings>();
         _axisOptionsColumnSettings = new List<GridColumnSettings>();
      }

      public IEnumerable<GridColumnSettings> DataBrowserColumnSettings
      {
         get { return _dataBrowserColumnSettings; }
      }

      public IEnumerable<GridColumnSettings> CurveOptionsColumnSettings
      {
         get { return _curveOptionsColumnSettings; }
      }

      public IEnumerable<GridColumnSettings> AxisOptionsColumnSettings
      {
         get { return _axisOptionsColumnSettings; }
      }

      public void AddDataBrowserColumnSetting(GridColumnSettings columnSettings)
      {
         _dataBrowserColumnSettings.Add(columnSettings);
      }

      public void AddCurveOptionsColumnSetting(GridColumnSettings columnSettings)
      {
         _curveOptionsColumnSettings.Add(columnSettings);
      }

      public void AddAxisOptionsColumnSetting(GridColumnSettings columnSettings)
      {
         _axisOptionsColumnSettings.Add(columnSettings);
      }
   }

   public class ChartEditorAndDisplaySettings
   {
      public ChartEditorSettings EditorSettings { get; set; }
      public string DockingLayout { get; set; }
   }

}