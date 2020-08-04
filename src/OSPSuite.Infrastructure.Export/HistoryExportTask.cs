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
      }
   }
}