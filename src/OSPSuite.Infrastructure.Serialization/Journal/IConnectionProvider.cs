using System.Data;
using Microsoft.Data.Sqlite;
using OSPSuite.Infrastructure.Serialization.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Journal
{
   public interface IConnectionProvider
   {
      IDbConnection CreateConnection(string databasePath);
   }

   public class ConnectionProvider : IConnectionProvider
   {
      public IDbConnection CreateConnection(string databasePath)
      {
         var cn = new SqliteConnection(ConnectionStringHelper.ConnectionStringFor(databasePath));
         cn.Open();
         return cn;
      }
   }
}