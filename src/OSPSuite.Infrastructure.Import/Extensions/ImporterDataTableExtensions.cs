using System.Collections.Generic;
using System.Data;

namespace OSPSuite.Infrastructure.Import.Extensions
{
   public static class ImporterDataTableExtensions
   {
      /// <summary>
      /// Adds a list of values as a row to a DataTable
      /// </summary>
      /// <param name="dataTable">Data Table to populate</param>
      /// <param name="rowValues">The values to be added to the DataTable</param>
      public static void AddRowToDataTable(this DataTable dataTable, IReadOnlyList<string> rowValues)
      {
         var dataRow = dataTable.NewRow();
         var i = 0;
         foreach (var cellContent in rowValues)
         {
            dataRow[i] = cellContent;
            i++;
         }

         dataTable.Rows.Add(dataRow);
      }
   }
}