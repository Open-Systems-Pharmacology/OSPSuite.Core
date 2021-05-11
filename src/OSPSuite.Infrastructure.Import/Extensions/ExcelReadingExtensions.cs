using System.Collections.Generic;
using System.Data;
using NPOI.SS.UserModel;

namespace OSPSuite.Infrastructure.Import.Extensions
{
   public static class ExcelReadingExtensions
   {
      /// <summary>
      /// Adds a list of values as a row to a DataTable
      /// </summary>
      /// <param name="rowValues">The values to be added to the DataTable</param>
      public static void AddRowToDataTable(this DataTable dataTable, IReadOnlyList<string> rowValues)
      {
         var dataRow = dataTable.NewRow();
         var i = 0; //IS THIS ZERO ACTUALLY RIGHT????
         foreach (var cellContent in rowValues)
         {
            dataRow[i] = cellContent;
         }

         dataTable.Rows.Add(dataRow);
      }
   }
}