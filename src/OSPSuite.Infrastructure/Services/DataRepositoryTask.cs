using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Infrastructure.Services
{
   public class DataRepositoryTask : IDataRepositoryTask
   {
      private readonly ICache<string, string> _idMap = new Cache<string, string>();
      private readonly NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      public DataRepository Clone(DataRepository dataRepositoryToClone)
      {
         _idMap.Clear();
         var cloneRepository = new DataRepository {Name = dataRepositoryToClone.Name, Description = dataRepositoryToClone.Description};

         var baseGridColumns = dataRepositoryToClone.Where(col => col.IsBaseGrid());
         baseGridColumns.Each(col => addBaseGridToRepository(cloneRepository, col));

         var nonBaseGridColumns = dataRepositoryToClone.Where(col => !col.IsBaseGrid());
         nonBaseGridColumns.Each(col => addColumnToRepository(cloneRepository, col));

         cloneRepository.ExtendedProperties.UpdateFrom(dataRepositoryToClone.ExtendedProperties);

         return cloneRepository;
      }

      private void addBaseGridToRepository(DataRepository dataRepository, DataColumn baseGrid)
      {
         if (isAlreadyCloned(baseGrid)) return;
         dataRepository.Add(cloneBaseGrid(baseGrid));
      }

      private DataColumn addColumnToRepository(DataRepository dataRepository, DataColumn sourceColumn)
      {
         if (isAlreadyCloned(sourceColumn))
            return dataRepository[idOfCloneFor(sourceColumn)];

         var baseGridColumn = dataRepository[idOfCloneFor(sourceColumn.BaseGrid)].DowncastTo<BaseGrid>();

         var newColumn = CloneColumn(sourceColumn, baseGridColumn);
         dataRepository.Add(newColumn);

         foreach (var relatedColumn in sourceColumn.RelatedColumns)
         {
            newColumn.AddRelatedColumn(addColumnToRepository(dataRepository, relatedColumn));
         }

         return newColumn;
      }

      private void updateColumnProperties(DataColumn sourceColumn, DataColumn targetColumn)
      {
         targetColumn.QuantityInfo = sourceColumn.QuantityInfo?.Clone();
         targetColumn.DataInfo = sourceColumn.DataInfo?.Clone();
         targetColumn.Values = sourceColumn.Values;
         targetColumn.IsInternal = sourceColumn.IsInternal;
      }

      public BaseGrid CloneBaseGrid(DataColumn baseGridToClone)
      {
         _idMap.Clear();
         return cloneBaseGrid(baseGridToClone);
      }

      private BaseGrid cloneBaseGrid(DataColumn baseGridToClone)
      {
         var newBaseGrid = new BaseGrid(baseGridToClone.Name, baseGridToClone.Dimension);
         updateColumnProperties(baseGridToClone, newBaseGrid);
         _idMap.Add(baseGridToClone.Id, newBaseGrid.Id);
         return newBaseGrid;
      }

      public DataColumn CloneColumn(DataColumn sourceColumn, BaseGrid clonedBaseGrid)
      {
         var newColumn = new DataColumn(sourceColumn.Name, sourceColumn.Dimension, clonedBaseGrid);
         updateColumnProperties(sourceColumn, newColumn);
         _idMap.Add(sourceColumn.Id, newColumn.Id);
         return newColumn;
      }

      private bool isAlreadyCloned(DataColumn column)
      {
         return _idMap.Contains(column.Id);
      }

      private string idOfCloneFor(DataColumn column)
      {
         return _idMap[column.Id];
      }

      public IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, bool formatOutput = false, bool useDisplayUnit = true)
      {
         return ToDataTable(dataColumns, c => c.Name, c => c.Dimension, formatOutput, useDisplayUnit);
      }

      public IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, Func<DataColumn, string> columnNameRetriever, Func<DataColumn, IDimension> dimensionRetrieverFunc, bool formatOutput = false, bool useDisplayUnit = true)
      {
         var allColumns = allColumnsWithRelatedColumnsFrom(dataColumns);
         var cacheName = retrieveUniqueNamesForTables(allColumns);
         return cacheName.KeyValues.Select(keyValuePair => createTableFor(keyValuePair.Value, keyValuePair.Key, allColumns, columnNameRetriever, dimensionRetrieverFunc, formatOutput, useDisplayUnit));
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

      public void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, bool launchExcel)
      {
         ExportToExcel(dataColumns, fileName, x => x.Name, x => x.Dimension, launchExcel);
      }

      public void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, Func<DataColumn, string> columnNameRetriever, Func<DataColumn, IDimension> dimensionRetriever, bool launchExcel = true)
      {
         ExportToExcel(ToDataTable(dataColumns, columnNameRetriever, dimensionRetriever), fileName, launchExcel);
      }

      public void ExportToExcel(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel)
      {
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, fileName, launchExcel);
      }

      private DataTable createTableFor(string tableName, DataColumn baseGridColumn, IEnumerable<DataColumn> columnsToExport, Func<DataColumn, string> columnNameRetriever, Func<DataColumn, IDimension> dimensionRetriever, bool formatOutput, bool useDisplayUnit)
      {
         var dataTable = new DataTable(tableName);

         //user string because we want to export the unit
         var allColumns = columnsToExport.Where(col => !col.IsBaseGrid())
            .Where(x => x.BaseGrid == baseGridColumn).ToList();

         moveDrugColumnsFirst(allColumns);

         allColumns.Insert(0, baseGridColumn);
         var cacheName = retrieveUniqueNameForColumns(allColumns, columnNameRetriever);
         var cacheDimensions = retrieveDimensionsFor(allColumns, dimensionRetriever);

         allColumns.Each(x =>
         {
            var column = dataTable.Columns.Add(cacheName[x], formatOutput ? typeof(string) : typeof(float));
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
               double value = dimension.BaseUnitValueToUnitValue(unit, columnToExport.Values[i]);

               if (formatOutput)
                  row[j] = _numericFormatter.Format(value);
               else
                  row[j] = value;
            }
            dataTable.Rows.Add(row);
         }

         return dataTable;
      }

      private Cache<DataColumn, IDimension> retrieveDimensionsFor(IReadOnlyList<DataColumn> allColumns, Func<DataColumn, IDimension> dimensionRetriever)
      {
         var cacheDimensionsForColumns = new Cache<DataColumn, IDimension>();
         allColumns.Each(x => cacheDimensionsForColumns.Add(x, dimensionRetriever(x)));
         return cacheDimensionsForColumns;
      }

      private void moveDrugColumnsFirst(List<DataColumn> allColumns)
      {
         var allColumnsNotDrug = allColumns.Where(x => !x.QuantityInfo.Type.Is(QuantityType.Drug)).ToList();
         allColumnsNotDrug.Each(c => allColumns.Remove(c));

         //add them add them again at the end at the beginning
         allColumnsNotDrug.Each(allColumns.Add);
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