using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Serialization.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Services;

public class SQLiteProjectCommandExecuter
{
   public virtual void ExecuteCommand(string projectFile, Action<DbConnection> command )
   {
      string file = projectFile.ToUNCPath();
      using (var sqlLite = new SqliteConnection(ConnectionStringHelper.ConnectionStringFor(file)))
      {
         sqlLite.Open();
         command(sqlLite);
      }
   }
}