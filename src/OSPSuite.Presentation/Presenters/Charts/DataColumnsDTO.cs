using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum BrowserColumns
   {
      RepositoryName,
      Simulation,
      TopContainer,
      Container,
      BottomCompartment,
      Molecule,
      Name,
      ColumnId,
      DimensionName,
      BaseGridName,
      HasRelatedColumns,
      Origin,
      Date,
      Category,
      Source,
      QuantityName,
      QuantityType,
      QuantityPath,
      OrderIndex,
      Used
   }

   /// <summary>
   ///    DtoDataColumns is a DataTable containing a row with relevant information of each DataColumn.
   ///    It serves as an adapter to display the data repository columns in the DataBrowser.
   /// </summary>
   public class DataColumnsDTO : DataTable
   {
      private readonly string _used;

      internal DataColumnsDTO()
      {
         initializeTableColumns();

         PrimaryKey = new[]
         {
            Columns[BrowserColumns.ColumnId.ToString()]
         };
         DisplayQuantityPath = col => new PathElements();

         _used = BrowserColumns.Used.ToString();
      }

      private void initializeTableColumns()
      {
         Columns.Add(BrowserColumns.RepositoryName.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Simulation.ToString(), typeof(string));
         Columns.Add(BrowserColumns.TopContainer.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Container.ToString(), typeof(string));
         Columns.Add(BrowserColumns.BottomCompartment.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Molecule.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Name.ToString(), typeof(string));

         Columns.Add(BrowserColumns.BaseGridName.ToString(), typeof(string));
         Columns.Add(BrowserColumns.ColumnId.ToString(), typeof(string));
         Columns.Add(BrowserColumns.OrderIndex.ToString(), typeof(int));
         Columns.Add(BrowserColumns.QuantityName.ToString(), typeof(string));
         Columns.Add(BrowserColumns.DimensionName.ToString(), typeof(string));
         Columns.Add(BrowserColumns.QuantityType.ToString(), typeof(string));

         Columns.Add(BrowserColumns.HasRelatedColumns.ToString(), typeof(bool));
         Columns.Add(BrowserColumns.Origin.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Date.ToString(), typeof(DateTime));
         Columns.Add(BrowserColumns.Category.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Source.ToString(), typeof(string));
         Columns.Add(BrowserColumns.Used.ToString(), typeof(bool));
      }

      public Func<DataColumn, PathElements> DisplayQuantityPath { get; set; }

      public void AddDataColumn(DataColumn dataColumn)
      {
         var dataRow = NewRow();
         dataRow[BrowserColumns.ColumnId.ToString()] = dataColumn.Id;
         dataRow[BrowserColumns.Used.ToString()] = false;
         Rows.Add(dataRow);
         UpdateDataFromDataRepositoryColumn(dataColumn);
      }

      public bool ContainsDataColumn(string dataColumnId)
      {
         return Rows.Contains(dataColumnId);
      }

      public void UpdateDataFromDataRepositoryColumn(DataColumn dataColumn)
      {
         var dataRow = Rows.Find(dataColumn.Id);
         if (dataRow == null)
            throw new KeyNotFoundException("No Row found for Value=" + dataColumn.Id);

         var pathColumnValues = DisplayQuantityPath(dataColumn);

         dataRow[BrowserColumns.RepositoryName.ToString()] = dataColumn.Repository.Name;

         dataRow[BrowserColumns.Simulation.ToString()] = pathColumnValues[PathElement.Simulation].DisplayName;
         dataRow[BrowserColumns.TopContainer.ToString()] = pathColumnValues[PathElement.TopContainer].DisplayName;
         dataRow[BrowserColumns.Container.ToString()] = pathColumnValues[PathElement.Container].DisplayName;
         dataRow[BrowserColumns.BottomCompartment.ToString()] = pathColumnValues[PathElement.BottomCompartment].DisplayName;
         dataRow[BrowserColumns.Molecule.ToString()] = pathColumnValues[PathElement.Molecule].DisplayName;
         dataRow[BrowserColumns.Name.ToString()] = pathColumnValues[PathElement.Name].DisplayName;

         dataRow[BrowserColumns.DimensionName.ToString()] = dataColumn.Dimension.DisplayName;
         dataRow[BrowserColumns.BaseGridName.ToString()] = dataColumn.BaseGrid.Name;
         dataRow[BrowserColumns.HasRelatedColumns.ToString()] = dataColumn.RelatedColumns.Any();
         dataRow[BrowserColumns.Origin.ToString()] = dataColumn.DataInfo.Origin.ToString();
         dataRow[BrowserColumns.Date.ToString()] = dataColumn.DataInfo.Date.ToIsoFormat();
         dataRow[BrowserColumns.Category.ToString()] = dataColumn.DataInfo.Category;
         dataRow[BrowserColumns.OrderIndex.ToString()] = dataColumn.QuantityInfo.OrderIndex;
         dataRow[BrowserColumns.Source.ToString()] = dataColumn.DataInfo.Source;
         dataRow[BrowserColumns.QuantityName.ToString()] = dataColumn.QuantityInfo.Name;
         dataRow[BrowserColumns.QuantityType.ToString()] = dataColumn.QuantityInfo.Type;
      }

      public void RemoveDataColumn(DataColumn dataColumn)
      {
         var dataRow = Rows.Find(dataColumn.Id);
         if (dataRow == null) return;
         BeginLoadData();
         Rows.Remove(dataRow);
         EndLoadData();
      }

      public void InitializeUsedColumn(IReadOnlyList<DataColumn> dataColumns)
      {
         foreach (DataRow dataRow in Rows)
         {
            if ((bool) dataRow[_used])
               dataRow[_used] = false;
         }

         setUsedValue(dataColumns, true);
      }

      private void setUsedValue(IReadOnlyList<DataColumn> dataColumns, bool value)
      {
         foreach (var column in dataColumns)
         {
            var dataRow = Rows.Find(column.Id);
            //Used BaseGrid Columns may be not available in Rows

            if (dataRow != null)
               dataRow[_used] = value;
         }
      }

      public void SetUsedValueForColumns(IReadOnlyList<DataColumn> dataColumns, bool state)
      {
         setUsedValue(dataColumns, state);
      }

      public IEnumerable<string> GetUsedDataRepositoryColumnIds()
      {
         var dataRepositoryColumnIds = new List<string>();
         foreach (DataRow dataRow in Rows)
         {
            if (dataRow.ValueAt<bool>(_used))
               dataRepositoryColumnIds.Add(dataRow.ValueAt<string>(BrowserColumns.ColumnId.ToString()));
         }
         return dataRepositoryColumnIds;
      }

      public bool IsUsed(DataColumn dataColumn)
      {
         var row = rowFor(dataColumn);
         return row?.ValueAt<bool>(_used) ?? false;
      }

      private DataRow rowFor(DataColumn dataColumn) => Rows.Find(dataColumn.Id);
   }
}