using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain.Data;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Core.Domain.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository Clone(DataRepository dataRepositoryToClone);

      /// <summary>
      ///    Returns one datable for each base grid and associated columns representing the content of the repository (one column
      ///    for each DataColumn)
      ///    The column headers will be set to the column name();
      ///    If the flag <paramref name="formatOutput" /> is set to true, the value will be formatted using the number of decimal
      ///    places defined in the application
      /// </summary>
      IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, bool formatOutput = false);

      /// <summary>
      ///    Returns one datable for each base grid and associated columns representing the content of the repository (one column
      ///    for each DataColumn)
      ///    The column header will be defined using the <paramref name="columnNameRetriever" /> given as parameter.
      ///    If the flag <paramref name="formatOutput" /> is set to true, the value will be formatted using the number of decimal
      ///    places defined in the application
      /// </summary>
      IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, Func<DataColumn, string> columnNameRetriever, bool formatOutput = false);

      /// <summary>
      ///    Export the given data repository to excel using the ToDataTable function
      /// </summary>
      void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel = true);

      /// <summary>
      ///    Export the given data repository to excel using the ToDataTable function
      /// </summary>
      void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, Func<DataColumn, string> columnNameRetriever, bool launchExcel = true);

      /// <summary>
      ///    Export the given data tables to excel
      /// </summary>
      void ExportToExcel(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel = true);

      BaseGrid CloneBaseGrid(DataColumn baseGridToClone);
      DataColumn CloneColumn(DataColumn sourceColumn, BaseGrid clonedBaseGrid);
   }
}