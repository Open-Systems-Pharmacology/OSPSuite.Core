using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Presenter of ChartDisplay component, which edits a CurveChart. Consists of subcomponents
   ///    - DataBrowser for selection of DataColumns from DataRepositories for xyData of a curve,
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
      ///    Removes all columns of all <paramref name="dataRepositories" /> from DataBrowser. This does not trigger a redraw of
      ///    the chart
      /// </summary>
      void RemoveDataRepositories(IEnumerable<DataRepository> dataRepositories);

      /// <summary>
      ///    Remove all data repositories used from DataBrowser. This does not trigger a redraw of the chart
      /// </summary>
      void RemoveAllDataRepositories();

      /// <summary>
      ///    Remove unused columns (e.g. not attached to a <see cref="DataRepository">.</see>This does not trigger a redraw of
      ///    the chart
      /// </summary>
      void RemoveUnusedColumns();

      /// <summary>
      ///    Adds <paramref name="dataRepositories" /> to DataBrowser. This does not trigger a redraw of the chart
      /// </summary>
      void AddDataRepositories(IEnumerable<DataRepository> dataRepositories);

      /// <summary>
      ///    sets the group row format in the gridView of the DataBrowserView to the specified string.
      /// </summary>
      void SetGroupRowFormat(GridGroupRowFormats format);

      /// <summary>
      ///    Returns all DataColumns used in the DataBrowser
      /// </summary>
      IReadOnlyList<DataColumn> AllDataColumns { get; }

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
      GridColumnSettings ColumnSettingsFor(BrowserColumns browserColumn);

      /// <summary>
      ///    Gets Display settings of CurveOptions column.
      /// </summary>
      GridColumnSettings ColumnSettingsFor(CurveOptionsColumns curveOptionsColumn);

      /// <summary>
      ///    Gets Display settings of AxisOptions column.
      /// </summary>
      GridColumnSettings ColumnSettingsFor(AxisOptionsColumns axisOptionsColumn);

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
      Curve AddCurveForColumn(DataColumn dataColumn, CurveOptions defaultCurveOptions = null, bool isLinkedDataToSimulation = false);

      /// <summary>
      ///    Adds a curves to the chart for all dataColumns in <paramref name="dataColumnList" /> if they do not already exist.
      ///    Curve options will be applied if they are specified and a new curve is created.All the created curves will have
      ///    the same color.
      /// </summary>
      void AddCurvesWithSameColorForColumn(IReadOnlyList<DataColumn> dataColumnList, CurveOptions defaultCurveOptions = null);

      /// <summary>
      ///    Add Button to ChartEditor.
      /// </summary>
      void AddButton(IMenuBarItem menuBarItem);

      /// <summary>
      ///    This method should be called to remove all the buttons previously defined
      /// </summary>
      void ClearButtons();

      /// <summary>
      ///    Event when ColumnSettings for data browser, curve options or axis options has changed.
      /// </summary>
      event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged;

      /// <summary>
      ///    Apply all column setting to the editor
      /// </summary>
      void ApplyAllColumnSettings();

      /// <summary>
      ///    Apply all column setting to the editor
      /// </summary>
      void ApplyColumnSettings(GridColumnSettings columnSettings);

      /// <summary>
      ///    Show the customization form (for internal user only)
      /// </summary>
      void ShowCustomizationForm();

      /// <summary>
      ///    Adds the control for setting multiple Used In properties at once
      /// </summary>
      void AddUsedInMenuItem();

      /// <summary>
      ///    Updates the UsedIn menu item checkbox check state.
      /// </summary>
      /// <param name="isUsed">true for checked, false for unchecked and null for indeterminate</param>
      void UpdateUsedForSelection(bool? isUsed);

      /// <summary>
      ///    Updates the LinkedOutputData menu item checkbox check state.
      /// </summary>
      /// <param name="isLinkedMappedOutputs">true for checked, false for unchecked</param>
      void UpdateLinkSimulationToDataSelection(bool isLinkedMappedOutputs);

      /// <summary>
      ///    Adds chart template menu
      /// </summary>
      void AddChartTemplateMenu(IWithChartTemplates withChartTemplates, Action<CurveChartTemplate> loadMenuFor);

      /// <summary>
      ///    Event is thrown every time a property of the chart is changed (either direct property or indirect such as axes or
      ///    curves modification)
      /// </summary>
      event Action ChartChanged;

      /// <summary>
      ///    Refresh the presenter with all values and settings from the underlying <see cref="CurveChart" />
      /// </summary>
      void Refresh();

      /// <summary>
      ///    Adds the output mappings to to the underlying data browser presenter (used for linking)
      /// </summary>
      void AddOutputMappings(OutputMappings outputMappings);

      /// <summary>
      ///    Removes the specified  output mappings from the underlying data browser presenter (used for linking)
      /// </summary>
      void RemoveOutputMappings(OutputMappings outputMappings);

      /// <summary>
      ///    Adds the control for linking the (de)selection of outputs and observed data
      /// </summary>
      void AddLinkSimDataMenuItem();

      /// <summary>
      ///    Sets the visibility of the control for linking the (de)selection of outputs and observed data
      /// </summary>
      void SetLinkSimDataMenuItemVisibility(bool isVisible);

      /// <summary>
      ///    Removes all the saved OutputMappings from the DataBrowser list
      /// </summary>
      void RemoveAllOutputMappings();

      void UpdateAutoUpdateChartMode(bool autoMode);

      void UpdateChartDisplay();
   }

   public class ChartEditorPresenter : AbstractCommandCollectorPresenter<IChartEditorView, IChartEditorPresenter>, IChartEditorPresenter
   {
      private Func<DataColumn, bool> _showDataColumnInDataBrowserDefinition;

      private readonly IAxisSettingsPresenter _axisSettingsPresenter;
      private readonly IChartSettingsPresenter _chartSettingsPresenter;
      private readonly IChartExportSettingsPresenter _chartExportSettingsPresenter;
      private readonly ICurveSettingsPresenter _curveSettingsPresenter;
      private readonly ICurveColorGroupingPresenter _curveColorGroupingPresenter;
      private readonly IDataBrowserPresenter _dataBrowserPresenter;
      private readonly IChartTemplateMenuPresenter _chartTemplateMenuPresenter;
      private readonly ICurveChartUpdater _chartUpdater;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDimensionFactory _dimensionFactory;
      private Func<DataColumn, string> _curveNameDefinition;
      private readonly List<IPresenterWithColumnSettings> _presentersWithColumnSettings;
      public CurveChart Chart { get; private set; }
      public event Action ChartChanged = delegate { };
      public event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged = delegate { };
      public event EventHandler<IDragEvent> DragOver = delegate { };
      public event EventHandler<IDragEvent> DragDrop = delegate { };

      public ChartEditorPresenter(
         IChartEditorView view,
         IAxisSettingsPresenter axisSettingsPresenter,
         IChartSettingsPresenter chartSettingsPresenter,
         IChartExportSettingsPresenter chartExportSettingsPresenter,
         ICurveSettingsPresenter curveSettingsPresenter,
         IDataBrowserPresenter dataBrowserPresenter,
         IChartTemplateMenuPresenter chartTemplateMenuPresenter,
         ICurveChartUpdater chartUpdater,
         IEventPublisher eventPublisher,
         IDimensionFactory dimensionFactory,
         ICurveColorGroupingPresenter curveColorGroupingPresenter)
         : base(view)
      {
         _showDataColumnInDataBrowserDefinition = col => col.DataInfo.Origin != ColumnOrigins.BaseGrid;

         _axisSettingsPresenter = axisSettingsPresenter;
         _chartSettingsPresenter = chartSettingsPresenter;
         _chartExportSettingsPresenter = chartExportSettingsPresenter;
         _curveSettingsPresenter = curveSettingsPresenter;
         _curveColorGroupingPresenter = curveColorGroupingPresenter;
         _dataBrowserPresenter = dataBrowserPresenter;
         _chartTemplateMenuPresenter = chartTemplateMenuPresenter;
         _chartUpdater = chartUpdater;
         _eventPublisher = eventPublisher;
         _dimensionFactory = dimensionFactory;
         _presentersWithColumnSettings = new List<IPresenterWithColumnSettings>
            { _dataBrowserPresenter, _curveSettingsPresenter, _axisSettingsPresenter };
         initPresentersWithColumnSettings();

         _dataBrowserPresenter.UsedChanged += (o, e) => onDataBrowserUsedChanged(e);
         _dataBrowserPresenter.SelectionChanged += (o, e) => onSelectionChanged(e.Columns);

         _chartExportSettingsPresenter.ChartExportSettingsChanged += (o, e) => onChartPropertiesChanged();
         _chartSettingsPresenter.ChartSettingsChanged += (o, e) => updateChart();

         _curveSettingsPresenter.AddCurves += (o, e) => addCurvesForColumns(e.Columns);
         _curveSettingsPresenter.RemoveCurve += (o, e) => removeCurve(e.Curve);
         _curveSettingsPresenter.CurvePropertyChanged += (o, e) => updateCurveProperty(e.Curve);

         _axisSettingsPresenter.AxisRemoved += (o, e) => onAxisRemoved(e.Axis);
         _axisSettingsPresenter.AxisAdded += (o, e) => onAxisAdded();
         _axisSettingsPresenter.AxisPropertyChanged += (o, e) => updateChart();

         _curveColorGroupingPresenter.ApplySelectedColorGrouping += (o, e) => onApplyColorGrouping(e.SelectedMetaData);

         AddSubPresenters(axisSettingsPresenter, chartSettingsPresenter, chartExportSettingsPresenter, curveSettingsPresenter, dataBrowserPresenter,
            curveColorGroupingPresenter);

         _view.SetAxisSettingsView(axisSettingsPresenter.View);
         _view.SetChartSettingsView(chartSettingsPresenter.View);
         _view.SetChartExportSettingsView(chartExportSettingsPresenter.View);
         _view.SetCurveSettingsView(curveSettingsPresenter.View);
         _view.SetCurveColorGroupingView(curveColorGroupingPresenter.View);
         _view.SetDataBrowserView(dataBrowserPresenter.View);
      }

      //gets all the common metaData of the observed data that correspond to active curves 
      private IReadOnlyList<string> getCommonMetaDataOfCurves()
      {
         var activeObservedDataList = _dataBrowserPresenter.GetAllUsedDataColumns().Where(x => x.IsObservation()).Select(x => x.Repository).ToList();

         if (!activeObservedDataList.Any())
            return new List<string>();

         var firstCurveMetaData = activeObservedDataList.First().ExtendedProperties.Keys.ToList();
         var commonMetaData = firstCurveMetaData
            .Where(x => activeObservedDataList.All(observedData => observedData.ExtendedProperties.Keys.Contains(x))).ToList();

         return commonMetaData;
      }

      private void onApplyColorGrouping(IReadOnlyList<string> eSelectedMetaData)
      {
         var groupedDataRepositories = groupDataRepositories(eSelectedMetaData);
         assignSameColorToGroupedCurves(groupedDataRepositories);
      }

      private void assignSameColorToGroupedCurves(List<IReadOnlyList<DataRepository>> groupedDataRepositories)
      {
         foreach (var group in groupedDataRepositories)
         {
            var curvesInGroup = findCurvesCorrespondingToRepositoryList(group);

            if (!curvesInGroup.Any())
               continue;

            var groupColor = Chart.SelectNewColor();
            curvesInGroup.Each(x => _curveSettingsPresenter.UpdateColorForCurve(x, groupColor));
         }
      }

      private List<IReadOnlyList<DataRepository>> groupDataRepositories(IReadOnlyList<string> groupingCriteria)
      {
         var activeObservedDataList = _dataBrowserPresenter.GetAllUsedDataColumns().Where(x => x.IsObservation()).Select(x => x.Repository);

         // we will group according to each criterion sequentially. the actual order of the criteria will not make a difference in the result.
         // we start with all the initial data repositories in one group. We will group them according to the first criterion resulting in x groups
         // then we will group each of the newly created groups with the next criterion and so on. 
         var groupedDataRepositories = new List<IReadOnlyList<DataRepository>> { activeObservedDataList.ToList() };
         foreach (var groupingMetaData in groupingCriteria)
         {
            var tempGroupedList = new List<IReadOnlyList<DataRepository>>();
            foreach (var existingGroup in groupedDataRepositories)
            {
               var dataReposGroupedBySingleMetaData =
                  existingGroup.GroupBy(x => x.ExtendedProperties[groupingMetaData].ValueAsObject).Select(group => @group.ToList());
               // we are using tempGroupedList to "flatten" the structure of the groups. Every time we apply .GroupBy() we will get a new layer of groups
               // but since we do not care about what the parent node criteria are, but only for the resulting groups and their contained elements we can 
               // avoid the extra complexity
               tempGroupedList.AddRange(dataReposGroupedBySingleMetaData);
            }

            groupedDataRepositories = tempGroupedList;
         }

         return groupedDataRepositories;
      }

      private IReadOnlyList<Curve> findCurvesCorrespondingToRepositoryList(IReadOnlyList<DataRepository> listOfDataRepositories)
      {
         var allColumnsInGroup = listOfDataRepositories.SelectMany(x => x.Columns).ToList();
         return allColumnsInGroup.Select(column => Chart.FindCurveWithSameData(column.BaseGrid, column)).Where(existingCurve => existingCurve != null)
            .ToList();
      }

      private void initPresentersWithColumnSettings()
      {
         _presentersWithColumnSettings.Each(x => x.ColumnSettingsChanged += ColumnSettingsChanged);
      }

      private void onAxisAdded()
      {
         using (_chartUpdater.UpdateTransaction(Chart, CurveChartUpdateModes.All))
         {
            Chart.AddNewAxis();
         }
      }

      private void onAxisRemoved(Axis axis)
      {
         if (axis.AxisType >= AxisTypes.Y2 && Chart.HasAxis(axis.AxisType + 1))
            throw new OSPSuiteException(Error.RemoveHigherAxisTypeFirst((axis.AxisType + 1).ToString()));

         using (_chartUpdater.UpdateTransaction(Chart, CurveChartUpdateModes.All))
         {
            Chart.RemoveAxis(axis);
         }
      }

      private void updateCurveProperty(Curve changedCurve) => _chartUpdater.Update(Chart, new[] { changedCurve }, CurveChartUpdateModes.Property);

      private void updateChart() => _chartUpdater.Update(Chart, CurveChartUpdateModes.All);

      private void onChartPropertiesChanged()
      {
         _eventPublisher.PublishEvent(new ChartPropertiesChangedEvent(Chart));
         ChartChanged();
      }

      private void onDataBrowserUsedChanged(UsedColumnsEventArgs e)
      {
         using (_chartUpdater.UpdateTransaction(Chart, e.Used ? CurveChartUpdateModes.Add : CurveChartUpdateModes.Remove))
         {
            updateColumnUsedProperty(e.Columns, e.Used, e.IsLinkedDataToSimulation);
         }
      }

      private void updateColumnUsedProperty(IReadOnlyList<DataColumn> columns, bool used, bool isLinkedDataToSimulations)
      {
         columns.Each(column =>
         {
            if (used)
            {
               var curve = AddCurveForColumn(column, isLinkedDataToSimulation: isLinkedDataToSimulations);
               updateCurveProperty(curve);
            }
            else
               Chart.RemoveCurvesForColumn(column);
         });
         
      }

      public void ApplyAllColumnSettings() => _presentersWithColumnSettings.Each(x => x.ApplyAllColumnSettings());

      public void ApplyColumnSettings(GridColumnSettings columnSettings) =>
         _presentersWithColumnSettings.Each(x => x.ApplyColumnSettings(columnSettings));

      public void ShowCustomizationForm() => _view.ShowCustomizationForm();

      public void AddUsedInMenuItem() => _view.AddUsedInMenuItemCheckBox();

      public void AddOutputMappings(OutputMappings outputMappings)
      {
         _dataBrowserPresenter.AddOutputMappings(outputMappings);
      }

      public void RemoveOutputMappings(OutputMappings outputMappings)
      {
         _dataBrowserPresenter.RemoveOutputMappings(outputMappings);
      }

      public void AddLinkSimDataMenuItem() => _view.AddLinkSimulationObservedMenuItemCheckBox();

      public void SetLinkSimDataMenuItemVisibility(bool isVisible) => _view.SetlinkSimDataMenuItemVisisbility(isVisible);

      public void RemoveAllOutputMappings()
      {
         _dataBrowserPresenter.RemoveAllOutputMappings();
      }

      public void UpdateAutoUpdateChartMode(bool autoMode)
      {
         Chart.AutoUpdateEnabled = autoMode;
         if (Chart.AutoUpdateEnabled)
            UpdateChartDisplay();
      }

      public void UpdateChartDisplay() => _eventPublisher.PublishEvent(new ApplyChangesEvent(Chart));

      public void OnDragDrop(IDragEvent dropEvent) => DragDrop(this, dropEvent);

      public void OnDragOver(IDragEvent dragEvent) => DragOver(this, dragEvent);

      public void UpdateUsedForSelection(bool? isUsed)
      {
         if (!isUsed.HasValue) return;

         _dataBrowserPresenter.UpdateUsedStateForSelection(used: isUsed.Value);
      }

      public void UpdateLinkSimulationToDataSelection(bool isLinkedMappedOutputs)
      {
         _dataBrowserPresenter.OutputObservedDataLinkingChanged(isLinkedMappedOutputs);
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
         _view.SetAutoUpdateModeCheckBox(Chart.AutoUpdateEnabled);
      }

      public void Clear()
      {
         _dataBrowserPresenter.Clear();
         _curveSettingsPresenter.Clear();
         _chartSettingsPresenter.Clear();
         _chartExportSettingsPresenter.Clear();
         _axisSettingsPresenter.Clear();
      }

      private void onSelectionChanged(IReadOnlyList<DataColumn> dataColumns)
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
         _dataBrowserPresenter.InitializeIsUsedForDataColumns(Chart.UsedColumns);

         //Once used columns are updated, we need to make sure we update the meta data related to used column as well
         refreshColorGroupingPresenter();
      }

      public void AddDataRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         var allColumnsToAdd = dataRepositories.SelectMany(x => x.Columns)
            .Where(c => !columnIsForInternalUseOnly(c))
            .Where(c => !hasColumn(c))
            .ToList();

         _dataBrowserPresenter.AddDataColumns(allColumnsToAdd);

         updateUsedColumns();
      }

      public void SetGroupRowFormat(GridGroupRowFormats format)
      {
         _dataBrowserPresenter.SetGroupRowFormat(format);
      }

      private bool hasColumn(DataColumn dataColumn) => _dataBrowserPresenter.ContainsDataColumn(dataColumn);

      public void RemoveAllDataRepositories() => _dataBrowserPresenter.Clear();

      public void RemoveUnusedColumns() => removeColumns(unusedColumns);

      private IReadOnlyList<DataColumn> unusedColumns
      {
         get { return AllDataColumns.Where(x => !x.IsInRepository() || columnIsForInternalUseOnly(x)).ToList(); }
      }

      private bool columnIsForInternalUseOnly(DataColumn dataColumn)
      {
         return dataColumn.IsInternal ||
                dataColumn.DataInfo.Origin == ColumnOrigins.ObservationAuxiliary ||
                !_showDataColumnInDataBrowserDefinition(dataColumn);
      }

      public void RemoveDataRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         if (dataRepositories == null) return;
         removeColumns(dataRepositories.SelectMany(x => x.Columns));
      }

      private void removeColumns(IEnumerable<DataColumn> dataColumns)
      {
         var columnsToRemove = dataColumns.ToList();
         _dataBrowserPresenter.RemoveDataColumns(columnsToRemove);
         Chart?.RemoveCurvesForColumns(columnsToRemove);
      }

      public IReadOnlyList<DataColumn> AllDataColumns => _dataBrowserPresenter.AllDataColumns;

      public void CopySettingsFrom(ChartEditorSettings settings)
      {
         CopySettingsFrom(settings, loadEditorLayout: true, loadColumnSettings: true);
      }

      private void copyColumnSettings(IEnumerable<GridColumnSettings> settings, Func<string, GridColumnSettings> columnSettingsInPresenterByName)
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
            copyColumnSettings(settings.DataBrowserColumnSettings, _dataBrowserPresenter.ColumnSettings);
            copyColumnSettings(settings.CurveOptionsColumnSettings, _curveSettingsPresenter.ColumnSettings);
            copyColumnSettings(settings.AxisOptionsColumnSettings, _axisSettingsPresenter.ColumnSettings);
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

      private void addSettingsFrom(IPresenterWithColumnSettings presenterWithColumnSettings, ChartEditorSettings settings,
         Func<ChartEditorSettings, Action<GridColumnSettings>> addAction)
      {
         presenterWithColumnSettings.AllColumnSettings.Each(x => addAction(settings)(new GridColumnSettings(x)));
      }

      public GridColumnSettings ColumnSettingsFor(BrowserColumns browserColumn) => _dataBrowserPresenter.ColumnSettings(browserColumn.ToString());

      public GridColumnSettings ColumnSettingsFor(CurveOptionsColumns curveOptionsColumn) =>
         _curveSettingsPresenter.ColumnSettings(curveOptionsColumn.ToString());

      public GridColumnSettings ColumnSettingsFor(AxisOptionsColumns axisOptionsColumn) =>
         _axisSettingsPresenter.ColumnSettings(axisOptionsColumn.ToString());

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

      public void AddCurvesWithSameColorForColumn(IReadOnlyList<DataColumn> dataColumnList, CurveOptions defaultCurveOptions = null)
      {
         var groupColor = Chart.SelectNewColor();
         foreach (var dataColumn in dataColumnList)
         {
            var (exists, curve) = createAndConfigureCurve(dataColumn);

            if (exists) continue;

            if (defaultCurveOptions != null)
               curve.CurveOptions.UpdateFrom(defaultCurveOptions);

            curve.Color = groupColor;
            curve.UpdateStyleForObservedData();

            Chart.AddCurve(curve);
         }
      }

      public Curve AddCurveForColumn(DataColumn dataColumn, CurveOptions defaultCurveOptions = null, bool isLinkedDataToSimulation = false)
      {
         var (exists, curve) = createAndConfigureCurve(dataColumn);

         Chart.UpdateCurveColorAndStyle(curve, dataColumn, AllDataColumns, isLinkedDataToSimulation);


         if (exists)
            return curve;

         if (defaultCurveOptions != null)
            curve.CurveOptions.UpdateFrom(defaultCurveOptions);

         Chart.AddCurve(curve);
         return curve;
      }
      
      private (bool exists, Curve curve) createAndConfigureCurve(DataColumn dataColumn)
      {
         var curve = Chart.FindCurveWithSameData(dataColumn.BaseGrid, dataColumn);
         if (curve != null)
            return (exists: true, Chart.CurveBy(curve.Id));

         curve = Chart.CreateCurve(dataColumn.BaseGrid, dataColumn, _curveNameDefinition(dataColumn), _dimensionFactory);
         return (exists: false, curve);
      }

      private void addCurvesForColumns(IEnumerable<DataColumn> columns, CurveOptions defaultCurveOptions = null)
      {
         using (_chartUpdater.UpdateTransaction(Chart, CurveChartUpdateModes.Add))
         {
            columns.Each(x => AddCurveForColumn(x, defaultCurveOptions));
         }
      }

      private void removeCurve(Curve curve)
      {
         using (_chartUpdater.UpdateTransaction(Chart, CurveChartUpdateModes.Remove))
         {
            Chart.RemoveCurve(curve);
         }
      }

      public void AddButton(IMenuBarItem menuBarItem)
      {
         _view.AddButton(menuBarItem);
      }

      public void ClearButtons()
      {
         _view.ClearButtons();
      }

      public void Refresh()
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

      public void Handle(ChartUpdatedEvent chartUpdatedEvent)
      {
         if (!canHandle(chartUpdatedEvent))
            return;

         Refresh();

         if (chartUpdatedEvent.PropagateChartChangeEvent)
            ChartChanged();
      }

      private void refreshColorGroupingPresenter()
      {
         _curveColorGroupingPresenter.SetMetadata(getCommonMetaDataOfCurves());
      }
   }
}