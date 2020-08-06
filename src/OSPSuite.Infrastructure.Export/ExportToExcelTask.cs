using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OSPSuite.Utility;

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
      public static void ExportDataTableToExcel(DataTable dataTable, string fileName, bool openExcel)
      {
         ExportDataTablesToExcel(new[] {dataTable}, fileName, openExcel);
      }

      /// <summary>
      ///    Exports the given dataTables to the file given as parameter. One sheet will be created per table
      /// </summary>
      /// <param name="dataTables">Data Tables to export</param>
      /// <param name="fileName">Target file</param>
      /// <param name="openExcel">If set to true, excel will be launched with the exported file</param>
      public static void ExportDataTablesToExcel(IEnumerable<DataTable> dataTables, string fileName, bool openExcel)
      {
         var tables = dataTables.ToList();
         var workBookConfiguration = new WorkbookConfiguration();
         workBookConfiguration.SetHeadersBold();

         for (var i = 0; i < tables.Count(); i++)
         {
            var dataTable = tables.ElementAt(i);
            exportDataTableToWorkBook(workBookConfiguration, dataTable);
         }

         saveWorkbook(fileName, workBookConfiguration.WorkBook);

         if (openExcel)
            FileHelper.TryOpenFile(fileName);
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

      private static void exportDataTableToWorkBook(IWorkbookConfiguration workBookConfiguration, DataTable dataTable)
      {
         // Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";

         var workBook = workBookConfiguration.WorkBook;

         ISheet sheet;

         if ( !dataTable.TableName.Equals(""))
         {
            sheet = workBook.CreateSheet(dataTable.TableName);
         }
         else
         {
            sheet = workBook.CreateSheet("Sheet1");
         }

         var rowCount = dataTable.Rows.Count;
         var columnCount = dataTable.Columns.Count;

         var row = sheet.CreateRow(0);

         for (var c = 0; c < columnCount; c++)
         {
            var cell = row.CreateCell(c);
            cell.SetCellValue(dataTable.Columns[c].ColumnName);
            cell.CellStyle = workBookConfiguration.HeadersStyle;
         }

 //        row.RowStyle = style;

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

         for (var c = 0; c < columnCount; c++)
         {
            sheet.AutoSizeColumn(c); 
         }
      }
   }
}