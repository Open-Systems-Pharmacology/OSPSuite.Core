using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using System;

namespace OSPSuite.Infrastructure.Export
{
   public class HistoryExportTask : IHistoryExportTask
   {
      public void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions)
      {
         var dataTable = historyManager.ToDataTable();
         dataTable.TableName = reportOptions.SheetName;

         if (reportOptions.ReportFullPath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            dataTable.ExportToCSV(reportOptions.ReportFullPath);
         else
            ExportToExcelTask.ExportDataTableToExcel(dataTable, reportOptions.ReportFullPath, true);
      }
   }
}