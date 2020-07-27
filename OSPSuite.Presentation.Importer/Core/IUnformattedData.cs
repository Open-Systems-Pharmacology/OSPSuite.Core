using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Utils.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IUnformattedData
   {
      IEnumerable<string> GetRow(int index);
      IEnumerable<string> GetColumn(string columnName);
      Cache<string, ColumnDescription> Headers { get; }

      IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter);

      bool AddRow(IEnumerable<string> row);
      void AddColumn(string columnName, int columnIndex);
      DataTable AsDataTable();
   }

   public class UnformattedData : IUnformattedData
   {
      private readonly List<List<string>> _rawDataTable = new List<List<string>>();

      public Cache<string, ColumnDescription> Headers { get; set; } =
         new Cache<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
         //the list of headers is somehow the definition of the table
      {
         Headers.Add(columnName, new ColumnDescription(columnIndex));
      }

      public void CalculateColumnDescription(List<ColumnDescription.MeasurementLevel> levels)
      {
         Headers.Each(header =>
         {
            if (header.Level == ColumnDescription.MeasurementLevel.NotSet) //hell, we could even not check here
               header.Level = levels[header.Index];
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
               if (header.Level == ColumnDescription.MeasurementLevel.Discrete)
               {
                  if (!header.ExistingValues.Contains(rowList.ElementAt(header.Index)))
                     header.ExistingValues.Add(rowList.ElementAt(header.Index));
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
         return GetColumn(Headers[columnName].Index);
      }

      public IEnumerable<string> GetRow(int index)
      {
         return _rawDataTable[index];
      }

      public IEnumerable<IEnumerable<string>> GetRows(Func<IEnumerable<string>, bool> filter)
      {
         return _rawDataTable.Where(filter);
      }

      public DataTable AsDataTable()
      {
         var resultTable = new DataTable();

         // Add columns.
         foreach (var header in Headers.Keys)
         {
            resultTable.Columns.Add(header, typeof(string));
         }

         foreach (var itemList in _rawDataTable)
         {
            resultTable.Rows.Add(itemList.ToArray());
         }

         return resultTable;
      }
   }
}