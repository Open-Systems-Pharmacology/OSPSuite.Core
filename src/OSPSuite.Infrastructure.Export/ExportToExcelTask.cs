using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Export
{
   public static class ExportToExcelTask
   {
      /// <summary>
      ///    Exports the given dataTable to the file given as parameter.
      /// </summary>
      /// <param name="dataTable">Data Table to export</param>
      /// <param name="fileName">Target file</param>
      /// <param name="openExcel">If set to true, excel will be launched with the exported file</param>
      /// <param name="workBookConfiguration"></param>
      public static void ExportDataTableToExcel(DataTable dataTable, string fileName, bool openExcel, IWorkbookConfiguration workBookConfiguration = null)
      {
         ExportDataTablesToExcel(new[] {dataTable}, fileName, openExcel, workBookConfiguration);
      }

      /// <summary>
      ///    Exports the given dataTables to the file given as parameter. One sheet will be created per table
      /// </summary>
      /// <param name="dataTables">Data Tables to export</param>
      /// <param name="fileName">Target file</param>
      /// <param name="openExcel">If set to true, excel will be launched with the exported file</param>
      /// <param name="workBookConfiguration"></param>
      public static void ExportDataTablesToExcel(IReadOnlyList<DataTable> dataTables, string fileName, bool openExcel, IWorkbookConfiguration workBookConfiguration = null)
      {
         var workBookConfigurationToUse = workBookConfiguration ?? new WorkbookConfiguration();
         updateTableNamesToMaxAllowedLengthAndUnicity(dataTables);

         dataTables.Each(x => exportDataTableToWorkBook(x, workBookConfigurationToUse));

         saveWorkbook(fileName, workBookConfigurationToUse.WorkBook);

         if (openExcel)
            FileHelper.TryOpenFile(fileName);
      }

      private static void updateTableNamesToMaxAllowedLengthAndUnicity(IReadOnlyList<DataTable> dataTables)
      {
         //31 is max allowed by excel, we remove some more to allow for uniqueness (we assume we won't have more than 100 number and one extra space for the separator => 4)
         const int maxTableNameLength = 31 - 4;

         //Make sure we have a name for each table
         dataTables.Where(x => string.IsNullOrEmpty(x.TableName)).ToList().Each(x => x.TableName = "Sheet");

         //Simply max the length to what's allowed and remove 2 chars to add the number if required 
         dataTables.Where(x => x.TableName.Length > maxTableNameLength).ToList()
            .Each(x => x.TableName = x.TableName.Substring(0, maxTableNameLength));

         //all names are now conformed to excel max length. But we may have doubled now. So we need to check for uniqueness
         foreach (var dataTable in dataTables)
         {
            //we cannot save the list outside of the loop
            var allOtherTableNames = dataTables.Except(new[] {dataTable}).Select(x => x.TableName).ToList();
            dataTable.TableName = ContainerTask.RetrieveUniqueName(allOtherTableNames, dataTable.TableName, canUseBaseName: true, "_");
         }
      }

      private static void saveWorkbook(string fileName, XSSFWorkbook workBook)
      {
         FileHelper.TrySaveFile(fileName, () =>
         {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
               workBook.Write(stream);
            }
         });
      }

      private static void exportDataTableToWorkBook(DataTable dataTable, IWorkbookConfiguration workBookConfiguration)
      {
         var workBook = workBookConfiguration.WorkBook;
         var sheet = workBook.CreateSheet(dataTable.TableName);
         var rowCount = dataTable.Rows.Count;
         var columnCount = dataTable.Columns.Count;

         var row = sheet.CreateRow(0);

         for (var j = 0; j < columnCount; j++)
         {
            var cell = row.CreateCell(j);
            cell.SetCellValue(dataTable.Columns[j].ColumnName);
            cell.CellStyle = workBookConfiguration.HeadersStyle;
         }

         for (var i = 0; i < rowCount; i++)
         {
            row = sheet.CreateRow(i + 1);
            for (var j = 0; j < columnCount; j++)
            {
               var cell = row.CreateCell(j);

               if (double.TryParse(dataTable.Rows[i][j].ToString(), out var value))
               {
                  cell.SetCellType(CellType.Numeric);
                  cell.SetCellValue(value);
                  cell.CellStyle = workBookConfiguration.BodyStyle;
               }
               else
               {
                  cell.SetCellValue(dataTable.Rows[i][j].ToString());
                  cell.CellStyle = workBookConfiguration.BodyStyle;
               }
            }
         }

         for (var j = 0; j < columnCount; j++)
         {
            sheet.AutoSizeColumn(j);
         }
      }
   }
}