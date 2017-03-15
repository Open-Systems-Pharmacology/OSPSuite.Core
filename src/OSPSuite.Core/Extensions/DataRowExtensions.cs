using System;
using System.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class DataRowExtensions
   {
      public static object[] GetPrimaryKeyValues(this DataRow dataRow)
      {
         var primaryKey = dataRow.Table.PrimaryKey;
         var values = new object[primaryKey.Length];

         for (int i = 0; i < primaryKey.Length; i++)
            values[i] = dataRow[primaryKey[i]];

         return values;
      }

      public static T ValueAt<T>(this DataRow dataRow, string columnName)
      {
         var value = dataRow[columnName];
         if (value == DBNull.Value)
            return default(T);

         return value.ConvertedTo<T>();
      }

      public static string StringAt(this DataRow dataRow, string columnName)
      {
         return ValueAt<string>(dataRow, columnName);
      }
   }
}