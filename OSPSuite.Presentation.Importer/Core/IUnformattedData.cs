using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.DataProcessing;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IUnformattedData
   {
      IEnumerable<string> GetRow(int index);
      IEnumerable<string> GetColumn(int columnIndex);
      IEnumerable<string> GetColumn(string columnName);
      Dictionary<string, ColumnDescription> Headers { get; }

      IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter);

      bool AddRow(IEnumerable<string> row);
      void AddColumn(string columnName, int columnIndex);
      DataTable GetSheetAsDataTable();
   }

   public class UnformattedData : IUnformattedData
   {
      private readonly List<List<string>> _rawDataTable = new List<List<string>>();

      public Dictionary<string, ColumnDescription> Headers { get; set; } =
         new Dictionary<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
         //the list of headers is somehow the definition of the table
      {
         Headers.Add(columnName, new ColumnDescription(columnIndex));
      }

      public void CalculateColumnDescription(List<ColumnDescription.MeasurementLevel> levels)
      {
         Headers.ForEach(header =>
         {
            if (header.Value.Level == ColumnDescription.MeasurementLevel.NotSet) //hell, we could even not check here
               header.Value.Level = levels[header.Value.Index];
         });
      }

      public bool AddRow( IEnumerable<string> row)
      {
         var rowList = row.ToList();
         //the not empty row part we could check explicitly
         if (Headers.Count == rowList.Count) //I suppose row.Count != 0, so we do not add Data to a DataSheet without column names
         {
            _rawDataTable.Add(rowList);

            foreach (var header in Headers)
            {
               if (header.Value.Level == ColumnDescription.MeasurementLevel.Discrete)
               {
                  if (!header.Value.ExistingValues.Contains(rowList.ElementAt(header.Value.Index)))
                     header.Value.ExistingValues.Add(rowList.ElementAt(header.Value.Index));
               }
            }
            return true;
         }

         return false;
      }

      public IEnumerable<string> GetColumn(int columnIndex)
      {
         var resultList = new List<string>();

         //change this
         for (var i = 0; i < _rawDataTable.Count; i++)     //should have at least one row
            resultList.Add(_rawDataTable[i][columnIndex]); //make sure the 2 indexes are correctly positioned here

         return resultList;
      }

      public IEnumerable<string> GetColumn(string columnName)
      {
         var columnIndex = Headers[columnName].Index;

         var resultColumn = new List<string>();

         for (var i = 0; i < _rawDataTable.Count; i++)
            resultColumn.Add(_rawDataTable[i][columnIndex]);

         return resultColumn;
      }

      public IEnumerable<string> GetRow(int index)
      {
         return _rawDataTable[index];
      }

      public IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter)
      {
         return _rawDataTable.Where(filter);
      }

      public DataTable GetSheetAsDataTable()
      {
         var resultTable = new DataTable();

         // Add columns.
         for (var i = 0; i < Headers.Count; i++)
         {
            resultTable.Columns.Add(Headers.ElementAt(i).Key, typeof(string));
         }

         foreach (var itemList in _rawDataTable)
         {
            resultTable.Rows.Add(itemList.ToArray());
         }

         return resultTable;
      }
   }
}