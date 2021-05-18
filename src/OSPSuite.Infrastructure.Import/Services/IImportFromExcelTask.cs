using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IImportFromExcelTask
   {
      /// <summary>
      /// Retrieves all the appropriate sheet names from an excel file
      /// </summary>
      /// <param name="fileName">The filename and path of the excel file</param>
      /// <param name="excludeEmptySheets">Excludes empty sheets from the list if set to true</param>
      /// <returns>The list of sheet names</returns>
      IReadOnlyList<string> RetrieveExcelSheets(string fileName, bool excludeEmptySheets = false);

      /// <summary>
      /// Gets the DataTable for the filepath, and sheet indicated
      /// </summary>
      /// <param name="filePath">The path to the excel file</param>
      /// <param name="sheetName">the sheet in the file to convert</param>
      /// <param name="firstRowAsCaption">Whether or not the first row in the sheet should be interpreted as a caption</param>
      /// <returns>Data table corresponding to the file and sheet</returns>
      DataTable GetDataTables(string filePath, string sheetName, bool firstRowAsCaption);

      /// <summary>
      /// Gets all the DataTables from the Excel workbook at the specified filePath
      /// </summary>
      /// <param name="filePath">The path to the excel file</param>
      /// <param name="firstRowAsCaption">Whether or not the first row in the sheet should be interpreted as a caption</param>
      /// <returns>The data tables corresponding to the file</returns>
      IReadOnlyList<DataTable> GetAllDataTables(string filePath, bool firstRowAsCaption);
   }

   public class ImportFromExcelTask : IImportFromExcelTask
   {
      public IReadOnlyList<string> RetrieveExcelSheets(string fileName, bool excludeEmptySheets = false)
      {
         try
         {
            var reader = new ExcelReader(fileName);
            return reader.RetrieveExcelSheets(excludeEmptySheets).ToList();
         }
         catch
         {
            return Array.Empty<string>();
         }
      }

      public IReadOnlyList<DataTable> GetAllDataTables(string filePath, bool firstRowAsCaption)
      {
         return getDataTables(filePath, string.Empty, firstRowAsCaption);
      }

      public DataTable GetDataTables(string filePath, string sheetName, bool firstRowAsCaption)
      {
         return getDataTables(filePath, sheetName, firstRowAsCaption).FirstOrDefault();
      }

      private static IReadOnlyList<DataTable> getDataTables(string fileName, string sheetName, bool firstRowAsCaption)
      {
         try
         {
            return dataTables(fileName, sheetName, firstRowAsCaption).ToList();
         }
         catch (DuplicateNameException exception)
         {
            throw new OSPSuiteException(exception.Message);
         }
      }

      private static IEnumerable<DataTable> dataTables(string fileName, string sheetName, bool firstRowAsCaption)
      {
         var reader = new ExcelReader(fileName);

         var dataTablesList = new List<DataTable>();
         while (reader.MoveToNextSheet())
         {
            if (!string.IsNullOrEmpty(sheetName) && !reader.CurrentSheet.SheetName.Equals(sheetName)) continue;
            var dataTable = new DataTable { TableName = reader.CurrentSheet.SheetName };
            if (!reader.MoveToNextRow())
            {
               dataTablesList.Add(dataTable);
               continue;
            }

            if (firstRowAsCaption)
            {
               foreach (var column in reader.CurrentRow.Select(header => new DataColumn(header)))
               {
                  dataTable.Columns.Add(column);
               }
            }
            else
               dataTable.AddRowToDataTable(reader.CurrentRow);

            while (reader.MoveToNextRow())
            {
               dataTable.AddRowToDataTable(reader.CurrentRow);
            }

            dataTablesList.Add(dataTable);
         }

         return dataTablesList;
      }
   }
}
