using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;
using ItemChangedEventArgs = OSPSuite.Core.Chart.ItemChangedEventArgs;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Presenter of ChartDisplay component, which displays a IChart.
   /// </summary>
   public interface IChartDisplayPresenter : IPresenter<IChartDisplayView>, IPresenterWithContextMenu<ICurve>, IPresenterWithContextMenu<IAxis>, IPresenterWithContextMenu<ICurveChart>, IBatchUpdatable
   {
      /// <summary>
      ///    Displayed IChart.
      /// </summary>
      ICurveChart DataSource { get; set; }

      /// <summary>
      ///    Associated ChartDisplayControl as Control.
      /// </summary>
      Control Control { get; }

      /// <summary>
      ///    Reloads xyData of Chart and refreshs display of Chart.
      /// </summary>
      void Refresh();

      /// <summary>
      ///    Event when something is dragged to Control. EventHandler should set Effect to Move if Drag is allowed.
      /// </summary>
      event DragEventHandler DragOver;

      /// <summary>
      ///    Event when something is dropped onto Control. EventHandler should set Effect.
      /// </summary>
      event DragEventHandler DragDrop;

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
      void RefreshAxisAdapters();

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
      ///    Sets Definition of Curve name in CurveOptions.
      /// </summary>
      void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition);

      /// <summary>
      ///    Resets the zoom of the chart to 0 (no zoom)
      /// </summary>
      void ResetZoom();

      /// <summary>
      ///    Copies the chart to the clipboard as an image
      /// </summary>
      void CopyToClipboard();

      /// <summary>
      ///    Returns the curve with the same id as <paramref name="seriesId" />
      /// </summary>
      /// <param name="seriesId">The id of the curve being found</param>
      /// <returns>the curve if found, otherwise null</returns>
      ICurve CurveFromSeriesId(string seriesId);

      /// <summary>
      ///    Activates the context menu first entry for the <paramref name="axis" />
      /// </summary>
      /// <param name="axis">The axis which is the subject of the context menu</param>
      void ActivateFirstContextMenuEntryFor(IAxis axis);

      /// <summary>
      ///    Activates the context menu first entry for the <paramref name="curve" />
      /// </summary>
      /// <param name="curve">The curve which is the subject of the context menu</param>
      void ActivateFirstContextMenuEntryFor(ICurve curve);

      /// <summary>
      ///    Returns the axis that matches the <paramref name="axisType" /> type
      /// </summary>
      /// <param name="axisType">The axis type to return</param>
      /// <returns>The matching axis for type otherwise null</returns>
      IAxis GetAxisFrom(AxisTypes axisType);

      /// <summary>
      ///    Shows or hides the curve in the legend
      /// </summary>
      /// <param name="curve">The curve which should be added or removed from the legend</param>
      /// <param name="show">if true, the legend entry should be shown, otherwise the legend entry is hidden</param>
      void ShowCurveInLegend(ICurve curve, bool show);

      /// <summary>
      ///    Moves the curve from it's current place to a target place in the legend
      ///    Subsequent entries are bumped down in the order
      /// </summary>
      /// <param name="movingCurve">The curve being moved</param>
      /// <param name="targetCurve">The place the curve should occupy after move</param>
      void MoveSeriesInLegend(ICurve movingCurve, ICurve targetCurve);

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

      bool ShouldIncludeOriginData();

      /// <summary>
      ///    Returns true if the chart being displayed has a curve containing a series with <paramref name="seriesId" />
      ///    otherwise false
      /// </summary>
      bool HasCurveWithId(string seriesId);

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
      string GetDisplayUnitsFor(string seriesId);

      string CurveDescriptionFromSeriesId(string seriesId);
      bool IsPointBelowLLOQ(double[] values, string curveId);
      int GetSourceIndexFromDataRow(string seriesId, DataRow row);
   }

   public class ChartDisplayPresenter : AbstractPresenter<IChartDisplayView, IChartDisplayPresenter>, IChartDisplayPresenter
   {
      private readonly ICache<AxisTypes, IAxisAdapter> _axisAdapters;
      private readonly IChartDisplayView _chartDisplayView;
      private readonly IDataRepositoryTask _dataRepositoryTask;
      private readonly IDialogCreator _dialogCreator;
      private readonly ICurveAdapterFactory _curveAdapterFactory;
      private readonly ICurveChartContextMenuFactory _curveChartContextMenuFactory;
      private readonly ICurveContextMenuFactory _curveContextMenuFactory;
      private readonly IAxisContextMenuFactory _axisContextMenuFactory;
      private readonly IExceptionManager _exceptionManager;
      private readonly ICache<string, ICurveAdapter> _curveAdapters;
      private readonly ICache<string, ICurveAdapter> _quickCurveAdapterCache;
      // The curveAdapters are stored explicitely in this presenter class to enable releasing the adapters
      private ICurveChart _chart;
      private Func<DataColumn, string> _curveNameDefinition = x => x.Name;
      public bool Updating { get; private set; }

      private readonly string _legendIndexPropertyName;

      public Action ExportToPDF { get; set; }

      public Action<int> HotTracked
      {
         set { View.HotTracked = value; }
      }

      public ChartDisplayPresenter(
         IChartDisplayView chartDisplayView,
         IDataRepositoryTask dataRepositoryTask,
         IDialogCreator dialogCreator,
         ICurveAdapterFactory curveAdapterFactory,
         ICurveChartContextMenuFactory curveChartContextMenuFactory,
         ICurveContextMenuFactory curveContextMenuFactory,
         IAxisContextMenuFactory axisContextMenuFactory,
         IExceptionManager exceptionManager)
         : base(chartDisplayView)
      {
         _chartDisplayView = chartDisplayView;
         _dataRepositoryTask = dataRepositoryTask;
         _dialogCreator = dialogCreator;
         _curveAdapterFactory = curveAdapterFactory;
         _curveChartContextMenuFactory = curveChartContextMenuFactory;
         _curveContextMenuFactory = curveContextMenuFactory;
         _axisContextMenuFactory = axisContextMenuFactory;
         _exceptionManager = exceptionManager;
         _chartDisplayView.AttachPresenter(this);
         _axisAdapters = new Cache<AxisTypes, IAxisAdapter>(a => a.AxisType);
         _curveAdapters = new Cache<string, ICurveAdapter>(c => c.Id);
         _quickCurveAdapterCache = new Cache<string, ICurveAdapter>(onMissingKey: key => null);
         ExportToPDF = () => { throw new OSPSuiteException(Error.NotImplemented); };
         _legendIndexPropertyName = Helpers.Property<ICurve>(x => x.LegendIndex).Name;
      }

      public ICurveChart DataSource
      {
         get { return _chart; }
         set
         {
            if (_chart != null)
            {
               removeAllCurves();
               removeAllAxes();
               BeginUpdate();
               _chart.StartUpdateEvent -= onChartUpdating;
               _chart.EndUpdateEvent -= onChartDoneUpdating;
            }

            _chart = value;

            if (_chart == null) return;
            
            EndUpdate();

            _chartDisplayView.UpdateSettings(_chart);

            //On curve changed activation kills performance
            _chart.Curves.ItemChanged -= onCurveChanged;
            _chart.PropertyChanged -= onChartPropertyChanged;
            addCurves(_chart.Curves);
            _chart.Curves.ItemChanged += onCurveChanged;
            _chart.PropertyChanged += onChartPropertyChanged;

            _chart.StartUpdateEvent += onChartUpdating;
            _chart.EndUpdateEvent += onChartDoneUpdating;
         }
      }

      public Control Control
      {
         get { return _chartDisplayView as Control; }
      }

      public void BeginUpdate()
      {
         if (_chart == null) return;

         Updating = true;
         _chart.PropertyChanged -= onChartPropertyChanged;
         _chart.Axes.CollectionChanged -= onChartAxesChanged;
         _chart.Curves.CollectionChanged -= onChartCurvesChanged;
         _chart.Curves.ItemChanged -= onCurveChanged;
         _chart.Curves.ItemPropertyChanged -= onCurvePropertyChanged;
         _chart.Axes.ItemChanged -= onAxisChanged;
      }

      public void EndUpdate()
      {
         if (_chart == null) return;
         Updating = false;

         _chart.PropertyChanged += onChartPropertyChanged;
         _chart.Axes.CollectionChanged += onChartAxesChanged;
         _chart.Curves.CollectionChanged += onChartCurvesChanged;
         _chart.Curves.ItemChanged += onCurveChanged;
         _chart.Curves.ItemPropertyChanged += onCurvePropertyChanged;
         _chart.Axes.ItemChanged += onAxisChanged;
      }

      private void onChartDoneUpdating(object sender, EventArgs e)
      {
         EndUpdate();
      }

      private void onChartUpdating(object sender, EventArgs e)
      {
         BeginUpdate();
      }

      private void onCurvePropertyChanged(object sender, ItemChangedEventArgs e)
      {
         if (string.Equals(e.PropertyName, _legendIndexPropertyName))
         {
            _view.ReOrderLegend();
         }
      }

      public void Refresh()
      {
         try
         {
            _chartDisplayView.BeginSeriesUpdate();
            _curveAdapters.Each(x => x.Refresh());
            RefreshAxisAdapters();
            RefreshCurvesVisibiltyInLegendAdapters();
            rebuildQuickCurveAdapterCache();
         }
         finally
         {
            _chartDisplayView.EndSeriesUpdate();
         }

         _chartDisplayView.RefreshData();
         _chartDisplayView.Refresh();
      }

      private void rebuildQuickCurveAdapterCache()
      {
         _quickCurveAdapterCache.Clear();
         _curveAdapters.Each(addCurvesToQuickCacheAdapter);
      }

      public event DragEventHandler DragOver
      {
         add { _chartDisplayView.DragOver += value; }
         remove { _chartDisplayView.DragOver -= value; }
      }

      public event DragEventHandler DragDrop
      {
         add { _chartDisplayView.DragDrop += value; }
         remove { _chartDisplayView.DragDrop -= value; }
      }

      public void ResetVisibleRange()
      {
         startAxisUpdate();
         _chart.Axes.Each(axis => { axis.SetRange(null, null); });
         RefreshAxisAdapters();
         endAxisUpdate();
      }

      public bool IsSeriesLLOQ(string seriesId)
      {
         var relatedAdapter = getRelatedCurveAdapter(seriesId);
         if (relatedAdapter == null)
            return false;

         return relatedAdapter.IsSeriesLLOQ(seriesId);
      }

      private ICurveAdapter getRelatedCurveAdapter(string seriesId)
      {
         return curveAdapterFromSeriesId(seriesId);
      }

      private ICurveAdapter curveAdapterFromSeriesId(string seriesId)
      {
         return _quickCurveAdapterCache[seriesId];
      }

      public string GetDisplayUnitsFor(string seriesId)
      {
         var relatedAdapter = getRelatedCurveAdapter(seriesId);
         if (relatedAdapter == null)
            return string.Empty;

         return _axisAdapters[relatedAdapter.Curve.yAxisType].Axis.UnitName;
      }

      private void endAxisUpdate()
      {
         _chart.Axes.ItemChanged += onAxisChanged;
      }

      private void startAxisUpdate()
      {
         _chart.Axes.ItemChanged -= onAxisChanged; //avoid repeated refresh during this action called from Control
      }

      private void onAxisUpdate(Action actionToRun)
      {
         startAxisUpdate();
         actionToRun();
         endAxisUpdate();
      }

      public void SetVisibleRange(float? xMin, float? xMax, float? yMin, float? yMax)
      {
         onAxisUpdate(() =>
         {
            allAxesExceptPrimary().Each(axisType =>
            {
               if (xMax.HasValue && xMin.HasValue && yMin.HasValue && yMax.HasValue)
               {
                  setSecondaryAxisRange(xMin.Value, xMax.Value, yMin.Value, yMax.Value, axisType);
               }
               else
               {
                  _chart.Axes[axisType].SetRange(null, null);
               }
            });

            _chart.Axes[AxisTypes.X].SetRange(xMin, xMax);
            _chart.Axes[AxisTypes.Y].SetRange(yMin, yMax);

            RefreshAxisAdapters();
         });
      }

      private void setSecondaryAxisRange(float xMin, float xMax, float yMin, float yMax, AxisTypes axisType)
      {
         var secondaryMin = _view.GetPointsForSecondaryAxis(xMin, yMin, axisType);
         var secondaryMax = _view.GetPointsForSecondaryAxis(xMax, yMax, axisType);
         _chart.Axes[axisType].SetRange(secondaryMin.Y, secondaryMax.Y);
      }

      private IEnumerable<AxisTypes> allAxesExceptPrimary()
      {
         return _chart.Axes.Keys.Except(new[] {AxisTypes.X, AxisTypes.Y});
      }

      public virtual void ExportToExcel()
      {
         if (_chart == null) return;
         var visibleCurves = _chart.Curves.Where(x => x.Visible).ToList();
         if (!visibleCurves.Any()) return;

         var fileName = _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(fileName)) return;

         _dataRepositoryTask.ExportToExcel(visibleCurves.Select(x => x.yData), fileName, _curveNameDefinition);
      }

      public void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition)
      {
         _curveNameDefinition = curveNameDefinition;
      }

      public void ResetZoom()
      {
         _view.ResetChartZoom();
      }

      public void CopyToClipboard()
      {
         _view.CopyToClipboardWithExportSettings();
      }

      public string CurveDescriptionFromSeriesId(string seriesId)
      {
         return CurveFromSeriesId(seriesId)?.Description;
      }

      public bool IsPointBelowLLOQ(double[] values, string curveId)
      {
         var adapter = curveAdapterFromSeriesId(curveId);
         return values.All(value => adapter.LLOQ != null && adapter.LLOQ.Value > value);
      }

      public int GetSourceIndexFromDataRow(string seriesId, DataRow row)
      {
         var curveAdapter = curveAdapterFromSeriesId(seriesId);
         return curveAdapter.OriginalCurveIndexForRow(row);
      }

      public ICurve CurveFromSeriesId(string seriesId)
      {
         var adapterFromSeriesId = curveAdapterFromSeriesId(seriesId);
         return adapterFromSeriesId?.Curve;
      }

      public int LegendIndexFromSeriesId(string id)
      {
         var curve = CurveFromSeriesId(id);
         return curve == null ? 0 : curve.LegendIndex.GetValueOrDefault(0);
      }

      public bool ShouldIncludeOriginData()
      {
         return DataSource != null && DataSource.IncludeOriginData;
      }

      public bool HasCurveWithId(string seriesId)
      {
         return CurveFromSeriesId(seriesId) != null;
      }

      public void DisableCurveAndAxisEdits()
      {
         View.DisableAxisEdit();
         View.DisableCurveEdit();
         View.DisableAxisHotTracking();
      }

      private void addCurves(IEnumerable<ICurve> curves)
      {
         // temporarily make curves (first would be enough) visible, because only after adding a visible series, 
         // the diagram is available, but availablility of diagram is assumed after adding the first curve.
         var curvesList = curves as IList<ICurve> ?? curves.OrderBy(x => x.LegendIndex).ToList();
         var invisibleCurves = curvesList.Where(curve => !curve.Visible).ToList();

         _chartDisplayView.BeginInit();
         foreach (var curve in curvesList)
         {
            if (curve == null) break;
            if (curve.xData == null) throw new MissingDataException("x", curve.Name);
            if (curve.yData == null) throw new MissingDataException("y", curve.Name);

            curve.Visible = true; // temporarily

            var curveAdapter = _curveAdapterFactory.CreateFor(curve, _chart.Axes[AxisTypes.X], _chart.Axes[curve.yAxisType]);


            _curveAdapters.Add(curveAdapter);
            addCurvesToQuickCacheAdapter(curveAdapter);
            _chartDisplayView.AddCurve(curveAdapter);
         }
         _chartDisplayView.ReOrderLegend();
         _chartDisplayView.EndInit(); //necessary to generate ChartControl.Axes and .Diagram, but for performance issues this should be called only if necessary (reason for change from addCurve to addCurves)

         // axisAdapters cannot be generated directly, because the ChartControl.Axes and .Diagram are generated after adding the first visible Series

         //update DiagramBackColor, because XY diagram only exists if at least a curve was selected=>hence 1
         _chartDisplayView.DiagramBackColor = _chart.ChartSettings.DiagramBackColor;

         // update axisAdapters
         foreach (var axis in _chart.Axes)
         {
            if (!_axisAdapters.Contains(axis.AxisType))
            {
               var axisAdapter = _chartDisplayView.GetAxisAdapter(axis);
               if (axisAdapter != null)
               {
                  _axisAdapters.Add(axisAdapter);
               }
            }
         }
         // DevExpress provides Axes first after at least one curve is added to diagram
         _curveAdapters.Each(x => x.AttachAxisAdapters(_axisAdapters));
         invisibleCurves.Each(c => c.Visible = false);

         updateYAxisVisibility(AxisTypes.Y);
         updateYAxisVisibility(AxisTypes.Y2);
         RefreshAxisAdapters();
         RefreshCurvesVisibiltyInLegendAdapters();
      }

      private void addCurvesToQuickCacheAdapter(ICurveAdapter curveAdapter)
      {
         curveAdapter.SeriesIds.Each(seriesId => _quickCurveAdapterCache[seriesId] = curveAdapter);
      }

      private void removeCurve(string curveId)
      {
         _chartDisplayView.RemoveCurve(curveId);

         if (_chartDisplayView.NoCurves())
            removeAllAxes();

         var curveAdapter = _curveAdapters[curveId];
         curveAdapter.Dispose();
         removeAdapterFromQuickCache(curveAdapter);
         _curveAdapters[curveId] = null;
         _curveAdapters.Remove(curveId);

         updateYAxisVisibility(AxisTypes.Y);
         updateYAxisVisibility(AxisTypes.Y2);
         RefreshAxisAdapters();
      }

      private void removeAdapterFromQuickCache(ICurveAdapter curveAdapter)
      {
         curveAdapter.SeriesIds.Each(seriesId => _quickCurveAdapterCache.Remove(seriesId));
      }

      public void RefreshAxisAdapters()
      {
         _axisAdapters.Each(x => x.RefreshRange(_chart.ChartSettings.SideMarginsEnabled, _chartDisplayView.GetDiagramSize()));
      }

      public void RefreshCurvesVisibiltyInLegendAdapters()
      {
         _curveAdapters.Each(x => x.ShowCurveInLegend(x.Curve.VisibleInLegend));
      }

      private void updateYAxisVisibility(AxisTypes yAxisType)
      {
         if (!_axisAdapters.Contains(yAxisType)) return;
         var axisTypeUsed = _chart.Curves.Any(curve => curve.Visible && curve.yAxisType == yAxisType);

         if (_axisAdapters[yAxisType].Visible == axisTypeUsed) return;

         _axisAdapters[yAxisType].Visible = axisTypeUsed;

         refreshLegendTexts();
         RefreshAxisAdapters();
      }

      private void removeAxis(AxisTypes axisType)
      {
         _chartDisplayView.RemoveAxis(axisType);
         if (_axisAdapters.Contains(axisType))
         {
            _axisAdapters[axisType].Dispose();
            _axisAdapters[axisType] = null;
            _axisAdapters.Remove(axisType);
         }
      }

      private void removeAllAxes()
      {
         foreach (var axisType in _axisAdapters.Keys.ToList())
         {
            removeAxis(axisType);
         }
      }

      private void removeAllCurves()
      {
         foreach (var curveId in _curveAdapters.Keys.ToList())
         {
            removeCurve(curveId);
         }
      }

      private void onChartCurvesChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         try
         {
            var curves = sender as INotifyCache<string, ICurve>;
            if (curves == null) return;
            switch (e.Action)
            {
               case NotifyCollectionChangedAction.Add:
                  foreach (var item in e.NewItems)
                  {
                     var curve = item as ICurve;
                     if (curve == null) continue;
                     addCurves(new List<ICurve> {curve});
                  }
                  break;
               case NotifyCollectionChangedAction.Remove:
                  foreach (var item in e.OldItems)
                  {
                     var curve = item as ICurve;
                     if (curve == null) continue;
                     removeCurve(curve.Id);
                  }
                  break;
               case NotifyCollectionChangedAction.Replace:
                  break;
               case NotifyCollectionChangedAction.Move:
                  break;
               case NotifyCollectionChangedAction.Reset:
                  removeAllCurves();
                  break;
               default:
                  throw new ArgumentOutOfRangeException();
            }
         }
         catch (Exception)
         {
            
         }
      }

      private void refreshLegendTexts()
      {
         _chartDisplayView.BeginSeriesUpdate();
         _curveAdapters.Each(c => c.RefreshLegendText());
         _chartDisplayView.EndSeriesUpdate();
      }

      private void onChartAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         var axes = sender as INotifyCache<AxisTypes, IAxis>;
         if (axes == null) return;
         try
         {
            switch (e.Action)
            {
               case NotifyCollectionChangedAction.Add:
                  foreach (var item in e.NewItems)
                  {
                     var axis = item as IAxis;
                     if (axis == null) continue;
                     if (axis.AxisType >= AxisTypes.Y2)
                     {
                        IAxisAdapter axisAdapter = _chartDisplayView.GetAxisAdapter(axis);
                        if (axisAdapter != null)
                        {
                           _axisAdapters.Add(axisAdapter);
                        }
                     }
                  }

                  refreshLegendTexts();
                  break;
               case NotifyCollectionChangedAction.Remove:
                  foreach (var item in e.OldItems)
                  {
                     var axis = item as IAxis;
                     if (axis == null) continue;
                     if (axis.AxisType >= AxisTypes.Y2) removeAxis(axis.AxisType);
                  }

                  refreshLegendTexts();
                  break;
               case NotifyCollectionChangedAction.Replace:
                  break;
               case NotifyCollectionChangedAction.Move:
                  break;
               case NotifyCollectionChangedAction.Reset:
                  removeAllAxes();
                  break;
               default:
                  throw new ArgumentOutOfRangeException();
            }
         }
         catch (Exception)
         {

         }
      }

      private void onCurveChanged(object obj, ItemChangedEventArgs args)
      {
         var curve = args.Item as ICurve;

         updateYAxisVisibility(AxisTypes.Y);
         updateYAxisVisibility(AxisTypes.Y2);
         updateYAxisVisibility(AxisTypes.Y3);

         if (curve != null && _curveAdapters.Contains(curve.Id))
            _curveAdapters[curve.Id].ShowCurveInLegend(curve.VisibleInLegend);

         _chartDisplayView.RefreshData();
      }

      private void onAxisChanged(object obj, ItemChangedEventArgs args)
      {
         _exceptionManager.Execute(() =>
         {
            _chartDisplayView.RefreshData();
            _chartDisplayView.Refresh();
            RefreshAxisAdapters();
         });
      }

      private void onChartPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         _exceptionManager.Execute(() =>
         {
            _chartDisplayView.UpdateSettings(_chart);
            RefreshAxisAdapters();

            if (_chart.PreviewSettings)
               setDisplay(areChartWidthAndHeightDefined() ? DockStyle.None : DockStyle.Fill, _chart.FontAndSize, true);
            else
               setDisplay(DockStyle.Fill, ChartFontAndSizeSettings.Default, previewOriginText: false);
         });
      }

      private void setDisplay(DockStyle dockStyle, ChartFontAndSizeSettings fontAndSizeSettings, bool previewOriginText)
      {
         _chartDisplayView.SetDockStyle(dockStyle);
         _chartDisplayView.SetFontAndSizeSettings(fontAndSizeSettings);
         if (previewOriginText)
            _chartDisplayView.PreviewOriginText();
         else
            _chartDisplayView.ClearOriginText();
      }

      private bool areChartWidthAndHeightDefined()
      {
         return (_chart.FontAndSize.ChartWidth.GetValueOrDefault() > 0 && _chart.FontAndSize.ChartHeight.GetValueOrDefault() > 0);
      }

      public void ActivateFirstContextMenuEntryFor(IAxis axis)
      {
         _axisContextMenuFactory.CreateFor(axis, this).ActivateFirstMenu();
      }

      public void ActivateFirstContextMenuEntryFor(ICurve curve)
      {
         _curveContextMenuFactory.CreateFor(curve, this).ActivateFirstMenu();
      }

      public IAxis GetAxisFrom(AxisTypes axisType)
      {
         return !_chart.Axes.Contains(axisType) ? null : _chart.Axes[axisType];
      }

      public void ShowCurveInLegend(ICurve curve, bool show)
      {
         _curveAdapters[curve.Id].ShowCurveInLegend(visibleInLegend: show);
         curve.VisibleInLegend = show;
      }

      public void MoveSeriesInLegend(ICurve movingCurve, ICurve targetCurve)
      {
         _chart.MoveSeriesInLegend(movingCurve, targetCurve);
      }

      public void SetNoCurvesSelectedHint(string hint)
      {
         _view.SetNoCurvesSelectedHint(hint);
      }

      public void ShowContextMenu(ICurve curve, Point popupLocation)
      {
         _curveContextMenuFactory.CreateFor(curve, this).Show(_view, popupLocation);
      }

      public void ShowContextMenu(IAxis axis, Point popupLocation)
      {
         _axisContextMenuFactory.CreateFor(axis, this).Show(_view, popupLocation);
      }

      public void ShowContextMenu(ICurveChart curveChart, Point popupLocation)
      {
         var contextMenu = _curveChartContextMenuFactory.CreateFor(curveChart, this);
         contextMenu.Show(_view, popupLocation);
      }
   }
}