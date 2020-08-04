using System;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.Export
{
   public class HistoryExportTask : IHistoryExportTask
   {
      public void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions)
      {
         var dataTable = historyManager.ToDataTable();
         dataTable.TableName = reportOptions.SheetName;

         ExportToExcelTask.ExportDataTableToExcel(dataTable,reportOptions.ReportFullPath, true);

/*
         var workBook = new XSSFWorkbook();
         var sheet = workBook.CreateSheet(reportOptions.SheetName);

         var rowCount = dataTable.Rows.Count;
         var columnCount = dataTable.Columns.Count;

         var row = sheet.CreateRow(0);
         for (var c = 0; c < columnCount; c++)
         {
            var cell = row.CreateCell(c);
            cell.SetCellValue(dataTable.Columns[c].ColumnName);
            sheet.AutoSizeColumn(c); //this should possibly be done right in the end
         }

         for (var i = 0; i < rowCount; i++)
         {
            row = sheet.CreateRow(i + 1);
            for (var j = 0; j < columnCount; j++)
            {
               var cell = row.CreateCell(j);

               if ((dataTable.Columns[j].DataType.GetType() == typeof(decimal)) && dataTable.Rows[i][j] != DBNull.Value)
               {
                  cell.SetCellType(CellType.Numeric);
                  //test

                  cell.SetCellValue((double)dataTable.Rows[i][j]); //could need to change
               }
               else
                  cell.SetCellValue(dataTable.Rows[i][j].ToString());
            }
         }

         for (var c = 0; c < columnCount; c++)
         {
            sheet.AutoSizeColumn(c); //this should possibly be done right in the end
         }


         ExportToExcelTask.SaveWorkbook(reportOptions.ReportFullPath, workBook);

         if (reportOptions.OpenReport)
            FileHelper.TryOpenFile(reportOptions.ReportFullPath);
         */
      }
   }
}