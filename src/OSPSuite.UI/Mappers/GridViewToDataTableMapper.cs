using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Mappers
{
   public interface IGridViewToDataTableMapper : IMapper<GridView, DataTable>
   {
      DataTable MapFrom(GridView gridView, IEnumerable<int> selectedRows);
      DataTable MapFrom(UxGridView gridView, int[] getSelectedRows, Func<int, GridColumn[]> columnsForRowRetriever);
   }

   public class GridViewToDataTableMapper : IGridViewToDataTableMapper
   {
      public DataTable MapFrom(GridView gridView)
      {
         return mapNewTable(gridView, rowIndex => true, i => gridView.VisibleColumns);
      }

      private static DataTable mapNewTable(GridView gridView, Func<int, bool> isRowSelected, Func<int, IEnumerable<GridColumn>> columnsForRowRetriever)
      {
         var dataTable = new DataTable();

         allColumnsForAllRows(gridView, isRowSelected, columnsForRowRetriever).Distinct().Each(column =>
         {
            var col = dataTable.Columns.Add(column.FieldName);
            col.Caption = columnDisplayNameFrom(column);
         });

         for (var i = 0; i < gridView.RowCount; i++)
         {
            var rowHandle = gridView.GetVisibleRowHandle(i);

            if (!isRowSelected(rowHandle)) continue;

            var row = dataTable.NewRow();
            if (gridView.IsGroupRow(rowHandle))
            {
               row[0] = gridView.GetGroupRowDisplayText(rowHandle);
            }
            else
            {
               columnsForRowRetriever(i).Each(column =>
               {
                  row[column.FieldName] = gridView.GetRowCellDisplayText(rowHandle, column);
               });
            }
            dataTable.Rows.Add(row);
         }

         return dataTable;
      }

      private static IEnumerable<GridColumn> allColumnsForAllRows(GridView gridView, Func<int, bool> isRowSelected, Func<int, IEnumerable<GridColumn>> columnsForRowRetriever)
      {
         for (var i = 0; i < gridView.RowCount; i++)
         {
            var rowHandle = gridView.GetVisibleRowHandle(i);
            if (!isRowSelected(rowHandle)) continue;

            foreach (var column in columnsForRowRetriever(rowHandle))
            {
               yield return column;
            }
         }
      }

      public DataTable MapFrom(GridView gridView, IEnumerable<int> selectedRows)
      {
         return mapNewTable(gridView, selectedRows.ContainsItem, i => gridView.VisibleColumns);
      }

      public DataTable MapFrom(UxGridView gridView, int[] selectedRows, Func<int, GridColumn[]> columnsForRowRetriever)
      {
         return mapNewTable(gridView, selectedRows.ContainsItem, columnsForRowRetriever);
      }

      private static string columnDisplayNameFrom(GridColumn column)
      {
         return string.IsNullOrEmpty(column.Caption) ? column.FieldName: column.Caption;
      }
   }
}