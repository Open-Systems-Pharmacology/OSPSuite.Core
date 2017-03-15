using System.Data;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IMatrixToDataTableMapper
   {
      /// <summary>
      ///    Creates a datatable from <paramref name="matrix" />
      ///    The matrix dimensions, and the number of captions should be the same.
      /// </summary>
      DataTable MapFrom(Matrix matrix);
   }

   public class MatrixToDataTableMapper : IMatrixToDataTableMapper
   {
      private const string PARAMETER = "Parameter";

      public DataTable MapFrom(Matrix matrix)
      {
         var dt = new DataTable("Matrix");

         addColumns(dt, matrix);

         addRows(dt, matrix);

         return dt;
      }

      private static void addRows(DataTable dt, Matrix matrix)
      {
         for (var i = 0; i < matrix.RowCount; i++)
         {
            var newRow = dt.NewRow();
            var norm = matrix[i][i];
            newRow[PARAMETER] = matrix.RowNames[i];

            for (var j = 0; j < matrix.ColumnCount; j++)
            {
               var value = matrix[i][j];
               newRow[j + 1] = value;
            }

            dt.Rows.Add(newRow);
         }
      }

      private static void addColumns(DataTable dt, Matrix matrix)
      {
         dt.AddColumn(PARAMETER);
         matrix.ColumnNames.Each(col => dt.AddColumn<double>(col));
      }
   }
}