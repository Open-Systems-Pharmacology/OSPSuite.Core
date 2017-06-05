using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IDataBrowserPresenter : IPresenter<IDataBrowserView>, IPresenterWithColumnSettings
   {
      void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition);
      void AddDataColumn(DataColumn dataColumn);
      void UpdateDataColumn(DataColumn dataColumn);
      bool ContainsDataColumn(DataColumn dataColumn);
      void RemoveDataColumn(DataColumn dataColumn);
      void ClearDataColumns();

      /// <summary>
      ///    Sets the Is Used property for each column in the <paramref name="usedDataColumns" /> to true
      ///    and sets the remaining columns Is Used to false
      /// </summary>
      /// <param name="usedDataColumns">Columns that are used</param>
      void InitializeIsUsedForColumns(IReadOnlyList<DataColumn> usedDataColumns);

      /// <summary>
      ///    Event raised when one or more data repository columns Is Used property changes
      /// </summary>
      event EventHandler<UsedChangedEventArgs> UsedChanged;

      void RaiseUsedChanged(IEnumerable<DataColumn> dataColumns, bool used);
      void RaiseUsedChanged(IEnumerable<string> dataColumnIds, bool used);

      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;

      /// <summary>
      ///    Event raised when the user has changed the selected data
      /// </summary>
      event Action<IReadOnlyList<DataColumn>> SelectedDataChanged;

      /// <summary>
      ///    Called when the selected data has changed
      /// </summary>
      /// <param name="dataColumnIds">A list of data repository columns that are currently selected</param>
      void UpdateDataSelection(IReadOnlyList<string> dataColumnIds);

      /// <summary>
      ///    For the column Id given, this returns whether or not the data is being used
      /// </summary>
      /// <param name="dataColumn">The data colum</param>
      /// <returns>true if the data is used in the chart otherwise false</returns>
      bool IsUsed(DataColumn dataColumn);

      /// <summary>
      ///    Update the used/not used state for a list of columns
      /// </summary>
      /// <param name="usedDataColumns">The list of columns</param>
      /// <param name="usedState">true if the columns should be used, otherwise false</param>
      void SetUsedState(IReadOnlyList<DataColumn> usedDataColumns, bool usedState);

      /// <summary>
      ///    Returns the descendant data repoository column ids. Distinct from <see cref="SelectedDataColumns" /> because
      ///    this will return all ancestor data from a group row in the data grid
      /// </summary>
      /// <returns>all selected rows, either by direct selection, or by parent relationship</returns>
      IReadOnlyList<DataColumn> SelectedDescendentDataRepositoryColumns { get; }

      IReadOnlyList<DataColumn> SelectedDataColumns { get; }
   }

   internal class DataBrowserPresenter : PresenterWithColumnSettings<IDataBrowserView, IDataBrowserPresenter>, IDataBrowserPresenter
   {
      private readonly DataColumnsDTO _dataColumnsDTO;
      private readonly Cache<string, DataColumn> _dataColumnCache;
      public event Action<IReadOnlyList<DataColumn>> SelectedDataChanged = delegate { };
      public event EventHandler<UsedChangedEventArgs> UsedChanged = delegate { };

      public DataBrowserPresenter(IDataBrowserView view) : base(view)
      {
         _dataColumnsDTO = new DataColumnsDTO();
         _dataColumnCache = new Cache<string, DataColumn>(x => x.Id);
         _view.SetDataSource(_dataColumnsDTO);
      }

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

      public void SetDisplayQuantityPathDefinition(Func<DataColumn, PathElements> displayQuantityPathDefinition)
      {
         _dataColumnsDTO.DisplayQuantityPath = displayQuantityPathDefinition;
      }

      public void AddDataColumn(DataColumn dataColumn)
      {
         _dataColumnCache.Add(dataColumn);
         _dataColumnsDTO.AddDataColumn(dataColumn);
      }

      public void UpdateDataColumn(DataColumn dataColumn)
      {
         _dataColumnsDTO.UpdateDataFromDataRepositoryColumn(dataColumn);
      }

      public bool ContainsDataColumn(DataColumn dataColumn)
      {
         return _dataColumnCache.Contains(dataColumn.Id);
      }

      public void RemoveDataColumn(DataColumn dataColumn)
      {
         if (!ContainsDataColumn(dataColumn)) return;

         _dataColumnCache.Remove(dataColumn.Id);
         _dataColumnsDTO.RemoveDataColumn(dataColumn);
         raiseUsedChanged(dataColumn, false);
      }

      public void ClearDataColumns()
      {
         foreach (var dataColumn in _dataColumnCache)
         {
            raiseUsedChanged(dataColumn, false);
         }

         _dataColumnCache.Clear();
         _dataColumnsDTO.Clear();
      }

      public IReadOnlyList<string> SelectedDataColumnIds => _view.SelectedDataColumnIds;

      public bool IsUsed(DataColumn dataColumn)
      {
         return _dataColumnsDTO.IsUsed(dataColumn);
      }

      public IReadOnlyList<DataColumn> SelectedDescendentDataRepositoryColumns => columnsFrom(_view.SelectedDescendentDataRepositoryColumnIds);
      public IReadOnlyList<DataColumn> SelectedDataColumns => columnsFrom(_view.SelectedDataColumnIds);

      public void SetUsedState(IReadOnlyList<DataColumn> dataColumns, bool usedState)
      {
         _dataColumnsDTO.SetUsedValueForColumns(dataColumns, usedState);
         RaiseUsedChanged(dataColumns, usedState);
      }

      public void InitializeIsUsedForColumns(IReadOnlyList<DataColumn> usedDataColumns)
      {
         _dataColumnsDTO.InitializeUsedColumn(usedDataColumns);
      }

      public event DragEventHandler DragOver
      {
         add => _view.DragOver += value;
         remove => _view.DragOver -= value;
      }

      public event DragEventHandler DragDrop
      {
         add => _view.DragDrop += value;
         remove => _view.DragDrop -= value;
      }

      public void RaiseUsedChanged(IEnumerable<DataColumn> dataColumns, bool used)
      {
         UsedChanged(this, new UsedChangedEventArgs(dataColumns, used));
      }

      private void raiseUsedChanged(DataColumn dataColumn, bool used)
      {
         RaiseUsedChanged(new[] {dataColumn}, used);
      }

      public void RaiseUsedChanged(IEnumerable<string> dataColumnIds, bool used)
      {
         RaiseUsedChanged(columnsFrom(dataColumnIds), used);
      }

      public void UpdateDataSelection(IReadOnlyList<string> dataColumnIds)
      {
         SelectedDataChanged(columnsFrom(dataColumnIds));
      }

      private IReadOnlyList<DataColumn> columnsFrom(IEnumerable<string> dataColumnIds)
      {
         return dataColumnIds.Select(x => _dataColumnCache[x]).ToList();
      }
   }
}