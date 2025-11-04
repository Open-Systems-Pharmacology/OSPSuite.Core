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
         // Note: The original System.Data.SQLite connection string included "Compress=True",
         // but Microsoft.Data.Sqlite does not support the Compress parameter.
         // SQLite works fine without compression, and compression adds CPU overhead.
         // See COMPRESS_INVESTIGATION.md for details.
         var connectionString = $"Data Source={databasePath};Foreign Keys=True";
         var cn = new SqliteConnection(connectionString);
         cn.Open();
         return cn;
      }
   }
}