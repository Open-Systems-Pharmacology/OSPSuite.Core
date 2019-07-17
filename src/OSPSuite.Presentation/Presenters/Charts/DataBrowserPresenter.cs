using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

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
      /// <param name="dataColumn">The data colum</param>
      /// <returns>true if the data is used in the chart otherwise false</returns>
      bool IsUsed(DataColumn dataColumn);

      /// <summary>
      ///    Update the used/not used state for a list of columns
      /// </summary>
      /// <param name="dataColumnDTOs">The list of columns</param>
      /// <param name="used">true if the columns should be used, otherwise false</param>
      void SetUsedState(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used);

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
      void SelectedDataColumnsChanged();
   }

   public class DataBrowserPresenter : PresenterWithColumnSettings<IDataBrowserView, IDataBrowserPresenter>, IDataBrowserPresenter
   {
      private readonly Cache<DataColumn, DataColumnDTO> _dataColumnDTOCache = new Cache<DataColumn, DataColumnDTO>(x => x.DataColumn, x => null);
      private readonly List<DataColumn> _allDataColumns = new List<DataColumn>();
      private Func<DataColumn, PathElements> _displayQuantityPathDefinition;
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
      }

      public void UpdateUsedStateForSelection(bool used)
      {
         SetUsedState(_view.SelectedDescendantColumns, used);
      }

      public void SelectedDataColumnsChanged()
      {
         updateDataSelection(_view.SelectedDescendantColumns);
      }

      public void SetUsedState(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used)
      {
         updateUsedStateForColumns(dataColumnDTOs, used);
         raiseUsedChanged(columnsFrom(dataColumnDTOs), used);
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

      private void raiseUsedChanged(IReadOnlyList<DataColumn> dataColumns, bool used)
      {
         UsedChanged(this, new UsedColumnsEventArgs(dataColumns, used));
      }

      private void raiseUsedChanged(DataColumn dataColumn, bool used)
      {
         raiseUsedChanged(new[] {dataColumn}, used);
      }

      private void updateDataSelection(IReadOnlyList<DataColumnDTO> selectedDataColumnDTOs)
      {
         SelectionChanged(this, new ColumnsEventArgs(columnsFrom(selectedDataColumnDTOs)));
      }

      private IReadOnlyList<DataColumn> columnsFrom(IEnumerable<DataColumnDTO> dataColumnDTOs) => dataColumnDTOs.Select(x => x.DataColumn).ToList();

      protected override void SetDefaultColumnSettings()
      {
         AddColumnSettings(BrowserColumns.RepositoryName).WithCaption(Captions.Chart.DataBrowser.RepositoryName).GroupIndex = 0;
         AddColumnSettings(BrowserColumns.Simulation).WithCaption(Captions.SimulationPath);
         AddColumnSettings(BrowserColumns.TopContainer).WithCaption(Captions.TopContainerPath);
         AddColumnSettings(BrowserColumns.Container).WithCaption(Captions.ContainerPath);
         AddColumnSettings(BrowserColumns.BottomCompartment).WithCaption(Captions.BottomCompartmentPath);
         AddColumnSettings(BrowserColumns.Molecule).WithCaption(Captions.MoleculePath);
         AddColumnSettings(BrowserColumns.Name).WithCaption(Captions.NamePath);
         AddColumnSettings(BrowserColumns.BaseGridName).WithCaption(Captions.Chart.DataBrowser.BaseGridName).WithVisible(false);
         AddColumnSettings(BrowserColumns.ColumnId).WithCaption(Captions.Chart.DataBrowser.ColumnId).WithVisible(false);
         AddColumnSettings(BrowserColumns.OrderIndex).WithCaption(Captions.Chart.DataBrowser.OrderIndex).WithVisible(false);
         AddColumnSettings(BrowserColumns.DimensionName).WithCaption(Captions.Chart.DataBrowser.DimensionName);
         AddColumnSettings(BrowserColumns.QuantityType).WithCaption(Captions.Chart.DataBrowser.QuantityType).WithVisible(false);
         AddColumnSettings(BrowserColumns.QuantityName).WithCaption(Captions.Chart.DataBrowser.QuantityName);
         AddColumnSettings(BrowserColumns.HasRelatedColumns).WithCaption(Captions.Chart.DataBrowser.HasRelatedColumns).WithVisible(false);
         AddColumnSettings(BrowserColumns.Origin).WithCaption(Captions.Chart.DataBrowser.Origin).WithVisible(false);
         AddColumnSettings(BrowserColumns.Date).WithCaption(Captions.Chart.DataBrowser.Date).WithVisible(false);
         AddColumnSettings(BrowserColumns.Category).WithCaption(Captions.Chart.DataBrowser.Category).WithVisible(false);
         AddColumnSettings(BrowserColumns.Source).WithCaption(Captions.Chart.DataBrowser.Source).WithVisible(false);
         AddColumnSettings(BrowserColumns.Used).WithCaption(Captions.Chart.DataBrowser.Used).WithVisible(true);
      }
   }
}