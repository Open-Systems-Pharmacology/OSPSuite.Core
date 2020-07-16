using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core
{
   //THIS IS NOT STRING, WE HAVE TO DO IT GENERIC
   public interface IUnformattedData
   {
      IList<string> GetRow(int index);
      IList<string> GetColumn(int columnIndex);
      IList<string> GetColumn(string columnName);
      Dictionary<string, ColumnDescription> Headers { get; set; }

      IList<IList<string>> GetRows(Func<List<string>, bool> filter);

      bool AddRow(List<string> row);
      void AddColumn(string columnName, int columnIndex);
   }

   public class UnformattedData : IUnformattedData
   {
      private readonly List<List<string>> rawDataTable = new List<List<string>>();

      //make setter private in class!!!
      public Dictionary<string, ColumnDescription> Headers { get; set; } = new Dictionary<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
                                                                //the list of headers is somehow the definition of the table
      {
         Headers.Add(columnName, new ColumnDescription(columnIndex));
      }
      public void CalculateColumnDescription( List<ColumnDescription.MeasurmentLevel> levels)
      {
         foreach (KeyValuePair<string, ColumnDescription> header in Headers)
         {
            if (header.Value.Level == ColumnDescription.MeasurmentLevel.NOT_SET) //hell, we could even not check here
               header.Value.Level = levels[header.Value.Index];
         }
      }

      public bool AddRow(List<string> row)
      {                                   //the not empty row part we could check explicitly
         if (Headers.Count == row.Count) //I suppose row.Count != 0, so we do not add Data to a DataSheet without column names
         {
            rawDataTable.Add(row);

            foreach (KeyValuePair<string, ColumnDescription> header in Headers)
            {
               if (header.Value.Level == ColumnDescription.MeasurmentLevel.DISCRETE)
               {
                  if (header.Value.ExistingValues.Contains(row[header.Value.Index]))
                     header.Value.ExistingValues.Add(row[header.Value.Index]);
               }
            }
            return true;
         }

         return false;
      }

      public IList<string> GetColumn(int columnIndex)
      {
         var resultList = new List<string>();

         for (int i = 0; i < rawDataTable.Count; i++) //should have at least one row
            resultList.Add(rawDataTable[i][columnIndex]); //make sure the 2 indexes are correctly positioned here

         return resultList;
      }

      public IList<string> GetColumn(string columnName)
      {
         var columnIndex = Headers[columnName].Index;

         var resultColumn = new List<string>();

         for (int i = 0; i < rawDataTable.Count; i++)
            resultColumn.Add(rawDataTable[i][columnIndex]);

         return resultColumn;
      }

      public IList<string> getHeadersList()
      {
         return Headers.Keys.ToList();
      }

      public IList<string> GetRow(int index)
      {
         return rawDataTable[index];
      }

      public IList<IList<string>> GetRows(Func<List<string>, bool> filter)
      {
         return (IList<IList<string>>)rawDataTable.Where(filter).ToList();
      }
   }
}
