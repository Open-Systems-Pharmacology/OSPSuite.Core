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
   public interface IUnformattedData
   {
      IEnumerable<string> GetColumn(string columnName);
      ColumnDescription GetColumnDescription(string columnName);
      IEnumerable<string> GetHeaders();
      string GetCell(string columnName, int rowIndex);
      IEnumerable<UnformattedRow> GetRows(Func<IEnumerable<string>, bool> filter);

      void AddRow(IEnumerable<string> row);
      void AddColumn(string columnName, int columnIndex);
      DataTable AsDataTable();
      UnformattedDataRow GetDataRow(int index);
      void RemoveEmptyColumns();
      void RemoveEmptyRows();
   }

   public class UnformattedData : IUnformattedData
   {
      private readonly List<List<string>> _rawDataTable = new List<List<string>>();
      private List<string> _emptyColumns = new List<string>();

      protected Cache<string, ColumnDescription> _headers =
         new Cache<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

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

      public void AddRow( IEnumerable<string> row)
      {
         var rowList = row.ToList();

         if (_headers.Count > rowList.Count)
         {
            for ( var i = rowList.Count; i < _headers.Count; i++  )
               rowList.Add("");
         }
         if (rowList.Count > _headers.Count)
         {
            rowList = rowList.GetRange(0, _headers.Count);
         }

         _rawDataTable.Add(rowList);

         foreach (var header in _headers)
         {
            if (header.Level != ColumnDescription.MeasurementLevel.Discrete) continue;
            if (!header.ExistingValues.Contains(rowList.ElementAt(header.Index)))
               header.ExistingValues.Add(rowList.ElementAt(header.Index));
         }
      }

      public IEnumerable<string> GetColumn(string columnName)
      {
         return getColumn(_headers[columnName].Index);
      }

      public IEnumerable<UnformattedRow> GetRows(Func<IEnumerable<string>, bool> filter)
      {
         return _rawDataTable.Select((data, index) => new UnformattedRow(index, data)).Where(row => filter(row.Data));
      }

      public DataTable AsDataTable()
      {
         var resultTable = new DataTable();
         var indexList = _headers.Select( h => h.Index);

         // Add columns.
         foreach (var header in _headers.Keys)
         {
            resultTable.AddColumn(header, typeof(string));
         }

         Func<IEnumerable<string>, IEnumerable<string>> maskFunction;

         if (_headers.Count != _rawDataTable.First().Count)
         {
            maskFunction = (inputList) => inputList.Where((v, i) => indexList.Contains(i));
         }
         else
         {
            maskFunction = (inputList) => inputList;
         }

         foreach (var itemList in _rawDataTable)
         {
            // ReSharper disable once CoVariantArrayConversion
            resultTable.Rows.Add(maskFunction(itemList).ToArray());
         }

         return resultTable;
      }

      public string GetCell(string columnName, int rowIndex)
      {
         return new UnformattedDataRow(_rawDataTable[rowIndex], _headers).GetCellValue(columnName);
      }

      public IEnumerable<string> GetHeaders()
      {
         return _headers.Keys;
      }

      public ColumnDescription GetColumnDescription(string columnName)
      {
         return _headers[columnName];
      }

      public UnformattedDataRow GetDataRow(int index)
      {
         return new UnformattedDataRow(_rawDataTable[index], _headers);
      }

      public void RemoveEmptyColumns()
      {
         foreach (var headerName in _emptyColumns)
         {
            _headers.Remove(headerName);
         }
      }

      public void RemoveEmptyRows()
      {
         for (var i = _rawDataTable.Count -1; i >= 0; i--)
         {
            if (_rawDataTable[i].TrueForAll(IsEmpty))
               _rawDataTable.RemoveAt(i);
            else
               break;
         }
      }

      private static bool IsEmpty(string s)
      {
         return s.IsNullOrEmpty();
      }

   }
}