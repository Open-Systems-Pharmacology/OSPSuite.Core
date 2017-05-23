using OSPSuite.Utility;
using SmartXLS;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Services
{
   internal static class SheetIndex
   {
      public static int Report;
   }

   public class ReportTask : IReportTask
   {
      public void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions)
      {
         var dataTable = historyManager.ToDataTable();

         using (var workBook = new WorkBook())
         {
            //add new sheet where the report will be written 
            workBook.insertSheets(SheetIndex.Report, 1);

            workBook.ImportDataTable(dataTable, true, 0, 0, dataTable.Rows.Count, dataTable.Columns.Count);

            for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
            {
               workBook.setColWidthAutoSize(colIndex, true);
            }

            workBook.setSheetName(SheetIndex.Report, reportOptions.SheetName);

            ExportToExcelTask.SaveWorkbook(reportOptions.ReportFullPath, workBook);

            if (reportOptions.OpenReport)
               FileHelper.TryOpenFile(reportOptions.ReportFullPath);
         }
      }
   }
}