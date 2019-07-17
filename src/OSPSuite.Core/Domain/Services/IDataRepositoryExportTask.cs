using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.UnitSystem;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Core.Domain.Services
{
   public class DataColumnExportOptions
   {
      /// <summary>
      ///    Defined the column header to use when exporting the column. If not defined, the name of the column will be used.
      /// </summary>
      public Func<DataColumn, string> ColumnNameRetriever { get; set; } = x => x.Name;

      /// <summary>
      ///    Defined the dimension used for the export of the column. If not defined, the dimension of the column will be used
      /// </summary>
      public Func<DataColumn, IDimension> DimensionRetriever { get; set; } = x => x.Dimension;

      /// <summary>
      ///    If set to <c>true</c> (default), the values will be exported using the display unit of the <see cref="DataColumn" />
      /// </summary>
      public bool UseDisplayUnit { get; set; } = true;

      /// <summary>
      ///    If set to <c>true</c> (default is <c>false</c>), the value will be
      ///    formatted using the number of decimal  places defined in the application
      /// </summary>
      public bool FormatOutput { get; set; } = false;

      /// <summary>
      ///    If set to <c>true</c> (default is <c>false</c>), the exported columns will have the type set to object instead of
      ///    float.
      ///    This is required when using the resulting DataTable for direct binding in the UI
      /// </summary>
      public bool ForceColumnTypeAsObject { get; set; } = false;
   }

   public interface IDataRepositoryExportTask
   {
      /// <summary>
      ///    Returns one datable for each base grid and associated columns representing the content of the repository (one column
      ///    for each DataColumn)
      /// </summary>
      IReadOnlyList<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, DataColumnExportOptions exportOptions = null);

      /// <summary>
      ///    Export the given data repository to excel using the ToDataTable function
      /// </summary>
      void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel = true, DataColumnExportOptions exportOptions = null);

      /// <summary>
      ///    Export the given data tables to excel
      /// </summary>
      void ExportToExcel(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel = true);

      /// <summary>
      ///    Export the given data repository to excel using the ToDataTable function
      /// </summary>
      Task ExportToExcelAsync(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel = true, DataColumnExportOptions exportOptions = null);

      /// <summary>
      ///    Export the given data tables to excel
      /// </summary>
      Task ExportToExcelAsync(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel = true);

      /// <summary>
      ///    Export the given data repository to excel using the ToDataTable function
      /// </summary>
      Task ExportToCsvAsync(IEnumerable<DataColumn> dataColumns, string fileName, DataColumnExportOptions exportOptions = null);
   }
}