using System.Collections.Generic;
using System.Data;

namespace OSPSuite.Core.Services
{
   /// <summary>
   /// Simple Wrapper over static function defined in ExportToExcelTask
   /// </summary>
   public interface IExportDataTableToExcelTask
   {
      void ExportDataTableToExcel(DataTable dataTable, string fileName, bool openExcel);
      void ExportDataTablesToExcel(IEnumerable<DataTable> dataTables, string fileName, bool openExcel);
   }
}