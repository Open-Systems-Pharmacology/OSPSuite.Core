using System;
using System.Linq;
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
   public abstract class concern_for_JournalTask : ContextSpecification<IJournalTask>
   {
      protected IJournalPageFactory _journalPageFactory;
      protected IDatabaseMediator _databaseMediator;
      protected IJournalRetriever _journalRetriever;
      protected IEventPublisher _eventPublisher;
      protected IJournalSessionConnector _journalSessionConnector;
      protected IRelatedItemFactory _relatedItemFactory;
      protected Journal.Journal _journal;
      protected JournalPage _journalPage;
      protected IJournalPageTask _journalPageTask;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _journalPageFactory = A.Fake<IJournalPageFactory>();
         _databaseMediator = A.Fake<IDatabaseMediator>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _journalSessionConnector = A.Fake<IJournalSessionConnector>();
         _relatedItemFactory = A.Fake<IRelatedItemFactory>();
         _journalPageTask = A.Fake<IJournalPageTask>();

         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new JournalTask(_journalPageFactory,
            _databaseMediator, _journalRetriever, _eventPublisher, _journalSessionConnector, _relatedItemFactory, _journalPageTask, _dialogCreator);


         _journal = new Journal.Journal();
         _journalRetriever.Current  = _journal;

         _journalPage = new JournalPage();
      }
   }

   public class When_creating_a_new_journal_page_when_a_journal_is_assigned_to_the_current_project : concern_for_JournalTask
   {
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _filePath = "WorkingJournalPath";
         A.CallTo(() => _journalRetriever.JournalFullPath).Returns(_filePath);
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(true);
      }

      protected override void Because()
      {
         sut.CreateJournalPage();
      }

      [Observation]
      public void should_not_ask_the_user_to_connect_to_a_journal()
      {
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_filePath)).MustNotHaveHappened();
      }
   }

   public class When_creating_a_new_journal_page_when_a_journal_is_not_connected_to_the_current_project : concern_for_JournalTask
   {
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _filePath = "WorkingJournalPath";
         A.CallTo(() => _journalRetriever.JournalFullPath).Returns(_filePath);
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(false);
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_filePath)).Returns(_journal);
      }

      protected override void Because()
      {
         sut.CreateJournalPage();
      }

      [Observation]
      public void should_ask_the_user_to_connect_to_a_journal()
      {
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_filePath)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_current_journal()
      {
         _journalRetriever.Current.ShouldBeEqualTo(_journal);
      }
   }

   public class When_creating_a_new_journal_page_when_a_journal_is_not_connected_to_the_current_project_and_the_user_cancels_the_selection : concern_for_JournalTask
   {
      private Journal.Journal _previousJournal;

      protected override void Context()
      {
         base.Context();
         _previousJournal = new Journal.Journal();
         _journalRetriever.Current = _previousJournal;
         A.CallTo(() => _journalRetriever.JournalFullPath).Returns(string.Empty);
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(false);
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_journalRetriever.JournalFullPath)).Returns(null);
      }

      protected override void Because()
      {
         sut.CreateJournalPage();
      }

      [Observation]
      public void should_ask_the_user_to_connect_to_a_journal()
      {
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_journalRetriever.JournalFullPath)).MustHaveHappened();
      }

      [Observation]
      public void should_not_update_the_current_journal()
      {
         _journalRetriever.Current.ShouldBeEqualTo(_previousJournal);
      }
   }

   public class When_the_user_is_connecting_to_a_journal : concern_for_JournalTask
   {
      private Journal.Journal _previousJournal;
      private Journal.Journal _newJournal;

      protected override void Context()
      {
         base.Context();
         _previousJournal = new Journal.Journal();
         _newJournal = new Journal.Journal();
         _journalRetriever.Current = _previousJournal;
         A.CallTo(() => _journalSessionConnector.SelectJournal()).Returns(_newJournal);
      }

      protected override void Because()
      {
         sut.SelectJournal();
      }

      [Observation]
      public void should_ask_the_user_to_connect_to_a_journal()
      {
         A.CallTo(() => _journalSessionConnector.SelectJournal()).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_current_journal()
      {
         _journalRetriever.Current.ShouldBeEqualTo(_newJournal);
      }

      [Observation]
      public void should_show_the_journal_to_the_user()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<ShowJournalEvent>._)).MustHaveHappened();
      }
   }

   public class When_reloading_a_journal : concern_for_JournalTask
   {
      private Journal.Journal _newJournal;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(true);
         _newJournal = new Journal.Journal();
         A.CallTo(() => _journalRetriever.JournalFullPath).Returns("XXX");
         A.CallTo(() => _journalSessionConnector.ConnectToJournal(_journalRetriever.JournalFullPath)).Returns(_newJournal);
      }

      protected override void Because()
      {
         sut.ReloadJournal();
      }

      [Observation]
      public void should_reload_the_journal_by_path_defined_in_the_current_project()
      {
         _journalRetriever.Current.ShouldBeEqualTo(_newJournal);
      }
   }

   public class When_the_user_is_saving_the_journal_diagram : concern_for_JournalTask
   {
      private JournalDiagram _journalDiagram;
      private UpdateJournalDiagram _payload;
      private JournalDiagramUpdatedEvent _event;

      protected override void Context()
      {
         base.Context();

         _journalDiagram = new JournalDiagram();

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<UpdateJournalDiagram>._))
            .Invokes(x => _payload = x.GetArgument<UpdateJournalDiagram>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalDiagramUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalDiagramUpdatedEvent>(0));
      }

      protected override void Because()
      {
         sut.SaveJournalDiagram(_journalDiagram);
      }

      [Observation]
      public void should_save_the_journal_diagram_to_the_database()
      {
         _payload.Diagram.ShouldBeEqualTo(_journalDiagram);
      }

      [Observation]
      public void should_notify_the_journal_diagram_save_event()
      {
         _event.Diagram.ShouldBeEqualTo(_journalDiagram);
      }
   }

   public class When_the_user_is_connecting_to_a_journal_and_decides_to_cancel_the_action : concern_for_JournalTask
   {
      private Journal.Journal _previousJournal;

      protected override void Context()
      {
         base.Context();
         _previousJournal = A.Fake<Journal.Journal>();
         _journalRetriever.Current = _previousJournal;
         A.CallTo(() => _journalSessionConnector.SelectJournal()).Returns(null);
      }

      protected override void Because()
      {
         sut.SelectJournal();
      }

      [Observation]
      public void should_ask_the_user_to_connect_to_a_journal()
      {
         A.CallTo(() => _journalSessionConnector.SelectJournal()).MustHaveHappened();
      }

      [Observation]
      public void should_not_update_the_current_journal()
      {
         _journalRetriever.Current.ShouldBeEqualTo(_previousJournal);
      }
   }

   public class When_saving_a_new_journal_page : concern_for_JournalTask
   {
      private CreateJournalPage _payload;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _databaseMediator.ExecuteCommand(A<CreateJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<CreateJournalPage>(0));
      }

      protected override void Because()
      {
         sut.SaveJournalPage(_journalPage);
      }

      [Observation]
      public void should_create_a_journal_page_in_the_database()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }

   public class When_saving_a_existing_journal_page : concern_for_JournalTask
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.Id = "XXX";
      }

      protected override void Because()
      {
         sut.SaveJournalPage(_journalPage);
      }

      [Observation]
      public void should_update_the_journal_page_in_the_database()
      {
         A.CallTo(() => _journalPageTask.UpdateJournalPage(_journalPage)).MustHaveHappened();
      }
   }

   public class When_deleting_a_journal_page : concern_for_JournalTask
   {
      private DeleteJournalPage _payload;
      private JournalPageDeletedEvent _event;

      protected override void Context()
      {
         base.Context();
         _journal.AddJournalPage(_journalPage);

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<DeleteJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<DeleteJournalPage>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalPageDeletedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalPageDeletedEvent>(0));

         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteJournalPage(_journalPage.Title))).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.DeleteJournalPage(_journalPage);
      }

      [Observation]
      public void should_asks_the_user_if_he_really_wants_to_delete_the_related_item()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteJournalPage(_journalPage.Title))).MustHaveHappened();
      }

      [Observation]
      public void should_delete_the_journal_page_from_the_database()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_remove_the_deleted_item_from_the_journal()
      {
         _journal.JournalPages.ShouldNotContain(_journalPage);
      }

      [Observation]
      public void should_notify_the_deletion_of_the_journal_page()
      {
         _event.JournalPage.ShouldBeEqualTo(_journalPage);
      }
   }

   public class When_deleting_multiple_pages : concern_for_JournalTask
   {
      private JournalPage _journalPage2;

      protected override void Context()
      {
         base.Context();
         _journalPage2 = new JournalPage();
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteMultipleRelatedItems)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.DeleteJournalPages(new[] {_journalPage, _journalPage2});
      }

      [Observation]
      public void should_asks_the_user_if_he_really_wants_to_delete_the_related_items()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyDeleteMultipleJournalPages)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_deleted_item_from_the_journal()
      {
         _journal.JournalPages.ShouldNotContain(_journalPage);
         _journal.JournalPages.ShouldNotContain(_journalPage2);
      }
   }

   public class When_the_user_triggers_the_deletion_of_an_existing_journal_page_and_cancels_the_action : concern_for_JournalTask
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.Title = "ABC";
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.DeleteJournalPage(_journalPage);
      }

      [Observation]
      public void should_not_delete_the_journal_page()
      {
         A.CallTo(() => _databaseMediator.ExecuteCommand(A<DeleteJournalPage>._)).MustNotHaveHappened();
      }
   }

   public class When_creating_a_new_journal_page_while_a_previous_journal_page_was_already_created : concern_for_JournalTask
   {
      private JournalPage _newJournalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage.Id = "XYZ";
         _newJournalPage = new JournalPage();
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(true);
         _journal.AddJournalPage(_journalPage);
         A.CallTo(() => _journalPageFactory.Create()).Returns(_newJournalPage);
      }

      protected override void Because()
      {
         sut.CreateJournalPage();
      }

      [Observation]
      public void should_set_the_last_created_journal_as_parent_of_the_newly_created_journal()
      {
         A.CallTo(() => _journalPageTask.AddAsParentTo(_newJournalPage, _journalPage)).MustHaveHappened();
      }
   }

   public class When_adding_a_related_item_to_the_journal_and_no_item_is_being_edited : concern_for_JournalTask
   {
      private IObjectBase _objectBase;
      private RelatedItem _relatedItem;
      private CreateJournalPage _payload;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(true);
         _objectBase = A.Fake<IObjectBase>();
         _relatedItem = new RelatedItem();
         A.CallTo(() => _relatedItemFactory.Create(_objectBase)).Returns(_relatedItem);
         A.CallTo(() => _journalPageFactory.Create()).Returns(_journalPage);
         _journal.Edited = null;

         A.CallTo(() => _databaseMediator.ExecuteCommand(A<CreateJournalPage>._))
            .Invokes(x => _payload = x.GetArgument<CreateJournalPage>(0));
      }

      protected override void Because()
      {
         sut.AddAsRelatedItemsToJournal(new[] {_objectBase});
      }

      [Observation]
      public void should_create_a_new_journal_page_and_add_the_realted_item_to_the_new_journal_page()
      {
         _payload.JournalPage.ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_add_the_new_related_item_to_the_database()
      {
         A.CallTo(() => _journalPageTask.AddRelatedItemTo(_journalPage, _relatedItem)).MustHaveHappened();
      }
   }

   public class When_adding_a_related_item_to_the_edited_journal_page : concern_for_JournalTask
   {
      private IObjectBase _objectBase;
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _journalSessionConnector.IsConnected).Returns(true);
         _objectBase = A.Fake<IObjectBase>();
         _relatedItem = new RelatedItem();
         A.CallTo(() => _relatedItemFactory.Create(_objectBase)).Returns(_relatedItem);
         _journal.Edited = _journalPage;
      }

      protected override void Because()
      {
         sut.AddAsRelatedItemsToJournal(new[] {_objectBase});
      }

      [Observation]
      public void should_add_the_new_related_item_to_the_database()
      {
         A.CallTo(() => _journalPageTask.AddRelatedItemTo(_journalPage, _relatedItem)).MustHaveHappened();
      }
   }

   public class When_creating_a_newline_limited_description_from_item_content : concern_for_JournalTask
   {
      private string _result;

      protected override void Because()
      {
         var documentContent = new string('5', 500);
         documentContent = documentContent.Insert(10, Environment.NewLine);
         _result = sut.CreateItemDescriptionFromContent(documentContent);
      }

      [Observation]
      public void should_be_limited_to_characters_before_newline()
      {
         _result.Count().ShouldBeEqualTo(10);
      }
   }

   public class When_creating_a_character_limited_description_from_item_content : concern_for_JournalTask
   {
      private string _result;

      protected override void Because()
      {
         _result = sut.CreateItemDescriptionFromContent(new string('5', 500));
      }

      [Observation]
      public void should_be_limited_to_50_characters()
      {
         _result.Count().ShouldBeEqualTo(50);
      }
   }
}