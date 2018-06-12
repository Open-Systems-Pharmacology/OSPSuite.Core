using System;
using System.Data;
using System.Text;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraPivotGrid;

namespace OSPSuite.UI.Mappers
{
   public class PivotGridCellsToDataTableMapper
   {
      private const string PARAMETER_COLUMN_NAME = "Parameter";
      private const string TABLE_NAME = "Pivot";
      private const string FIELD_SEPARATOR = " - ";

      public DataTable MapSelectionFrom(PivotGridCells pivotGridCells, Func<string, string> displayNameRetreiver, Func<PivotCellEventArgs, object> valueToDisplayConverter = null)
      {
         return map(pivotGridCells, displayNameRetreiver, valueToDisplayConverter, y => selectionContainsRow(pivotGridCells, y), x => selectionContainsColumn(pivotGridCells, x));
      }

      public DataTable MapFrom(PivotGridCells pivotGridCells, Func<string, string> displayNameRetreiver, Func<PivotCellEventArgs, object> valueToDisplayConverter = null)
      {
         return map(pivotGridCells, displayNameRetreiver, valueToDisplayConverter, y => true, x => true);
      }

      private static DataTable map(PivotGridCells pivotGridCells, Func<string, string> displayNameRetreiver, Func<PivotCellEventArgs, object> valueToDisplayConverter, Func<int, bool> shouldUseRow, Func<int, bool> shouldUseColumn)
      {
         var dt = new DataTable(TABLE_NAME);
         configureDataTable(pivotGridCells, dt, shouldUseColumn);
         fillTableFromSource(pivotGridCells, dt, displayNameRetreiver, valueToDisplayConverter ?? getDoubleForDataTable, shouldUseRow, shouldUseColumn);
         return dt;
      }

      private bool selectionContainsRow(PivotGridCells pivotGridCells, int y)
      {
         // When selection is empty, use focused cell instead. DevExpress single focused cell is not 'Selection'
         if (pivotGridCells.Selection.IsEmpty)
            return isFfocusedCellInRow(pivotGridCells, y);

         return pivotGridCells.Selection.Contains(pivotGridCells.Selection.X, y);
      }

      private static bool isFfocusedCellInRow(PivotGridCells pivotGridCells, int y)
      {
         return pivotGridCells.FocusedCell.Y == y;
      }

      private static bool selectionContainsColumn(PivotGridCells pivotGridCells, int x)
      {
         // When selection is empty, use focused cell instead. DevExpress single focused cell is not 'Selection'
         if (pivotGridCells.Selection.IsEmpty)
            return isFocusedCellInColumn(pivotGridCells, x);

         return pivotGridCells.Selection.Contains(x, pivotGridCells.Selection.Y);
      }

      private static bool isFocusedCellInColumn(PivotGridCells pivotGridCells, int x)
      {
         return pivotGridCells.FocusedCell.X == x;
      }

      private static void fillTableFromSource(PivotGridCells pivotGridCells,
         DataTable dt, Func<string, string> displayNameRetreiver,
         Func<PivotCellEventArgs, object> valueToDisplayConverter,
         Func<int, bool> shouldUseRow, Func<int, bool> shouldUseColumn)
      {
         for (var rowIndex = 0; rowIndex < pivotGridCells.RowCount; rowIndex++)
         {
            if (!shouldUseRow(rowIndex))
               continue;

            var row = dt.NewRow();
            row[PARAMETER_COLUMN_NAME] = getRowName(pivotGridCells, rowIndex, displayNameRetreiver);
            for (var i = 0; i < pivotGridCells.ColumnCount; i++)
            {
               if (shouldUseColumn(i))
                  row[getColumnName(pivotGridCells, i)] = valueToDisplayConverter(pivotGridCells.GetCellInfo(i, rowIndex));
            }
            dt.Rows.Add(row);
         }
      }

      private static object getDoubleForDataTable(PivotCellEventArgs cellEventArgs)
      {
         var cellValue = cellEventArgs.Value;
         if (cellValue == null)
            return DBNull.Value;

         var value = cellValue.ConvertedTo<double>();
         if (double.IsNaN(value))
            return DBNull.Value;

         return value;
      }

      private static void configureDataTable(PivotGridCells input, DataTable dt, Func<int, bool> useColumn)
      {
         dt.AddColumn(PARAMETER_COLUMN_NAME);
         dt.Columns[PARAMETER_COLUMN_NAME].Caption = string.Empty;
         for (var i = 0; i < input.ColumnCount; i++)
         {
            if (useColumn(i))
               dt.AddColumn<double>(getColumnName(input, i));
         }
      }

      private static string getRowName(PivotGridCells input, int rowIndex, Func<string, string> displayNameRetreiver)
      {
         var pivotCellEventArgs = input.GetCellInfo(0, rowIndex);
         return displayNameRetreiver(pivotCellEventArgs.GetFieldValue(pivotCellEventArgs.RowField).ToString());
      }

      private static string getColumnName(PivotGridCells input, int columnIndex)
      {
         var stringBuilder = new StringBuilder();
         var pivotCellEventArgs = input.GetCellInfo(columnIndex, 0);
         pivotCellEventArgs.GetColumnFields().Each(field =>
         {
            stringBuilder.Append(pivotCellEventArgs.GetFieldValue(field).ToString());
            stringBuilder.Append(FIELD_SEPARATOR);
         });

         return stringBuilder.ToString().Trim(FIELD_SEPARATOR.ToCharArray());
      }
   }
}
