using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Presenter of ChartDisplay component, which edits a ICurveChart. Consists of subcomponents
   ///    - DataBrowser for selection of McDataColumns from McDataRepositories for xyData of a curve,
   ///    - CurveOptions for editing properties of curves,
   ///    - AxisOptions for editing properties of axes,
   ///    - ChartOptions for editing properties of chartSettings.
   /// </summary>
   public interface IChartEditorPresenter : IPresenter<IChartEditorView>,
      IListener<ChartUpdatedEvent>
   {
      CurveChart Chart { get; }

      void Edit(CurveChart chart);

      void Clear();

      /// <summary>
      ///    Adds DataColumns of DataRepository to DataBrowser.
      /// </summary>
      void AddDataRepository(DataRepository dataRepository);

      /// <summary>
      ///    Removes DataColumns of DataRepository from DataBrowser and depending curves from Chart.
      /// </summary>
      void RemoveDataRepository(DataRepository dataRepository);

      /// <summary>
      ///    Refreshs DataColumns of DataRepository in DataBrowser and and depending curves in Chart.
      /// </summary>
      void RefreshDataRepository(DataRepository dataRepository);

      /// <summary>
      ///    Adds DataRepositories to DataBrowser.
      /// </summary>
      void AddDataRepositories(IEnumerable<DataRepository> dataRepositories);

      /// <summary>
      ///    Get all DataColumns of DataBrowser.
      /// </summary>
      IEnumerable<DataColumn> AllDataColumns();

      /// <summary>
      ///    Creates a ChartEditorSettings object from settings of this presenter. Used for serialization.
      /// </summary>
      ChartEditorSettings CreateSettings();

      /// <summary>
      ///    Copy settings of this presenter from a ChartEditorSettings object. Used for deserialization.
      /// </summary>
      void CopySettingsFrom(ChartEditorSettings settings);

      /// <summary>
      ///    Initialize layout from the current settings. If <paramref name="loadEditorLayout" /> is set to true, editor layout
      ///    will be loaded.
      ///    if <paramref name="loadColumnSettings" /> is set to true, column settings will be loaded
      /// </summary>
      void CopySettingsFrom(ChartEditorSettings settings, bool loadEditorLayout, bool loadColumnSettings);

      /// <summary>
      ///    Gets Display settings of DataBrowser column.
      /// </summary>
      GridColumnSettings DataBrowserColumnSettingsFor(BrowserColumns browserColumn);

      /// <summary>
      ///    Gets Display settings of CurveOptions column.
      /// </summary>
      GridColumnSettings CurveOptionsColumnSettingsFor(CurveOptionsColumns curveOptionsColumn);

      /// <summary>
      ///    Gets Display settings of AxisOptions column.
      /// </summary>
      GridColumnSettings AxisOptionsColumnSettingsFor(AxisOptionsColumns axisOptionsColumn);

      /// <summary>
      ///    Sets Definition of displayed QuantityPath in DataBrowser.
      /// </summary>
      void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition);

      /// <summary>
      ///    Sets Definition, whether DataColumn is shown in DataBrowser.
      /// </summary>
      void SetShowDataColumnInDataBrowserDefinition(Func<DataColumn, bool> showDataColumnInDataBrowserDefinition);

      /// <summary>
      ///    Sets Definition of Curve name in CurveOptions.
      /// </summary>
      void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition);

      /// <summary>
      ///    Adds a new curve to a the chart for the <paramref name="dataColumn" /> if one does not already exist.
      ///    Curve options will be applied if they are specified and a new curve is created.
      /// </summary>
      /// <returns>
      ///    The new curve or existing if chart already contains a curve for the specified <paramref name="dataColumn" />
      /// </returns>
      Curve AddCurveForColumn(DataColumn dataColumn, CurveOptions defaultCurveOptions = null);

      /// <summary>
      ///    Add Button to ChartEditor.
      /// </summary>
      void AddButton(IMenuBarItem menuBarItem);

      /// <summary>
      ///    This method should be called to remove all the buttons previously defined
      /// </summary>
      void ClearButtons();

      /// <summary>
      ///    Event when something is dragged to Control. EventHandler should set Effect to Move if Drag is allowed.
      /// </summary>
      event DragEventHandler DragOver;

      /// <summary>
      ///    Event when something is dropped onto Control. EventHandler should set Effect.
      /// </summary>
      event DragEventHandler DragDrop;

      /// <summary>
      ///    Event when ColumnSettings for data browser, curve options or axis options has changed.
      /// </summary>
      event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged;

      /// <summary>
      ///    Apply all column setting to the editor
      /// </summary>
      void ApplyColumnSettings();

      /// <summary>
      ///    Show the customaization form (for internal user only)
      /// </summary>
      void ShowCustomizationForm();

      /// <summary>
      ///    Adds the control for setting multiple Used In properties at once
      /// </summary>
      void AddUsedInMenuItem();

      /// <summary>
      ///    Updates the UsedIn menu item checkbox checkstate.
      /// </summary>
      /// <param name="isUsed">true for checked, false for unchecked and null for indeterminate</param>
      void UpdateUsedForSelection(bool? isUsed);

      /// <summary>
      ///    Adds chart template menu
      /// </summary>
      void AddChartTemplateMenu(IWithChartTemplates withChartTemplates, Action<CurveChartTemplate> loadMenuFor);
   }

   /// <summary>
   ///    This class contains the subpresenters, an IChart datasource
   ///    and a list of McDataColumns, which are displayed in the DataBrowser.
   ///    It forwards the DataColumn selection from the DataBrowser to the CurveOptions presenter,
   ///    and forwards the Curve selection from the CurveOptions to DataBrowser presenter.
   ///    It provides methods for
   ///    - maintenance of McDataRepositories
   ///    - access to the GridColumnSettings of the subcomponents
   ///    - some settings of ChartEditor and subcomponents
   /// </summary>
   public class ChartEditorPresenter : AbstractCommandCollectorPresenter<IChartEditorView, IChartEditorPresenter>, IChartEditorPresenter
   {
      private readonly ICache<string, DataColumn> _dataColumns;
      private Func<DataColumn, bool> _showDataColumnInDataBrowserDefinition;

      private readonly IAxisSettingsPresenter _axisSettingsPresenter;
      private readonly IChartSettingsPresenter _chartSettingsPresenter;
      private readonly IChartExportSettingsPresenter _chartExportSettingsPresenter;
      private readonly ICurveSettingsPresenter _curveSettingsPresenter;
      private readonly IDataBrowserPresenter _dataBrowserPresenter;
      private readonly IChartTemplateMenuPresenter _chartTemplateMenuPresenter;
      private readonly IChartUpdater _chartUpdater;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDimensionFactory _dimensionFactory;
      private Func<DataColumn, string> _curveNameDefinition;
      public CurveChart Chart { get; private set; }

      public event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged = delegate { };

      public ChartEditorPresenter(IChartEditorView view, IAxisSettingsPresenter axisSettingsPresenter,
         IChartSettingsPresenter chartSettingsPresenter, IChartExportSettingsPresenter chartExportSettingsPresenter,
         ICurveSettingsPresenter curveSettingsPresenter, IDataBrowserPresenter dataBrowserPresenter,
         IChartTemplateMenuPresenter chartTemplateMenuPresenter, IChartUpdater chartUpdater, IEventPublisher eventPublisher,
         IDimensionFactory dimensionFactory)
         : base(view)
      {
         _dataColumns = new Cache<string, DataColumn>(x => x.Id);
         _showDataColumnInDataBrowserDefinition = col => col.DataInfo.Origin != ColumnOrigins.BaseGrid;

         _axisSettingsPresenter = axisSettingsPresenter;
         _chartSettingsPresenter = chartSettingsPresenter;
         _chartExportSettingsPresenter = chartExportSettingsPresenter;
         _curveSettingsPresenter = curveSettingsPresenter;
         _dataBrowserPresenter = dataBrowserPresenter;
         _chartTemplateMenuPresenter = chartTemplateMenuPresenter;
         _chartUpdater = chartUpdater;
         _eventPublisher = eventPublisher;
         _dimensionFactory = dimensionFactory;

         _dataBrowserPresenter.UsedChanged += onDataBrowserUsedChanged;
         _dataBrowserPresenter.SelectedDataChanged += onSelectedDataChanged;

         _dataBrowserPresenter.ColumnSettingsChanged += ColumnSettingsChanged;
         _curveSettingsPresenter.ColumnSettingsChanged += ColumnSettingsChanged;
         _axisSettingsPresenter.ColumnSettingsChanged += ColumnSettingsChanged;

         _chartExportSettingsPresenter.StatusChanged += (o, e) => onChartPropertiesChanged();
         _chartSettingsPresenter.StatusChanged += (o, e) => onChartPropertiesChanged();

         _curveSettingsPresenter.AddCurves += columns => addCurvesForColumns(columns);
         _curveSettingsPresenter.RemoveCurve += removeCurve;
         _curveSettingsPresenter.CurvePropertyChanged += curve => updateChart();

         _axisSettingsPresenter.AxisRemoved += onAxisRemoved;
         _axisSettingsPresenter.AxisAdded += onAxisAdded;
         _axisSettingsPresenter.AxisPropertyChanged += axis => updateChart();

         AddSubPresenters(axisSettingsPresenter, chartSettingsPresenter, chartExportSettingsPresenter, curveSettingsPresenter, dataBrowserPresenter);

         _view.SetAxisSettingsView(axisSettingsPresenter.View);
         _view.SetChartSettingsView(chartSettingsPresenter.View);
         _view.SetChartExportSettingsView(chartExportSettingsPresenter.View);
         _view.SetCurveSettingsView(curveSettingsPresenter.View);
         _view.SetDataBrowserView(dataBrowserPresenter.View);
      }

      private void onAxisAdded()
      {
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            Chart.AddNewAxis();
         }
      }

      private void onAxisRemoved(Axis axis)
      {
         if (axis.AxisType >= AxisTypes.Y2 && Chart.HasAxis(axis.AxisType + 1))
            throw new OSPSuiteException(Error.RemoveHigherAxisTypeFirst((axis.AxisType + 1).ToString()));

         using (_chartUpdater.UpdateTransaction(Chart))
         {
            Chart.RemoveAxis(axis);
         }
      }

      private void updateChart()
      {
         _chartUpdater.Update(Chart);
      }

      private void onChartPropertiesChanged()
      {
         _eventPublisher.PublishEvent(new ChartPropertiesChangedEvent(Chart));
      }

      private void onDataBrowserUsedChanged(object sender, UsedChangedEventArgs e)
      {
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            e.DataColumns.Each(column =>
            {
               if (e.Used)
                  addCurve(column);
               else
                  Chart.RemoveCurvesForColumn(column);
            });
         }
      }

      public void ApplyColumnSettings()
      {
         _dataBrowserPresenter.ApplyAllColumnSettings();
         _curveSettingsPresenter.ApplyAllColumnSettings();
         _axisSettingsPresenter.ApplyAllColumnSettings();
      }

      public void ShowCustomizationForm()
      {
         _view.ShowCustomizationForm();
      }

      public void AddUsedInMenuItem()
      {
         _view.AddUsedInMenuItemCheckBox();
      }

      public void UpdateUsedForSelection(bool? isUsed)
      {
         if (!isUsed.HasValue) return;

         _dataBrowserPresenter.UpdateUsedForSelection(used: isUsed.Value);
      }

      public void AddChartTemplateMenu(IWithChartTemplates withChartTemplates, Action<CurveChartTemplate> loadMenuFor)
      {
         AddButton(_chartTemplateMenuPresenter.CreateChartTemplateButton(withChartTemplates, () => Chart, loadMenuFor));
      }

      public void Edit(CurveChart chart)
      {
         Chart = chart;
         _curveSettingsPresenter.Edit(Chart);
         _chartSettingsPresenter.Edit(Chart);
         _chartExportSettingsPresenter.Edit(Chart);
         _axisSettingsPresenter.Edit(Chart.Axes);
      }

      public void Clear()
      {
         SetCurveNameDefinition(null);
         SetDisplayQuantityPathDefinition(null);
         _dataBrowserPresenter.Clear();
         _dataColumns.Clear();
         _curveSettingsPresenter.Clear();
         _axisSettingsPresenter.Clear();
         _chartSettingsPresenter.Clear();
         _chartExportSettingsPresenter.Clear();
      }

      private void onSelectedDataChanged(IReadOnlyList<DataColumn> dataColumns)
      {
         if (allColumnsAreUsed(dataColumns))
         {
            _view.SetSelectAllCheckBox(true);
         }
         else if (noColumnsAreUsed(dataColumns))
         {
            _view.SetSelectAllCheckBox(false);
         }
         else
         {
            _view.SetSelectAllCheckBox(null);
         }
      }

      private bool noColumnsAreUsed(IEnumerable<DataColumn> dataColumns)
      {
         return dataColumns.All(x => !_dataBrowserPresenter.IsUsed(x));
      }

      private bool allColumnsAreUsed(IEnumerable<DataColumn> dataColumns)
      {
         return dataColumns.All(x => _dataBrowserPresenter.IsUsed(x));
      }

      private void updateUsedColumns()
      {
         if (Chart == null) return;
         _dataBrowserPresenter.InitializeIsUsedForColumns(Chart.UsedColumns);
      }

      public void AddDataRepository(DataRepository dataRepository)
      {
         foreach (var dataColumn in dataRepository.Where(c => !columnIsForInternalUseOnly(c)))
         {
            if (!_dataColumns.Contains(dataColumn.Id))
            {
               _dataColumns.Add(dataColumn);
               _dataBrowserPresenter.AddDataColumn(dataColumn);
            }
         }

         updateUsedColumns();
      }

      public void RefreshDataRepository(DataRepository dataRepository)
      {
         removeColumns(unusedColumns);
         AddDataRepository(dataRepository);
      }

      private IReadOnlyList<DataColumn> unusedColumns
      {
         get { return _dataColumns.Where(x => !x.IsInRepository() || columnIsForInternalUseOnly(x)).ToList(); }
      }

      private bool columnIsForInternalUseOnly(DataColumn dataColumn)
      {
         return dataColumn.IsInternal ||
                dataColumn.DataInfo.Origin == ColumnOrigins.ObservationAuxiliary ||
                !_showDataColumnInDataBrowserDefinition(dataColumn);
      }

      public void RemoveDataRepository(DataRepository dataRepository)
      {
         removeColumns(dataRepository);
      }

      private void removeColumns(IEnumerable<DataColumn> dataColumns)
      {
         dataColumns.Each(removeColumn);
      }

      private void removeColumn(DataColumn dataColumn)
      {
         _dataBrowserPresenter.RemoveDataColumn(dataColumn);
         _dataColumns.Remove(dataColumn.Id);
      }

      public void AddDataRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         dataRepositories.Each(AddDataRepository);
      }

      public IEnumerable<DataColumn> AllDataColumns()
      {
         return _dataColumns;
      }

      public void CopySettingsFrom(ChartEditorSettings settings)
      {
         CopySettingsFrom(settings, loadEditorLayout: true, loadColumnSettings: true);
      }

      private void copyColumSettings(IEnumerable<GridColumnSettings> settings, Func<string, GridColumnSettings> columnSettingsInPresenterByName)
      {
         foreach (var layoutColumnSettings in settings)
         {
            var presenterColumnSettings = columnSettingsInPresenterByName(layoutColumnSettings.ColumnName);
            presenterColumnSettings?.CopyFrom(layoutColumnSettings);
         }
      }

      public void CopySettingsFrom(ChartEditorSettings settings, bool loadEditorLayout, bool loadColumnSettings)
      {
         if (loadColumnSettings)
         {
            copyColumSettings(settings.DataBrowserColumnSettings, _dataBrowserPresenter.ColumnSettings);
            copyColumSettings(settings.CurveOptionsColumnSettings, _curveSettingsPresenter.ColumnSettings);
            copyColumSettings(settings.AxisOptionsColumnSettings, _axisSettingsPresenter.ColumnSettings);
         }
         if (loadEditorLayout)
            _view.LoadLayoutFromString(settings.DockingLayout);
      }

      public ChartEditorSettings CreateSettings()
      {
         var settings = new ChartEditorSettings();
         addSettingsFrom(_dataBrowserPresenter, settings, x => x.AddDataBrowserColumnSetting);
         addSettingsFrom(_curveSettingsPresenter, settings, x => x.AddCurveOptionsColumnSetting);
         addSettingsFrom(_axisSettingsPresenter, settings, x => x.AddAxisOptionsColumnSetting);
         settings.DockingLayout = _view.SaveLayoutToString();

         return settings;
      }

      private void addSettingsFrom(IPresenterWithColumnSettings presenterWithColumnSettings, ChartEditorSettings settings, Func<ChartEditorSettings, Action<GridColumnSettings>> addAction)
      {
         presenterWithColumnSettings.AllColumnSettings().Each(x => addAction(settings)(new GridColumnSettings(x)));
      }

      public GridColumnSettings DataBrowserColumnSettingsFor(BrowserColumns browserColumn)
      {
         return _dataBrowserPresenter.ColumnSettings(browserColumn.ToString());
      }

      public GridColumnSettings CurveOptionsColumnSettingsFor(CurveOptionsColumns curveOptionsColumn)
      {
         return _curveSettingsPresenter.ColumnSettings(curveOptionsColumn.ToString());
      }

      public GridColumnSettings AxisOptionsColumnSettingsFor(AxisOptionsColumns axisOptionsColumn)
      {
         return _axisSettingsPresenter.ColumnSettings(axisOptionsColumn.ToString());
      }

      public void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition)
      {
         _dataBrowserPresenter.SetDisplayQuantityPathDefinition(displayQuantityPathDefinition);
      }

      public void SetShowDataColumnInDataBrowserDefinition(Func<DataColumn, bool> showDataColumnInDataBrowserDefinition)
      {
         _showDataColumnInDataBrowserDefinition = showDataColumnInDataBrowserDefinition;
      }

      public void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition)
      {
         _curveNameDefinition = curveNameDefinition;
         _curveSettingsPresenter.CurveNameDefinition = curveNameDefinition;
      }

      public Curve AddCurveForColumn(DataColumn dataColumn, CurveOptions defaultCurveOptions = null)
      {
         return addCurvesForColumns(new[] {dataColumn}, defaultCurveOptions).FirstOrDefault();
      }

      private IReadOnlyList<Curve> addCurvesForColumns(IReadOnlyList<DataColumn> columns, CurveOptions defaultCurveOptions = null)
      {
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            return columns.Select(x => addCurve(x, defaultCurveOptions)).ToList();
         }
      }

      private void removeCurve(Curve curve)
      {
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            Chart.RemoveCurve(curve);
         }
      }

      private Curve addCurve(DataColumn dataColumn, CurveOptions defaultCurveOptions = null)
      {
         var curve = Chart.CreateCurve(dataColumn.BaseGrid, dataColumn, _curveNameDefinition(dataColumn), _dimensionFactory);

         if (Chart.HasCurve(curve.Id))
            return Chart.CurveBy(curve.Id);

         Chart.UpdateCurveColorAndStyle(curve, dataColumn, _dataColumns);

         if (defaultCurveOptions != null)
            curve.CurveOptions.UpdateFrom(defaultCurveOptions);

         Chart.AddCurve(curve);
         return curve;
      }

      public void AddButton(IMenuBarItem menuBarItem)
      {
         _view.AddButton(menuBarItem);
      }

      public void ClearButtons()
      {
         _view.ClearButtons();
      }

      public event DragEventHandler DragOver
      {
         add => _dataBrowserPresenter.DragOver += value;
         remove => _dataBrowserPresenter.DragOver -= value;
      }

      public event DragEventHandler DragDrop
      {
         add => _dataBrowserPresenter.DragDrop += value;
         remove => _dataBrowserPresenter.DragDrop -= value;
      }

      public void Handle(ChartUpdatedEvent chartUpdatedEvent)
      {
         if (!canHandle(chartUpdatedEvent))
            return;

         refresh();
      }

      private void refresh()
      {
         Chart.SynchronizeDataDisplayUnit();
         _curveSettingsPresenter.Refresh();
         _axisSettingsPresenter.Refresh();
         updateUsedColumns();
      }

      private bool canHandle(ChartEvent chartEvent)
      {
         return Equals(chartEvent.Chart, Chart);
      }
   }
}