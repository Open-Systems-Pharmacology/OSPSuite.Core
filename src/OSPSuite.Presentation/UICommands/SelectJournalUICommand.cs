using OSPSuite.Core.Journal;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class SelectJournalUICommand : IUICommand
   {
      private readonly IJournalTask _journalTask;

      public SelectJournalUICommand(IJournalTask journalTask)
      {
         _journalTask = journalTask;
      }

      public void Execute()
      {
         _journalTask.SelectJournal();
      }
   }
}