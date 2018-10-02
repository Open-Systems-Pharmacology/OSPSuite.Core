using System.Data;
using System.Globalization;

namespace OSPSuite.Core.Extensions
{
   public static class DbConnectionExtensions
   {
      public static DataTable ExecuteQueryForDataTable(this IDbConnection connection, string query)
      {
         var rawData = new DataTable {Locale = CultureInfo.InvariantCulture};

         using (var command = connection.CreateCommand())
         {
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            using (var reader = command.ExecuteReader())
            {
               rawData.Load(reader);
            }
         }
         return rawData;
      }

      public static DataRow ExecuteQueryForSingleRow(this IDbConnection connection, string query)
      {
         return ExecuteQueryForDataTable(connection, query).Rows[0];
      }

      public static int ExecuteNonQuery(this IDbConnection sqlLite, string query)
      {
         using (var command = sqlLite.CreateCommand())
         {
            command.CommandText = query;
            return command.ExecuteNonQuery();
         }
      }
   }
}