using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Presenter of ChartDisplay component, which displays a IChart.
   /// </summary>
   public interface IChartDisplayPresenter : 
      IPresenter<IChartDisplayView>,
      ICanCopyToClipboard,
      IPresenterWithContextMenu<IViewItem>,
      IListener<ChartUpdatedEvent>,
      IListener<ChartPropertiesChangedEvent>
   {
      /// <summary>
      ///    Displayed Chart.
      /// </summary>
      CurveChart Chart { get; }

      /// <summary>
      ///    Reloads xyData of Chart and refreshes display of Chart.
      /// </summary>
      void Refresh();

      /// <summary>
      ///    Event when something is dragged to Control. EventHandler should set Effect to Move if Drag is allowed.
      /// </summary>
      event EventHandler<IDragEvent> DragOver;

      /// <summary>
      ///    Event when something is dropped onto Control. EventHandler should set Effect.
      /// </summary>
      event EventHandler<IDragEvent> DragDrop;

      /// <summary>
      ///    This Property is for internal use only and should not be used by an application directly.
      ///    Set Chart.Axes properties instead.
      ///    Sets the visible range of Charts x and y Axes in Axis unit.
      ///    Control is not refreshed in this action.
      /// </summary>
      void SetVisibleRange(float? xMin, float? xMax, float? yMin, float? yMax);

      /// <summary>
      ///    Refreshes AxisAdapters with Axes scaling
      /// </summary>
      void RefreshAxisBinders();

      /// <summary>
      ///    Action to call when exporting to PDF
      /// </summary>
      Action ExportToPDF { get; set; }

      /// <summary>
      ///    Action to call when a point is hot tracked in the chart view
      /// </summary>
      Action<int> HotTracked { set; }

      /// <summary>
      ///    Exports the selected curves to excel
      /// </summary>
      void ExportToExcel();

      /// <summary>
      ///    Resets the zoom of the chart to 0 (no zoom)
      /// </summary>
      void ResetZoom();

      /// <summary>
      ///    Returns the curve with the same id as <paramref name="seriesId" />
      /// </summary>
      /// <param name="seriesId">The id of the curve being found</param>
      /// <returns>the curve if found, otherwise null</returns>
      Curve CurveFromSeriesId(string seriesId);

      /// <summary>
      ///    Activates the context menu first entry for the <paramref name="axis" />
      /// </summary>
      /// <param name="axis">The axis which is the subject of the context menu</param>
      void ActivateFirstContextMenuEntryFor(Axis axis);

      /// <summary>
      ///    Activates the context menu first entry for the <paramref name="curve" />
      /// </summary>
      /// <param name="curve">The curve which is the subject of the context menu</param>
      void ActivateFirstContextMenuEntryFor(Curve curve);

      /// <summary>
      ///    Returns the axis that matches the <paramref name="axisType" /> type
      /// </summary>
      /// <param name="axisType">The axis type to return</param>
      /// <returns>The matching axis for type otherwise null</returns>
      Axis AxisBy(AxisTypes axisType);

      /// <summary>
      ///    Shows or hides the curve in the legend
      /// </summary>
      /// <param name="curve">The curve which should be added or removed from the legend</param>
      /// <param name="show">if true, the legend entry should be shown, otherwise the legend entry is hidden</param>
      void ShowCurveInLegend(Curve curve, bool show);

      /// <summary>
      ///    Moves the curve from it's current place to a target place in the legend
      ///    Subsequent entries are bumped down in the order
      /// </summary>
      /// <param name="movingCurve">The curve being moved</param>
      /// <param name="targetCurve">The place the curve should occupy after move</param>
      void MoveSeriesInLegend(Curve movingCurve, Curve targetCurve);

      /// <summary>
      ///    If no curves have been added to the chart, then <paramref name="hint" /> text will appear in place of the empty
      ///    chart
      /// </summary>
      void SetNoCurvesSelectedHint(string hint);

      /// <summary>
      ///    Returns the index of the curve with id equals to <paramref name="id" /> or 0 if the curve does not
      ///    exist with the given <paramref name="id" />
      /// </summary>
      int LegendIndexFromSeriesId(string id);

      /// <summary>
      ///    Disables editing the curve and axis through the chart view
      /// </summary>
      void DisableCurveAndAxisEdits();

      void ResetVisibleRange();

      /// <summary>
      ///    Tests if the <paramref name="seriesId" /> represents the id of a series representing lower limit of quantification
      /// </summary>
      /// <returns>true if series represents lower limit of quantification, otherwise false</returns>
      bool IsSeriesLLOQ(string seriesId);

      /// <summary>
      ///    Gets the display unit as a string for the series
      /// </summary>
      /// <param name="seriesId">The id of the series whose unit is being retrieved</param>
      /// <returns>The unit as a string</returns>
      string DisplayUnitsFor(string seriesId);

      string CurveDescriptionFromSeriesId(string seriesId);
      bool IsPointBelowLLOQ(double[] values, string seriesId);
      int GetSourceIndexFromDataRow(string seriesId, DataRow row);

      void Edit(CurveChart chart);

      /// <summary>
      /// Edit the <paramref name="chart"/>using the <paramref name="displayChartFontAndSizeSettings"/>to display the chart in the view
      /// </summary>
      /// <param name="chart">Chart to edit</param>
      /// <param name="displayChartFontAndSizeSettings">Default font and size use to display the chart. This is not what will be used to export the chart</param>
      void Edit(CurveChart chart, ChartFontAndSizeSettings displayChartFontAndSizeSettings);

      void Clear();

      void OnDragDrop(IDragEvent dropEvent);

      void OnDragOver(IDragEvent dragEvent);
   }

   public class ChartDisplayPresenter : AbstractPresenter<IChartDisplayView, IChartDisplayPresenter>, IChartDisplayPresenter
   {
      private readonly ICurveBinderFactory _curveBinderFactory;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      private readonly IAxisBinderFactory _axisBinderFactory;
      private readonly ICurveToDataModeMapper _dataModeMapper;
      private readonly ICurveChartExportTask _chartExportTask;
      private readonly IApplicationSettings _applicationSettings;
      private readonly Cache<AxisTypes, IAxisBinder> _axisBinders;
      private readonly Cache<string, ICurveBinder> _curveBinders;
      private readonly Cache<string, ICurveBinder> _quickCurveBinderCache;
      private bool _isLLOQVisible;
      private ChartFontAndSizeSettings _displayChartFontAndSizeSettings;
      public Action ExportToPDF { get; set; }
      public event EventHandler<IDragEvent> DragOver = delegate { };
      public event EventHandler<IDragEvent> DragDrop = delegate { };

      public Action<int> HotTracked
      {
         set => View.HotTracked = value;
      }

      public ChartDisplayPresenter(IChartDisplayView chartDisplayView,
         ICurveBinderFactory curveBinderFactory,
         IViewItemContextMenuFactory contextMenuFactory,
         IAxisBinderFactory axisBinderFactory,
         ICurveToDataModeMapper dataModeMapper,
         ICurveChartExportTask chartExportTask,
         IApplicationSettings applicationSettings)
         : base(chartDisplayView)
      {
         _curveBinderFactory = curveBinderFactory;
         _contextMenuFactory = contextMenuFactory;
         _axisBinderFactory = axisBinderFactory;
         _dataModeMapper = dataModeMapper;
         _chartExportTask = chartExportTask;
         _applicationSettings = applicationSettings;
         _axisBinders = new Cache<AxisTypes, IAxisBinder>(a => a.AxisType, onMissingKey: key => null);
         _curveBinders = new Cache<string, ICurveBinder>(c => c.Id, onMissingKey: key => null);
         _quickCurveBinderCache = new Cache<string, ICurveBinder>(onMissingKey: key => null);
         _displayChartFontAndSizeSettings = new ChartFontAndSizeSettings();
         ExportToPDF = () => throw new OSPSuiteException(Error.NotImplemented);
      }

      public CurveChart Chart { get; private set; }

      public virtual void Edit(CurveChart chart)
      {
         Edit(chart, _displayChartFontAndSizeSettings);
      }

      public virtual void Edit(CurveChart chart, ChartFontAndSizeSettings displayChartFontAndSizeSettings)
      {
         Clear();
         _displayChartFontAndSizeSettings = displayChartFontAndSizeSettings;
         Chart = chart;
         updateChart();
      }

      public virtual void Clear()
      {
         _curveBinders.ToList().Each(removeCurveBinder);
         _axisBinders.ToList().Each(removeAxisBinder);
         Chart = null;
      }

      public void OnDragDrop(IDragEvent dropEvent) => DragDrop(this, dropEvent);

      public void OnDragOver(IDragEvent dragEvent) => DragOver(this, dragEvent);

      public void Refresh()
      {
         updateChart();
      }

      private void rebuildQuickCurveBinderCache()
      {
         _quickCurveBinderCache.Clear();
         _curveBinders.Each(addCurvesToQuickCacheAdapter);
      }
     
      public void ResetVisibleRange()
      {
         Chart.Axes.Each(axis => axis.ResetRange());
         RefreshAxisBinders();
      }

      private ICurveBinder curveBinderFromSeriesId(string seriesId)
      {
         return _quickCurveBinderCache[seriesId];
      }

      public string DisplayUnitsFor(string seriesId)
      {
         var curveBinder = curveBinderFromSeriesId(seriesId);
         if (curveBinder == null)
            return string.Empty;

         return Chart.YAxisFor(curveBinder.Curve).UnitName;
      }

      public void SetVisibleRange(float? xMin, float? xMax, float? yMin, float? yMax)
      {
         Chart.AllUsedSecondaryAxisTypes.Each(axisType =>
         {
            var rangeIsDefined = xMax.HasValue && xMin.HasValue && yMin.HasValue && yMax.HasValue;
            if (rangeIsDefined)
            {
               setSecondaryAxisRange(xMin.Value, xMax.Value, yMin.Value, yMax.Value, axisType);
            }
            else
            {
               Chart.AxisBy(axisType).ResetRange();
            }
         });

         Chart.AxisBy(AxisTypes.X).SetRange(xMin, xMax);
         Chart.AxisBy(AxisTypes.Y).SetRange(yMin, yMax);

         RefreshAxisBinders();
      }

      private void setSecondaryAxisRange(float xMin, float xMax, float yMin, float yMax, AxisTypes axisType)
      {
         var secondaryMin = _view.GetPointsForSecondaryAxis(xMin, yMin, axisType);
         var secondaryMax = _view.GetPointsForSecondaryAxis(xMax, yMax, axisType);
         Chart.AxisBy(axisType).SetRange(secondaryMin.Y, secondaryMax.Y);
      }

      public virtual void ExportToExcel()
      {
         _chartExportTask.ExportToExcel(Chart);
      }

      public void ResetZoom()
      {
         ResetVisibleRange();
      }

      public void CopyToClipboard()
      {
         _view.CopyToClipboard(_applicationSettings.WatermarkTextToUse);
      }

      public string CurveDescriptionFromSeriesId(string seriesId)
      {
         return CurveFromSeriesId(seriesId)?.Description;
      }

      public bool IsSeriesLLOQ(string seriesId)
      {
         var curveBinder = curveBinderFromSeriesId(seriesId);
         return curveBinder?.IsSeriesLLOQ(seriesId) ?? false;
      }

      public bool IsPointBelowLLOQ(double[] values, string seriesId)
      {
         if (!_isLLOQVisible)
            return false;

         var curveBinder = curveBinderFromSeriesId(seriesId);

         if (curveBinder?.LLOQ == null)
            return false;

         return values.All(value => curveBinder.LLOQ > value);
      }

      public int GetSourceIndexFromDataRow(string seriesId, DataRow row)
      {
         var curveBinder = curveBinderFromSeriesId(seriesId);
         return curveBinder?.OriginalCurveIndexForRow(row) ?? 0;
      }

      public Curve CurveFromSeriesId(string seriesId)
      {
         return curveBinderFromSeriesId(seriesId)?.Curve;
      }

      public int LegendIndexFromSeriesId(string id)
      {
         var curve = CurveFromSeriesId(id);
         return curve == null ? 0 : curve.LegendIndex.GetValueOrDefault(0);
      }

      public void DisableCurveAndAxisEdits()
      {
         View.DisableAxisEdit();
         View.DisableCurveEdit();
         View.DisableAxisHotTracking();
      }

      private void updateChart()
      {
         using (new BatchUpdate(View))
         {
            updateAxes();

            updateCurves();

            rebuildQuickCurveBinderCache();

            bindChartToView();

            //maybe can be done better
            View.ReOrderLegend();
         }
         updateViewLayout();

         RefreshAxisBinders();
      }

      private void updateViewLayout()
      {
         if (Chart.Curves.Any())
            _view.ShowChart();
         else
            _view.ShowHint();
      }

      private void updateAxes()
      {
         pruneAxes();

         Chart.Axes.Each(refreshAxisBinderFor);

         updateYAxesVisibility();
      }

      private void pruneAxes() => pruneBinders(_axisBinders, Chart.Axes, x => x.Axis, removeAxisBinder);

      private void refreshAxisBinderFor(Axis axis) => getOrCreateAxisBinderFor(axis).Refresh();

      private void removeAxisBinder(IAxisBinder axisBinder)
      {
         axisBinder.Dispose();
         _axisBinders.Remove(axisBinder.AxisType);
      }

      private void updateCurves()
      {
         pruneCurves();

         Chart.Curves.Each(refreshCurveBinderFor);

         _isLLOQVisible = _curveBinders.Any(x => x.LLOQ.HasValue && x.Curve.ShowLLOQ);
      }

      private void pruneCurves() => pruneBinders(_curveBinders, Chart.Curves, x => x.Curve, removeCurveBinder);

      private void pruneBinders<TBinder, TBoundObject>(IEnumerable<TBinder> binders, IReadOnlyCollection<TBoundObject> boundObjects, Func<TBinder, TBoundObject> retrieveBoundObjectFunc, Action<TBinder> removeBinderAction)
      {
         //Remove using reference to ensure that the bound object in the binder is the same as the one in the chart
         binders.Where(x => !boundObjects.Contains(retrieveBoundObjectFunc(x))).ToList().Each(removeBinderAction);
      }

      private void refreshCurveBinderFor(Curve curve) => getOrCreateCurveBinderFor(curve).Refresh();

      private IAxisBinder getOrCreateAxisBinderFor(Axis axis)
      {
         var axisBinder = _axisBinders[axis.AxisType];
         if (axisBinder == null)
         {
            axisBinder = _axisBinderFactory.Create(axis, View.ChartControl, Chart);
            _axisBinders.Add(axisBinder);
         }

         return axisBinder;
      }

      private ICurveBinder getOrCreateCurveBinderFor(Curve curve)
      {
         var curveBinder = curveBinderFor(curve);
         var yAxisBinder = _axisBinders[curve.yAxisType];

         if (curveBinder != null)
         {
            if (curveBinder.IsValidFor(_dataModeMapper.MapFrom(curve), curve.yAxisType))
               return curveBinder;

            removeCurveBinder(curveBinder);
         }
         curveBinder = _curveBinderFactory.CreateFor(curve, View.ChartControl, Chart, yAxisBinder);
         _curveBinders.Add(curveBinder);

         return curveBinder;
      }

      private void removeCurveBinder(ICurveBinder curveBinder)
      {
         curveBinder.Dispose();
         _curveBinders.Remove(curveBinder.Id);
         removeAdapterFromQuickCache(curveBinder);
      }

      private void addCurvesToQuickCacheAdapter(ICurveBinder curveBinder)
      {
         curveBinder.SeriesIds.Each(seriesId => _quickCurveBinderCache[seriesId] = curveBinder);
      }

      private void removeAdapterFromQuickCache(ICurveBinder curveBinder)
      {
         curveBinder.SeriesIds.Each(seriesId => _quickCurveBinderCache.Remove(seriesId));
      }

      public void RefreshAxisBinders()
      {
         var diagramSize = View.GetDiagramSize();
         _axisBinders.Each(x => x.RefreshRange(Chart.ChartSettings.SideMarginsEnabled, diagramSize));
      }

      private void updateYAxesVisibility()
      {
         Chart.AllUsedYAxisTypes.Each(updateYAxisVisibility);
      }

      private void updateYAxisVisibility(AxisTypes yAxisType)
      {
         var axisBinder = _axisBinders[yAxisType];
         if (axisBinder == null) return;

         var axisTypeUsed = Chart.AllVisibleCurvesOnYAxis(yAxisType).Any();
         axisBinder.Visible = axisTypeUsed;
      }

      public Axis AxisBy(AxisTypes axisType) => Chart.AxisBy(axisType);

      public void ShowCurveInLegend(Curve curve, bool show)
      {
         curveBinderFor(curve).ShowCurveInLegend(showInLegend: show);
         curve.VisibleInLegend = show;
      }

      public void MoveSeriesInLegend(Curve movingCurve, Curve targetCurve)
      {
         Chart.MoveCurvesInLegend(movingCurve, targetCurve);
         updateChart();
      }

      public void SetNoCurvesSelectedHint(string hint)
      {
         _view.SetNoCurvesSelectedHint(hint);
      }

      public void Handle(ChartUpdatedEvent chartUpdatedEvent)
      {
         if (!canHandle(chartUpdatedEvent))
            return;

         updateChart();
      }

      public void Handle(ChartPropertiesChangedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         bindChartToView();
      }

      private ICurveBinder curveBinderFor(Curve curve)
      {
         return _curveBinders[curve.Id];
      }

      private void bindChartToView()
      {
         View.UpdateSettings(Chart);

         var areChartWidthAndHeightDefined = Chart.FontAndSize.SizeIsDefined;
     
         if (Chart.PreviewSettings)
            setDisplay(areChartWidthAndHeightDefined ? Dock.None : Dock.Fill, Chart.FontAndSize, showingPreview: true);
         else
            setDisplay(Dock.Fill, _displayChartFontAndSizeSettings, showingPreview: false);
      }

      private void setDisplay(Dock dockStyle, ChartFontAndSizeSettings fontAndSizeSettings, bool showingPreview)
      {
         View.SetDockStyle(dockStyle);
         View.SetFontAndSizeSettings(fontAndSizeSettings);
         var showOriginText = showingPreview && Chart.IncludeOriginData;
         View.ShowOriginText = showOriginText;
         var watermark = showingPreview ? _applicationSettings.WatermarkTextToUse : null;
         View.ShowWatermark(watermark);
      }

      private bool canHandle(ChartEvent chartEvent)
      {
         return Equals(Chart, chartEvent.Chart);
      }

      public void ActivateFirstContextMenuEntryFor(Axis axis)
      {
         _contextMenuFactory.CreateFor(viewItemFor(axis), this).ActivateFirstMenu();
      }

      public void ActivateFirstContextMenuEntryFor(Curve curve)
      {
         _contextMenuFactory.CreateFor(viewItemFor(curve), this).ActivateFirstMenu();
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         var contextMenu = _contextMenuFactory.CreateFor(viewItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      private IViewItem viewItemFor(Curve curve) => new CurveViewItem(Chart, curve);
      private IViewItem viewItemFor(Axis axis) => new AxisViewItem(Chart, axis);
   }
}