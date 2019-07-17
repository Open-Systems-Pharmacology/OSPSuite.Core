using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using SmartXLS;

namespace OSPSuite.Presentation.Services
{
   public class Importer
   {
      private readonly IImportDataTableToDataRepositoryMapper _dataRepositoryMapper;
      private readonly IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly IImporterTask _importerTask;

      private readonly Cache<string, DataTable> _excelTableCache = new Cache<string, DataTable>();
      private readonly IDialogCreator _dialogCreator;

      public Importer(IImportDataTableToDataRepositoryMapper dataRepositoryMapper, IReadOnlyList<ColumnInfo> columnInfos, IImporterTask importerTask, IDialogCreator dialogCreator)
      {
         _dataRepositoryMapper = dataRepositoryMapper;
         _columnInfos = columnInfos;
         _importerTask = importerTask;
         _dialogCreator = dialogCreator;
      }

      /// <summary>
      ///    This clears the cache of read excel sheets.
      /// </summary>
      public void ClearCache()
      {
         _excelTableCache.Clear();
      }

      /// <summary>
      ///    This is the method for importing one sheet of an excel file.
      /// </summary>
      /// <param name="dataTableTemplate">Object with all specification information.</param>
      /// <param name="fileName">Name of the excel file containing the data.</param>
      /// <param name="sheetName">Name of the sheet of that excel file containing the data.</param>
      /// <param name="captionRow">Row where the column captions are stored.</param>
      /// <param name="unitRow">Row where the unit information is stored.</param>
      /// <param name="dataStartRow">Row where the data begins.</param>
      /// <param name="dataEndRow">Row where the data ends. If value is <c>-1</c> all rows will be read until end.</param>
      /// <param name="mapping">Mapping information which identifies all needed columns in the excel file.</param>
      /// <remarks>
      ///    <para>
      ///       If <paramref name="unitRow" /> is equal to <paramref name="captionRow" /> the unit information is searched in
      ///       the excel column captions. It is assumed that the unit is enclosed in scare brakets.
      ///    </para>
      /// </remarks>
      /// <exception cref="DifferentDataTypeException"> is thrown when there is a mapped data column with not matching data type.</exception>
      /// <returns>A list of filled import data table objects.</returns>
      public IList<ImportDataTable> ImportDataTables(ImportDataTable dataTableTemplate, string fileName, string sheetName, int captionRow, int unitRow, int dataStartRow, int dataEndRow, IList<ColumnMapping> mapping)
      {
         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            _importerTask.SelectSheet(fileName, wb, sheetName);

            using (var excelTable = GetExcelTable(wb, captionRow, unitRow, dataStartRow, dataEndRow, dataTableTemplate, mapping))
            {
               wb.unLock();
               return _importerTask.GetDataTables(fileName, sheetName, dataTableTemplate, mapping, excelTable);
            }
         }
      }

      /// <summary>
      ///    This is the method for importing one sheet of an excel file.
      /// </summary>
      /// <param name="dataTableTemplate">Object with all specification information.</param>
      /// <param name="fileName">Name of the excel file containing the data.</param>
      /// <param name="sheetName">Name of the sheet of that excel file containing the data.</param>
      /// <param name="mapping">Mapping information which identifies all needed columns in the excel file.</param>
      /// <param name="rangesCache">The cache of allowable ranges</param>
      /// <remarks>
      ///    <para>The rows containing captions and unit information are determined automatically.</para>
      ///    <para>If the unit information is within the captions it is assumed that the units are enclosed in scare brakets.</para>
      ///    <para>The first data row and the last data row are also determined automatically.</para>
      /// </remarks>
      /// <exception cref="DifferentDataTypeException"> is thrown when there is a mapped data column with not matching data type.</exception>
      /// <returns>A list of filled import data table objects.</returns>
      public IList<ImportDataTable> ImportDataTables(ImportDataTable dataTableTemplate, string fileName, string sheetName, IList<ColumnMapping> mapping, Cache<string, Rectangle> rangesCache = null)
      {
         IList<ImportDataTable> tables = null;

         Action<DataTable> action = excelTable => { tables = _importerTask.GetDataTables(fileName, sheetName, dataTableTemplate, mapping, excelTable); };

         runActionOnExcelTable(dataTableTemplate, fileName, sheetName, mapping, action, rangesCache);

         checkTablesCompatibilityWithRepository(tables);

         return tables;
      }

      /// <summary>
      ///    Counts the number of data tables that will be imported with the given configuration
      /// </summary>
      /// <param name="dataTableTemplate">The template ImportDataTable</param>
      /// <param name="fileName">The filename of the excel file</param>
      /// <param name="sheetName">The sheetname of the excel sheet</param>
      /// <param name="mapping">The current column mapping configuration</param>
      /// <param name="rangesCache">The cache of allowable ranges</param>
      /// <returns>The number of data tables that would be imported for the given configuration</returns>
      public int CountDataTables(ImportDataTable dataTableTemplate, string fileName, string sheetName, IList<ColumnMapping> mapping, Cache<string, Rectangle> rangesCache)
      {
         var count = 0;
         Action<DataTable> action = excelTable => count = _importerTask.CountDataTables(fileName, sheetName, dataTableTemplate, mapping, excelTable);

         runActionOnExcelTable(dataTableTemplate, fileName, sheetName, mapping, action, rangesCache);

         return count;
      }

      /// <summary>
      ///    This method checks whether the imported data could be converted to data repositories.
      /// </summary>
      /// <remarks>
      ///    This is needed to avoid errors later when the imported data is converted for chart display.
      ///    When the charts have been introduced it has been decided to use data repositories.
      ///    Now the component is not generic anymore.
      /// </remarks>
      /// <param name="tables"></param>
      private void checkTablesCompatibilityWithRepository(IEnumerable<ImportDataTable> tables)
      {
         //check whether tables can be converted to DataRepositories
         //if not an error is thrown
         var error = false;
         tables.Each(t => _dataRepositoryMapper.ConvertImportDataTable(t, _columnInfos, out error));
      }

      private void runActionOnExcelTable(ImportDataTable dataTableTemplate, string fileName, string sheetName, IList<ColumnMapping> mapping, Action<DataTable> action, Cache<string, Rectangle> rangesCache)
      {
         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            _importerTask.SelectSheet(fileName, wb, sheetName);
            int dataStartRow, dataEndRow, unitRow, captionRow;
            if (rangesCache != null && rangesCache.Contains(sheetName))
            {
               var range = rangesCache[sheetName];

               dataEndRow = range.Y + range.Height - 1;
               unitRow = _importerTask.GetUnitRowGuess(wb, range);
               captionRow = _importerTask.GetCaptionRowGuess(wb, range);
               dataStartRow = Math.Max(range.Y, captionRow + 1);
            }
            else
            {
               dataStartRow = _importerTask.GetFirstDataRowGuess(wb);
               dataEndRow = -1;
               unitRow = _importerTask.GetUnitRowGuess(wb);
               captionRow = _importerTask.GetCaptionRowGuess(wb);
            }

            using (var excelTable = GetExcelTable(wb, captionRow, unitRow, dataStartRow, dataEndRow, dataTableTemplate, mapping))
            {
               wb.unLock();

               action(excelTable);
            }
         }
      }

      /// <summary>
      ///    This function retrieves a list of all sheet names used in the given excel file.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <returns>A list containing all sheet names of the excel file.</returns>
      public IList<string> GetSheetNames(string fileName)
      {
         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            var retVal = _importerTask.GetSheetNames(wb);
            wb.unLock();
            return retVal;
         }
      }

      /// <summary>
      ///    This function retrieves a list of all columns used in the given sheet of the excel file.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="sheetName">Name of the sheet.</param>
      /// <param name="captionRow">Row where the column captions are stored.</param>
      /// <returns>A list containing all column names used in the given sheet of the excel file.</returns>
      public IList<string> GetColumnNames(string fileName, string sheetName, int captionRow)
      {
         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            _importerTask.SelectSheet(fileName, wb, sheetName);
            var retVal = _importerTask.GetColumnNames(wb, captionRow);
            wb.unLock();
            return retVal;
         }
      }

      private static string getKey(WorkBook wb, int captionRow, int unitRow, int startColumn, int endColumn, int startRow, int endRow)
      {
         if (endRow == -1 || endRow > wb.LastRow)
            endRow = wb.LastRow;
         return $"{wb.Sheet}.{captionRow}.{unitRow}.{startColumn}.{endColumn}.{startRow}.{endRow}";
      }

      private DataTable getSheetTable(WorkBook wb, int captionRow, int unitRow, int firstColumn, int lastColumn, int firstRow, int lastRow)
      {
         DataTable sheetTable;
         var key = getKey(wb, captionRow, unitRow, firstColumn, lastColumn, firstRow, lastRow);
         if (_excelTableCache.Contains(key))
            sheetTable = _excelTableCache[key];
         else
         {
            sheetTable = _importerTask.ExportDataTable(wb, firstColumn, (lastColumn - firstColumn) + 1, firstRow, (lastRow - firstRow) + 1, captionRow, unitRow);
            _excelTableCache.Add(key, sheetTable);
         }

         return sheetTable;
      }

      /// <summary>
      ///    This methods reads the table data from a given workbook.
      /// </summary>
      /// <param name="wb">An open excel workbook set to a single sheet.</param>
      /// <param name="captionRow">Number of row containing captions.</param>
      /// <param name="unitRow">Number of row containing unit information.</param>
      /// <param name="dataStartRow">Number of row where the data starts.</param>
      /// <param name="dataEndRow">Number of row where the import should end or the data ends.</param>
      /// <param name="dataTableTemplate">Specification of need table.</param>
      /// <param name="mapping">Mapping between excel columns and needed table column.</param>
      /// <remarks>
      ///    <para>There are some checks done against the given import data table specification.</para>
      ///    <para>All required data column must be mapped.</para>
      ///    <para>The unit of the excel column must be supported by the import data column.</para>
      ///    <para>Only mapped excel column are taken into the new data table.</para>
      /// </remarks>
      /// <returns>A data table containing the data of the excel sheet.</returns>
      public DataTable GetExcelTable(WorkBook wb, int captionRow, int unitRow, int dataStartRow, int dataEndRow, ImportDataTable dataTableTemplate, IList<ColumnMapping> mapping)
      {
         //check arguments
         if (dataEndRow == -1 || dataEndRow > wb.LastRow) dataEndRow = wb.LastRow;
         if (captionRow < 0 || captionRow > wb.LastRow) throw new OSPSuiteException(Error.CaptionRowOutOfRange(captionRow, wb.LastRow, wb.GetCurrentSheetName()));
         if (unitRow == -1) unitRow = captionRow;
         if (unitRow < 0 || unitRow > wb.LastRow) throw new OSPSuiteException(Error.UnitRowOutOfRange(unitRow, wb.LastRow, wb.GetCurrentSheetName()));
         if (dataStartRow < 0 || dataStartRow > wb.LastRow) throw new OSPSuiteException(Error.FirstDataRowOutOfRange(dataStartRow, wb.LastRow, wb.GetCurrentSheetName()));
         if (dataEndRow < dataStartRow) throw new OSPSuiteException(Error.LastDataRowLessThanFirstDataRow(dataEndRow, dataStartRow, wb.GetCurrentSheetName()));

         //check that for each table column there is a mapping to an excel column.
         _importerTask.CheckWhetherAllDataColumnsAreMapped(dataTableTemplate.Columns, mapping);

         //read the excel captions and check whether all mapped columns are available.
         var columnNames = _importerTask.GetColumnNames(wb, captionRow);
         _importerTask.CheckWhetherMappedExcelColumnExist(mapping, columnNames);

         //read the units of the excel file 
         IList<string> units = (unitRow == captionRow) ? _importerTask.GetUnits(columnNames) : _importerTask.GetUnits(wb, unitRow);

         //check whether the units of all mapped columns are supported.
         _importerTask.CheckUnitSupportForAllMappedColumns(dataTableTemplate.Columns, mapping, columnNames, units);

         //read the excel file
         var excelTable = getSheetTable(wb, captionRow, unitRow, 0, wb.LastCol + 1, dataStartRow, dataEndRow);

         //check data types of mapped columns
         _importerTask.CheckDataTypes(dataTableTemplate.Columns, mapping, excelTable.Columns);
         return excelTable;
      }

      /// <summary>
      ///    This method retrieves a <see cref="DataSet" /> containing a <see cref="DataTable" /> for each excel sheet with first
      ///    rows filled.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="rangesCache">A cache of the allowable ranges for sheets in the file</param>
      /// <returns>A <see cref="DataSet" /> object containing a <see cref="DataTable" /> for each sheet of the excel file.</returns>
      public DataSet GetPreview(string fileName, Cache<string, Rectangle> rangesCache)
      {
         const int previewerMaxRows = 1000;
         return GetPreview(fileName, previewerMaxRows, rangesCache);
      }

      /// <summary>
      ///    This method retrieves a <see cref="DataSet" /> containing a <see cref="DataTable" /> for each excel sheet with first
      ///    rows filled.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="maxRows">Number of rows to be preview.</param>
      /// <param name="rangesCache">A cache of the allowable ranges for sheets in the file</param>
      /// <returns>A <see cref="DataSet" /> object containing a <see cref="DataTable" /> for each sheet of the excel file.</returns>
      public DataSet GetPreview(string fileName, int maxRows, Cache<string, Rectangle> rangesCache)
      {
         if (maxRows <= 0) throw new ArgumentOutOfRangeException(nameof(maxRows));

         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            if (wb == null) return null;
            var retValue = new DataSet(fileName);
            var errorCounter = 0;
            for (var i = 0; i < wb.NumSheets; i++)
            {
               try
               {
                  wb.Sheet = i;

                  DataTable sheetTable;
                  if (!rangesCache.Keys.Contains(wb.getSheetName(wb.Sheet)))
                  {
                     var unitRow = _importerTask.GetUnitRowGuess(wb);
                     var firstRow = _importerTask.GetFirstDataRowGuess(wb);
                     var captionRow = _importerTask.GetCaptionRowGuess(wb);
                     sheetTable = getSheetTable(wb, captionRow, unitRow, 0, wb.LastCol, firstRow, Math.Min(firstRow + maxRows - 1, wb.LastRow));
                  }
                  else
                  {
                     var range = rangesCache[wb.getSheetName(wb.Sheet)];
                     var unitRow = _importerTask.GetUnitRowGuess(wb, range);
                     var captionRow = _importerTask.GetCaptionRowGuess(wb, range);
                     sheetTable = getSheetTable(wb, captionRow, unitRow, range.X, range.X + range.Width - 1, range.Y, range.Y + range.Height - 1);
                  }

                  foreach (DataColumn col in sheetTable.Columns)
                     col.ReadOnly = true;
                  retValue.Tables.Add(sheetTable.Copy());
               }
               catch
               {
                  errorCounter++;
               }
            }

            if (errorCounter > 0)
               _dialogCreator.MessageBoxError($"The preview could not be generated for {errorCounter} sheet(s).");

            wb.unLock();
            return retValue;
         }
      }

      /// <summary>
      ///    This method should give an educated guess for the caption row.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="sheetName">Name of the sheet.</param>
      /// <returns>The number of the caption row.</returns>
      public int GetCaptionRowGuess(string fileName, string sheetName)
      {
         return getRowGuess(fileName, sheetName, _importerTask.GetCaptionRowGuess);
      }

      /// <summary>
      ///    This method should give an educated guess for the unit row.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="sheetName">Name of the sheet.</param>
      /// <returns>The number of the unit row.</returns>
      public int GetUnitRowGuess(string fileName, string sheetName)
      {
         return getRowGuess(fileName, sheetName, _importerTask.GetUnitRowGuess);
      }

      /// <summary>
      ///    This method should give an educated guess for the data starting row.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <param name="sheetName">Name of the sheet.</param>
      /// <returns>The number of the first row with real data in it.</returns>
      public int GetFirstDataRowGuess(string fileName, string sheetName)
      {
         return getRowGuess(fileName, sheetName, _importerTask.GetFirstDataRowGuess);
      }

      private int getRowGuess(string fileName, string sheetName, Function<int, WorkBook> function)
      {
         using (var wb = _importerTask.ReadExcelFile(fileName))
         {
            _importerTask.SelectSheet(fileName, wb, sheetName);
            var retVal = function(wb);
            wb.unLock();
            return retVal;
         }
      }
   }
}