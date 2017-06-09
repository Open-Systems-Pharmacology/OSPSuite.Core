using System;
using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartDisplayView : IView<IChartDisplayPresenter>, IBatchUpdatable
   {
      /// <summary>
      ///    When a series point is hot tracked in the chart, this action is invoked
      /// </summary>
      Action<int> HotTracked { set; }

      Size GetDiagramSize();

      void SetDockStyle(DockStyle dockStyle);
      void SetFontAndSizeSettings(ChartFontAndSizeSettings fontAndSizeSettings);

      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;

      /// <summary>
      ///    Copies the chart to clipboard as an image using export settings if defined
      ///    Otherwise uses current visual settings
      /// </summary>
      void CopyToClipboardWithExportSettings();

      /// <summary>
      ///    Re orders the legend according to given index
      /// </summary>
      void ReOrderLegend();

      /// <summary>
      ///    If no curves have been added to the chart, then <paramref name="hint" /> text will appear in place of the empty
      ///    chart
      /// </summary>
      void SetNoCurvesSelectedHint(string hint);

      void UpdateSettings(CurveChart chart);

      void PreviewOriginText();
      void ClearOriginText();

      /// <summary>
      ///    Disable the control from showing the axis editor
      /// </summary>
      void DisableAxisEdit();

      /// <summary>
      ///    Disable the control from showing the curve editor
      /// </summary>
      void DisableCurveEdit();

      /// <summary>
      ///    Converts <paramref name="x" /> and <paramref name="y" /> coordinates for a primary Y axis
      ///    to a secondary Y axis coordinate
      /// </summary>
      PointF GetPointsForSecondaryAxis(float x, float y, AxisTypes yAxisType);

      /// <summary>
      ///    Disables hot tracking for axes
      /// </summary>
      void DisableAxisHotTracking();

      object ChartControl { get; }

      void ShowHint();

      void ShowChart();
   }
}