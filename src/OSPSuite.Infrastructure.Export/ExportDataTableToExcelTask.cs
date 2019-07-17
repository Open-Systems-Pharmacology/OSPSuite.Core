using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Export
{
   public class ExportDataTableToExcelTask : IExportDataTableToExcelTask
   {
      public void ExportDataTableToExcel(DataTable dataTable, string fileName, bool openExcel)
      {
         ExportToExcelTask.ExportDataTableToExcel(dataTable, fileName, openExcel);
      }

      public void ExportDataTablesToExcel(IEnumerable<DataTable> dataTables, string fileName, bool openExcel)
      {
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, fileName, openExcel);
      }
   }
}