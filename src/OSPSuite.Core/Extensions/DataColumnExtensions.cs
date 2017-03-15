using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Extensions
{
   public static class DataColumnExtensions
   {
      public static bool IsCalculationColumn(this DataColumn column)
      {
         return column.DataInfo.Origin == ColumnOrigins.Calculation;
      }

      public static bool IsCalculationAuxiliaryColumn(this DataColumn column)
      {
         return column.DataInfo.Origin == ColumnOrigins.CalculationAuxiliary;
      }
   }
}
