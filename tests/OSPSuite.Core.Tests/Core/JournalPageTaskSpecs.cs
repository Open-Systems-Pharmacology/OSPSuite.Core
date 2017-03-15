using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Journal;
using OSPSuite.Infrastructure.Journal.Commands;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalPageTask : ContextSpecification<IJournalPageTask>
   {
      protected IEventPublisher _eventPublisher;
      protected IDatabaseMediator _databaseMediator;
      protected JournalPage _journalPage;
      protected IDialogCreator _dialogCreator;
      private IJournalRetriever _journalRetriever;
      protected Journal.Journal _journal;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _databaseMediator = A.Fake<IDatabaseMediator>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         _journal = new Journal.Journal();

         A.CallTo(() => _journalRetriever.Current).Returns(_journal);

         sut = new JournalPageTask(_databaseMediator, _eventPublisher, _dialogCreator);

         _journalPage = new JournalPage();
         _journal.AddJournalPage(_journalPage);
      }
   }

   public class When_adding_a_related_item_to_a_journal_page : concern_for_JournalPageTask
   {
      private AddRelatedPageToJournalPage _payload;
      private JournalPageUpdatedEvent _event;
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem();

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<AddRelatedPageToJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<AddRelatedPageToJournalPage>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalPageUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalPageUpdatedEvent>(0));
      }

      protected override void Because()
      {
         sut.AddRelatedItemTo(_journalPage, _relatedItem);
      }

      [Observation]
      public void should_add_the_related_item_to_the_database()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
         _payload.RelatedItem.ShouldBeEqualTo(_relatedItem);
      }

      [Observation]
      public void should_add_the_related_item_to_the_journal_page()
      {
         _journalPage.RelatedItems.ShouldContain(_relatedItem);
      }

      [Observation]
      public void should_notify_the_update_of_the_journal_page()
      {
         _event.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }

   public class When_deleting_multiple_related_items_from_a_journal_page : concern_for_JournalPageTask
   {
      private RelatedItem _relatedItem;
      private RelatedItem _relatedItem2;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem();
         _relatedItem2 = new RelatedItem();
         _journalPage.AddRelatedItem(_relatedItem);
         _journalPage.AddRelatedItem(_relatedItem2);
         _journal = new Journal.Journal();
         _journal.AddJournalPage(_journalPage);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteMultipleRelatedItems)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.DeleteRelatedItemsFrom(_journal, new[] {_relatedItem, _relatedItem2});
      }

      [Observation]
      public void should_asks_the_user_if_he_really_wants_to_delete_the_related_items()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteMultipleRelatedItems)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_related_item_from_the_journal_page()
      {
         _journalPage.RelatedItems.ShouldNotContain(_relatedItem);
      }
   }

   public class When_deleting_a_related_item_from_a_journal_page : concern_for_JournalPageTask
   {
      private RelatedItem _relatedItem;
      private JournalPageUpdatedEvent _event;
      private DeleteRelatedItemFromJournalPage _payload;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem();

         _journalPage.AddRelatedItem(_relatedItem);

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<DeleteRelatedItemFromJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<DeleteRelatedItemFromJournalPage>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalPageUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalPageUpdatedEvent>(0));

         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteRelatedItem(_relatedItem.Display))).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.DeleteRelatedItemFrom(_journal, _relatedItem);
      }

      [Observation]
      public void should_asks_the_user_if_he_really_wants_to_delete_the_related_item()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteRelatedItem(_relatedItem.Display))).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_related_item_from_the_database()
      {
         _payload.RelatedItem.ShouldBeEqualTo(_relatedItem);
      }

      [Observation]
      public void should_remove_the_related_item_from_the_journal_page()
      {
         _journalPage.RelatedItems.ShouldNotContain(_relatedItem);
      }

      [Observation]
      public void should_notify_the_update_of_the_journal_page()
      {
         _event.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }


   public class When_the_user_deletes_a_related_item_from_the_edited_journal_page_and_cancels_the_action : concern_for_JournalPageTask
   {
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem { Name = "Sim" };
         _journalPage.AddRelatedItem(_relatedItem);
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.DeleteRelatedItemFrom(_journal, _relatedItem);
      }

      [Observation]
      public void should_not_delete_the_related_item()
      {
         A.CallTo(() => _databaseMediator.ExecuteCommand(A<DeleteRelatedItemFromJournalPage>._)).MustNotHaveHappened();
      }
   }

   public class When_adding_a_journal_as_parent_of_a_journal_page : concern_for_JournalPageTask
   {
      private JournalPageUpdatedEvent _event;
      private UpdateJournalPage _payload;
      private JournalPage _parentJournalPage;

      protected override void Context()
      {
         base.Context();

         _parentJournalPage = new JournalPage().WithId("X");

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<UpdateJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<UpdateJournalPage>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalPageUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalPageUpdatedEvent>(0));
      }

      protected override void Because()
      {
         sut.AddAsParentTo(_journalPage, _parentJournalPage);
      }

      [Observation]
      public void should_update_the_parent_id()
      {
         _journalPage.ParentId.ShouldBeEqualTo(_parentJournalPage.Id);
      }

      [Observation]
      public void should_notify_the_update_of_the_journal_page()
      {
         _event.JournalPage.ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_update_the_journal_page_in_the_database()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }

   public class When_deleting_the_parent_of_a_journal_page : concern_for_JournalPageTask
   {
      private JournalPageUpdatedEvent _event;
      private UpdateJournalPage _payload;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<UpdateJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<UpdateJournalPage>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalPageUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalPageUpdatedEvent>(0));

         _journalPage.ParentId = "XXX";
      }

      protected override void Because()
      {
         sut.DeleteParentFrom(_journalPage);
      }

      [Observation]
      public void should_update_the_parent_id()
      {
         _journalPage.ParentId.ShouldBeNull();
      }

      [Observation]
      public void should_notify_the_update_of_the_journal_page()
      {
         _event.JournalPage.ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_update_the_journal_page_in_the_database()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }

}