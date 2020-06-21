using System;
using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Serialization.Journal
{
   public abstract class JournalDatabaseAction
   {
      protected readonly IJournalSession _journalSession;

      protected JournalDatabaseAction(IJournalSession journalSession)
      {
         _journalSession = journalSession;
      }

      protected JournalDatabase Db
      {
         get
         {
            if (!_journalSession.IsOpen)
               throw new ArgumentException(Error.JournalNotOpen);

            return _journalSession.Current;
         }
      }
   }
}