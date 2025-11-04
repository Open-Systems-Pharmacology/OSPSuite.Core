using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace OSPSuite.Infrastructure.Serialization.Services
{
   public class SessionFactoryProvider : ISessionFactoryProvider
   {
      public ISessionFactory InitializeSessionFactoryFor(string dataSource)
      {
         var configuration = CreateConfiguration(dataSource);
         var sessionFactory = configuration.BuildSessionFactory();
         return sessionFactory;
      }

      public ISessionFactory OpenSessionFactoryFor(string dataSource)
      {
         var configuration = CreateConfiguration(dataSource);
         var sessionFactory = configuration.BuildSessionFactory();
         return sessionFactory;
      }

      public SchemaExport GetSchemaExport(string dataSource)
      {
         var configuration = CreateConfiguration(dataSource);
         return new SchemaExport(configuration);
      }

      private Configuration CreateConfiguration(string dataSource)
      {
         var configuration = new Configuration();
         
         // Configure database integration with Microsoft.Data.Sqlite
         configuration.DataBaseIntegration(db =>
         {
            // Use the SQLite driver that works with Microsoft.Data.Sqlite
            db.Driver<NHibernate.Driver.SQLite20Driver>();
            db.Dialect<SQLiteDialect>();
            db.ConnectionString = $"Data Source={dataSource};Foreign Keys=True";
            db.Timeout = 20;
         });

         // Add mapping assemblies (you may need to adjust these based on your actual mapping assemblies)
         configuration.AddAssembly(typeof(SessionFactoryProvider).Assembly);
         
         return configuration;
      }
   }
}