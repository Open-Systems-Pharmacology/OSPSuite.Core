using System;
using System.Collections.Generic;
using System.Data;
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
         var workBook = new XSSFWorkbook();

         for (var i = 0; i < tables.Count(); i++)
         {
            var dataTable = tables.ElementAt(i);
            exportDataTableToWorkBook(workBook, dataTable);
         }

         SaveWorkbook(fileName, workBook);

         if (openExcel)
            FileHelper.TryOpenFile(fileName);
      }

      public static void SaveWorkbook(string fileName, XSSFWorkbook workBook)
      {
         FileHelper.TrySaveFile(fileName, () =>
         {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
               workBook.Write(stream);
            }
         });
      }

      private static void exportDataTableToWorkBook(IWorkbook workBook, DataTable dataTable)
      {
        // Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";

         var sheet = workBook.CreateSheet(dataTable.TableName);

         var rowCount = dataTable.Rows.Count;
         var columnCount = dataTable.Columns.Count;

         var row = sheet.CreateRow(0);

         var font = workBook.CreateFont();
         font.FontHeightInPoints = 11;
         font.FontName = "Arial";
         font.IsBold = true;

         var style = workBook.CreateCellStyle();
         style.SetFont(font);

         row.RowStyle = style;



         for (var c = 0; c < columnCount; c++)
         {
            var cell = row.CreateCell(c);
            cell.SetCellValue(dataTable.Columns[c].ColumnName);
            cell.CellStyle = style;
         }

         row.RowStyle = style;

         //font.IsBold = false; we need a new font for this. or we needto provide with the copy

         for (var i = 0; i < rowCount; i++)
         {
            row = sheet.CreateRow(i + 1);
            row.RowStyle = style;
            for (var j = 0; j < columnCount; j++)
            {
               var cell = row.CreateCell(j);

               if (double.TryParse(dataTable.Rows[i][j].ToString(), out var value)) //Thread.CurrentThread.CurrentCulture.NumberFormat
               {
                  cell.SetCellType(CellType.Numeric);
                  cell.SetCellValue(value);
                  cell.CellStyle = style;
               }
               else
               {
                  cell.SetCellValue(dataTable.Rows[i][j].ToString());
                  cell.CellStyle = style;
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