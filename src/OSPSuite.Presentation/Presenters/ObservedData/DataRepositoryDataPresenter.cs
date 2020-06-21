using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using DataColumn = System.Data.DataColumn;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryDataPresenter : IBaseDataRepositoryDataPresenter<IDataRepositoryDataView>, IDataRepositoryItemPresenter,
      IUnitsInColumnPresenter<int>,
      IListener<ObservedDataValueChangedEvent>,
      IListener<ObservedDataTableChangedEvent>
   {
      void ValueIsSet(CellValueChangedDTO cellValueChangedDTO);
      void RemoveData(int rowIndex);
      void AddRow();
      void AddData(int rowIndex);
      IEnumerable<string> GetCellValidationErrorMessages(int rowIndex, int columnIndex, string newValue);
      int NumberOfObservations { get; }
   }

   public class DataRepositoryDataPresenter : BaseDataRepositoryDataPresenter<IDataRepositoryDataView, IDataRepositoryDataPresenter>, IDataRepositoryDataPresenter
   {
      private readonly IDataRepositoryExportTask _dataRepositoryTask;
      private readonly IEditObservedDataTask _editObservedDataTask;

      public DataRepositoryDataPresenter(IDataRepositoryDataView view, IDataRepositoryExportTask dataRepositoryTask, IEditObservedDataTask editObservedDataTask)
         : base(view)
      {
         _dataRepositoryTask = dataRepositoryTask;
         _editObservedDataTask = editObservedDataTask;
      }

      protected override DataTable MapDataTableFromColumns()
      {
         return _dataRepositoryTask.ToDataTable(_observedData, new DataColumnExportOptions { ForceColumnTypeAsObject = true }).First();
      }

      public void ValueIsSet(CellValueChangedDTO cellValueChangedDTO)
      {
         this.DoWithinLatch(() => AddCommand(_editObservedDataTask.SetValue(_observedData, mapFrom(cellValueChangedDTO))));
      }

      public void RemoveData(int rowIndex)
      {
         if (_observedData.BaseGrid.Count > rowIndex)
            AddCommand(_editObservedDataTask.RemoveValue(_observedData, rowIndex));

         else if (_datatable.Rows.Count > rowIndex)
            _datatable.Rows.RemoveAt(rowIndex);
      }

      public void AddRow()
      {
         var identificationsUsingObservedData = _editObservedDataTask.ParameterIdentificationsUsingDataRepository(_observedData);
         if (identificationsUsingObservedData.Any())
            throw new OSPSuiteException(Captions.ParameterIdentification.CannotAddObservedDataPointBeingUsedByParameterIdentification(_observedData.Name, identificationsUsingObservedData));

         _datatable.Rows.Add(_datatable.NewRow());
      }

      public void AddData(int rowIndex)
      {
         var dataRowData = getDataForRow(rowIndex);

         if (isNotSuitableForAddRemoveOperation(dataRowData)) return;

         AddCommand(_editObservedDataTask.AddValue(_observedData, dataRowData));
      }

      /// <summary>
      ///    This method validates the proposed new value in the context of the other values in the table.
      /// </summary>
      /// <param name="rowIndex">The row of the proposed change</param>
      /// <param name="columnIndex">The column of the proposed change</param>
      /// <param name="newValue">The proposed new value</param>
      /// <returns></returns>
      public IEnumerable<string> GetCellValidationErrorMessages(int rowIndex, int columnIndex, string newValue)
      {
         if (string.IsNullOrWhiteSpace(newValue))
            return new[] {Error.ValueIsRequired};

         var proposedValue = newValue.ConvertedTo<float>();
         var editedColumnId = GetColumnIdFromColumnIndex(columnIndex);

         if (string.Equals(editedColumnId, _observedData.BaseGrid.Id))
            return validateBaseGridValueChange(proposedValue, rowIndex);

         return Enumerable.Empty<string>();
      }

      public int NumberOfObservations => _observedData.BaseGrid.Count;

      private IEnumerable<string> validateBaseGridValueChange(float proposedValue, int rowIndex)
      {
         var result = new List<string>();
         var proposedBaseValue = _observedData.ConvertBaseValueForColumn(_observedData.BaseGrid.Id, proposedValue);

         // The proposed value is the same as the current value
         if (_observedData.BaseGrid.Values.Count > rowIndex && ValueComparer.AreValuesEqual(proposedValue, _observedData.BaseGrid.Values[rowIndex]))
            return result;

         // The proposed value is already in the basegrid somewhere else
         if (_observedData.BaseGrid.Values.Contains(proposedBaseValue))
            result.Add(Error.ExistingValueInDataRepository(_observedData.BaseGrid.Name, proposedValue, _observedData.BaseGrid.DisplayUnit.ToString()));


         return result;
      }

      private bool isNotSuitableForAddRemoveOperation(DataRowData dataRowData)
      {
         // Must not have empty cells
         if (dataRowData.Data.Any(x => !x.IsValid()))
            return true;

         // Must have values for all columns
         if (!dataRowData.Data.Keys.ContainsAll(_observedData.Columns.Where(x => !x.IsBaseGrid()).Select(x => x.Id)))
            return true;

         if (dataRowData.BaseGridValue < 0)
            return true;

         return false;
      }

      private DataRowData getDataForRow(int rowIndex)
      {
         var rowData = new DataRowData();

         allButBaseGrid().Each(column =>
            rowData.Data.Add(getRepositoryColumnIdFromDataColumn(column), _observedData.ConvertBaseValueForColumn(ColumnIdFromColumn(column), convertCellToFloat(rowIndex, column))));

         var baseGrid = baseGridColumn();
         rowData.BaseGridValue = _observedData.ConvertBaseValueForColumn(ColumnIdFromColumn(baseGrid), convertCellToFloat(rowIndex, baseGrid));
         return rowData;
      }

      private DataColumn baseGridColumn()
      {
         var baseGridId = _observedData.BaseGrid.Id;
         return _datatable.Columns.Cast<DataColumn>().First(x => getRepositoryColumnIdFromDataColumn(x).Equals(baseGridId));
      }

      private IEnumerable<DataColumn> allButBaseGrid()
      {
         var baseGridId = _observedData.BaseGrid.Id;
         return _datatable.Columns.Cast<DataColumn>().Where(x => !getRepositoryColumnIdFromDataColumn(x).Equals(baseGridId));
      }

      private string getRepositoryColumnIdFromDataColumn(DataColumn column)
      {
         return GetColumnIdFromColumnIndex(_datatable.Columns.IndexOf(column));
      }

      private float convertCellToFloat(int rowIndex, DataColumn column)
      {
         if (_datatable.Rows[rowIndex][column] is DBNull)
            return float.NaN;

         return _datatable.Rows[rowIndex][column].ConvertedTo<float>();
      }

      private CellValueChanged mapFrom(CellValueChangedDTO dto)
      {
         var columnId = GetColumnIdFromColumnIndex(dto.ColumnIndex);
         var column = _observedData[columnId];
         return new CellValueChanged
         {
            ColumnId = columnId,
            RowIndex = dto.RowIndex,
            OldValue = valueInBaseUnit(dto.OldDisplayValue, column),
            NewValue = valueInBaseUnit(dto.NewDisplayValue, column),
         };
      }

      private static float valueInBaseUnit(float valueInDisplayUnit, OSPSuite.Core.Domain.Data.DataColumn column)
      {
         return Convert.ToSingle(column.ConvertToBaseUnit(valueInDisplayUnit));
      }

      public void ChangeUnit(int columnIndex, Unit newUnit)
      {
         var columnId = GetColumnIdFromColumnIndex(columnIndex);
         AddCommand(_editObservedDataTask.SetUnit(_observedData, columnId, newUnit));
      }

      public IEnumerable<Unit> AvailableUnitsFor(int columnIndex)
      {
         var col = columnFromColumnIndex(columnIndex);
         return col != null ? col.Dimension.Units : Enumerable.Empty<Unit>();
      }

      public Unit DisplayUnitFor(int columnIndex)
      {
         var col = columnFromColumnIndex(columnIndex);
         return col?.DisplayUnit;
      }

      private OSPSuite.Core.Domain.Data.DataColumn columnFromColumnIndex(int columnIndex)
      {
         var id = GetColumnIdFromColumnIndex(columnIndex);
         return _observedData.Contains(id) ? _observedData[id] : null;
      }
   }
}