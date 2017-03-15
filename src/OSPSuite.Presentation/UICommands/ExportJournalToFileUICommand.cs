using System.Linq;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ExportJournalToFileUICommand : IUICommand
   {
      private readonly IJournalExportTask _journalExportTask;
      private readonly IJournalPresenter _journalPresenter;

      public ExportJournalToFileUICommand(IJournalExportTask journalExportTask, IJournalPresenter journalPresenter)
      {
         _journalExportTask = journalExportTask;
         _journalPresenter = journalPresenter;
      }

      public void Execute()
      {
         _journalExportTask.ExportSelectedPagesToWordFile(_journalPresenter.VisibleJournalPages().OrderBy(page => page.UniqueIndex).ToList());
      }
   }
}