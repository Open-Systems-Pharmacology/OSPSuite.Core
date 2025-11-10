using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Services
{
   public class SQLiteProjectCommandExecuter
   {
      public virtual void ExecuteCommand(string projectFile, Action<DbConnection> command )
      {
         string file = projectFile.ToUNCPath();
         using (var sqlLite = new SqliteConnection($"Data Source={file};Foreign Keys=False;Pooling=False;Cache=Shared"))
         {
            sqlLite.Open();
            command(sqlLite);
         }
      }

      public virtual TResult ExecuteCommand<TResult>(string projectFile, Func<DbConnection, TResult> command)
      {
         string file = projectFile.ToUNCPath();
         using (var sqlLite = new SqliteConnection($"Data Source={file};Foreign Keys=False;Pooling=False;Cache=Shared"))
         {
            sqlLite.Open();
           return command(sqlLite);
         }
      }
   }
}