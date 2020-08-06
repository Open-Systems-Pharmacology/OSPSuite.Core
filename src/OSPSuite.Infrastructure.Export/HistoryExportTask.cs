using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Export
{
   public class HistoryExportTask : IHistoryExportTask
   {
      public void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions)
      {
         var dataTable = historyManager.ToDataTable();
         dataTable.TableName = reportOptions.SheetName;

         ExportToExcelTask.ExportDataTableToExcel(dataTable,reportOptions.ReportFullPath, true);
      }
   }
}