using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Charts;
using ItemChangedEventArgs = OSPSuite.Core.Chart.ItemChangedEventArgs;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Presenter of ChartDisplay component, which edits a ICurveChart. Consists of subcomponents
   ///    - DataBrowser for selection of McDataColumns from McDataRepositories for xyData of a curve,
   ///    - CurveOptions for editing properties of curves,
   ///    - AxisOptions for editing properties of axes,
   ///    - ChartOptions for editing properties of chartSettings.
   /// </summary>
   public interface IChartEditorPresenter : IPresenter<IChartEditorView>
   {
      /// <summary>
      ///    Displayed Chart.
      /// </summary>
      ICurveChart DataSource { set; get; }

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
      ///    Clears all DataColumns of DataRepository from DataBrowser and depending curves from Chart.
      /// </summary>
      void ClearDataRepositories();

      /// <summary>
      ///    Adds DataRepositories to DataBrowser.
      /// </summary>
      void AddDataRepositories(IEnumerable<DataRepository> dataRepositories);

      /// <summary>
      ///    Get all DataColumns of DataBrowser.
      /// </summary>
      IEnumerable<DataColumn> GetAllDataColumns();

      /// <summary>
      ///    Get a DataColumn of DataBrowser by id. Returns null, if not available.
      /// </summary>
      DataColumn GetDataColumn(string id);

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
      GridColumnSettings GetDataBrowserColumnSettings(BrowserColumns browserColumn);

      /// <summary>
      ///    Gets Display settings of CurveOptions column.
      /// </summary>
      GridColumnSettings GetCurveOptionsColumnSettings(CurveOptionsColumns curveOptionsColumn);

      /// <summary>
      ///    Gets Display settings of AxisOptions column.
      /// </summary>
      GridColumnSettings GetAxisOptionsColumnSettings(AxisOptionsColumns axisOptionsColumn);

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
      ///    Adds a new curve to a the chart for the <paramref name="columnId" /> if one does not already exist.
      ///    Curve options will be applied if they are specified and a new curve is created.
      /// </summary>
      /// <returns>
      ///    The new curve or existing if chart already contains a curve for the specified <paramref name="columnId" />
      /// </returns>
      ICurve AddCurveForColumn(string columnId, CurveOptions defaultCurveOptions = null);

      /// <summary>
      ///    Adds Columns selected in DataBrowser to Chart and CurveOptions.
      /// </summary>
      void SelectDataColumns();

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
      event Action<GridColumnSettings> ColumnSettingsChanged;

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
      private ICurveChart _chart;
      private Func<DataColumn, bool> _showDataColumnInDataBrowserDefinition;

      private readonly IAxisSettingsPresenter _axisSettingsPresenter;
      private readonly IChartSettingsPresenter _chartSettingsPresenter;
      private readonly IChartExportSettingsPresenter _chartExportSettingsPresenter;
      private readonly ICurveSettingsPresenter _curveSettingsPresenter;
      private readonly IDataBrowserPresenter _dataBrowserPresenter;
      private readonly IChartTemplateMenuPresenter _chartTemplateMenuPresenter;
      private readonly string _xDataName;
      private readonly string _yDataName;

      public event Action<GridColumnSettings> ColumnSettingsChanged = delegate { };

      public ChartEditorPresenter(IChartEditorView view, IAxisSettingsPresenter axisSettingsPresenter,
         IChartSettingsPresenter chartSettingsPresenter, IChartExportSettingsPresenter chartExportSettingsPresenter,
         ICurveSettingsPresenter curveSettingsPresenter, IDataBrowserPresenter dataBrowserPresenter, IChartTemplateMenuPresenter chartTemplateMenuPresenter)
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
         _dataBrowserPresenter.UsedChanged += onDataBrowserUsedChanged;
         _dataBrowserPresenter.ColumnSettingsChanged += ColumnSettingsChanged;
         _dataBrowserPresenter.SelectedDataChanged += onSelectedDataChanged;

         AddSubPresenters(axisSettingsPresenter, chartSettingsPresenter, chartExportSettingsPresenter, curveSettingsPresenter, dataBrowserPresenter);

         _curveSettingsPresenter.ColumnSettingsChanged += ColumnSettingsChanged;
         _axisSettingsPresenter.ColumnSettingsChanged += ColumnSettingsChanged;
         _xDataName = Helpers.Property<ICurve>(c => c.xData).Name;
         _yDataName = Helpers.Property<ICurve>(c => c.yData).Name;
         _view.SetAxisSettingsView(axisSettingsPresenter.View);
         _view.SetChartSettingsView(chartSettingsPresenter.View);
         _view.SetChartExportSettingsView(chartExportSettingsPresenter.View);
         _view.SetCurveSettingsView(curveSettingsPresenter.View);
         _view.SetDataBrowserView(dataBrowserPresenter.View);
         _curveSettingsPresenter.DataColumns = _dataColumns;
      }

      private void onDataBrowserUsedChanged(object sender, UsedChangedEventArgs e)
      {
         e.ColumnIds.Each(id =>
         {
            if (e.Used)
               _curveSettingsPresenter.AddCurveForColumn(id);
            else
               _chart.RemoveCurvesForColumn(id);
         });
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

         _dataBrowserPresenter.SetUsedState(_dataBrowserPresenter.SelectedDescendentDataRepositoryColumnIds, usedState: isUsed.Value);
      }

      public void AddChartTemplateMenu(IWithChartTemplates withChartTemplates, Action<CurveChartTemplate> loadMenuFor)
      {
         AddButton(_chartTemplateMenuPresenter.CreateChartTemplateButton(withChartTemplates, () => DataSource, loadMenuFor));
      }

      public ICurveChart DataSource
      {
         get { return _chart; }
         set
         {
            if (_chart != null)
            {
               _chart.Curves.CollectionChanged -= onCurvesCollectionChanged;
               _chart.Curves.ItemPropertyChanged -= onCurvesItemChanged;
            }
            _chart = value;
            if (_chart != null)
            {
               _curveSettingsPresenter.SetDatasource(_chart);
               _chartSettingsPresenter.BindTo(_chart);
               _chartExportSettingsPresenter.BindTo(_chart);
               _axisSettingsPresenter.SetDataSource(_chart.Axes);
               _chart.Curves.CollectionChanged += onCurvesCollectionChanged;
               onCurvesCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, _chart.Curves));

               _chart.Curves.ItemPropertyChanged += onCurvesItemChanged;
            }
            else
            {
               _curveSettingsPresenter.SetDatasource(null);
               _axisSettingsPresenter.SetDataSource(null);
               _chartSettingsPresenter.DeleteBinding();
               _chartExportSettingsPresenter.DeleteBinding();
            }
         }
      }

      private void onCurvesItemChanged(object sender, ItemChangedEventArgs e)
      {
         var curve = e.Item as ICurve;
         if (curve == null) return;

         if (e.PropertyName.IsOneOf(_xDataName, _yDataName))
            updateUsedColumns();
      }

      private void onCurvesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         updateUsedColumns();
      }

      private void onSelectedDataChanged(IEnumerable<string> dataRepositoryColumnIds)
      {
         var dataRepositoryColumnIdList = dataRepositoryColumnIds.ToList();
         if (allColumnsAreUsed(dataRepositoryColumnIdList))
         {
            _view.SetSelectAllCheckBox(true);
         }
         else if (noColumnsAreUsed(dataRepositoryColumnIdList))
         {
            _view.SetSelectAllCheckBox(false);
         }
         else
         {
            _view.SetSelectAllCheckBox(null);
         }
      }

      private bool noColumnsAreUsed(IEnumerable<string> dataRepositoryColumnIds)
      {
         return _dataColumns.Where(x => dataRepositoryColumnIds.Contains(x.Id)).All(x => !_dataBrowserPresenter.IsUsed(x.Id));
      }

      private bool allColumnsAreUsed(IEnumerable<string> dataRepositoryColumnIds)
      {
         return _dataColumns.Where(x => dataRepositoryColumnIds.Contains(x.Id)).All(x => _dataBrowserPresenter.IsUsed(x.Id));
      }

      private void updateUsedColumns()
      {
         if (_chart == null) return;
         _dataBrowserPresenter.InitializeIsUsedForColumns(_chart.UsedColumnIds);
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
            else
               _dataBrowserPresenter.UpdateDataColumn(dataColumn);
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

      public void ClearDataRepositories()
      {
         _dataBrowserPresenter.ClearDataColumns();
         _dataColumns.Clear();
      }

      public void AddDataRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         dataRepositories.Each(AddDataRepository);
      }

      public IEnumerable<DataColumn> GetAllDataColumns()
      {
         return _dataColumns;
      }

      public DataColumn GetDataColumn(string id)
      {
         return _dataColumns.Contains(id) ? _dataColumns[id] : null;
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
            if (presenterColumnSettings != null)
               presenterColumnSettings.CopyFrom(layoutColumnSettings);
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

      public GridColumnSettings GetDataBrowserColumnSettings(BrowserColumns browserColumn)
      {
         return _dataBrowserPresenter.ColumnSettings(browserColumn.ToString());
      }

      public GridColumnSettings GetCurveOptionsColumnSettings(CurveOptionsColumns curveOptionsColumn)
      {
         return _curveSettingsPresenter.ColumnSettings(curveOptionsColumn.ToString());
      }

      public GridColumnSettings GetAxisOptionsColumnSettings(AxisOptionsColumns axisOptionsColumn)
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
         _curveSettingsPresenter.CurveNameDefinition = curveNameDefinition;
      }

      public ICurve AddCurveForColumn(string columnId, CurveOptions defaultCurveOptions = null)
      {
         return _curveSettingsPresenter.AddCurveForColumn(columnId, defaultCurveOptions);
      }

      public void SelectDataColumns()
      {
         var usedColumnsIds = _chart.UsedColumnIds;
         foreach (var column in selectedDataColumns().Where(column => !usedColumnsIds.Contains(column.Id)))
         {
            _curveSettingsPresenter.AddCurveForColumn(column.Id);
         }
      }

      private IEnumerable<DataColumn> selectedDataColumns()
      {
         return _dataBrowserPresenter.SelectedDataColumnIds.Select(id => _dataColumns[id]);
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
         add { _dataBrowserPresenter.DragOver += value; }
         remove { _dataBrowserPresenter.DragOver -= value; }
      }

      public event DragEventHandler DragDrop
      {
         add { _dataBrowserPresenter.DragDrop += value; }
         remove { _dataBrowserPresenter.DragDrop -= value; }
      }
   }
}