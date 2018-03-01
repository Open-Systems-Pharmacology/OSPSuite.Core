using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using DevExpress.Utils;
using SmartXLS;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;

namespace OSPSuite.Presentation.Services.Importer
{
   public interface IImporterTask
   {
      /// <summary>
      ///    Counts the number of data tables that would be returned for this import configuration
      /// </summary>
      /// <param name="fileName">The excel file name and path</param>
      /// <param name="sheetName">The sheet name within the excel file</param>
      /// <param name="dataTable">The template datatable</param>
      /// <param name="mapping">The current mapping configuration</param>
      /// <param name="excelTable"></param>
      /// <returns>The count of tables that would be imported</returns>
      int CountDataTables(string fileName, string sheetName, ImportDataTable dataTable, IList<ColumnMapping> mapping, DataTable excelTable);

      /// <summary>
      ///    This methods creates a list new import data table filled with data from excel data table.
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="sheetName"></param>
      /// <param name="dataTable"></param>
      /// <param name="mapping"></param>
      /// <param name="excelTable"></param>
      /// <returns></returns>
      IList<ImportDataTable> GetDataTables(string fileName, string sheetName, ImportDataTable dataTable, IList<ColumnMapping> mapping, DataTable excelTable);

      /// <summary>
      ///    This function reads a given excel file into a SmartXLS workbook object.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <returns>A <see cref="WorkBook" /> object.</returns>
      WorkBook ReadExcelFile(string fileName);

      /// <summary>
      ///    This method retrieves a list of all sheet names of a workbook.
      /// </summary>
      /// <param name="workBook">A <see cref="WorkBook" /> object.</param>
      /// <returns>List of strings representing the sheet names of the workbook.</returns>
      IList<string> GetSheetNames(WorkBook workBook);

      /// <summary>
      ///    This method sets the given sheet name as actual sheet in given workbook.
      /// </summary>
      /// <param name="fileName">Name of the excel file.</param>
      /// <param name="workBook"><see cref="WorkBook" /> object.</param>
      /// <param name="sheetName">Name of the sheet to be the selected one.</param>
      /// <exception cref="SheetNotFoundException"> is thrown, when sheet not exists in workbook.</exception>
      void SelectSheet(string fileName, WorkBook workBook, string sheetName);

      /// <summary>
      ///    This method reads the specified row and retrieves all column values as a list of strings.
      /// </summary>
      /// <param name="workBook">A <see cref="WorkBook" /> object already set to a sheet.</param>
      /// <param name="captionRow"></param>
      /// <returns>List of strings representing the names of the columns.</returns>
      IList<string> GetColumnNames(WorkBook workBook, int captionRow);

      /// <summary>
      ///    This method checks whether all data column are mapped.
      /// </summary>
      /// <param name="dataColumns">A <see cref="ImportDataColumnCollection"></see> object with all specified columns.</param>
      /// <param name="mapping">A list of column mapping information.</param>
      /// <exception cref="NoMappingForDataColumnException"> is thrown, when there is no mapping information for a data column</exception>
      void CheckWhetherAllDataColumnsAreMapped(ImportDataColumnCollection dataColumns, IList<ColumnMapping> mapping);

      /// <summary>
      ///    Checks whether each mapped excel column could be found in the excel file.
      /// </summary>
      /// <param name="mapping">List of mapping information.</param>
      /// <param name="columnNames">List of strings representing the column names.</param>
      /// <exception cref="ExcelColumnNotFoundException"> is thrown when a mapped column is missing in the list.</exception>
      void CheckWhetherMappedExcelColumnExist(IList<ColumnMapping> mapping, IList<string> columnNames);

      /// <summary>
      ///    This method checks whether all units of mapped columns are supported.
      /// </summary>
      /// <param name="dataColumns">A <see cref="ImportDataColumnCollection"></see> object with specified data columns./></param>
      /// <param name="mapping">A list of <see cref="ColumnMapping"></see> information./></param>
      /// <param name="columnNames">A list of strings representing column names.</param>
      /// <param name="units">A list of strings representing units.</param>
      /// <exception cref="InvalidUnitForExcelColumnException">is thrown when the found unit is invalid for the data column.</exception>
      /// <exception cref="DataColumnHasNoUnitInformationException">
      ///    is thrown when the data column has no dimension information
      ///    but the excel column has a unit.
      /// </exception>
      void CheckUnitSupportForAllMappedColumns(ImportDataColumnCollection dataColumns, IList<
         ColumnMapping> mapping, IList<string> columnNames, IList<string> units);

      /// <summary>
      ///    This method sets the active unit and dimension of an import data column to the given unit.
      /// </summary>
      /// <param name="column">Import data Column to set the active unit and dimension information.</param>
      /// <param name="unitName">Name of the unit to set.</param>
      /// <param name="explicitlySet">True for indication a explicitly setting of a unit.</param>
      /// <returns>True if unit could be found.</returns>
      bool SetColumnUnit(ImportDataColumn column, string unitName, bool explicitlySet);

      /// <summary>
      ///    Method determing the correct icon according to current data status of the column.
      /// </summary>
      /// <param name="importDataColumn"></param>
      /// <returns></returns>
      int GetImageIndex(ImportDataColumn importDataColumn);

      /// <summary>
      ///    This method builds a super tool tip for a import data column.
      /// </summary>
      /// <param name="column">Import data column.</param>
      /// <returns>A super tool tip with information about the given column.</returns>
      SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn column);

      /// <summary>
      ///    This function retrieves the data from an excel sheet as data table object.
      /// </summary>
      /// <param name="workbook">
      ///    A SmartXLS workbook object with a loaded excel file. The sheet must be set to the one which
      ///    should get loaded.
      /// </param>
      /// <param name="firstRow">First row of the sheet.</param>
      /// <param name="firstColumn">First column of the sheet.</param>
      /// <param name="maxRows">Number of rows to load.</param>
      /// <param name="maxColumns">Number of columns to load.</param>
      /// <param name="captionRow">This row contains names for the columns. If <c>-1</c> no header row.</param>
      /// <param name="unitRow">This row contains unit information for the column. If <c>-1</c> no unit row.</param>
      /// <returns>The filled data table object. Table name is set to the sheet name.</returns>
      DataTable ExportDataTable(WorkBook workbook, int firstColumn, int maxColumns, int firstRow, int maxRows, int captionRow, int unitRow);

      /// <summary>
      ///    This method should give an educated guess for the data starting row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <returns>The number of the first row with real data in it.</returns>
      int GetFirstDataRowGuess(WorkBook workBook);

      /// <summary>
      ///    This method should give an educated guess for the data starting row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <param name="range">The allowable range for the guess</param>
      /// <returns>The number of the first row with real data in it.</returns>
      int GetFirstDataRowGuess(WorkBook workBook, Rectangle range);

      /// <summary>
      ///    This method reads the specified row and retrieves all column values as a list of strings.
      /// </summary>
      /// <param name="workBook">A <see cref="WorkBook" /> object already set to a sheet.</param>
      /// <param name="unitRow"></param>
      /// <returns>List of strings representing units.</returns>
      IList<string> GetUnits(WorkBook workBook, int unitRow);

      /// <summary>
      ///    This method gets the unit information from the column names.
      /// </summary>
      /// <param name="columnNames">List with names of the columns</param>
      /// <returns>List of strings representing units.</returns>
      IList<string> GetUnits(IList<string> columnNames);

      string GetUnit(string text);

      /// <summary>
      ///    This methods reads the unit information from the extended properties.
      /// </summary>
      /// <param name="sourceColumn"></param>
      /// <returns></returns>
      string GetUnit(DataColumn sourceColumn);

      /// <summary>
      ///    This method should give an educated guess for the unit row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <param name="range">The allowable range for the guess</param>
      /// <returns>The number of the unit row.</returns>
      int GetUnitRowGuess(WorkBook workBook, Rectangle range);

      /// <summary>
      ///    This method should give an educated guess for the unit row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <returns>The number of the unit row.</returns>
      int GetUnitRowGuess(WorkBook workBook);

      /// <summary>
      ///    This method should give an educated guess for the caption row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <param name="range">The allowable range for the guess</param>
      /// <returns>The number of the caption row.</returns>
      int GetCaptionRowGuess(WorkBook workBook, Rectangle range);

      /// <summary>
      ///    This method should give an educated guess for the caption row.
      /// </summary>
      /// <param name="workBook">An <see cref="WorkBook" /> object with an open excel sheet.</param>
      /// <returns>The number of the caption row.</returns>
      int GetCaptionRowGuess(WorkBook workBook);

      /// <summary>
      ///    This method checks whether the datatype are compatible.
      /// </summary>
      /// <param name="dataColumns">Target table columns.</param>
      /// <param name="mapping">A list of <see cref="ColumnMapping"></see> information.</param>
      /// <param name="sourceColumns">Source table columns.</param>
      void CheckDataTypes(ImportDataColumnCollection dataColumns, IList<ColumnMapping> mapping, DataColumnCollection sourceColumns);
   }

   public class ImporterTask : IImporterTask
   {
      private const string UNIT_PROPERTY = "Unit";

      private readonly IColumnCaptionHelper _columnCaptionHelper;
      private readonly ILowerLimitOfQuantificationTask _lowerLimitOfQuantificationTask;

      public ImporterTask(IColumnCaptionHelper columnCaptionHelper, ILowerLimitOfQuantificationTask lowerLimitOfQuantificationTask)
      {
         _columnCaptionHelper = columnCaptionHelper;
         _lowerLimitOfQuantificationTask = lowerLimitOfQuantificationTask;
      }

      public int CountDataTables(string fileName, string sheetName, ImportDataTable dataTable, IList<ColumnMapping> mapping, DataTable excelTable)
      {
         var columnMapping = new List<ColumnMapping>();
         var groupByMapping = new List<ColumnMapping>();

         getMappingLists(dataTable, mapping, columnMapping, groupByMapping, groupByMapping);

         var normalizedColumnMapping = normalizeMultipleMappings(columnMapping).Count;
         if (groupByMapping.Count != 0)
            return normalizedColumnMapping * excelTable.DefaultView.ToTable(true, groupByMapping.Select(x => x.SourceColumn).ToArray()).Rows.Count;

         return normalizedColumnMapping;
      }

      public IList<ImportDataTable> GetDataTables(string fileName, string sheetName, ImportDataTable dataTable, IList<ColumnMapping> mapping, DataTable excelTable)
      {
         //excelTable must be filtered by distinct values of grouped columns
         //set metadata to current value of grouped columns
         //check validity of grouped value for metadata column
         var columnMapping = new List<ColumnMapping>();
         var metaDataMapping = new List<ColumnMapping>();
         var groupByMapping = new List<ColumnMapping>();

         getMappingLists(dataTable, mapping, columnMapping, metaDataMapping, groupByMapping);

         var normalizedColumnMapping = normalizeMultipleMappings(columnMapping);
         var returnValue = new List<ImportDataTable>();

         if (groupByMapping.Count == 0 && metaDataMapping.Count == 0)
         {
            foreach (var tableMapping in normalizedColumnMapping)
            {
               var newTable = getDataTable(fileName, sheetName, dataTable, tableMapping, excelTable);
               if (newTable.Rows.Count == 0) continue;
               returnValue.Add(newTable);
            }
         }
         else
         {
            var groupByColumnNames = new List<string>();
            foreach (var groupBy in groupByMapping)
               groupByColumnNames.Add(groupBy.SourceColumn);
            foreach (var metaData in metaDataMapping)
               groupByColumnNames.Add(metaData.SourceColumn);

            foreach (DataRow filterRow in excelTable.DefaultView.ToTable(true, groupByColumnNames.ToArray()).Rows)
            {
               var rowFilter = string.Empty;
               foreach (var groupByColumnName in groupByColumnNames)
               {
                  if (rowFilter.Length > 0) rowFilter += " AND ";
                  var groupByColumn = excelTable.Columns[groupByColumnName];
                  var filterValue = filterRow[groupByColumnName];
                  if (filterValue == null || filterValue == DBNull.Value)
                     rowFilter += $"[{groupByColumnName}] Is Null";
                  else if (groupByColumn.DataType == typeof(double))
                     rowFilter += $"[{groupByColumnName}] = {((double) filterRow[groupByColumnName]).ToString("G17", CultureInfo.InvariantCulture.NumberFormat)}";
                  else
                     rowFilter += $"[{groupByColumnName}] = '{filterRow[groupByColumnName]}'";
               }
               foreach (var tableMapping in normalizedColumnMapping)
               {
                  var excelView = new DataView(excelTable, rowFilter.Replace("]]", "\\]]"), null, DataViewRowState.CurrentRows);
                  var newTable = getDataTable(fileName, sheetName, dataTable, tableMapping, excelView.ToTable());
                  if (newTable.Rows.Count == 0) continue;

                  //rename tables to group by values
                  var tableNameAddon = string.Empty;
                  foreach (var groupBy in groupByMapping)
                  {
                     if (tableNameAddon.Length > 0) tableNameAddon += ", ";
                     tableNameAddon += $"{groupBy.SourceColumn} = {filterRow[groupBy.SourceColumn]}";
                  }
                  newTable.TableName += tableNameAddon;


                  foreach (var groupByData in groupByMapping)
                  {
                     newTable.ExtendedProperties.Add(groupByData.SourceColumn, filterRow[groupByData.SourceColumn]);
                  }

                  //set meta data default according to mapped filter columns
                  if (newTable.MetaData != null && metaDataMapping.Count > 0)
                  {
                     foreach (var metaData in metaDataMapping)
                     {
                        if (!newTable.MetaData.Columns.ContainsName(metaData.Target)) continue;
                        var metaDataColumn = newTable.MetaData.Columns.ItemByName(metaData.Target);
                        if (!filterRow.Table.Columns.Contains(metaData.SourceColumn)) continue;

                        var defaultValue = filterRow[metaData.SourceColumn].ToString();

                        if(!metaDataColumn.IsListOfValuesFixed || metaDataColumn.ListOfValues.ContainsValue(defaultValue))
                           metaDataColumn.DefaultValue = defaultValue;

                        metaDataColumn.IsColumnUsedForGrouping = true;
                     }
                  }
                  returnValue.Add(newTable);
               }
            }
         }
         return returnValue;
      }

      private void getMappingLists(ImportDataTable dataTable, IEnumerable<ColumnMapping> mapping, ICollection<ColumnMapping> columnMapping, ICollection<ColumnMapping> metaDataMapping, ICollection<ColumnMapping> groupByMapping)
      {
         foreach (var cm in mapping.Where(cm => !string.IsNullOrEmpty(cm.Target)))
         {
            if (dataTable.Columns.ContainsName(cm.Target))
            {
               columnMapping.Add(cm);
               continue;
            }
            if (dataTable.MetaData != null && dataTable.MetaData.Columns.ContainsName(cm.Target))
            {
               metaDataMapping.Add(cm);
               continue;
            }
            groupByMapping.Add(cm);
         }
      }

      public WorkBook ReadExcelFile(string fileName)
      {
         return SmartXLS.ReadExcelFile(fileName);
      }

      public IList<string> GetSheetNames(WorkBook workBook)
      {
         return SmartXLS.GetSheetNames(workBook);
      }

      public void SelectSheet(string fileName, WorkBook workBook, string sheetName)
      {
         SmartXLS.SelectSheet(fileName, workBook, sheetName);
      }

      public IList<string> GetColumnNames(WorkBook workBook, int captionRow)
      {
         var retVal = new List<string>(SmartXLS.GetRowValues(workBook, captionRow));
         IDictionary<string, string> names = new Dictionary<string, string>();
         for (var i = 0; i < retVal.Count; i++)
         {
            var s = retVal[i];
            var j = 1;
            if (string.IsNullOrEmpty(retVal[i]))
            {
               s = $"Column{j}";
               while (names.ContainsKey(s))
                  s = $"Column{j++}";
            }
            else
            {
               while (names.ContainsKey(s))
                  s = $"{retVal[i]}({j++})";
            }
            names.Add(s, s);
            retVal[i] = s;
         }
         return retVal;
      }

      public void CheckWhetherAllDataColumnsAreMapped(ImportDataColumnCollection dataColumns, IList<ColumnMapping> mapping)
      {
         foreach (ImportDataColumn col in dataColumns)
         {
            if (!col.Required) continue;
            var found = (from cm in mapping where cm.Target == col.ColumnName select (cm.SourceColumn.Length > 0)).FirstOrDefault();
            if (!found)
               throw new NoMappingForDataColumnException(col.ColumnName);
         }
      }

      public void CheckWhetherMappedExcelColumnExist(IList<ColumnMapping> mapping, IList<string> columnNames)
      {
         foreach (ColumnMapping cm in mapping)
         {
            if (!columnNames.Contains(cm.SourceColumn))
               throw new ExcelColumnNotFoundException(cm.SourceColumn);
         }
      }

      public void CheckUnitSupportForAllMappedColumns(ImportDataColumnCollection dataColumns, IList<
         ColumnMapping> mapping, IList<string> columnNames, IList<string> units)
      {
         foreach (var cm in mapping)
         {
            var unitName = units[columnNames.IndexOf(cm.SourceColumn)];
            if (string.IsNullOrEmpty(unitName)) continue;
            if (string.IsNullOrEmpty(cm.Target)) continue;
            if (!dataColumns.ContainsName(cm.Target)) continue;

            var col = dataColumns.ItemByName(cm.Target);

            if (col.Dimensions == null)
               throw new DataColumnHasNoUnitInformationException(unitName, cm.SourceColumn, cm.Target);
            if (cm.SelectedUnit.Name == null) continue;
            if (!findUnit(col.Dimensions, unitName) && cm.SelectedUnit.IsEqual(unitName))
               throw new InvalidUnitForExcelColumnException(unitName, cm.Target);
         }
      }

      public void CheckDataTypes(ImportDataColumnCollection dataColumns, IList<ColumnMapping> mapping, DataColumnCollection sourceColumns)
      {
         foreach (var cm in mapping)
         {
            if (string.IsNullOrEmpty(cm.Target)) continue;
            if (!dataColumns.ContainsName(cm.Target)) continue;
            checkDataType(dataColumns.ItemByName(cm.Target), sourceColumns[cm.SourceColumn]);
         }
      }

      /// <summary>
      ///    This method checks whether the datatypes of source column and mapped column are compatible.
      /// </summary>
      private void checkDataType(DataColumn dataColumn, DataColumn sourceColumn)
      {
         if (dataColumn.DataType != sourceColumn.DataType
             && sourceColumn.DataType != typeof(string))
            throw new DifferentDataTypeException(dataColumn.ColumnName, sourceColumn.ColumnName);
      }

      public bool SetColumnUnit(ImportDataColumn column, string unitName, bool explicitlySet)
      {
         if (column.Dimensions == null) return false;
         //firstly search for unit in default dimension
         var defaultDimension = DimensionHelper.GetDefaultDimension(column.Dimensions);
         var found = setColumnUnit(defaultDimension, column, unitName, explicitlySet);
         if (found)
            return true;
         //if unit could not be found in default dimension, search in others
         foreach (var dimension in column.Dimensions)
         {
            if (dimension == defaultDimension) continue;
            found = setColumnUnit(dimension, column, unitName, explicitlySet);
            if (found) break;
         }
         return found;
      }

      /// <summary>
      ///    This method sets the active unit and dimension of an inport data column for a given dimension and unit name.
      /// </summary>
      /// <param name="dimension">Dimension to search for unit.</param>
      /// <param name="column">Import data Column to set the active unit and dimension information.</param>
      /// <param name="unitName">Name of the unit to set.</param>
      /// <param name="explicitlySet">True for indication a explicitly setting of a unit.</param>
      /// <returns>True if unit could be found.</returns>
      private bool setColumnUnit(Dimension dimension, ImportDataColumn column, string unitName, bool explicitlySet)
      {
         if (!dimension.HasUnit(unitName))
            return false;

         var unit = dimension.FindUnit(unitName);
         column.ActiveDimension = dimension;
         column.ActiveUnit = unit;
         column.IsUnitExplicitlySet = explicitlySet;

         if (dimension.MetaDataConditions != null && (column.MetaData != null && column.MetaData.Rows.Count > 0))
         {
            var metadata = column.MetaData.Rows.ItemByIndex(0);
            var valid = metadata.CheckConditions(dimension.MetaDataConditions);
            if (!valid)
            {
               foreach (var condition in dimension.MetaDataConditions)
               {
                  metadata[condition.Key] = condition.Value;
               }
            }
         }
         return true;
      }

      public int GetImageIndex(ImportDataColumn importDataColumn)
      {
         var metaData = importDataColumn.MetaData;
         var imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.EmptyIcon);
         if (importDataColumn.Dimensions != null)
            imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);
         if (metaData != null)
            imageIndex = importDataColumn.HasMissingRequiredMetaData
               ? ApplicationIcons.IconIndex(ApplicationIcons.MissingMetaData)
               : importDataColumn.Dimensions == null
                  ? ApplicationIcons.IconIndex(ApplicationIcons.MetaData)
                  : ApplicationIcons.IconIndex(ApplicationIcons.MetaDataAndUnitInformation);

         if (importDataColumn.HasRequiredInputParameters)
            imageIndex = (metaData != null)
               ? importDataColumn.HasMissingRequiredMetaData
                  ? ApplicationIcons.IconIndex(ApplicationIcons.MissingMetaData)
                  : ApplicationIcons.IconIndex(ApplicationIcons.MetaDataAndUnitInformation)
               : ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);

         if (importDataColumn.HasMissingInputParameters || (importDataColumn.Dimensions != null && !importDataColumn.IsUnitExplicitlySet))
            imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.MissingUnitInformation);
         return imageIndex;
      }

      public SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn column)
      {
         if (column == null) return null;
         var retValue = new SuperToolTip();
         retValue.Items.AddTitle(column.DisplayName);
         retValue.Items.Add(column.Description);
         if (!string.IsNullOrEmpty(column.Source))
            retValue.Items.Add($"Source = {column.Source}");
         if (column.Dimensions != null)
         {
            retValue.Items.AddSeparator();
            var item = new ToolTipTitleItem {Text = Captions.Importer.UnitInformation, Image = ApplicationIcons.UnitInformation.ToImage()};
            retValue.Items.Add(item);
            retValue.Items.Add($"Dimension = {column.ActiveDimension.DisplayName}[{column.ActiveDimension.Name}]");
            retValue.Items.Add($"Unit = {column.ActiveUnit.DisplayName}[{column.ActiveUnit.Name}]");
            if (column.ActiveDimension.InputParameters != null && column.ActiveDimension.InputParameters.Count > 0)
            {
               retValue.Items.AddTitle("Input Parameters");
               foreach (var inputParameter in column.ActiveDimension.InputParameters)
                  retValue.Items.Add($"{inputParameter.DisplayName} = {inputParameter.Value} {inputParameter.Unit.Name}");
            }
         }
         if (column.MetaData != null)
         {
            retValue.Items.AddSeparator();
            var item = new ToolTipTitleItem {Text = Captions.Importer.MetaData, Image = ApplicationIcons.MetaData.ToImage()};
            retValue.Items.Add(item);
            foreach (MetaDataColumn metaData in column.MetaData.Columns)
               retValue.Items.Add($"{metaData.DisplayName} = {((column.MetaData.Rows.Count > 0) ? column.MetaData.Rows.ItemByIndex(0)[metaData.ColumnName] : string.Empty)}");
         }

         //Add help text when data is missed
         var imageindex = GetImageIndex(column);
         if (imageindex == ApplicationIcons.IconIndex(ApplicationIcons.MissingUnitInformation))
         {
            retValue.Items.AddSeparator();
            retValue.Items.Add(new ToolTipItem
            {
               Image = ApplicationIcons.MissingUnitInformation.ToImage(),
               Text = Captions.Importer.TheUnitInformationMustBeEnteredOrConfirmed
            });
         }
         if (imageindex == ApplicationIcons.IconIndex(ApplicationIcons.MissingMetaData))
         {
            retValue.Items.AddSeparator();
            retValue.Items.Add(new ToolTipItem
            {
               Image = ApplicationIcons.MissingMetaData.ToImage(),
               Text = Captions.Importer.TheMetaDataInformationMustBeEnteredOrConfirmed
            });
         }
         return retValue;
      }

      public DataTable ExportDataTable(WorkBook workbook, int firstColumn, int maxColumns, int firstRow, int maxRows, int captionRow, int unitRow)
      {
         const int maxrows = 0x10000; // 65536
         const int maxcols = 0x100; // 256

         if ((firstRow < 0) || (firstRow > maxrows))
            throw new ArgumentOutOfRangeException(nameof(firstRow));
         if ((firstColumn < 0) || (firstColumn > maxcols))
            throw new ArgumentOutOfRangeException(nameof(firstColumn));

         var returnTable = new DataTable(workbook.getSheetName(workbook.Sheet));
         var importProtocol = new StringBuilder();
         maxColumns = Math.Min(maxColumns, (maxcols - firstColumn) + 1);
         maxRows = Math.Min(maxRows, ((maxrows - firstRow) + 1));

         for (int iCol = 0; iCol < maxColumns; iCol++)
         {
            var cellType = getColumnType(workbook, firstColumn + iCol, firstRow, firstRow + maxrows);

            // set the data type of the data column according to the found cell type of the column.
            var dataColumn = new DataColumn();
            Type dataType = typeof(string);
            if (cellType == WorkBook.TypeNumber)
               dataType = SmartXLS.IsDateColumn(workbook, firstColumn + iCol, firstRow, maxRows)
                  ? typeof(string)
                  : typeof(double);
            else if (cellType == WorkBook.TypeLogical)
               dataType = typeof(bool);
            dataColumn.DataType = dataType;
            if (captionRow >= 0)
               dataColumn.ColumnName = workbook.getText(captionRow, firstColumn + iCol);

            if (unitRow >= 0)
               if (unitRow != captionRow)
                  dataColumn.ExtendedProperties.Add(UNIT_PROPERTY, workbook.getText(unitRow, firstColumn + iCol).Trim('[', ']'));
               else
                  dataColumn.ExtendedProperties.Add(UNIT_PROPERTY, GetUnit(workbook.getText(unitRow, firstColumn + iCol)));

            //find unique name for column
            var columnName = dataColumn.ColumnName;
            var i = 1;
            while (returnTable.Columns.Contains(dataColumn.ColumnName))
               dataColumn.ColumnName = $"{columnName}({i++})";

            returnTable.Columns.Add(dataColumn);
         }

         // load the data from the sheet to the data table
         returnTable.BeginLoadData();
         for (var iRow = 0; iRow < maxRows; iRow++)
         {
            var curRow = firstRow + iRow;
            var dataRow = returnTable.NewRow();
            for (var iCol = 0; iCol < maxColumns; iCol++)
            {
               var curCol = firstColumn + iCol;
               var cellType = workbook.getType(curRow, curCol);
               var dataType = returnTable.Columns[iCol].DataType;

               try
               {
                  if (isFormula(cellType))
                  {
                     if (valueMightBeWrong(workbook, curRow, curCol))
                     {
                        var formula = workbook.getFormula(curRow, curCol);
                        if (isFormulaReferenceToExternalData(formula))
                        {
                           var message = $"Unsupported link formula: {formula}";
                           protocolError(dataRow, iCol, importProtocol, message, curCol, curRow, returnTable);
                           continue;
                        }
                     }
                  }
                  cellType = Math.Abs(cellType);

                  switch (cellType)
                  {
                     case WorkBook.TypeEmpty:
                        dataRow[iCol] = DBNull.Value;
                        break;
                     case WorkBook.TypeNumber:
                        if (dataType == typeof(DateTime))
                           dataRow[iCol] = WorkBook.ConvertNumberToDateTime(workbook.getNumber(curRow, curCol));
                        else if (dataType == typeof(string))
                           dataRow[iCol] = workbook.getText(curRow, curCol);
                        else
                           dataRow[iCol] = workbook.getNumber(curRow, curCol);
                        break;
                     case WorkBook.TypeText:
                        dataRow[iCol] = workbook.getText(curRow, curCol);
                        break;
                     case WorkBook.TypeLogical:
                        if (dataType == typeof(bool))
                           dataRow[iCol] = (workbook.getNumber(curRow, curCol) != 0);
                        else
                           dataRow[iCol] = workbook.getText(curRow, curCol);
                        break;
                  }
               }
               catch (Exception e)
               {
                  protocolError(dataRow, iCol, importProtocol, e.Message, curCol, curRow, returnTable);
               }
            }
            returnTable.LoadDataRow(dataRow.ItemArray, true);
         }
         returnTable.EndLoadData();

         var importProtocolString = importProtocol.ToString();
         if (!string.IsNullOrEmpty(importProtocolString))
            returnTable.ExtendedProperties.Add("Protocol", importProtocolString);
         return returnTable;
      }

      /// <summary>
      ///    Finds the correct data type for a column. Looking in the <paramref name="workbook" /> for the column defined by
      ///    <paramref name="columnIndex" />
      ///    and in the range of cells from <paramref name="firstRow" /> up to <paramref name="lastRow" />.
      ///    To get numeric or logical types, all cells must contain that type, otherwise text type will be returned
      /// </summary>
      private int getColumnType(WorkBook workbook, int columnIndex, int firstRow, int lastRow)
      {
         int firstCellType = WorkBook.TypeText;
         var latchedType = false;
         var iRow = firstRow;
         do
         {
            int thisCellType = Math.Abs(workbook.getType(iRow++, columnIndex));

            switch (thisCellType)
            {
               case WorkBook.TypeText:
                  return WorkBook.TypeText;
               case WorkBook.TypeEmpty:
               case WorkBook.TypeError:
                  continue;
            }

            if (!latchedType)
               firstCellType = thisCellType;

            latchedType = true;

            if (thisCellType != firstCellType)
               return WorkBook.TypeText;
         } while (iRow < lastRow);

         return firstCellType;
      }

      private void protocolError(DataRow dataRow, int iCol, StringBuilder importProtocol, string message, int curCol, int curRow, DataTable returnTable)
      {
         dataRow[iCol] = DBNull.Value;
         importProtocol.Append($"Value error({message}) occured in column({curCol}) of row({curRow}).\n\n");
         if (!returnTable.Columns[iCol].ExtendedProperties.ContainsKey("Protocol"))
            returnTable.Columns[iCol].ExtendedProperties.Add("Protocol", string.Empty);
         var protocol = new StringBuilder(returnTable.Columns[iCol].ExtendedProperties["Protocol"].ToString());
         protocol.AppendFormat("Value error({0}) occured in row ({1}).\n\n", message, curRow);
         returnTable.Columns[iCol].ExtendedProperties["Protocol"] = protocol.ToString();
      }

      /// <summary>
      ///    This method tries to find out whether the result of a formula might be wrong.
      ///    The SmartXLS component has no real error flag.
      ///    For the error case a 0 is returned. Such a 0 can not be differentiated from an original 0.
      ///    Therefore it is only a guess that it might be caused by an error.
      /// </summary>
      private bool valueMightBeWrong(WorkBook workBook, int row, int col)
      {
         var numberValue = workBook.getNumber(row, col);
         var textValue = workBook.getText(row, col);
         return (numberValue.ToString(CultureInfo.InvariantCulture) == textValue && numberValue.Equals(0D));
      }

      private bool isFormula(short cellType)
      {
         return (cellType < 0);
      }

      private bool isFormulaReferenceToExternalData(string formula)
      {
         return formula.Contains("[") && formula.Contains("]");
      }

      #region FirstRow

      public int GetFirstDataRowGuess(WorkBook workBook)
      {
         return GetFirstDataRowGuess(workBook, SmartXLS.DefaultRange(workBook));
      }

      public int GetFirstDataRowGuess(WorkBook workBook, Rectangle range)
      {
         if (workBook == null) throw new ArgumentNullException("workBook");
         var row = range.Y;
         var rowFound = false;

         while (row <= Math.Min(workBook.LastRow, range.Y + range.Height))
         {
            for (var col = 0; col <= workBook.LastCol; col++)
            {
               var ct = workBook.getType(row, col);
               switch (ct)
               {
                  case -1: //Formula used
                     rowFound = workBook.getNumber(row, col).ToString(CultureInfo.InvariantCulture) == workBook.getText(row, col);
                     break;
                  case WorkBook.TypeNumber:
                     rowFound = true;
                     break;
                  case WorkBook.TypeLogical:
                     rowFound = true;
                     break;
               }
               if (rowFound) break;
            }
            if (rowFound) break;
            row++;
         }

         return row;
      }

      #endregion

      #region UnitRow

      public IList<string> GetUnits(WorkBook workBook, int unitRow)
      {
         return SmartXLS.GetRowValues(workBook, unitRow);
      }

      public IList<string> GetUnits(IList<string> columnNames)
      {
         IList<string> units = new List<string>(columnNames.Count);
         foreach (string s in columnNames)
            units.Add(_columnCaptionHelper.GetUnit(s));
         return units;
      }

      public string GetUnit(string text)
      {
         return _columnCaptionHelper.GetUnit(text);
      }

      public string GetUnit(DataColumn sourceColumn)
      {
         return sourceColumn.ExtendedProperties.ContainsKey(UNIT_PROPERTY)
            ? sourceColumn.ExtendedProperties[UNIT_PROPERTY].ToString()
            : null;
      }

      public int GetUnitRowGuess(WorkBook workBook, Rectangle range)
      {
         if (workBook == null) throw new ArgumentNullException(nameof(workBook));
         var row = range.Y;
         var rowFound = false;

         var firstDataRow = GetFirstDataRowGuess(workBook, range);
         while (row < firstDataRow)
         {
            var values = SmartXLS.GetRowValues(workBook, row);
            var units = (row == range.Y ? GetUnits(values) : GetUnits(workBook, row));

            foreach (var unit in units)
            {
               rowFound = !string.IsNullOrEmpty(unit);
               if (rowFound) break;
            }

            if (rowFound) break;
            row++;
         }
         if (row == firstDataRow && !rowFound) return -1;
         return row;
      }

      public int GetUnitRowGuess(WorkBook workBook)
      {
         return GetUnitRowGuess(workBook, SmartXLS.DefaultRange(workBook));
      }

      #endregion

      #region CaptionRow

      public int GetCaptionRowGuess(WorkBook workBook, Rectangle range)
      {
         if (workBook == null) throw new ArgumentNullException(nameof(workBook));

         var firstDataRow = GetFirstDataRowGuess(workBook, range);
         var row = range.Y;
         var data = SmartXLS.GetRowValues(workBook, firstDataRow);

         while (row < firstDataRow)
         {
            var captions = SmartXLS.GetRowValues(workBook, row);

            var rowFound = true;
            foreach (var captionAndData in captions.Zip(data, (cap, datum) => new Tuple<string, string>(cap, datum)))
            {
               rowFound &= !(hasCaption(captionAndData) ^ hasData(captionAndData));
               if (!rowFound) break;
            }

            if (rowFound) break;
            row++;
         }
         if (row == firstDataRow && row > range.Y) row--;
         return row;
      }

      private bool hasData(Tuple<string, string> captionAndData)
      {
         return string.IsNullOrEmpty(captionAndData.Item2);
      }

      private bool hasCaption(Tuple<string, string> captionAndData)
      {
         return string.IsNullOrEmpty(captionAndData.Item1);
      }

      public int GetCaptionRowGuess(WorkBook workBook)
      {
         return GetCaptionRowGuess(workBook, SmartXLS.DefaultRange(workBook));
      }

      #endregion

      #region privates

      /// <summary>
      ///    This method load the data from the excel table to the data table and checks whether there are null values which have
      ///    to be skipped.
      /// </summary>
      /// <param name="mapping">Mapping of excel columns to data columns.</param>
      /// <param name="excelTable">Excel data in form of a data table object.</param>
      /// <param name="dataTable">Target data table.</param>
      private void loadData(IEnumerable<ColumnMapping> mapping, DataTable excelTable, ImportDataTable dataTable)
      {
         var mappingList = mapping.ToList();
         //read the data and check whether there are null values for column with SkipNullValueRows = true
         dataTable.BeginLoadData();
         foreach (DataRow row in excelTable.Rows)
         {
            var values = new object[dataTable.Columns.Count];
            var skipRow = false;
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
               var col = dataTable.Columns.ItemByIndex(i);
               foreach (var cm in mappingList)
               {
                  if (cm.Target != col.ColumnName) continue;

                  var stringValue = row[cm.SourceColumn].ToString();
                  if (_lowerLimitOfQuantificationTask.IsLLOQ(stringValue))
                  {
                     var lowerLimitOfQuantification = _lowerLimitOfQuantificationTask.ParseLLOQ(stringValue);
                     row[cm.SourceColumn] = _lowerLimitOfQuantificationTask.GetInterpretationOfLLOQ(stringValue, col);
                     stringValue = row[cm.SourceColumn].ToString();
                     _lowerLimitOfQuantificationTask.AttachLLOQ(col, lowerLimitOfQuantification);
                  }

                  if (excelTable.Columns[cm.SourceColumn].DataType == typeof(string))
                  {
                     values[i] = getValueFromString(col, stringValue);
                  }
                  else
                     values[i] = row[cm.SourceColumn];

                  skipRow = col.SkipNullValueRows && values[i] == DBNull.Value;
                  if (skipRow) break;
               }
               if (skipRow) break;
            }
            if (!skipRow)
               dataTable.LoadDataRow(values, fAcceptChanges: true);
         }
         dataTable.EndLoadData();
      }

      private static bool columnIsType(DataColumn col, Type dataType)
      {
         return col.DataType == dataType;
      }

      private static object getValueFromString(DataColumn col, string stringValue)
      {
         if (columnIsType(col, typeof(string)))
            return stringValue;

         if (columnIsType(col, typeof(double)))
         {
            double newVal;
            return double.TryParse(stringValue, out newVal)
               ? (object) newVal
               : DBNull.Value;
         }
         if (columnIsType(col, typeof(int)))
         {
            int newVal;
            return int.TryParse(stringValue, out newVal)
               ? (object) newVal
               : DBNull.Value;
         }
         if (columnIsType(col, typeof(DateTime)))
         {
            DateTime newVal;
            return DateTime.TryParse(stringValue, out newVal)
               ? (object) newVal
               : DBNull.Value;
         }
         if (columnIsType(col, typeof(bool)))
         {
            bool newVal;
            return bool.TryParse(stringValue, out newVal)
               ? (object) newVal
               : DBNull.Value;
         }

         return DBNull.Value;
      }

      /// <summary>
      ///    This method sets the units found in the excel file for mapped table column.
      /// </summary>
      /// <param name="mapping">Mapping information.</param>
      /// <param name="excelTable">Data of excel file with unit information for each column.</param>
      /// <param name="dataTable">Data table with columns to get unit information.</param>
      private void setUnits(IEnumerable<ColumnMapping> mapping, DataTable excelTable, ImportDataTable dataTable)
      {
         var mappingList = mapping.ToList();
         foreach (ImportDataColumn col in dataTable.Columns)
         {
            foreach (ColumnMapping cm in mappingList)
            {
               if (string.IsNullOrEmpty(cm.Target)) continue;
               if (cm.Target != col.ColumnName) continue;
               var unit = GetUnit(excelTable.Columns[cm.SourceColumn]);
               if (string.IsNullOrEmpty(unit)) continue;
               if (col.SupportsUnit(unit) && !cm.IsUnitExplicitlySet)
               {
                  SetColumnUnit(col, unit, true);
                  cm.SelectedUnit = col.ActiveUnit;
                  cm.IsUnitExplicitlySet = col.IsUnitExplicitlySet;
               }
               else if (col.SupportsUnit(cm.SelectedUnit.Name))
                  SetColumnUnit(col, cm.SelectedUnit.Name, cm.IsUnitExplicitlySet);
            }
         }
      }

      /// <summary>
      ///    This methods creates a new import data table filled with data from excel data table.
      /// </summary>
      /// <param name="fileName">Name of the source file.</param>
      /// <param name="sheetName">Name of the source sheet.</param>
      /// <param name="dataTableTemplate">Template of data table.</param>
      /// <param name="mapping">List of column mappings.</param>
      /// <param name="excelTable">Source data.</param>
      /// <returns>Filled ImportDataTable</returns>
      private ImportDataTable getDataTable(string fileName, string sheetName, ImportDataTable dataTableTemplate, IEnumerable<ColumnMapping> mapping, DataTable excelTable)
      {
         var mappingList = mapping.ToList();
         var returnTable = dataTableTemplate.Clone();

         setUnits(mappingList, excelTable, returnTable);
         loadData(mappingList, excelTable, returnTable);
         removeMetaDataFromUnmappedColumns(mappingList, returnTable);
         setMetaDataFromMapping(mappingList, returnTable);
         setUnitInformationFromMapping(mappingList, returnTable);

         returnTable.File = fileName;
         returnTable.Sheet = sheetName;

         return returnTable;
      }

      private void setUnitInformationFromMapping(IEnumerable<ColumnMapping> mapping, ImportDataTable dataTable)
      {
         foreach (var cm in mapping)
         {
            if (string.IsNullOrEmpty(cm.Target)) continue;
            var tableColumn = dataTable.Columns.ItemByName(cm.Target);
            if (tableColumn == null) continue;
            if (cm.SelectedUnit.Name == null) continue;
            if (tableColumn.SupportsUnit(cm.SelectedUnit.Name))
               SetColumnUnit(tableColumn, cm.SelectedUnit.Name, cm.IsUnitExplicitlySet);
            if (cm.InputParameters == null) continue;
            if (tableColumn.ActiveDimension.InputParameters == null) return;
            tableColumn.ActiveDimension.InputParameters = new List<InputParameter>(cm.InputParameters);
         }
      }

      private static void setMetaDataFromMapping(IEnumerable<ColumnMapping> mapping, ImportDataTable dataTable)
      {
         foreach (var cm in mapping)
         {
            if (string.IsNullOrEmpty(cm.Target)) continue;
            var tableColumn = dataTable.Columns.ItemByName(cm.Target);
            tableColumn.Source = cm.SourceColumn;
            if (cm.MetaData == null) continue;
            tableColumn.MetaData = cm.MetaData.Copy();
         }
      }

      private static void removeMetaDataFromUnmappedColumns(IEnumerable<ColumnMapping> mapping, ImportDataTable returnTable)
      {
         var mappingList = mapping.ToList();
         foreach (ImportDataColumn column in returnTable.Columns)
         {
            if (column.Required) continue;
            if (column.MetaData == null) continue;
            var found = false;
            foreach (var cm in mappingList)
            {
               if (cm.Target != column.ColumnName) continue;
               found = true;
               break;
            }
            if (!found)
               column.MetaData = null;
         }
      }

      /// <summary>
      ///    This method checks whether a given unit can be found in a given dimension definition list.
      /// </summary>
      /// <param name="dimensions">List of dimensions.</param>
      /// <param name="unitName">Name of the unit to search for.</param>
      private static bool findUnit(IEnumerable<Dimension> dimensions, string unitName)
      {
         var found = false;
         foreach (var dimension in dimensions)
         {
            foreach (var unit in dimension.Units)
            {
               found = unit.IsEqual(unitName);
               if (found) break;
            }
            if (found) break;
         }
         return found;
      }

      /// <summary>
      ///    This method builds a cartesion product of a given matrix of column mappings and a new list of column mappings.
      /// </summary>
      /// <param name="list">Actual matrix of column mappings.</param>
      /// <param name="columnMappings">Column mappings for a new row in the list.</param>
      /// <returns>A matrix of column mappings.</returns>
      private static IList<IList<ColumnMapping>> joinColumnMappings(IEnumerable<IList<ColumnMapping>> list, IEnumerable<ColumnMapping> columnMappings)
      {
         var mappingList = list.ToList();
         var retVal = new List<IList<ColumnMapping>>();

         foreach (var columnMapping in columnMappings)
         {
            List<IList<ColumnMapping>> copy = getCopy(mappingList);
            foreach (var entry in copy)
               entry.Add(columnMapping.Clone());
            retVal.AddRange(copy);
         }
         return retVal;
      }

      /// <summary>
      ///    This method retrieves a copy of a given matrix of column mappings.
      /// </summary>
      /// <param name="list">Matrix of column mappings.</param>
      /// <returns>Copy of given list.</returns>
      private static List<IList<ColumnMapping>> getCopy(IEnumerable<IList<ColumnMapping>> list)
      {
         var copy = new List<IList<ColumnMapping>>();
         foreach (var entry in list)
         {
            var cms = new List<ColumnMapping>();
            foreach (var cm in entry)
               cms.Add(cm.Clone());
            copy.Add(cms);
         }
         return copy;
      }

      /// <summary>
      ///    This method creates normalized mappings if given mapping consists of multiple table column mappings.
      /// </summary>
      /// <param name="columnMapping">List of column mappings which can have multiple mappings for a table column.</param>
      /// <returns>A list of column mapping lists having only mappings where a table column is only mapped to one excel column.</returns>
      private static IList<IList<ColumnMapping>> normalizeMultipleMappings(IEnumerable<ColumnMapping> columnMapping)
      {
         IList<IList<ColumnMapping>> newList = new List<IList<ColumnMapping>>();
         var tableColumns = new Dictionary<string, List<ColumnMapping>>();

         //determine the list of mapped table columns with all there mappings
         foreach (var cmRow in columnMapping)
         {
            if (string.IsNullOrEmpty(cmRow.Target)) continue;
            if (!tableColumns.ContainsKey(cmRow.Target))
            {
               tableColumns.Add(cmRow.Target, new List<ColumnMapping> {cmRow});
            }
            else
            {
               tableColumns[cmRow.Target].Add(cmRow);
            }
         }

         //create a new list
         newList.Add(new List<ColumnMapping>());

         //build the cartesian product
         foreach (var tc in tableColumns)
            newList = joinColumnMappings(newList, tc.Value);

         return newList;
      }

      #endregion
   }
}