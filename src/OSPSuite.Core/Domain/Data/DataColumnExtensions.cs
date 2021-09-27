using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public static class DataColumnExtensions
   {
      public static bool IsBaseGrid(this DataColumn dataColumn) => dataColumn.IsAnImplementationOf<BaseGrid>();

      public static bool IsObservation(this DataColumn column) => dataColumnIs(column, ColumnOrigins.Observation);

      public static bool IsCalculation(this DataColumn column) => dataColumnIs(column, ColumnOrigins.Calculation);

      public static bool IsCalculationAuxiliary(this DataColumn column) => dataColumnIs(column, ColumnOrigins.CalculationAuxiliary);

      public static bool IsObservationAuxiliary(this DataColumn column) => dataColumnIs(column, ColumnOrigins.ObservationAuxiliary);

      private static bool dataColumnIs(DataColumn column, ColumnOrigins columnOrigin) => column.DataInfo?.Origin == columnOrigin;
   }
}