using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.DTO;
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
   ///   DtoDataColumns is a DataTable containing a row with relevant information of each DataColumn.
   ///   It serves as an adapter to display the data repository columns in the DataBrowser.
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
         Columns.Add(BrowserColumns.RepositoryName.ToString(), typeof (string));

         Columns.Add(BrowserColumns.Simulation.ToString(), typeof (string));
         Columns.Add(BrowserColumns.TopContainer.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Container.ToString(), typeof (string));
         Columns.Add(BrowserColumns.BottomCompartment.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Molecule.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Name.ToString(), typeof (string));

         Columns.Add(BrowserColumns.BaseGridName.ToString(), typeof (string));
         Columns.Add(BrowserColumns.ColumnId.ToString(), typeof (string));
         Columns.Add(BrowserColumns.OrderIndex.ToString(), typeof (int));
         Columns.Add(BrowserColumns.QuantityName.ToString(), typeof (string));
         Columns.Add(BrowserColumns.DimensionName.ToString(), typeof (string));
         Columns.Add(BrowserColumns.QuantityType.ToString(), typeof (string));

         Columns.Add(BrowserColumns.HasRelatedColumns.ToString(), typeof (bool));
         Columns.Add(BrowserColumns.Origin.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Date.ToString(), typeof (DateTime));
         Columns.Add(BrowserColumns.Category.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Source.ToString(), typeof (string));
         Columns.Add(BrowserColumns.Used.ToString(), typeof (bool));
      }

      public Func<DataColumn, PathElements> DisplayQuantityPath { get; set; }

      public bool Contains(string dataRepositoryColumnId)
      {
         return Rows.Contains(dataRepositoryColumnId);
      }

      public void AddDataColumn(DataColumn dataRepositoryColumn)
      {
         var dataRow = NewRow();
         dataRow[BrowserColumns.ColumnId.ToString()] = dataRepositoryColumn.Id;
         dataRow[BrowserColumns.Used.ToString()] = false;
         Rows.Add(dataRow);
         UpdateDataFromDataRepositoryColumn(dataRepositoryColumn);
      }

      public bool ContainsDataColumn(string dataColumnId)
      {
         return Rows.Contains(dataColumnId);
      }

      public void UpdateDataFromDataRepositoryColumn(DataColumn dataRepositoryColumn)
      {
         var dataRow = Rows.Find(dataRepositoryColumn.Id);
         if (dataRow == null) 
            throw new KeyNotFoundException("No Row found for Value=" + dataRepositoryColumn.Id);

         var pathColumnValues = DisplayQuantityPath(dataRepositoryColumn);

         dataRow[BrowserColumns.RepositoryName.ToString()] = dataRepositoryColumn.Repository.Name;
         
         dataRow[BrowserColumns.Simulation.ToString()] = pathColumnValues[PathElement.Simulation].DisplayName;
         dataRow[BrowserColumns.TopContainer.ToString()] = pathColumnValues[PathElement.TopContainer].DisplayName;
         dataRow[BrowserColumns.Container.ToString()] = pathColumnValues[PathElement.Container].DisplayName;
         dataRow[BrowserColumns.BottomCompartment.ToString()] = pathColumnValues[PathElement.BottomCompartment].DisplayName;
         dataRow[BrowserColumns.Molecule.ToString()] = pathColumnValues[PathElement.Molecule].DisplayName;
         dataRow[BrowserColumns.Name.ToString()] = pathColumnValues[PathElement.Name].DisplayName;

         dataRow[BrowserColumns.DimensionName.ToString()] = dataRepositoryColumn.Dimension.DisplayName;
         dataRow[BrowserColumns.BaseGridName.ToString()] = dataRepositoryColumn.BaseGrid.Name;
         dataRow[BrowserColumns.HasRelatedColumns.ToString()] = dataRepositoryColumn.RelatedColumns.Any();
         dataRow[BrowserColumns.Origin.ToString()] = dataRepositoryColumn.DataInfo.Origin.ToString();
         dataRow[BrowserColumns.Date.ToString()] = dataRepositoryColumn.DataInfo.Date.ToIsoFormat();
         dataRow[BrowserColumns.Category.ToString()] = dataRepositoryColumn.DataInfo.Category;
         dataRow[BrowserColumns.OrderIndex.ToString()] = dataRepositoryColumn.QuantityInfo.OrderIndex;
         dataRow[BrowserColumns.Source.ToString()] = dataRepositoryColumn.DataInfo.Source;
         dataRow[BrowserColumns.QuantityName.ToString()] = dataRepositoryColumn.QuantityInfo.Name;
         dataRow[BrowserColumns.QuantityType.ToString()] = dataRepositoryColumn.QuantityInfo.Type;
      }

      public void RemoveDataColumn(string dataRepositoryColumnId)
      {
         var dataRow = Rows.Find(dataRepositoryColumnId);
         if (dataRow == null) return;
         BeginLoadData();
         Rows.Remove(dataRow);
         EndLoadData();
      }

      public void InitializeUsedColumn(IEnumerable<string> dataRepositoryColumnIds)
      {
         foreach (DataRow dataRow in Rows)
            if ((bool)dataRow[_used]) dataRow[_used] = false;

         setUsedValue(dataRepositoryColumnIds, true);
      }

      private void setUsedValue(IEnumerable<string> dataRepositoryColumnIds, bool value)
      {
         foreach (var id in dataRepositoryColumnIds)
         {
            var dataRow = Rows.Find(id); 
            //Used BaseGrid Columns may be not available in Rows

            if (dataRow != null) 
               dataRow[_used] = value;
         }
      }

      public void SetUsedValueForColumns(IEnumerable<string> dataRepositoryColumnIds, bool state)
      {
         setUsedValue(dataRepositoryColumnIds, state);
      }

      public IEnumerable<string> GetUsedDataRepositoryColumnIds()
      {
         IList<string> dataRepositoryColumnIds = new List<string>();
         foreach (DataRow dataRow in Rows)
         {
            if ((bool) dataRow[BrowserColumns.Used.ToString()])
               dataRepositoryColumnIds.Add((string) dataRow[BrowserColumns.ColumnId.ToString()]);
         }
         return dataRepositoryColumnIds;
      }
   }
}