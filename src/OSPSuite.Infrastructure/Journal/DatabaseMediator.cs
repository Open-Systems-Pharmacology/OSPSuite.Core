using OSPSuite.Utility.Container;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure.Journal
{
   public interface IDatabaseMediator
   {
      IJournalDatabaseCommand<T> CommandFor<T>();
      void ExecuteCommand<T>(T payload);

      IJournalDatabaseQuery<TResponse> QueryFor<TResponse>(IQuery<TResponse> query);
      TResponse ExecuteQuery<TResponse>(IQuery<TResponse> query);
   }

   public class DatabaseMediator : IDatabaseMediator
   {
      private readonly IContainer _container;

      public DatabaseMediator(IContainer container)
      {
         _container = container;
      }

      public IJournalDatabaseCommand<T> CommandFor<T>()
      {
         return _container.Resolve<IJournalDatabaseCommand<T>>();
      }

      public void ExecuteCommand<T>(T payload)
      {
         var command = CommandFor<T>();
         command.ExecuteInTransaction(payload);
      }

      public IJournalDatabaseQuery<TResponse> QueryFor<TResponse>(IQuery<TResponse> query) 
      {
         return _container.Resolve<IJournalDatabaseQuery<TResponse>>(query.GetType().Name);
      }

      public TResponse ExecuteQuery<TResponse>(IQuery<TResponse> query) 
      {
         var queryHandler = QueryFor(query);
         return queryHandler.QueryInConnection(query);
      }
   }
}