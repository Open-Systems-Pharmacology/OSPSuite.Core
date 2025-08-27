using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Assets.Captions.Chart.DataBrowser;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IDataBrowserPresenter : IPresenter<IDataBrowserView>, IPresenterWithColumnSettings
   {
      void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition);

      void AddDataColumns(IEnumerable<DataColumn> dataColumns);

      bool ContainsDataColumn(DataColumn dataColumn);

      void RemoveDataColumns(IEnumerable<DataColumn> dataColumns);

      void Clear();

      /// <summary>
      ///    Sets the Is Used property for each column in the <paramref name="usedDataColumns" /> to true
      ///    and sets the remaining columns Is Used to false
      /// </summary>
      /// <param name="usedDataColumns">Columns that are used</param>
      void InitializeIsUsedForDataColumns(IReadOnlyList<DataColumn> usedDataColumns);

      /// <summary>
      ///    Event raised when one or more data columns used state changes
      /// </summary>
      event EventHandler<UsedColumnsEventArgs> UsedChanged;

      /// <summary>
      ///    Event raised when the user has changed the selected data
      /// </summary>
      event EventHandler<ColumnsEventArgs> SelectionChanged;

      /// <summary>
      ///    For the column Id given, this returns whether or not the data is being used
      /// </summary>
      /// <param name="dataColumn">The data column</param>
      /// <returns>true if the data is used in the chart otherwise false</returns>
      bool IsUsed(DataColumn dataColumn);

      /// <summary>
      ///    Update the used/not used state for a list of columns
      /// </summary>
      /// <param name="dataColumnDTOs">The list of columns</param>
      /// <param name="used">true if the columns should be used, otherwise false</param>
      /// <param name="isLinkedDataToSimulations">true if the columns should be linked to the simulation, so same color should be applied</param>
      void SetUsedState(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used, bool isLinkedDataToSimulations = false);

      /// <summary>
      ///    Returns all <see cref="DataColumn" /> currently selected by the user
      /// </summary>
      IReadOnlyList<DataColumn> SelectedDataColumns { get; }

      /// <summary>
      ///    Returns all <see cref="DataColumn" /> available
      /// </summary>
      IReadOnlyList<DataColumn> AllDataColumns { get; }

      /// <summary>
      ///    Is called from the view when the <paramref name="used" /> state is changed for the <paramref name="dataColumnDTO" />
      /// </summary>
      void UsedChangedFor(DataColumnDTO dataColumnDTO, bool used);

      /// <summary>
      ///    Update the used state for all selected column to <paramref name="used" />
      /// </summary>
      /// <param name="used"></param>
      void UpdateUsedStateForSelection(bool used);

      /// <summary>
      ///    Is called from the view when the column selection is changed by the user
      /// </summary>
      /// s
      void SelectedDataColumnsChanged();

      /// <summary>
      ///    Adds the output mappings (reference used to link observed data to output)
      /// </summary>
      void AddOutputMappings(OutputMappings outputMappings);

      /// <summary>
      ///    Returns all the DataColumns for the curves that are visible in the chart
      /// </summary>
      IReadOnlyList<DataColumn> GetAllUsedDataColumns();

      /// <summary>
      ///    Changes the bool that defines whether the corresponding observed data used state
      ///    should be updated when their linked output used state is updated
      /// </summary>
      void OutputObservedDataLinkingChanged(bool isLinkedMappedOutputs);

      /// <summary>
      ///    sets the group row format of the gridView to the specified string.
      /// </summary>
      void SetGroupRowFormat(GridGroupRowFormats format);

      /// <summary>
      ///    Removed the specified output mappings from the list the DataBrowser keeps.
      /// </summary>
      void RemoveOutputMappings(OutputMappings outputMappings);

      /// <summary>
      ///    Removed all the output mappings from the list the DataBrowser keeps.
      /// </summary>
      void RemoveAllOutputMappings();
   }

   public class DataBrowserPresenter : PresenterWithColumnSettings<IDataBrowserView, IDataBrowserPresenter>, IDataBrowserPresenter
   {
      private readonly Cache<DataColumn, DataColumnDTO> _dataColumnDTOCache = new Cache<DataColumn, DataColumnDTO>(x => x.DataColumn, x => null);
      private readonly List<DataColumn> _allDataColumns = new List<DataColumn>();
      private Func<DataColumn, PathElements> _displayQuantityPathDefinition;
      private bool _isLinkedMappedOutputs;
      private readonly HashSet<OutputMappings> _allOutputMappings = new HashSet<OutputMappings>();
      public event EventHandler<ColumnsEventArgs> SelectionChanged = delegate { };
      public event EventHandler<UsedColumnsEventArgs> UsedChanged = delegate { };

      public DataBrowserPresenter(IDataBrowserView view) : base(view)
      {
      }

      public void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition)
      {
         _displayQuantityPathDefinition = displayQuantityPathDefinition;
      }

      public void AddDataColumns(IEnumerable<DataColumn> dataColumns)
      {
         dataColumns.Each(c =>
         {
            _dataColumnDTOCache.Add(mapFrom(c));
            _allDataColumns.Add(c);
         });
         bindToView();
      }

      private void bindToView()
      {
         _view.BindTo(_dataColumnDTOCache);
      }

      private DataColumnDTO mapFrom(DataColumn dataColumn) => new DataColumnDTO(dataColumn, _displayQuantityPathDefinition);

      public bool ContainsDataColumn(DataColumn dataColumn) => _dataColumnDTOCache.Contains(dataColumn);

      public void RemoveDataColumns(IEnumerable<DataColumn> dataColumns)
      {
         var columnsToRemove = dataColumns.ToList();
         columnsToRemove.Each(c =>
         {
            _dataColumnDTOCache.Remove(c);
            _allDataColumns.Remove(c);
         });
         bindToView();
      }

      public void Clear()
      {
         _dataColumnDTOCache.Clear();
         _allDataColumns.Clear();
         bindToView();
      }

      public bool IsUsed(DataColumn dataColumn) => _dataColumnDTOCache[dataColumn]?.Used ?? false;

      public IReadOnlyList<DataColumn> SelectedDataColumns => columnsFrom(_view.SelectedColumns);

      public IReadOnlyList<DataColumn> AllDataColumns => _allDataColumns;

      public void UsedChangedFor(DataColumnDTO dataColumnDTO, bool used)
      {
         raiseUsedChanged(dataColumnDTO.DataColumn, used);
         updateDataSelection(_view.SelectedColumns);
         updateLinkedObservedData(dataColumnDTO.DataColumn, used);
      }

      private void updateLinkedObservedData(DataColumn dataColumn, bool used)
      {
         if (!_isLinkedMappedOutputs) return;

         var linkedObservedData = getLinkedObservedDataFromOutputPath(dataColumn.PathAsString);
         SetUsedState(linkedObservedData, used);
      }

      private IReadOnlyList<DataColumnDTO> getLinkedObservedDataFromOutputPath(string outputPath)
      {
         var linkedObservedDataRepositories = _allOutputMappings.SelectMany(x => x.AllDataRepositoryMappedTo(outputPath));
         return getDataColumnDTOsFromDataRepositories(linkedObservedDataRepositories);
      }

      private IReadOnlyList<DataColumnDTO> getDataColumnDTOsFromDataRepositories(IEnumerable<DataRepository> linkedObservedDataRepositories)
      {
         return _dataColumnDTOCache.Where(x => linkedObservedDataRepositories.Contains(x.DataColumn.Repository)).ToList();
      }

      public void AddOutputMappings(OutputMappings outputMappings) => _allOutputMappings.Add(outputMappings);

      public void RemoveOutputMappings(OutputMappings outputMappings)
      {
         _allOutputMappings.Remove(outputMappings);
      }

      public void RemoveAllOutputMappings()
      {
         _allOutputMappings.Clear();
      }

      public void UpdateUsedStateForSelection(bool used)
      {
         SetUsedState(_view.SelectedDescendantColumns, used);
      }

      public void SelectedDataColumnsChanged()
      {
         updateDataSelection(_view.SelectedDescendantColumns);
      }

      public IReadOnlyList<DataColumn> GetAllUsedDataColumns()
      {
         return _dataColumnDTOCache.Where(x => x.Used).Select(x => x.DataColumn).ToList();
      }

      public void OutputObservedDataLinkingChanged(bool isLinkedMappedOutputs)
      {
         _isLinkedMappedOutputs = isLinkedMappedOutputs;

         if (!_isLinkedMappedOutputs) return;

         //loop only through the simulation outputs data columns
         foreach (var dataColumnDTO in _dataColumnDTOCache.Where(x => x.Category == Captions.Chart.GroupRowFormat.Simulation))
         {
            var outputColumnUsed = dataColumnDTO.Used;
            var linkedObservedData = getLinkedObservedDataFromOutputPath(dataColumnDTO.DataColumn.PathAsString);
            SetUsedState(linkedObservedData, outputColumnUsed, true);
         }
      }

      public void SetGroupRowFormat(GridGroupRowFormats format)
      {
         _view.SetGroupRowFormat(format);
      }

      public void SetUsedState(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used, bool isLinkedDataToSimulations = false)
      {
         updateUsedStateForColumns(dataColumnDTOs, used);
         raiseUsedChanged(columnsFrom(dataColumnDTOs), used, isLinkedDataToSimulations);
         updateDataSelection(_view.SelectedColumns);
      }

      public void InitializeIsUsedForDataColumns(IReadOnlyList<DataColumn> usedDataColumns)
      {
         _dataColumnDTOCache.Each(dto => dto.Used = false);
         updateUsedStateForColumns(usedDataColumns, true);
      }

      private void updateUsedStateForColumns(IReadOnlyList<DataColumn> dataColumns, bool used)
      {
         var allDataColumnDTOs = dataColumns.Select(col => _dataColumnDTOCache[col]).Where(dto => dto != null).ToList();
         updateUsedStateForColumns(allDataColumnDTOs, used);
      }

      private void updateUsedStateForColumns(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used)
      {
         dataColumnDTOs.Each(dto => dto.Used = used);
      }

      private void raiseUsedChanged(IReadOnlyList<DataColumn> dataColumns, bool used, bool isLinkedDataToSimulations = false)
      {
         UsedChanged(this, new UsedColumnsEventArgs(dataColumns, used, isLinkedDataToSimulations));
      }

      private void raiseUsedChanged(DataColumn dataColumn, bool used, bool isLinkedDataToSimulations = false)
      {
         raiseUsedChanged(new[] { dataColumn }, used, isLinkedDataToSimulations);
      }

      private void updateDataSelection(IReadOnlyList<DataColumnDTO> selectedDataColumnDTOs)
      {
         SelectionChanged(this, new ColumnsEventArgs(columnsFrom(selectedDataColumnDTOs)));
      }

      private IReadOnlyList<DataColumn> columnsFrom(IEnumerable<DataColumnDTO> dataColumnDTOs) => dataColumnDTOs.Select(x => x.DataColumn).ToList();

      protected override void SetDefaultColumnSettings()
      {
         //-1 actively removes the grouping for this column if there was one till now - specifying none leaves everything as is.
         AddColumnSettings(BrowserColumns.RepositoryName).WithCaption(RepositoryName).GroupIndex = -1;
         AddColumnSettings(BrowserColumns.Simulation).WithCaption(Captions.SimulationPath);
         AddColumnSettings(BrowserColumns.TopContainer).WithCaption(Captions.TopContainerPath);
         AddColumnSettings(BrowserColumns.Container).WithCaption(Captions.ContainerPath);
         AddColumnSettings(BrowserColumns.BottomCompartment).WithCaption(Captions.BottomCompartmentPath);
         AddColumnSettings(BrowserColumns.Molecule).WithCaption(Captions.MoleculePath);
         AddColumnSettings(BrowserColumns.Name).WithCaption(Captions.NamePath);
         AddColumnSettings(BrowserColumns.BaseGridName).WithCaption(BaseGridName).WithVisible(false);
         AddColumnSettings(BrowserColumns.ColumnId).WithCaption(ColumnId).WithVisible(false);
         AddColumnSettings(BrowserColumns.OrderIndex).WithCaption(OrderIndex).WithVisible(false);
         AddColumnSettings(BrowserColumns.DimensionName).WithCaption(DimensionName);
         AddColumnSettings(BrowserColumns.QuantityType).WithCaption(Captions.Chart.DataBrowser.QuantityType).WithVisible(false);
         AddColumnSettings(BrowserColumns.QuantityName).WithCaption(QuantityName);
         AddColumnSettings(BrowserColumns.HasRelatedColumns).WithCaption(HasRelatedColumns).WithVisible(false);
         AddColumnSettings(BrowserColumns.Origin).WithCaption(Captions.Chart.DataBrowser.Origin).WithVisible(false);
         AddColumnSettings(BrowserColumns.Date).WithCaption(Date).WithVisible(false);
         AddColumnSettings(BrowserColumns.Category).WithCaption(Category).WithVisible(false);
         AddColumnSettings(BrowserColumns.Source).WithCaption(Source).WithVisible(false);
         AddColumnSettings(BrowserColumns.Used).WithCaption(Used).WithVisible(true);
      }
   }
}