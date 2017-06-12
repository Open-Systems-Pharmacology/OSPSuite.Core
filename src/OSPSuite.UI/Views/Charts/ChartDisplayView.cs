using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Charts.Native;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using Axis = OSPSuite.Core.Chart.Axis;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartDisplayView : BaseUserControl, IChartDisplayView, IViewWithPopup
   {
      private IChartDisplayPresenter _presenter;
      private readonly IFormatter<double> _doubleFormatter;
      private DiagramZoomRectangleService _diagramZoomRectangleService;
      private readonly LabelControl _hintControl;
      private ChartTitle _previewChartOrigin;
      private bool _axisEditEnabled;
      private bool _curveEditEnabled;
      private bool _axisHotTrackingEnabled;
      private Series _dummySeries;
      public bool Updating { get; private set; }

      public ChartDisplayView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _doubleFormatter = new FormatterFactory().CreateFor<double>();
         _hintControl = new LabelControl();
         _barManager.Images = imageListRetriever.AllImagesForContextMenu;
         SetDockStyle(DockStyle.Fill);
      }

      public override Color BackColor
      {
         get => _chartControl.BackColor;
         set
         {
            _chartControl.BackColor = value;
            _hintControl.BackColor = value;
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _axisEditEnabled = true;
         _curveEditEnabled = true;
         _axisHotTrackingEnabled = true;

         _chartControl.SelectionMode = ElementSelectionMode.Multiple;

         // create temporary XYDiagram, although the diagram is deleted after removal of dummy series,
         // the diagrams properties are stored somehow. See DevExpress.B145291
         var s1 = new Series("s1", ViewType.Line);
         s1.Points.Add(new SeriesPoint("A", 1));
         _chartControl.Series.Add(s1);

         _chartControl.Legend.MarkerSize = DataChartConstants.Display.LegendMarkerSize;
         _chartControl.Legend.Direction = LegendDirection.BottomToTop;
         xyDiagram.EnableAxisXZooming = true;
         xyDiagram.EnableAxisYZooming = true;
         xyDiagram.ZoomingOptions.UseKeyboard = false;
         xyDiagram.EnableAxisXScrolling = false;
         xyDiagram.EnableAxisYScrolling = false;

         _diagramZoomRectangleService = new DiagramZoomRectangleService(_chartControl, zoomAction);
         _diagramZoomRectangleService.InitializeZoomFor(xyDiagram);

         SetFontAndSizeSettings(ChartFontAndSizeSettings.Default);

         _chartControl.Series.Remove(s1);

         _chartControl.CrosshairEnabled = DefaultBoolean.False;

         _chartControl.ObjectHotTracked += (o, e) => OnEvent(() => onObjectHotTracked(e));
         _chartControl.ObjectSelected += (o, e) => OnEvent(() => onObjectSelected(e));
         _chartControl.Zoom += (o, e) => OnEvent(() => onZoom(e));
         _chartControl.Scroll += (o, e) => OnEvent(() => onScroll(e));

         _chartControl.CustomDrawSeriesPoint += (o, e) => OnEvent(() => drawSeriesPoint(e));

         AllowDrop = true;

         _chartControl.Click += (o, e) => OnEvent(() => chartClick(e as MouseEventArgs));
         _chartControl.DoubleClick += (o, e) => OnEvent(() => chartDoubleClick(e as MouseEventArgs));

         _chartControl.MouseMove += (o, e) => OnEvent(() => OnChartMouseMove(e));
         _chartControl.DragOver += (o, e) => OnEvent(() => OnDragOver(e));
         _chartControl.DragDrop += (o, e) => OnEvent(() => OnDragDrop(e));
         _chartControl.Resize += (o, e) => OnEvent(() => OnResize(e));

         initializeHintControl();
      }

      private void drawSeriesPoint(CustomDrawSeriesPointEventArgs e)
      {
         if (_presenter.IsSeriesLLOQ(e.Series.Name))
            return;

         if (!_presenter.IsPointBelowLLOQ(e.SeriesPoint.Values, e.Series.Name))
            return;

         var drawOptions = e.SeriesDrawOptions as PointDrawOptions;
         if (drawOptions == null)
            return;
         drawOptions.Marker.BorderColor = drawOptions.Color;
         drawOptions.Color = Color.Transparent;
      }

      private void initializeHintControl()
      {
         _hintControl.BackColor = _chartControl.BackColor;
         _hintControl.AutoSizeMode = LabelAutoSizeMode.None;
         _hintControl.Font = new Font(_chartControl.Legend.Font.FontFamily, 20);
         _hintControl.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
         _hintControl.Dock = DockStyle.Fill;
      }

      private static bool canDropMovingLegendHere(ChartHitInfo hitInfo)
      {
         return inLegendSeries(hitInfo);
      }

      private static bool inLegendSeries(ChartHitInfo hitInfo)
      {
         return hitInfo.InLegend && hitInfo.InSeries;
      }

      private void dropLegendHere(ISeries series, ISeries seriesBeingMoved)
      {
         _presenter.MoveSeriesInLegend(_presenter.CurveFromSeriesId(seriesBeingMoved.Name), _presenter.CurveFromSeriesId(series.Name));
      }

      protected void OnChartMouseMove(MouseEventArgs e)
      {
         OnMouseMove(e);
         var hitInfo = _chartControl.CalcHitInfo(e.Location);
         if (canMoveLegendItem(e, hitInfo) && e.Button.IsLeft())
         {
            _chartControl.DoDragDrop(hitInfo.Series.DowncastTo<Series>(), DragDropEffects.Move);
         }
      }

      private static bool canMoveLegendItem(MouseEventArgs e, ChartHitInfo hitInfo)
      {
         return hitInfo.InLegend && hitInfo.InSeries && e.Button == MouseButtons.Left;
      }

      protected override void OnDragDrop(DragEventArgs dragDropEventArgs)
      {
         base.OnDragDrop(dragDropEventArgs);
         var hitInfo = _chartControl.CalcHitInfo(chartControlPointFromDragDropEventArgs(dragDropEventArgs));
         if (!canDropMovingLegendHere(hitInfo)) return;

         var seriesBeingMoved = dragDropEventArgs.Data.GetData(typeof(Series)).DowncastTo<Series>();
         if (seriesBeingMoved != null)
            dropLegendHere(hitInfo.Series.DowncastTo<Series>(), seriesBeingMoved);
      }

      protected override void OnDragOver(DragEventArgs dragOverEventArgs)
      {
         base.OnDragOver(dragOverEventArgs);

         if (inLegendSeries(_chartControl.CalcHitInfo(chartControlPointFromDragDropEventArgs(dragOverEventArgs))))
         {
            dragOverEventArgs.Effect = DragDropEffects.Move;
         }
      }

      private Point chartControlPointFromDragDropEventArgs(DragEventArgs dragOverEventArgs)
      {
         return _chartControl.PointToClient(new Point(dragOverEventArgs.X, dragOverEventArgs.Y));
      }

      protected override void OnResize(EventArgs eventArgs)
      {
         base.OnResize(eventArgs);
         // reset Axis Scaling
         _presenter?.RefreshAxisBinders();
      }

      private void chartDoubleClick(MouseEventArgs mouseEventArgs)
      {
         if (!mouseEventArgs.Button.IsLeft()) return;

         _toolTipController.HideHint();

         var hitInfo = _chartControl.CalcHitInfo(mouseEventArgs.Location);
         activateFirstContextMenuFor(hitInfo);
      }

      private void chartClick(MouseEventArgs mouseEventArgs)
      {
         if (!mouseEventArgs.Button.IsRight()) return;

         _toolTipController.HideHint();

         var hitInfo = _chartControl.CalcHitInfo(mouseEventArgs.Location);
         showContextMenuFor(hitInfo, mouseEventArgs.Location);
      }

      private void doContextMenuActionFor(ChartHitInfo hitInfo, Action<Curve> doActionForCurve, Action<Axis> doActionForAxis, Action doDefaultAction)
      {
         var axis = getAxisThatIsWithinRange(hitInfo);
         if (hitInfo.InSeries)
         {
            if (!_curveEditEnabled) return;

            var curve = _presenter.CurveFromSeriesId(hitInfo.Series.DowncastTo<Series>().Name);
            if (curve != null)
               doActionForCurve(curve);
         }
         else if (axis != null)
         {
            if (!_axisEditEnabled) return;
            doActionForAxis(axis);
         }
         else
         {
            doDefaultAction?.Invoke();
         }
      }

      private void activateFirstContextMenuFor(ChartHitInfo hitInfo)
      {
         doContextMenuActionFor(hitInfo, _presenter.ActivateFirstContextMenuEntryFor, _presenter.ActivateFirstContextMenuEntryFor, doDefaultAction: null);
      }

      private void showContextMenuFor(ChartHitInfo hitInfo, Point location)
      {
         doContextMenuActionFor(hitInfo,
            curve => _presenter.ShowContextMenu(new CurveViewItem(_presenter.Chart, curve),location),
            axis => _presenter.ShowContextMenu(new AxisViewItem(_presenter.Chart, axis), location),
            () => _presenter.ShowContextMenu(new CurveChartViewItem(_presenter.Chart), location));
      }

      private Axis getAxisThatIsWithinRange(ChartHitInfo hitInfo)
      {
         if (isInXAxis(hitInfo))
            return _presenter.AxisBy(AxisTypes.X);

         if (xyDiagram == null)
            return null;

         if (isCloseEnoughToYAxis(hitInfo, xyDiagram.AxisY))
            return _presenter.AxisBy(AxisTypes.Y);

         if (isCloseEnoughToYAxis(hitInfo, xyDiagram.SecondaryAxesY.GetAxisByName(AxisTypes.Y2.ToString())))
            return _presenter.AxisBy(AxisTypes.Y2);

         if (isCloseEnoughToYAxis(hitInfo, xyDiagram.SecondaryAxesY.GetAxisByName(AxisTypes.Y3.ToString())))
            return _presenter.AxisBy(AxisTypes.Y3);

         return null;
      }

      private bool isCloseEnoughToYAxis(ChartHitInfo hitInfo, AxisBase axisY)
      {
         return hitInfo.InAxis && areAxesEqual(hitInfo.Axis, axisY);
      }

      private bool areAxesEqual(AxisBase axis, AxisBase anotherAxis)
      {
         return axis.Equals(anotherAxis);
      }

      private bool isInXAxis(ChartHitInfo hitInfo)
      {
         return hitInfo.InAxis && areAxesEqual(hitInfo.Axis, xyDiagram.AxisX);
      }

      private void zoomAction(Control control, Rectangle rectangle)
      {
         var cc = control as IChartContainer;
         if (cc != null && cc.Chart != null && !rectangle.IsEmpty)
            cc.Chart.PerformZoomIn(rectangle);
      }


      private XYDiagram xyDiagram => _chartControl.XYDiagram;

      private Color diagramBackColor
      {
         get => xyDiagram?.DefaultPane.BackColor ?? Color.Empty;
         set
         {
            if (xyDiagram == null) return;
            xyDiagram.DefaultPane.FillStyle.FillMode = FillMode.Solid;
            xyDiagram.DefaultPane.BackColor = value;
         }
      }

      public void AttachPresenter(IChartDisplayPresenter presenter)
      {
         _presenter = presenter;
      }

      public Action<int> HotTracked { private get; set; } = i => { };

      private string title
      {
         set => _chartControl.Title = value;
      }

      private string description
      {
         set => _chartControl.Description = value;
      }

      private LegendPositions legendPosition
      {
         set => _chartControl.Legend.LegendPosition(value);
      }

      public Size GetDiagramSize()
      {
         try
         {
            var xAxisRange = xyDiagram.AxisX.VisualRange;
            var yAxisRange = xyDiagram.AxisX.VisualRange;

            int minX = xyDiagram.DiagramToPoint(Convert.ToSingle(xAxisRange.MinValue), Convert.ToSingle(yAxisRange.MinValue)).Point.X;
            int maxX = xyDiagram.DiagramToPoint(Convert.ToSingle(xAxisRange.MaxValue), Convert.ToSingle(yAxisRange.MinValue)).Point.X;
            int minY = xyDiagram.DiagramToPoint(Convert.ToSingle(xAxisRange.MinValue), Convert.ToSingle(yAxisRange.MinValue)).Point.Y;
            int maxY = xyDiagram.DiagramToPoint(Convert.ToSingle(xAxisRange.MinValue), Convert.ToSingle(yAxisRange.MaxValue)).Point.Y;
            return new Size(maxX - minX, maxY - minY);
         }
         catch
         {
            // not understood, under which circumstances exception is thrown
            return new Size(0, 0);
         }
      }

      public void SetDockStyle(DockStyle dockStyle)
      {
         _chartControl.Dock = dockStyle;
      }

      private void onScroll(ChartScrollEventArgs e)
      {
         var xRange = e.NewXRange;
         var yRange = e.NewYRange;
         _presenter.SetVisibleRange(
            Convert.ToSingle(xRange.MinValue),
            Convert.ToSingle(xRange.MaxValue),
            Convert.ToSingle(yRange.MinValue),
            Convert.ToSingle(yRange.MaxValue));
      }

      private void onZoom(ChartZoomEventArgs e)
      {
         postZoom(e.NewXRange, e.NewYRange);
      }

      private void postZoom(RangeInfo xRange, RangeInfo yRange)
      {
         // if Range is complete, set min = max = 0 as convention for AutoScale
         float? xMin = null, xMax = null, yMin = null, yMax = null;

         if (!rangeComplete(xRange, xyDiagram.AxisX))
         {
            xMin = Convert.ToSingle(xRange.MinValue);
            xMax = Convert.ToSingle(xRange.MaxValue);
         }

         if (!rangeComplete(yRange, xyDiagram.AxisY))
         {
            yMin = Convert.ToSingle(yRange.MinValue);
            yMax = Convert.ToSingle(yRange.MaxValue);
         }

         _presenter.SetVisibleRange(xMin, xMax, yMin, yMax);
      }

      public PointF GetPointsForSecondaryAxis(float x, float y, AxisTypes yAxisType)
      {
         var primaryYAxisCoordinate = getPrimaryYAxisCoordinate(x, y);

         var axisToConvertTo = getAxisFromType(yAxisType);

         if (axisToConvertTo == null)
            return PointF.Empty;

         return new PointF(Convert.ToSingle(primaryYAxisCoordinate.NumericalArgument), Convert.ToSingle(primaryYAxisCoordinate.GetAxisValue(axisToConvertTo).NumericalValue));
      }

      private DiagramCoordinates getPrimaryYAxisCoordinate(float x, float y)
      {
         var controlCoordinate = xyDiagram.DiagramToPoint(x, y);
         return xyDiagram.PointToDiagram(controlCoordinate.Point);
      }

      private AxisBase getAxisFromType(AxisTypes axisTypeToConvertTo)
      {
         if (axisTypeToConvertTo == AxisTypes.Y)
            return xyDiagram.AxisY;

         if (axisTypeToConvertTo == AxisTypes.Y2 && xyDiagram.SecondaryAxesY.Count > 0)
            return xyDiagram.SecondaryAxesY[0];

         if (axisTypeToConvertTo == AxisTypes.Y3 && xyDiagram.SecondaryAxesY.Count > 1)
            return xyDiagram.SecondaryAxesY[1];

         return null;
      }

      private static bool rangeComplete(RangeInfo range, DevExpress.XtraCharts.Axis axis)
      {
         return range.Min == axis.WholeRange.MinValueInternal
                && range.Max == axis.WholeRange.MaxValueInternal;
      }

      private void onObjectHotTracked(HotTrackEventArgs e)
      {
         if (toolTipIsForSeries(e.Object))
         {
            showToolTipForSeries(e, e.Object.DowncastTo<Series>());
         }
         else if (toolTipIsForAxis(e.Object) && _axisHotTrackingEnabled)
         {
            showToolTipForAxis();
         }
         else
         {
            e.Cancel = true;
            _toolTipController.HideHint();
         }
      }

      private bool toolTipIsForSeries(object eventObject)
      {
         var series = eventObject as Series;
         return series != null && series.Points.Any() && series.View.IsAnImplementationOf<XYDiagramSeriesViewBase>();
      }

      private bool toolTipIsForAxis(object eventObject)
      {
         return eventObject is AxisBase;
      }

      private void showToolTipForAxis()
      {
         if (!_axisEditEnabled) return;

         var toolTipString = ToolTips.ToolTipForAxis;
         _toolTipController.ShowHint(toolTipString);
      }

      private void showToolTipForSeries(HotTrackEventArgs e, Series series)
      {
         if (!e.HitInfo.InLegend)
         {
            raiseHotTrackAction(e);
            _toolTipController.ShowHint(generateToolTipForSeriesPoint(series, e.HitInfo.HitPoint));
         }
         else
         {
            _toolTipController.ShowHint(generateToolTipForLegend(series.LegendText));
         }
      }

      private void raiseHotTrackAction(HotTrackEventArgs e)
      {
         var seriesPoint = findPointInSeries(e.HitInfo.HitPoint, e.Series());
         var dataRowView = seriesPoint.Tag as DataRowView;
         if (dataRowView == null) return;

         var index = _presenter.GetSourceIndexFromDataRow(e.Series().Name, dataRowView.Row);
         HotTracked(index);
      }

      private string generateToolTipForLegend(string legendText)
      {
         return ToolTips.ToolTipForLegendEntry(legendText, editable: _curveEditEnabled);
      }

      private string generateToolTipForSeriesPoint(Series series, Point hitPoint)
      {
         if (_presenter.IsSeriesLLOQ(series.Name))
            return generateToolTipForLLOQ(series, hitPoint);
         return generateToolTipForSeriesDataPoint(series, hitPoint);
      }

      private string generateToolTipForLLOQ(Series series, Point hitPoint)
      {
         var legendText = series.LegendText;
         var lowerLimitOfQuantification = _doubleFormatter.Format(findPointInSeries(hitPoint, series).Values[0]);
         var displayUnit = _presenter.DisplayUnitsFor(series.Name);

         return ToolTips.ToolTipForLLOQ(legendText, $"{lowerLimitOfQuantification} {displayUnit}");
      }

      private string generateToolTipForSeriesDataPoint(Series series, Point hitPoint)
      {
         var seriesView = series.View.DowncastTo<XYDiagramSeriesViewBase>();
         var xAxisTitle = seriesView.AxisX.Title.Text;
         var yAxisTitle = seriesView.AxisY.Title.Text;
         var legendText = series.LegendText;
         var curveDescription = _presenter.CurveDescriptionFromSeriesId(series.Name);

         var nextPoint = findPointInSeries(hitPoint, series);
         return ToolTips.ToolTipForSeriesPoint(
            legendText,
            xAxisTitle,
            yAxisTitle,
            _doubleFormatter.Format(nextPoint.NumericalArgument),
            _doubleFormatter.Format(nextPoint.Values[0]), nextPoint.Values.Length > 1
               ? _doubleFormatter.Format(nextPoint.Values[1])
               : null, editable: _curveEditEnabled, description: curveDescription);
      }

      private SeriesPoint findPointInSeries(Point hitPoint, Series series)
      {
         var hitPointCoord = xyDiagram.PointToDiagram(hitPoint);
         // find next Point from Series
         var nextPoint = series.Points[0];
         foreach (SeriesPoint point in series.Points)
         {
            if (Math.Abs(point.NumericalArgument - hitPointCoord.NumericalArgument)
                < Math.Abs(nextPoint.NumericalArgument - hitPointCoord.NumericalArgument))
               nextPoint = point;
         }
         return nextPoint;
      }

      private void onObjectSelected(HotTrackEventArgs e)
      {
         e.Cancel = true;
      }

      public void SetFontAndSizeSettings(ChartFontAndSizeSettings fontAndSizeSettings)
      {
         _chartControl.SetFontAndSizeSettings(fontAndSizeSettings, _chartControl.Size);
      }

      public void CopyToClipboardWithExportSettings()
      {
         _presenter.Chart.CopyToClipboard(_chartControl);
      }

      public void ReOrderLegend()
      {
         var seriesList = _chartControl.Series.ToArray();

         _chartControl.Series.Clear();

         var sortedSeries = seriesList.OrderBy(series =>
            _presenter.LegendIndexFromSeriesId(series.Name)).Reverse().ToArray();

         _chartControl.Series.AddRange(sortedSeries);
      }

      public void SetNoCurvesSelectedHint(string hint)
      {
         _hintControl.Text = hint;
      }

      public void UpdateSettings(CurveChart chart)
      {
         title = chart.Title;
         description = chart.Description;
         Name = chart.Name;
         legendPosition = chart.ChartSettings.LegendPosition;
         BackColor = chart.ChartSettings.BackColor;
         diagramBackColor = chart.ChartSettings.DiagramBackColor;
      }

      public void PreviewOriginText()
      {
         ClearOriginText();
         if (_presenter.ShouldIncludeOriginData())
            _previewChartOrigin = _presenter.Chart.AddOriginData(_chartControl);
      }

      public void ClearOriginText()
      {
         if (_chartControl.Titles.Contains(_previewChartOrigin))
            _chartControl.Titles.Remove(_previewChartOrigin);
      }

      public void DisableAxisEdit()
      {
         _axisEditEnabled = false;
      }

      public void DisableAxisHotTracking()
      {
         _axisHotTrackingEnabled = false;
      }

      public object ChartControl => _chartControl;

      public void BeginUpdate()
      {
         beginChartUpdate();
         Updating = true;
      }

      public void EndUpdate()
      {
         endChartUpdate();
         Updating = false;
      }

      private void beginChartUpdate()
      {
         //Required to add at least one series in order to create the Diagram
         if (xyDiagram == null)
         {
            _dummySeries = new Series("dummy", ViewType.ScatterLine);
            _chartControl.Series.Add(_dummySeries);
         }

         _chartControl.BeginInit();
      }

      private void endChartUpdate()
      {
         try
         {
            if (_dummySeries == null)
               return;

            _chartControl.Series.Remove(_dummySeries);
         }
         finally
         {
            _chartControl.EndInit();
            _dummySeries = null;
         }
      }

      public void DisableCurveEdit()
      {
         _curveEditEnabled = false;
      }

      public void ShowChart()
      {
         if (Controls.Contains(_chartControl))
            return;

         this.FillWith(_chartControl);
      }

      public void ShowHint()
      {
         if (Controls.Contains(_hintControl) || string.IsNullOrEmpty(_hintControl.Text))
            return;

         this.FillWith(_hintControl);
      }

      public BarManager PopupBarManager => _barManager;
   }
}