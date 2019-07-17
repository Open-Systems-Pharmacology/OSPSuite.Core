using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Journal
{
   public interface IDatabaseMediator
   {
      void ExecuteCommand<T>(T payload);

      TResponse ExecuteQuery<TResponse>(IQuery<TResponse> query);
   }

   public class DatabaseMediator : IDatabaseMediator
   {
      private readonly IContainer _container;

      public DatabaseMediator(IContainer container)
      {
         _container = container;
      }

      private IJournalDatabaseCommand<T> commandFor<T>()
      {
         return _container.Resolve<IJournalDatabaseCommand<T>>();
      }

      public void ExecuteCommand<T>(T payload)
      {
         var command = commandFor<T>();
         command.ExecuteInTransaction(payload);
      }

      private IJournalDatabaseQuery<TResponse> queryFor<TResponse>(IQuery<TResponse> query)
      {
         return _container.Resolve<IJournalDatabaseQuery<TResponse>>(query.GetType().Name);
      }

      public TResponse ExecuteQuery<TResponse>(IQuery<TResponse> query)
      {
         var queryHandler = queryFor(query);
         return queryHandler.QueryInConnection(query);
      }
   }
}