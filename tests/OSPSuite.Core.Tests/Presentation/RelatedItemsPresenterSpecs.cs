using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RelatedItemsPresenter : ContextSpecification<IRelatedItemsPresenter>
   {
      protected IRelatedItemsView _view;
      protected IDialogCreator _dialogCreator;
      protected JournalPage _journalPage;
      protected IJournalPageTask _journalPageTask;
      private IRelatedItemComparablePresenter _relatedItemComparablePresenter;
      protected IReloadRelatedItemTask _reloadRelatedItemTask;
      protected IJournalRetriever _journalRetriever;

      protected override void Context()
      {
         _view = A.Fake<IRelatedItemsView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _journalPageTask = A.Fake<IJournalPageTask>();
         _relatedItemComparablePresenter = A.Fake<IRelatedItemComparablePresenter>();
         _reloadRelatedItemTask = A.Fake<IReloadRelatedItemTask>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         sut = new RelatedItemsPresenter(_view, _journalRetriever, _journalPageTask, _relatedItemComparablePresenter, _reloadRelatedItemTask);
         _journalPage = new JournalPage();
      }
   }

   public class When_the_user_deletes_a_related_item_from_the_edited_journal_page : concern_for_RelatedItemsPresenter
   {
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem {Name = "Sim"};
         _journalPage.AddRelatedItem(_relatedItem);
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.DeleteRelatedItem(_relatedItem);
      }

      [Observation]
      public void should_really_delete_the_related_item()
      {
         A.CallTo(() => _journalPageTask.DeleteRelatedItemFrom(_journalRetriever.Current, _relatedItem)).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_reload_a_related_item_into_the_project : concern_for_RelatedItemsPresenter
   {
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem {Name = "Sim"};
      }

      protected override void Because()
      {
         sut.ReloadRelatedItem(_relatedItem);
      }

      [Observation]
      public void should_leverage_the_reload_related_item_task_to_reload_the_item()
      {
         A.CallTo(() => _reloadRelatedItemTask.Load(_relatedItem)).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_add_related_item_from_file : concern_for_RelatedItemsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.AddRelatedItemFromFile();
      }

      [Observation]
      public void should_leverage_the_journal_page_task_to_add_a_related_item_to_the_edited_page()
      {
         A.CallTo(() => _journalPageTask.AddRelatedItemFromFile(_journalPage)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_view()
      {
         //one for edit and one for add items
         A.CallTo(() => _view.BindTo(_journalPage.RelatedItems)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_user_decides_to_reimport_all_related_items_into_the_app : concern_for_RelatedItemsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
        sut.ReloadAllRelatedItems();
      }

      [Observation]
      public void should_levrage_the_reload_item_task_to_load_all_application_items_back_into_the_app()
      {
         A.CallTo(() => _reloadRelatedItemTask.ImportAllIntoApplication(_journalPage.RelatedItems)).MustHaveHappened();   
      }
   }
}