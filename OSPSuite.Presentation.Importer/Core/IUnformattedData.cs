using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IUnformattedData
   {
      IEnumerable<string> GetColumn(string columnName);
      ColumnDescription GetColumnDescription(string columnName);
      IEnumerable<string> GetHeaders();
      string GetCell(string columnName, int rowIndex);
      IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter);

      void AddRow(IEnumerable<string> row);
      void AddColumn(string columnName, int columnIndex);
      DataTable AsDataTable();
      UnformattedDataRow GetDataRow(int index);
   }

   public class UnformattedData : IUnformattedData
   {
      private readonly List<List<string>> _rawDataTable = new List<List<string>>();

      protected Cache<string, ColumnDescription> _headers =
         new Cache<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

      private IEnumerable<string> getColumn(int columnIndex)
      {
         return _rawDataTable.Select(column => column[columnIndex]).ToList();
      }

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
         //the list of headers is somehow the definition of the table
      {
         if (_headers.Keys.Contains(columnName))
            columnName = Guid.NewGuid().ToString();
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

         //could it actually 
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

      public IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter)
      {
         return _rawDataTable.Where(filter);
      }

      public DataTable AsDataTable()
      {
         var resultTable = new DataTable();

         // Add columns.
         foreach (var header in _headers.Keys)
         {
            resultTable.Columns.Add(header, typeof(string));
         }

         foreach (var itemList in _rawDataTable)
         {
            resultTable.Rows.Add(itemList.ToArray()); //TODO Resharper
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
   }
}