using System.Data;
using System.Data.SQLite;

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
         var connectionString = $"Data Source={databasePath};Version=3;New=True;Compress=True;foreign keys=True";
         var cn = new SQLiteConnection(connectionString);
         cn.Open();
         return cn;
      }
   }
}