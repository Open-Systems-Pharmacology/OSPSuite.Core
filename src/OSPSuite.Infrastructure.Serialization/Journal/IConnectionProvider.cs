using System.Data;
using Microsoft.Data.Sqlite;

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
         var connectionString = $"Data Source={databasePath};Foreign Keys=False;Pooling=False;Cache=Shared";
         var cn = new SqliteConnection(connectionString);
         cn.Open();
         return cn;
      }
   }
}