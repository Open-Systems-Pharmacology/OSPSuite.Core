using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.UICommands
{
   public class JournalEditorVisibiliyUICommand : ObjectUICommand<IJournalPageEditorFormPresenter>
   {
      private readonly IJournalPageEditorFormPresenter _journalPagePresenter;

      public JournalEditorVisibiliyUICommand(IJournalPageEditorFormPresenter journalPagePresenter)
      {
         _journalPagePresenter = journalPagePresenter;
      }

      protected override void PerformExecute()
      {
         _journalPagePresenter.Display();
      }
   }
}