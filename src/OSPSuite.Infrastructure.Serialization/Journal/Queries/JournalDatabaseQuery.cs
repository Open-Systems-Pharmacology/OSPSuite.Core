using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Journal.Queries
{
   public interface IQuery<out TResponse>
   {
      
   }

   public interface IJournalDatabaseQuery<TResponse>
   {
      /// <summary>
      /// Execute the command in a connection
      /// </summary>
      TResponse QueryInConnection(IQuery<TResponse> payload);
   }

   public interface IJournalDatabaseQuery<in TQuery, TResponse> : IJournalDatabaseQuery<TResponse>
      where TQuery:IQuery<TResponse>
   {
      /// <summary>
      ///    Execute the command in a connection
      /// </summary>
      TResponse QueryInConnection(TQuery payload);

      /// <summary>
      ///    Executes the command assuming that database connection was already established
      /// </summary>
      TResponse Query(TQuery query);
   }

   public abstract class JournalDatabaseQuery<TQuery, TResponse> : JournalDatabaseAction, IJournalDatabaseQuery<TQuery, TResponse>
            where TQuery : IQuery<TResponse>

   {
      protected JournalDatabaseQuery(IJournalSession journalSession)
         : base(journalSession)
      {
      }

      public TResponse QueryInConnection(TQuery payload)
      {
         return Db.DoInConnection(() => Query(payload));
      }

      public abstract TResponse Query(TQuery query);

      public TResponse QueryInConnection(IQuery<TResponse> payload)
      {
         return QueryInConnection(payload.DowncastTo<TQuery>());
      }
   }
}