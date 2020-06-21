using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Infrastructure.Export
{
   public class DataRepositoryExportTask : IDataRepositoryExportTask
   {
      private readonly NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      public void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel = true, DataColumnExportOptions exportOptions = null) =>
         ExportToExcel(ToDataTable(dataColumns, exportOptions), fileName, launchExcel);

      public void ExportToExcel(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel = true) =>
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, fileName, launchExcel);

      public Task ExportToExcelAsync(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel = true, DataColumnExportOptions exportOptions = null) =>
         ExportToExcelAsync(ToDataTable(dataColumns, exportOptions), fileName, launchExcel);

      public Task ExportToExcelAsync(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel = true) =>
         Task.Run(() => ExportToExcelTask.ExportDataTablesToExcel(dataTables, fileName, launchExcel));

      public Task ExportToCsvAsync(IEnumerable<DataColumn> dataColumns, string fileName, DataColumnExportOptions exportOptions = null)
      {
         return Task.Run(() =>
         {
            var dataTables = ToDataTable(dataColumns, exportOptions);
            if (dataTables.Count == 1)
               dataTables[0].ExportToCSV(fileName);

            else if (dataTables.Count > 1)
               throw new ArgumentException(Error.ExportToCsvNotSupportedForDifferentBaseGrid);
         });
      }

      public IReadOnlyList<DataTable> ToDataTable(
         IEnumerable<DataColumn> dataColumns,
         DataColumnExportOptions exportOptions = null
      )
      {
         var options = exportOptions ?? new DataColumnExportOptions();
         var allColumns = allColumnsWithRelatedColumnsFrom(dataColumns);
         var cacheName = retrieveUniqueNamesForTables(allColumns);
         var valueFormatter = valueFormatterFor(options.FormatOutput);
         var columnType = options.ForceColumnTypeAsObject ? typeof(object) : options.FormatOutput ? typeof(string) : typeof(float);
         var columnNameRetrieverFunc = options.ColumnNameRetriever;
         var dimensionRetrieverFunc = options.DimensionRetriever;
         var useDisplayUnit = options.UseDisplayUnit;
         return cacheName.KeyValues.Select(keyValuePair => createTableFor(keyValuePair.Value, keyValuePair.Key, allColumns, columnNameRetrieverFunc, dimensionRetrieverFunc, useDisplayUnit, columnType, valueFormatter)).ToList();
      }

      private Func<double, object> valueFormatterFor(bool formatOutput)
      {
         if (formatOutput)
            return v => _numericFormatter.Format(v);

         return v => v;
      }

      private HashSet<DataColumn> allColumnsWithRelatedColumnsFrom(IEnumerable<DataColumn> dataColumns)
      {
         var allColumns = new HashSet<DataColumn>();
         dataColumns.Each(column =>
         {
            allColumns.Add(column);
            column.RelatedColumns.Each(x => allColumns.Add(x));
         });

         return allColumns;
      }

      private ICache<BaseGrid, string> retrieveUniqueNamesForTables(IEnumerable<DataColumn> allColumns)
      {
         var tableNameCache = new Cache<BaseGrid, string>();
         //one table per basegrid
         foreach (var baseGrid in allColumns.Select(x => x.BaseGrid).Distinct())
         {
            int index = 1;
            string defaultName = tableNameFrom(baseGrid);
            string currentName = defaultName;
            while (tableNameCache.Contains(currentName))
            {
               currentName = $"{defaultName}_{index++}";
            }

            tableNameCache.Add(baseGrid, currentName);
         }

         return tableNameCache;
      }

      private static string tableNameFrom(DataColumn baseGrid)
      {
         const string defaultTableName = "Table";
         var tableName = baseGrid.Repository != null ? baseGrid.Repository.Name : defaultTableName;
         if (string.IsNullOrEmpty(tableName))
            tableName = defaultTableName;

         return new string(FileHelper.RemoveIllegalCharactersFrom(tableName).Take(Constants.MAX_NUMBER_OF_CHAR_IN_TABLE_NAME).ToArray());
      }

      private DataTable createTableFor(
         string tableName,
         DataColumn baseGridColumn,
         IEnumerable<DataColumn> columnsToExport,
         Func<DataColumn, string> columnNameRetriever,
         Func<DataColumn, IDimension> dimensionRetriever,
         bool useDisplayUnit,
         Type columnType,
         Func<double, object> valueFunc)
      {
         var dataTable = new DataTable(tableName);

         var allColumns = sortColumnsForExport(columnsToExport.Where(x => !x.IsBaseGrid() && x.BaseGrid == baseGridColumn));


         allColumns.Insert(0, baseGridColumn);
         var cacheName = retrieveUniqueNameForColumns(allColumns, columnNameRetriever);
         var cacheDimensions = retrieveDimensionsFor(allColumns, dimensionRetriever);

         allColumns.Each(x =>
         {
            var column = dataTable.Columns.Add(cacheName[x], columnType);
            column.ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, x.Id);
         });

         //add units information
         for (int i = 0; i < dataTable.Columns.Count; i++)
         {
            var col = dataTable.Columns[i];
            var dataColumn = allColumns[i];
            var dimension = cacheDimensions[dataColumn];
            var unit = unitFor(dimension, dataColumn, useDisplayUnit);

            if (!string.IsNullOrWhiteSpace(unit.Name))
               col.ColumnName = $"{col.ColumnName} [{unit.Name}]";
         }

         for (int i = 0; i < baseGridColumn.Values.Count; i++)
         {
            var row = dataTable.NewRow();
            for (int j = 0; j < allColumns.Count; j++)
            {
               var columnToExport = allColumns[j];
               var dimension = cacheDimensions[columnToExport];
               var unit = unitFor(dimension, columnToExport, useDisplayUnit);
               var value = dimension.BaseUnitValueToUnitValue(unit, columnToExport.Values[i]);
               row[j] = valueFunc(value);
            }

            dataTable.Rows.Add(row);
         }

         return dataTable;
      }

      private Cache<DataColumn, IDimension> retrieveDimensionsFor(IEnumerable<DataColumn> allColumns, Func<DataColumn, IDimension> dimensionRetriever)
      {
         var cacheDimensionsForColumns = new Cache<DataColumn, IDimension>();
         allColumns.Each(x => cacheDimensionsForColumns.Add(x, dimensionRetriever(x)));
         return cacheDimensionsForColumns;
      }

      private List<DataColumn> sortColumnsForExport(IEnumerable<DataColumn> allColumns)
      {
         var allDrugColumns = new List<DataColumn>(allColumns);

         var allColumnsNotDrug = allDrugColumns
            .Where(x => !x.QuantityInfo.Type.Is(QuantityType.Drug))
            .ToList();

         allColumnsNotDrug.Each(x => allDrugColumns.Remove(x));

         var allAuxiliariesColumns = allColumnsNotDrug
            .Where(x => x.DataInfo.Origin.IsOneOf(ColumnOrigins.CalculationAuxiliary, ColumnOrigins.ObservationAuxiliary))
            .ToList();

         allAuxiliariesColumns.Each(x => allColumnsNotDrug.Remove(x));

         //First return the drug columns, then non drug columns that are not auxiliary columns and last auxiliary columns
         return allDrugColumns.Union(allColumnsNotDrug).Union(allAuxiliariesColumns).ToList();
      }

      private Cache<DataColumn, string> retrieveUniqueNameForColumns(IReadOnlyList<DataColumn> allColumns, Func<DataColumn, string> columnNameRetriever)
      {
         var cacheNameForColumns = new Cache<DataColumn, string>();
         foreach (var column in allColumns)
         {
            int index = 1;
            string defaultName = columnNameRetriever(column);
            string currentName = defaultName;
            while (cacheNameForColumns.Contains(currentName))
            {
               currentName = $"{defaultName}_{index++}";
            }

            cacheNameForColumns.Add(column, currentName);
         }

         return cacheNameForColumns;
      }

      private Unit unitFor(IDimension dimension, DataColumn column, bool useDisplayUnit)
      {
         if (useDisplayUnit)
            return dimension.UnitOrDefault(column.DataInfo.DisplayUnitName);

         return dimension.BaseUnit;
      }
   }
}