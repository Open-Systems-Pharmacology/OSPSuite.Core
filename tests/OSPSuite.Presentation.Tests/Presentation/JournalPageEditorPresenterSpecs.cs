using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_JournalPageEditorPresenter : ContextSpecification<IJournalPageEditorPresenter>
   {
      protected IJournalPageEditorView _view;
      protected IContentLoader _contentLoader;
      protected IEventPublisher _eventPublisher;
      protected JournalPage _journalPage;
      protected IJournalPageToJournalPageDTOMapper _mapper;
      protected JournalPageDTO _journalPageDTO;
      protected IJournalTask _journalTask;
      protected JournalSearch _journalSearch;
      private IPresentationUserSettings _userSettings;
      protected IJournalRetriever _journalRetriever;
      protected Journal _journal;

      protected override void Context()
      {
         _view = A.Fake<IJournalPageEditorView>();
         _contentLoader = A.Fake<IContentLoader>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _journalTask = A.Fake<IJournalTask>();
         _mapper = A.Fake<IJournalPageToJournalPageDTOMapper>();
         _journalSearch = new JournalSearch();
         _userSettings = A.Fake<IPresentationUserSettings>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         sut = new JournalPageEditorPresenter(_view, _contentLoader, _mapper, _journalTask, _journalRetriever, _userSettings);

         _journalPage = new JournalPage();
         _journalPageDTO = new JournalPageDTO(_journalPage);

         _journal = new Journal();
         _journal.AddJournalPage(_journalPage);
         A.CallTo(() => _journalRetriever.Current).Returns(_journal);
         A.CallTo(_mapper).WithReturnType<JournalPageDTO>().Returns(_journalPageDTO);
      }
   }

   public class when_changing_title : concern_for_JournalPageEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         _journalPageDTO.UniqueIndex = 5;
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.TitleChanged("newTitle");
      }

      [Observation]
      public void the_caption_of_the_view_must_have_been_updated()
      {
         _view.Caption.ShouldBeEqualTo("5 - newTitle");
      }

      [Observation]
      public void the_journal_page_dto_must_be_marked_as_edited()
      {
         _journalPageDTO.Edited.ShouldBeTrue();
      }

      [Observation]
      public void a_call_to_enable_the_save_button_in_the_view_must_be_made()
      {
         A.CallTo(() => _view.EnableSaveButton()).MustHaveHappened();
      }
   }

   public class When_editing_a_page_and_checking_if_that_page_is_already_being_edited : concern_for_JournalPageEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_journalPage);
      }

      [Observation]
      public void should_return_true_when_asked_if_page_being_edited_matches()
      {
         sut.AlreadyEditing(_journalPage).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_when_asked_if_page_being_edited_is_wrong_page()
      {
         sut.AlreadyEditing(new JournalPage()).ShouldBeFalse();
      }
   }

   public class when_changing_tags : concern_for_JournalPageEditorPresenter
   {
      private IEnumerable<string> _tags;

      protected override void Context()
      {
         base.Context();
         _tags = new List<string>();
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.TagsChanged(_tags);
      }

      [Observation]
      public void the_journal_page_dto_must_be_marked_as_edited()
      {
         _journalPageDTO.Edited.ShouldBeTrue();
      }

      [Observation]
      public void the_tags_on_the_dto_must_be_updated()
      {
         _journalPageDTO.Tags.ShouldBeEqualTo(_tags);
      }

      [Observation]
      public void a_call_to_enable_the_save_button_in_the_view_must_be_made()
      {
         A.CallTo(() => _view.EnableSaveButton()).MustHaveHappened();
      }
   }

   public class when_editing_journal_page : concern_for_JournalPageEditorPresenter
   {
      protected override void Because()
      {
         sut.Edit(_journalPage);
      }

      [Observation]
      public void content_must_be_loaded()
      {
         A.CallTo(() => _contentLoader.Load(_journalPage)).MustHaveHappened();
      }

      [Observation]
      public void view_must_be_bound_to_the_item()
      {
         A.CallTo(() => _view.BindTo(_journalPageDTO)).MustHaveHappened();
      }
   }

   public class when_handling_a_edit_start_command_when_already_edting_the_page : concern_for_JournalPageEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.Handle(new EditJournalPageStartedEvent(_journalPage, true));
      }

      [Observation]
      public void a_call_to_load_content_must_not_have_happened()
      {
         A.CallTo(() => _contentLoader.Load(_journalPage)).MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void the_presenter_should_not_edit_the_item_contained_in_the_event()
      {
         A.CallTo(() => _view.BindTo(_journalPageDTO)).MustHaveHappenedOnceExactly();
      }
   }

   public class when_handling_a_edit_start_command : concern_for_JournalPageEditorPresenter
   {
      protected override void Because()
      {
         sut.Handle(new EditJournalPageStartedEvent(_journalPage, true));
      }

      [Observation]
      public void the_presenter_must_edit_the_item_contained_in_the_event()
      {
         // Checking that the edit was performed by making sure the content was loaded
         // and bound to the view
         A.CallTo(() => _contentLoader.Load(_journalPage)).MustHaveHappened();
         A.CallTo(() => _view.BindTo(_journalPageDTO)).MustHaveHappened();
      }
   }

   public class When_notifiy_that_the_search_form_should_be_shown : concern_for_JournalPageEditorPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ShowJournalSearchEvent(_journalSearch));
      }

      [Observation]
      public void should_show_the_form_in_the_view()
      {
         A.CallTo(() => _view.ShowSearch(_journalSearch)).MustHaveHappened();
      }
   }

   public class When_save_before_exit_is_confirmed : concern_for_JournalPageEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.HasChanges()).Returns(true);
         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void the_view_must_be_used_to_save_the_content()
      {
         A.CallTo(() => _view.SaveDocument()).MustHaveHappened();
      }
   }

   public class When_navigating_to_the_previous_page : concern_for_JournalPageEditorPresenter
   {
      private JournalPage _journalPage1;
      private JournalPage _journalPage3;

      protected override void Context()
      {
         base.Context();
         _journal.Remove(_journalPage);
         _journalPage1 = new JournalPage();
         _journalPage3 = new JournalPage();
         _journal.AddJournalPage(_journalPage1);
         _journal.AddJournalPage(_journalPage);
         _journal.AddJournalPage(_journalPage3);

         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.NavigateToPreviousPage();
      }

      [Observation]
      public void should_retrieve_the_previous_page_from_the_journal_and_start_the_edit_process()
      {
         A.CallTo(() => _journalTask.Edit(_journalPage1, true, null)).MustHaveHappened();
      }
   }

   public class When_navigating_to_the_next_page : concern_for_JournalPageEditorPresenter
   {
      private JournalPage _journalPage1;
      private JournalPage _journalPage3;

      protected override void Context()
      {
         base.Context();
         _journal.Remove(_journalPage);
         _journalPage1 = new JournalPage();
         _journalPage3 = new JournalPage();
         _journal.AddJournalPage(_journalPage1);
         _journal.AddJournalPage(_journalPage);
         _journal.AddJournalPage(_journalPage3);

         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.NavigateToNextPage();
      }

      [Observation]
      public void should_retrieve_the_previous_page_from_the_journal_and_start_the_edit_process()
      {
         A.CallTo(() => _journalTask.Edit(_journalPage3, true, null)).MustHaveHappened();
      }
   }

   public class When_navigating_to_the_previous_page_from_the_first_page : concern_for_JournalPageEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         _journal.AddJournalPage(_journalPage);

         sut.Edit(_journalPage);
      }

      protected override void Because()
      {
         sut.NavigateToPreviousPage();
      }

      [Observation]
      public void should_not_navigate_to_a_new_page()
      {
         A.CallTo(() => _journalTask.Edit(A<JournalPage>._, A<bool>._, null)).MustNotHaveHappened();
      }
   }
}