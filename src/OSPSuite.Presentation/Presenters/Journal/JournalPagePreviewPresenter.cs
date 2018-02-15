using System.Collections.Generic;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalPagePreviewPresenter : IPresenter
   {
      void Preview(JournalPageDTO journalPageDTO);
      void ClearPreview();
      IEnumerable<string> AllKnownTags { get; }
      void UpdateTags(IEnumerable<string> tags);
   }

   public class JournalPagePreviewPresenter : AbstractCommandCollectorPresenter<IJournalPagePreviewView, IJournalPagePreviewPresenter>, IJournalPagePreviewPresenter
   {
      private readonly IJournalTask _journalTask;
      private readonly IJournalPageTask _journalPageTask;
      private readonly IRelatedItemsPresenter _relatedItemsPresenter;
      private JournalPageDTO _journalPageDTO;

      public JournalPagePreviewPresenter(
         IJournalPagePreviewView view, 
         IJournalTask journalTask,
         IJournalPageTask journalPageTask, 
         IRelatedItemsPresenter relatedItemsPresenter)
         : base(view)
      {
         _journalTask = journalTask;
         _journalPageTask = journalPageTask;
         _relatedItemsPresenter = relatedItemsPresenter;
         _view.AddRelatedItemsView(_relatedItemsPresenter.View);
         AddSubPresenters(_relatedItemsPresenter);
      }

      public void Preview(JournalPageDTO journalPageDTO)
      {
         _journalPageDTO = journalPageDTO;
         if (_journalPageDTO != null)
            rebind();
         else
            ClearPreview();
      }

      private void rebind()
      {
         _view.BindTo(_journalPageDTO);
         _relatedItemsPresenter.Edit(journalPage);
      }

      public void ClearPreview()
      {
         _journalPageDTO = null;
         _view.DeleteBinding();
         _relatedItemsPresenter.DeleteBinding();
      }

      public IEnumerable<string> AllKnownTags => _journalTask.AllKnownTags;

      public void UpdateTags(IEnumerable<string> tags)
      {
         _journalPageTask.UpdateTags(journalPage, tags);
      }

      private JournalPage journalPage => _journalPageDTO.JournalPage;
   }
}