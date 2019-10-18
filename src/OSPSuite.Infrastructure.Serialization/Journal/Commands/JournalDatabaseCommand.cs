using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Commands
{
   public abstract class JournalPagePayload
   {
      public JournalPage JournalPage { get; set; }
   }

   public interface IJournalDatabaseCommand<T>
   {
      /// <summary>
      ///    Execute the command in a transaction
      /// </summary>
      void ExecuteInTransaction(T payload);

      /// <summary>
      ///    Executes the command assuming that database connection was already established
      /// </summary>
      void Execute(T payload);
   }

   public abstract class JournalDatabaseCommand<T> : JournalDatabaseAction, IJournalDatabaseCommand<T>
   {
      protected JournalDatabaseCommand(IJournalSession journalSession) : base(journalSession)
      {
      }

      public virtual void ExecuteInTransaction(T payload)
      {
         Db.DoInTransaction(() => Execute(payload));
      }

      public abstract void Execute(T payload);
   }
}