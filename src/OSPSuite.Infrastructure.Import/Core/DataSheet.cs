using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnformattedRow
   {
      public int Index { get; private set; }
      public IEnumerable<string> Data { get; private set; }

      public UnformattedRow(int index, IEnumerable<string> data)
      {
         Index = index;
         Data = data;
      }
   }

   public class DataSheet
   {
      private readonly List<List<string>> _rawDataTable = new List<List<string>>();
      private readonly List<string> _emptyColumns = new List<string>();

      protected Cache<string, ColumnDescription> _headers =
         new Cache<string, ColumnDescription>(); //we have to ensure headers and RawSheetData sizes match

      public DataSheet(DataSheet reference)
      {
         _headers = new Cache<string, ColumnDescription>();
         foreach (var header in reference.GetHeaders())
         {
            _headers.Add(header, reference.GetColumnDescription(header));
         }

         _rawDataTable = new List<List<string>>();
         SheetName = reference.SheetName;
      }

      public DataSheet()
      {
         _emptyColumns = new List<string>();
         _headers = new Cache<string, ColumnDescription>();
         _rawDataTable = new List<List<string>>();
      }

      private IEnumerable<string> getColumn(int columnIndex)
      {
         return _rawDataTable.Select(column => column[columnIndex]).ToList();
      }

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
         //the list of headers is somehow the definition of the table
      {
         if (columnName.IsNullOrEmpty())
         {
            columnName = Guid.NewGuid().ToString();
            _emptyColumns.Add(columnName);
         }

         _headers.Add(columnName, new ColumnDescription(columnIndex));
      }

      public void CalculateColumnDescription(List<ColumnDescription.MeasurementLevel> levels)
      {
         _headers.Each(header =>
         {
            if (header.Level == ColumnDescription.MeasurementLevel.NotSet)
               header.Level = levels[header.Index];
         });
      }

      public void AddRow(IEnumerable<string> row)
      {
         var rowList = row.ToList();

         if (_headers.Count > rowList.Count)
         {
            for (var i = rowList.Count; i < _headers.Count; i++)
               rowList.Add("");
         }

         if (rowList.Count > _headers.Count)
         {
            rowList = rowList.GetRange(0, _headers.Count);
         }

         _rawDataTable.Add(rowList);

         foreach (var header in _headers)
         {
            if (!header.ExistingValues.Contains(rowList.ElementAt(header.Index)))
               header.AddExistingValues(rowList.ElementAt(header.Index));
         }
      }

      public virtual IEnumerable<string> GetColumn(string columnName)
      {
         return getColumn(_headers[columnName].Index);
      }

      public virtual IEnumerable<UnformattedRow> GetRows(Func<IEnumerable<string>, bool> filter)
      {
         return _rawDataTable.Select((data, index) => new UnformattedRow(index, data)).Where(row => filter(row.Data));
      }

      public DataTable ToDataTable()
      {
         var resultTable = new DataTable();
         var indexList = _headers.Select(h => h.Index);

         // Add columns.
         foreach (var header in _headers.Keys)
         {
            resultTable.AddColumn(header);
         }

         Func<IEnumerable<string>, IEnumerable<string>> maskFunction;

         //we filter columns that don't have a header, using the index in the cases
         //where the length of the row is longer than the headers
         if (_rawDataTable.Count > 0 && (_headers.Count != _rawDataTable.First().Count))
         {
            maskFunction = (inputList) => inputList.Where((v, i) => indexList.Contains(i));
         }
         else
         {
            maskFunction = (inputList) => inputList;
         }

         foreach (var itemList in _rawDataTable)
         {
            resultTable.Rows.Add(maskFunction(itemList).ToArray<object>());
         }

         return resultTable;
      }

      public string GetCell(string columnName, int rowIndex)
      {
         return new UnformattedDataRow(_rawDataTable[rowIndex], _headers).GetCellValue(columnName);
      }

      public virtual IEnumerable<string> GetHeaders()
      {
         return _headers.Keys;
      }

      public virtual ColumnDescription GetColumnDescription(string columnName)
      {
         return _headers.Contains(columnName) ? _headers[columnName] : null;
      }

      public UnformattedDataRow GetDataRow(int index)
      {
         return new UnformattedDataRow(_rawDataTable[index], _headers);
      }

      public void RemoveEmptyColumns()
      {
         foreach (var headerName in _emptyColumns)
         {
            var index = _headers[headerName].Index;
            foreach (var header in _headers.Where(h => h.Index > index))
            {
               header.DecrementIndex();
            }

            _headers.Remove(headerName);
            _rawDataTable.ForEach(row => row.RemoveAt(index));
         }
      }

      public void RemoveEmptyRowsAtTheEnd()
      {
         for (var i = _rawDataTable.Count - 1; i >= 0; i--)
         {
            if (_rawDataTable[i].All(x => x.IsNullOrEmpty()))
               _rawDataTable.RemoveAt(i);
            else
               break;
         }
      }

      public void RemoveEmptyRows()
      {
         for (var i = _rawDataTable.Count - 1; i >= 0; i--)
         {
            if (_rawDataTable[i].All(x => x.IsNullOrEmpty()))
               _rawDataTable.RemoveAt(i);
         }
      }

      public string SheetName { get; set; }
   }
}