using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.UICommands
{
   public class SearchJournalUICommand : IUICommand
   {
      private readonly IJournalPresenter _journalPresenter;

      public SearchJournalUICommand(IJournalPresenter journalPresenter)
      {
         _journalPresenter = journalPresenter;
      }

      public void Execute()
      {
         _journalPresenter.ShowSearch();
      }
   }
}