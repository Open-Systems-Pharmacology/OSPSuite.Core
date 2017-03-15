using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public static class DataColumnExtensions
   {
      public static bool IsBaseGrid(this DataColumn dataColumn)
      {
         return dataColumn.IsAnImplementationOf<BaseGrid>();
      }

      public static bool IsObservedData(this DataColumn dataColumn)
      {
         return dataColumn.DataInfo != null && dataColumn.DataInfo.Origin == ColumnOrigins.Observation;
      }
   }
}