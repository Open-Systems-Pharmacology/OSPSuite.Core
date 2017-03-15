using OSPSuite.Core.Journal;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class RefreshJournalUICommand : IUICommand
   {
      private readonly IJournalTask _journalTask;

      public RefreshJournalUICommand(IJournalTask journalTask)
      {
         _journalTask = journalTask;
      }

      public void Execute()
      {
         _journalTask.ReloadJournal();
      }
   }
}