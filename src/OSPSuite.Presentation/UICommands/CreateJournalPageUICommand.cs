using OSPSuite.Core.Journal;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class CreateJournalPageUICommand : IUICommand
   {
      private readonly IJournalTask _journalTask;

      public CreateJournalPageUICommand(IJournalTask journalTask)
      {
         _journalTask = journalTask;
      }

      public void Execute()
      {
         _journalTask.CreateJournalPage(showEditor: true);
      }
   }
}