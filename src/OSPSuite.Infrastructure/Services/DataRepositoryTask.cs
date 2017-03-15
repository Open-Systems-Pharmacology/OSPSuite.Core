using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
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
         targetColumn.QuantityInfo = sourceColumn.QuantityInfo != null ? sourceColumn.QuantityInfo.Clone() : null;
         targetColumn.DataInfo = sourceColumn.DataInfo != null ? sourceColumn.DataInfo.Clone() : null;
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

      public IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, bool formatOutput = false)
      {
         return ToDataTable(dataColumns, c => c.Name, formatOutput);
      }

      public IEnumerable<DataTable> ToDataTable(IEnumerable<DataColumn> dataColumns, Func<DataColumn, string> columnNameRetriever, bool formatOutput = false)
      {
         var allColumns = allColumnsWithRelatedColumnsFrom(dataColumns);
         var cacheName = retrieveUniqueNamesForTables(allColumns);
         return cacheName.KeyValues.Select(keyValuePair => createTableFor(keyValuePair.Value, keyValuePair.Key, allColumns, columnNameRetriever, formatOutput));
      }

      private HashSet<DataColumn> allColumnsWithRelatedColumnsFrom(IEnumerable<DataColumn> dataColumns)
      {
         var allColumns = new HashSet<DataColumn>();
         dataColumns.Each(column =>
         {
            allColumns.Add(column);
            column.RelatedColumns.Each(x=>allColumns.Add(x));
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
         ExportToExcel(dataColumns, fileName, x => x.Name, launchExcel);
      }

      public void ExportToExcel(IEnumerable<DataColumn> dataColumns, string fileName, Func<DataColumn, string> columnNameRetriever, bool launchExcel = true)
      {
         ExportToExcel(ToDataTable(dataColumns, columnNameRetriever), fileName, launchExcel);
      }

      public void ExportToExcel(IEnumerable<DataTable> dataTables, string fileName, bool launchExcel)
      {
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, fileName, launchExcel);
      }

      private DataTable createTableFor(string tableName, DataColumn baseGridColumn, IEnumerable<DataColumn> columnsToExport, Func<DataColumn, string> columnNameRetriever, bool formatOutput)
      {
         var dataTable = new DataTable(tableName);

         //user string because we want to export the unit
         var allColumns = columnsToExport.Where(col => !col.IsBaseGrid())
            .Where(x => x.BaseGrid == baseGridColumn).ToList();

         moveDrugColumnsFirst(allColumns);

         allColumns.Insert(0, baseGridColumn);
         var cacheName = retrieveUniqueNameForColumns(allColumns, columnNameRetriever);

         allColumns.Each(c => dataTable.Columns.Add(cacheName[c], formatOutput ? typeof (string) : typeof (float)).ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, c.Id));

         //add units information
         for (int i = 0; i < dataTable.Columns.Count; i++)
         {
            var col = dataTable.Columns[i];
            var unit = unitFor(allColumns[i]);

            if (!string.IsNullOrWhiteSpace(unit.Name))
               col.ColumnName = string.Format("{0} [{1}]", col.ColumnName, unit.Name);
         }

         for (int i = 0; i < baseGridColumn.Values.Count; i++)
         {
            DataRow row = dataTable.NewRow();
            for (int j = 0; j < allColumns.Count; j++)
            {
               var columnToExport = allColumns[j];
               var unit = unitFor(columnToExport);
               double value = columnToExport.Dimension.BaseUnitValueToUnitValue(unit, columnToExport.Values[i]);
               if (formatOutput)
                  row[j] = _numericFormatter.Format(value);
               else
                  row[j] = value;
            }
            dataTable.Rows.Add(row);
         }

         return dataTable;
      }

      private void moveDrugColumnsFirst(List<DataColumn> allColumns)
      {
         var allColumnsNotDrug = allColumns.Where(x => !x.QuantityInfo.Type.Is(QuantityType.Drug)).ToList();
         allColumnsNotDrug.Each(c => allColumns.Remove(c));

         //add them add them again at the end at the beginning
         allColumnsNotDrug.Each(allColumns.Add);
      }

      private ICache<DataColumn, string> retrieveUniqueNameForColumns(IEnumerable<DataColumn> allRelatedColumns, Func<DataColumn, string> columnNameRetriever)
      {
         var cacheNameForColumns = new Cache<DataColumn, string>();
         foreach (var column in allRelatedColumns)
         {
            int index = 1;
            string defaultName = columnNameRetriever(column);
            string currentName = defaultName;
            while (cacheNameForColumns.Contains(currentName))
            {
               currentName = string.Format("{0}_{1}", defaultName, index++);
            }
            cacheNameForColumns.Add(column, currentName);
         }

         return cacheNameForColumns;
      }

      private Unit unitFor(DataColumn column)
      {
         return column.Dimension.UnitOrDefault(column.DataInfo.DisplayUnitName);
      }
   }
}