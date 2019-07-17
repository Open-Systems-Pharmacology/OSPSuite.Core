using System;
using System.Data.Common;
using System.Data.SQLite;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Infrastructure.Services
{
   public class SQLiteProjectCommandExecuter
   {
      public virtual void ExecuteCommand(string projectFile, Action<DbConnection> command )
      {
         string file = projectFile.ToUNCPath();
         using (var sqlLite = new SQLiteConnection($"Data Source={file}"))
         {
            sqlLite.Open();
            command(sqlLite);
         }
      }

      public virtual TResult ExecuteCommand<TResult>(string projectFile, Func<DbConnection, TResult> command)
      {
         string file = projectFile.ToUNCPath();
         using (var sqlLite = new SQLiteConnection($"Data Source={file}"))
         {
            sqlLite.Open();
           return command(sqlLite);
         }
      }
   }
}