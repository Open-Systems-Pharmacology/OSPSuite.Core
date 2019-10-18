using MathNet.Numerics.LinearAlgebra;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public static class MathNetExtensionsForSpecs
   {
      public static void ShouldBeEqualTo(this Matrix<double> actual, Matrix<double> expected, double tolerance=1e-2)
      {
         actual.ColumnCount.ShouldBeEqualTo(expected.ColumnCount);
         actual.RowCount.ShouldBeEqualTo(expected.RowCount);

         for (int i = 0; i < actual.RowCount; i++)
         {
            for (int j = 0; j < actual.ColumnCount; j++)
            {
               actual[i,j].ShouldBeEqualTo(expected[i,j], tolerance);
            }
         }
      }
   }
}