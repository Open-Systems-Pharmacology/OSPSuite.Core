using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Charts;

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
      IReadOnlyList<string> SelectedDataColumnIds { get; }

      /// <summary>
      /// Sets the Is Used property for each column in the <paramref name="usedDataRepositoryColumns"/> to true
      /// and sets the remaining columns Is Used to false
      /// </summary>
      /// <param name="usedDataRepositoryColumns">Columns that are used</param>
      void InitializeIsUsedForColumns(IEnumerable<string> usedDataRepositoryColumns);

      /// <summary>
      /// Event raised when one or more data repository columns Is Used property changes
      /// </summary>
      event EventHandler<UsedChangedEventArgs> UsedChanged;

      void RaiseUsedChanged(IEnumerable<string> dataRepositoryColumnIds, bool used);
      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;

      /// <summary>
      /// Event raised when the user has changed the selected data
      /// </summary>
      event Action<IEnumerable<string>> SelectedDataChanged;

      /// <summary>
      /// Called when the selected data has changed
      /// </summary>
      /// <param name="dataRepositoryColumnIds">A list of data repository columns that are currently selected</param>
      void UpdateDataSelection(IEnumerable<string> dataRepositoryColumnIds);

      /// <summary>
      /// For the column Id given, this returns whether or not the data is being used
      /// </summary>
      /// <param name="dataRepositoryColumnId">The data column id</param>
      /// <returns>true if the data is used in the chart otherwise false</returns>
      bool IsUsed(string dataRepositoryColumnId);

      /// <summary>
      /// Update the used/not used state for a list of columns
      /// </summary>
      /// <param name="usedDataRepositoryColumnIds">The list of columns</param>
      /// <param name="usedState">true if the columns should be used, otherwise false</param>
      void SetUsedState(IEnumerable<string> usedDataRepositoryColumnIds, bool usedState);

      /// <summary>
      /// Returns the descendant data repoository column ids. Distinct from <see cref="SelectedDataColumnIds"/> because
      /// this will return all ancestor data from a group row in the data grid
      /// </summary>
      /// <returns>all selected rows, either by direct selection, or by parent relationship</returns>
      IReadOnlyList<string> SelectedDescendentDataRepositoryColumnIds{get;}
   }

   /// <summary>
   ///    Presenter for DataBrowser component.
   ///    DataBrowser displays data repository columns as rows in a datatable in a XtraGrid with according
   ///    filter/grouping/sorting functions.
   /// </summary>
   internal class DataBrowserPresenter : PresenterWithColumnSettings<IDataBrowserView, IDataBrowserPresenter>, IDataBrowserPresenter
   {
      private readonly DataColumnsDTO _dataColumnsDTO;
      public event Action<IEnumerable<string>> SelectedDataChanged;
      public event EventHandler<UsedChangedEventArgs> UsedChanged;

      public void RaiseUsedChanged(IEnumerable<string> dataRepositoryColumnIds, bool used)
      {
         UsedChanged(this, new UsedChangedEventArgs(dataRepositoryColumnIds , used));
      }

      private void raiseUsedChanged(string dataRepositoryColumnId, bool used)
      {
         RaiseUsedChanged(new[] {dataRepositoryColumnId}, used);
      }

      public DataBrowserPresenter(IDataBrowserView view)
         : base(view)
      {
         _dataColumnsDTO = new DataColumnsDTO();
         _view.SetDataSource(_dataColumnsDTO);
         SelectedDataChanged += delegate {  };
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
         _dataColumnsDTO.AddDataColumn(dataColumn);
      }

      public void UpdateDataColumn(DataColumn dataColumn)
      {
         _dataColumnsDTO.UpdateDataFromDataRepositoryColumn(dataColumn);
      }

      public bool ContainsDataColumn(DataColumn dataColumn)
      {
         return _dataColumnsDTO.ContainsDataColumn(dataColumn.Id);
      }

      public void RemoveDataColumn(DataColumn dataColumn)
      {
         if (!_dataColumnsDTO.Contains(dataColumn.Id)) return;
         raiseUsedChanged(dataColumn.Id, false);
         _dataColumnsDTO.RemoveDataColumn(dataColumn.Id);
      }

      public void ClearDataColumns()
      {
         foreach (var dataColumnId in _dataColumnsDTO.GetUsedDataRepositoryColumnIds())
            raiseUsedChanged(dataColumnId, false);
         _dataColumnsDTO.Clear();
      }

      public IReadOnlyList<string> SelectedDataColumnIds
      {
         get { return _view.SelectedDataColumnIds; }
      }

      public bool IsUsed(string dataRepositoryColumnId)
      {
         return _dataColumnsDTO.GetUsedDataRepositoryColumnIds().Contains(dataRepositoryColumnId);
      }

      public IReadOnlyList<string> SelectedDescendentDataRepositoryColumnIds
      {
         get{return _view.SelectedDescendentDataRepositoryColumnIds;}
      } 

      public void SetUsedState(IEnumerable<string> usedDataRepositoryColumnIds, bool usedState)
      {
         var columnIds = usedDataRepositoryColumnIds.ToList();

         _dataColumnsDTO.SetUsedValueForColumns(columnIds, usedState);
         RaiseUsedChanged(columnIds, usedState);
      }

      public void InitializeIsUsedForColumns(IEnumerable<string> usedDataRepositoryColumns)
      {
         _dataColumnsDTO.InitializeUsedColumn(usedDataRepositoryColumns);
      }

      public event DragEventHandler DragOver
      {
         add { _view.DragOver += value; }
         remove { _view.DragOver -= value; }
      }

      public event DragEventHandler DragDrop
      {
         add { _view.DragDrop += value; }
         remove { _view.DragDrop -= value; }
      }

      public void UpdateDataSelection(IEnumerable<string> dataRepositoryColumnIds)
      {
         SelectedDataChanged(dataRepositoryColumnIds);
      }
   }
}