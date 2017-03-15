using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   public class Matrix
   {
      public double[][] Rows { get; }

      public IReadOnlyList<string> ColumnNames { get; }
      public IReadOnlyList<string> RowNames { get; }
      public int ColumnCount => ColumnNames.Count;
      public int RowCount => RowNames.Count;

      public Matrix(IEnumerable<string> rowNames, IEnumerable<string> columnNames)
      {
         ColumnNames = columnNames.ToList();
         RowNames = rowNames.ToList();
         Rows = new double[RowCount][];
      }

      public void SetRow(int index, double[] rowValues)
      {
         if (rowValues.Length != ColumnCount)
            throw new InvalidArgumentException($"No the expected number of values ({rowValues.Length} vs {ColumnCount})");

         Rows[index] = rowValues;
      }

      public double Max
      {
         get { return Rows.Max(x => x.Max()); }
      }

      public double[] this[int rowIndex] => Rows[rowIndex];
      public double At(int rowIndex, int colIndex) => this[rowIndex][colIndex];
   }
}