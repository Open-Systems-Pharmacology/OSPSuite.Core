using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

      void AddDataColumn(DataColumn dataColumn);

      bool ContainsDataColumn(DataColumn dataColumn);

      void RemoveDataColumn(DataColumn dataColumn);

      void Clear();

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

      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;

      /// <summary>
      ///    Event raised when the user has changed the selected data
      /// </summary>
      event Action<IReadOnlyList<DataColumn>> SelectedDataChanged;

      /// <summary>
      ///    Called when the selected data has changed
      /// </summary>
      /// <param name="selectedDataColumnDTOs">A list of data repository columns that are currently selected</param>
      void UpdateDataSelection(IReadOnlyList<DataColumnDTO> selectedDataColumnDTOs);

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

      IReadOnlyList<DataColumn> SelectedDataColumns { get; }

      void UsedChangedFor(DataColumnDTO dto, bool used);
      void UpdateUsedForSelection(bool used);
   }

   internal class DataBrowserPresenter : PresenterWithColumnSettings<IDataBrowserView, IDataBrowserPresenter>, IDataBrowserPresenter
   {
      private readonly NotifyCache<DataColumn, DataColumnDTO> _dataColumnDTOCache;
      private Func<DataColumn, PathElements> _displayQuantityPathDefinition;
      public event Action<IReadOnlyList<DataColumn>> SelectedDataChanged = delegate { };
      public event EventHandler<UsedChangedEventArgs> UsedChanged = delegate { };

      public DataBrowserPresenter(IDataBrowserView view) : base(view)
      {
         _dataColumnDTOCache = new NotifyCache<DataColumn, DataColumnDTO>(x => x.DataColumn, x => null);
         _view.BindTo(_dataColumnDTOCache);
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
         _displayQuantityPathDefinition = displayQuantityPathDefinition;
      }

      public void AddDataColumn(DataColumn dataColumn)
      {
         //Required because adding a column to a bound collection triggers a bunch of devexpress events
         _view.DoWithoutColumnSettingsUpdateNotification(() =>
         {
            _dataColumnDTOCache.Add(new DataColumnDTO(dataColumn, _displayQuantityPathDefinition));
         });
      }

      public bool ContainsDataColumn(DataColumn dataColumn)
      {
         return _dataColumnDTOCache.Contains(dataColumn);
      }

      public void RemoveDataColumn(DataColumn dataColumn)
      {
         if (!ContainsDataColumn(dataColumn)) return;

         _dataColumnDTOCache.Remove(dataColumn);
         raiseUsedChanged(dataColumn, false);
      }

      public void Clear()
      {
         _view.DeleteBinding();
         _dataColumnDTOCache.Clear();
      }

      public bool IsUsed(DataColumn dataColumn)
      {
         return _dataColumnDTOCache[dataColumn]?.Used ?? false;
      }

      public IReadOnlyList<DataColumn> SelectedDataColumns => columnsFrom(_view.SelectedDataColumns);

      public void UsedChangedFor(DataColumnDTO dto, bool used)
      {
         raiseUsedChanged(dto.DataColumn, used);
         UpdateDataSelection(_view.SelectedDataColumns);
      }

      public void UpdateUsedForSelection(bool used)
      {
         SetUsedState(_view.SelectedDescendentDataRepositoryColumns, used);
      }

      public void SetUsedState(IReadOnlyList<DataColumnDTO> dataColumnDTOs, bool used)
      {
         updateUsedStateForColumns(dataColumnDTOs, used);
         RaiseUsedChanged(columnsFrom(dataColumnDTOs), used);
         UpdateDataSelection(_view.SelectedDataColumns);
      }

      public void InitializeIsUsedForColumns(IReadOnlyList<DataColumn> usedDataColumns)
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

      public void UpdateDataSelection(IReadOnlyList<DataColumnDTO> selectedDataColumnDTOs)
      {
         SelectedDataChanged(columnsFrom(selectedDataColumnDTOs));
      }

      private IReadOnlyList<DataColumn> columnsFrom(IEnumerable<DataColumnDTO> dataColumnDTOs) => dataColumnDTOs.Select(x => x.DataColumn).ToList();
   }
}