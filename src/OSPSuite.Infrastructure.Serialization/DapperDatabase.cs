using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Journal;

namespace OSPSuite.Infrastructure
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
   public class IgnorePropertyAttribute : Attribute
   {
      public IgnorePropertyAttribute(bool ignore)
      {
         Value = ignore;
      }

      public bool Value { get; set; }
   }

   /// <summary>
   ///    A container for a database, assumes all the tables have an Id column named Id
   /// </summary>
   /// <typeparam name="TDatabase"></typeparam>
   public abstract class Database<TDatabase> where TDatabase : Database<TDatabase>, new()
   {
      public class Table<T, TId>
      {
         private readonly Database<TDatabase> _database;
         public string TableName { get; private set; }

         public Table(Database<TDatabase> database, string tableName = null)
         {
            _database = database;
            TableName = tableName ?? typeof (T).Name;
         }

      
         /// <summary>
         ///    Insert rows into the db and returns the number of rows successfuly inserted
         /// </summary>
         /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
         public virtual int Insert(dynamic data)
         {
            dynamic o = data;
            var enumerable = data as IEnumerable;
            if (enumerable!=null)
            {
               if (!Enumerable.Any(data))
                  return 0;

               o = Enumerable.First(data);
            }

            List<string> paramNames = GetParamNames(o);
            string cols = string.Join(",", paramNames);
            string cols_params = string.Join(",", paramNames.Select(p => "@" + p));
            var sql = "INSERT INTO " + TableName + " (" + cols + ") VALUES (" + cols_params + ")";

            return _database.Execute(sql, data);
         }

         /// <summary>
         ///    Update a record in the DB
         /// </summary>
         /// <param name="id"></param>
         /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
         public int Update(TId id, dynamic data)
         {
            List<string> paramNames = GetParamNames((object) data);

            var builder = new StringBuilder();
            builder.Append("UPDATE `").Append(TableName).Append("` SET ");
            builder.AppendLine(string.Join(",", paramNames.Where(n => n != "Id").Select(p => p + "= @" + p)));
            builder.Append("WHERE Id = @Id");

            DynamicParameters parameters = new DynamicParameters(data);
            parameters.Add("Id", id);

            return _database.Execute(builder.ToString(), parameters);
         }

         /// <summary>
         ///    Insert a row into the db or update when key is duplicated
         /// </summary>
         /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
         public long InsertOrUpdate(dynamic data)
         {
            var o = (object) data;
            List<string> paramNames = GetParamNames(o);

            string cols = string.Join(",", paramNames);
            string cols_params = string.Join(",", paramNames.Select(p => "@" + p));
            var sql = @"INSERT OR REPLACE INTO " + TableName + " (" + cols + ") VALUES (" + cols_params +
                      "); SELECT LAST_INSERT_ROWID()";

            return _database.Query<long>(sql, o).Single();
         }

         /// <summary>
         ///    Delete a record for the DB
         /// </summary>
         /// <param name="id">Id of record to delete</param>
         public bool Delete(TId id)
         {
            return DeleteWhere("Id = @id", new {id});
         }

         /// <summary>
         ///    Delete a record for the DB
         /// </summary>
         public bool DeleteWhere(string deleteCondition, dynamic data)
         {
            return _database.Execute($"DELETE FROM {TableName} WHERE {deleteCondition}", data) > 0;
         }

         /// <summary>
         ///    Grab a record with a particular Id from the DB
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public T Get(TId id)
         {
            return GetWhere("Id = @id", new {id}).FirstOrDefault();
         }

         public IEnumerable<T> GetWhere(string getCondition, dynamic data)
         {
            return _database.DoInConnection(() => _database.Query<T>($"SELECT * FROM {TableName}  WHERE {getCondition}", data));
         }

         public T First()
         {
            return _database.DoInConnection(() => _database.Query<T>($"SELECT * FROM {TableName} LIMIT 1").FirstOrDefault());
         }

         public IEnumerable<T> All()
         {
            return _database.DoInConnection(() => _database.Query<T>($"SELECT * FROM {TableName}"));
         }

         private static readonly ConcurrentDictionary<Type, List<string>> paramNameCache = new ConcurrentDictionary<Type, List<string>>();

         internal static List<string> GetParamNames(object o)
         {
            if (o is DynamicParameters)
            {
               return (o as DynamicParameters).ParameterNames.ToList();
            }

            List<string> paramNames;
            if (!paramNameCache.TryGetValue(o.GetType(), out paramNames))
            {
               paramNames = new List<string>();
               foreach (var prop in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
               {
                  var attribs = prop.GetCustomAttributes(typeof (IgnorePropertyAttribute), true);
                  var attr = attribs.FirstOrDefault() as IgnorePropertyAttribute;
                  if (attr == null || (attr != null && !attr.Value))
                  {
                     paramNames.Add(prop.Name);
                  }
               }
               paramNameCache[o.GetType()] = paramNames;
            }
            return paramNames;
         }
      }

      public class Table<T> : Table<T, string>
      {
         public Table(Database<TDatabase> database, string likelyTableName)
            : base(database, likelyTableName)
         {
         }
      }

      private int _commandTimeout;
      private IConnectionProvider _connectionProvider;
      private string _databasePath;
      private IDbConnection _connection;
      private IDbTransaction _transaction;

      public static TDatabase Init(IConnectionProvider connectionProvider, string databasePath, int commandTimeout = 50)
      {
         TDatabase db = new TDatabase();
         db.InitDatabase(connectionProvider, databasePath, commandTimeout);
         using (db.OpenConnection())
         {
            //try to open a connection to ensure that the database is valid. Nothing to do here
         }
         return db;
      }

      internal void InitDatabase(IConnectionProvider connectionProvider, string databasePath, int commandTimeout = 50)
      {
         _commandTimeout = commandTimeout;
         _connectionProvider = connectionProvider;
         _databasePath = databasePath.ToUNCPath();
      }

      private bool tableExists(string name)
      {
         return DoInConnection(() => Query("SELECT name FROM sqlite_master WHERE type='table' AND name=@name", new {name}).Count() == 1);
      }

      public int Execute(string sql, dynamic param = null)
      {
         return _connection.Execute(sql, param as object, _transaction, commandTimeout: _commandTimeout);
      }

      public IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true)
      {
         return _connection.Query<T>(sql, param as object, _transaction, buffered, _commandTimeout);
      }

      public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
      {
         return _connection.Query(sql, map, param as object, _transaction, buffered, splitOn);
      }

      public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
      {
         return _connection.Query(sql, map, param as object, _transaction, buffered, splitOn);
      }

      public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
      {
         return _connection.Query(sql, map, param as object, _transaction, buffered, splitOn);
      }

      public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
      {
         return _connection.Query(sql, map, param as object, _transaction, buffered, splitOn);
      }

      public IEnumerable<dynamic> Query(string sql, dynamic param = null, bool buffered = true)
      {
         return _connection.Query(sql, param as object, _transaction, buffered);
      }

      public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, int? commandTimeout = null, CommandType? commandType = null)
      {
         return SqlMapper.QueryMultiple(_connection, sql, param, null, commandTimeout, commandType);
      }

      public TResult DoInConnection<TResult>(Func<TResult> action)
      {
         if (_connection != null)
            return action();

         using (var connection = OpenConnection())
         {
            try
            {
               _connection = connection;
               return action();
            }
            finally
            {
               _connection = null;
            }
         }
      }

      public void DoInTransaction(Action action)
      {
         if (_connection != null && _transaction != null)
         {
            action();
            return;
         }

         using (var connection = OpenConnection())
         using (var transaction = connection.BeginTransaction())
         {
            try
            {
               _connection = connection;
               _transaction = transaction;
               action();
               transaction.Commit();
            }
            catch
            {
               transaction.Rollback();
               throw;
            }
            finally
            {
               _connection = null;
               _transaction = null;
            }
         }
      }

      public IDbConnection OpenConnection()
      {
         return _connectionProvider.CreateConnection(_databasePath);
      }

      /// <summary>
      /// Returns the active open connection if available or throws an exception if not defined
      /// </summary>
      public IDbConnection Connection
      {
         get
         {
            if(_connection==null)
               throw new ArgumentException("Connection not initialized. Use OpenConnection to create a new connection");

            return _connection;
         }
      }
   }
}