using System;
using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartDisplayView : IView<IChartDisplayPresenter>
   {
      /// <summary>
      /// When a series point is hot tracked in the chart, this action is invoked
      /// </summary>
      Action<int> HotTracked { set; }

      string Title { get; set; }
      string Description { get; set; }
      LegendPositions LegendPosition { set; }
      Color BackColor { get; set; }
      Color DiagramBackColor { get; set; }
      bool XGridLine { get; set; }
      bool YGridLine { get; set; }
      Size GetDiagramSize();
      void RemoveCurve(string id);
      bool NoCurves();

      /// <summary>
      ///    Forces the control to invalidate its client area
      ///    and immediately redraw itself and any child controls. (Windows.Forms.Control)
      /// </summary>
      void Refresh();

      /// <summary>
      ///    This Method is for internal use only and should not be used by an application directly.
      ///    Reloads data from the underlying data source and repaints the diagram area.
      /// </summary>
      void RefreshData();

      void RemoveAxis(AxisTypes axisType);

      void SetDockStyle(DockStyle dockStyle);
      void SetFontAndSizeSettings(ChartFontAndSizeSettings fontAndSizeSettings);

      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;
      IAxisAdapter GetAxisAdapter(IAxis axis);
      void BeginSeriesUpdate();
      void EndSeriesUpdate();
      void BeginInit();
      void EndInit();
      void AddCurve(ICurveAdapter curveAdapter);

      /// <summary>
      /// Resets the zoom level of the chart to no zoom
      /// </summary>
      void ResetChartZoom();

      /// <summary>
      /// Copies the chart to clipboard as an image using export settings if defined
      /// Otherwise uses current visual settings
      /// </summary>
      void CopyToClipboardWithExportSettings();

      /// <summary>
      /// Re orders the legend according to given index
      /// </summary>
      void ReOrderLegend();

      /// <summary>
      /// If no curves have been added to the chart, then <paramref name="hint"/> text will appear in place of the empty chart
      /// </summary>
      void SetNoCurvesSelectedHint(string hint);

      void UpdateSettings(ICurveChart chart);
      void PreviewOriginText();
      void ClearOriginText();

      /// <summary>
      /// Disable the control from showing the axis editor
      /// </summary>
      void DisableAxisEdit();

      /// <summary>
      /// Disable the control from showing the curve editor
      /// </summary>
      void DisableCurveEdit();

      /// <summary>
      /// Converts <paramref name="x"/> and <paramref name="y"/> coordinates for a primary Y axis
      /// to a secondary Y axis coordinate
      /// </summary>
      PointF GetPointsForSecondaryAxis(float x, float y, AxisTypes axisTypeToConvertTo);

      /// <summary>
      /// Disables hot tracking for axes
      /// </summary>
      void DisableAxisHotTracking();
   }
}