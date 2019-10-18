using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace OSPSuite.Infrastructure.Services
{
   public interface ISessionFactoryProvider
   {
      ISessionFactory InitalizeSessionFactoryFor(string dataSource);
      ISessionFactory OpenSessionFactoryFor(string dataSource);
      SchemaExport GetSchemaExport(string dataSource);
   }
}