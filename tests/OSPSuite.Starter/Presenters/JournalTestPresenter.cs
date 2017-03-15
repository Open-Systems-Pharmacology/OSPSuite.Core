using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Services;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IJournalTestPresenter : IPresenter<IJournalTestView>
   {
      void AddNewPageToJournal();
      void SelectJournal();
      void ExportJournal();
      void SaveDiagram();
      void SearchJournal();
      void RestoreChronologicalOrder();
      void ShowInStandardViewer();
   }

   public class JournalTestPresenter : AbstractPresenter<IJournalTestView, IJournalTestPresenter>, IJournalTestPresenter
   {
      private readonly IJournalTask _journalTask;
      private readonly IJournalExportTask _journalExportTask;
      private readonly IJournalRetriever _journalRetriever;
      private readonly IJournalPresenter _journalPresenter;
      private readonly IJournalPageEditorFormPresenter _journalPageEditorFormPresenter;
      private readonly IJournalDiagramPresenter _journalDiagramPresenter;
      private readonly IJournalRichEditFormPresenter _journalRichEditFormPresenter;

      public JournalTestPresenter(IJournalTestView view, IJournalDiagramPresenter journalDiagramPresenter, IJournalTask journalTask, IJournalExportTask journalExportTask, IJournalRetriever journalRetriever, IJournalPresenter journalPresenter, IJournalPageEditorFormPresenter journalPageEditorFormPresenter, IJournalRichEditFormPresenter journalRichEditFormPresenter) : base(view)
      {
         _view.AddDiagram(journalDiagramPresenter.View);
         
         _journalTask = journalTask;
         _journalExportTask = journalExportTask;
         _journalRetriever = journalRetriever;
         _journalPresenter = journalPresenter;
         _journalDiagramPresenter = journalDiagramPresenter;
         _journalPageEditorFormPresenter = journalPageEditorFormPresenter;
         _journalRichEditFormPresenter = journalRichEditFormPresenter;
      }

      public void AddNewPageToJournal()
      {
         _journalTask.CreateJournalPage();
      }

      public void SelectJournal()
      {
         _journalTask.SelectJournal();
      }

      public void ExportJournal()
      {
         _journalExportTask.ExportSelectedPagesToWordFile(getCurrentPages());
      }

      private List<JournalPage> getCurrentPages()
      {
         return _journalPresenter.VisibleJournalPages().OrderBy(page => page.UniqueIndex).ToList();
      }

      public void SaveDiagram()
      {
         _journalTask.SaveJournalDiagram(_journalRetriever.Current.Diagram);
      }

      public void SearchJournal()
      {
         _journalPresenter.ShowSearch();
      }

      public void RestoreChronologicalOrder()
      {
         _journalDiagramPresenter.RestoreChronologicalOrder();
      }

      public void ShowInStandardViewer()
      {
         _journalRichEditFormPresenter.ShowJournalPages(getCurrentPages().First());
      }
   }
}
