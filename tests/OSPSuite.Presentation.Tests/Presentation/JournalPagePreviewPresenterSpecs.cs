using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_JournalPagePreviewPresenter : ContextSpecification<IJournalPagePreviewPresenter>
   {
      protected IJournalPagePreviewView _view;
      protected IDialogCreator _dialogCreator;
      protected IJournalTask _journalTask;
      protected JournalPageDTO _journalPageDTO;
      protected IJournalPageTask _journalPageTask;
      protected IRelatedItemsPresenter _relatedItemsPresenter;

      protected override void Context()
      {
         base.Context();
         _view = A.Fake<IJournalPagePreviewView>();
         _journalTask = A.Fake<IJournalTask>();
         _journalPageTask = A.Fake<IJournalPageTask>();
         _relatedItemsPresenter = A.Fake<IRelatedItemsPresenter>();
         sut = new JournalPagePreviewPresenter(_view, _journalTask, _journalPageTask, _relatedItemsPresenter);

         _journalPageDTO = new JournalPageDTO(new JournalPage());
      }
   }

   public class When_the_journal_page_preview_presenter_is_previewing_a_well_defined_journal_page : concern_for_JournalPagePreviewPresenter
   {
      protected override void Because()
      {
         sut.Preview(_journalPageDTO);
      }

      [Observation]
      public void should_preview_the_journal_page()
      {
         A.CallTo(() => _view.BindTo(_journalPageDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_related_items()
      {
         A.CallTo(() => _relatedItemsPresenter.Edit(_journalPageDTO.JournalPage)).MustHaveHappened();
      }
   }

   public class When_the_journal_page_preview_presenter_is_previewing_an_undefined_journal_page : concern_for_JournalPagePreviewPresenter
   {
      protected override void Because()
      {
         sut.Preview(null);
      }

      [Observation]
      public void should_clear_the_preview()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_related_items()
      {
         A.CallTo(() => _relatedItemsPresenter.DeleteBinding()).MustHaveHappened();
      }
   }

   public class When_the_journal_page_preview_presenter_is_clearing_the_preview : concern_for_JournalPagePreviewPresenter
   {
      protected override void Because()
      {
         sut.ClearPreview();
      }

      [Observation]
      public void should_clear_the_binding()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_related_items()
      {
         A.CallTo(() => _relatedItemsPresenter.DeleteBinding()).MustHaveHappened();
      }
   }
}