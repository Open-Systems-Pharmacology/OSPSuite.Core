using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_JournalPageEditorFormPresenter : ContextSpecification<JournalPageEditorFormPresenter>
   {
      protected IJournalPageEditorFormView _journalPageEditorFormView;
      protected IJournalPageEditorPresenter _journalPageEditorPresenter;
      protected JournalSearch _journalSearch;
      protected IPresentationUserSettings _userSettings;

      protected override void Context()
      {
         _journalPageEditorFormView = A.Fake<IJournalPageEditorFormView>();
         _journalPageEditorPresenter = A.Fake<IJournalPageEditorPresenter>();
         _userSettings = A.Fake<IPresentationUserSettings>();

         _journalSearch = new JournalSearch();
         sut = new JournalPageEditorFormPresenter(
            _journalPageEditorFormView,
            _journalPageEditorPresenter, _userSettings);
      }
   }

   public class When_closing_the_form : concern_for_JournalPageEditorFormPresenter
   {
      private Size _size;
      private Point _location;

      protected override void Context()
      {
         base.Context();
         _size = new Size(100, 200);
         _location = new Point(300, 400);
      }

      protected override void Because()
      {
         sut.FormClosing(_location, _size);
      }

      [Observation]
      public void the_user_settings_should_be_set_to_the_current_location_and_size()
      {
         _userSettings.JournalPageEditorSettings.Location.ShouldBeEqualTo(_location);
         _userSettings.JournalPageEditorSettings.Size.ShouldBeEqualTo(_size);
      }
   }

   public class When_handling_start_edit_event_and_the_editor_should_be_shown : concern_for_JournalPageEditorFormPresenter
   {
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage();
      }

      protected override void Because()
      {
         sut.Handle(new EditJournalPageStartedEvent(_journalPage, true));
      }

      [Observation]
      public void should_result_in_view_being_displayed()
      {
         A.CallTo(() => _journalPageEditorFormView.Display()).MustHaveHappened();
      }
   }

   public class When_handling_start_edit_event_and_the_editor_should_not_be_shown : concern_for_JournalPageEditorFormPresenter
   {
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage();
      }

      protected override void Because()
      {
         sut.Handle(new EditJournalPageStartedEvent(_journalPage, false));
      }

      [Observation]
      public void should_not_display_the_view()
      {
         A.CallTo(() => _journalPageEditorFormView.Display()).MustNotHaveHappened();
      }
   }

   public class When_handling_project_closing_event : concern_for_JournalPageEditorFormPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ProjectClosingEvent());
      }

      [Observation]
      public void should_result_in_the_form_being_hidden()
      {
         A.CallTo(() => _journalPageEditorFormView.Hide()).MustHaveHappened();
      }

      [Observation]
      public void should_not_result_in_a_call_to_save_the_current_journal_page()
      {
         A.CallTo(() => _journalPageEditorPresenter.Save()).MustNotHaveHappened();
      }
   }

   public class When_handling_a_page_deleted_event_and_page_is_not_currently_being_edited : concern_for_JournalPageEditorFormPresenter
   {
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage();

         A.CallTo(() => _journalPageEditorPresenter.AlreadyEditing(_journalPage)).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(new JournalPageDeletedEvent(_journalPage));
      }

      [Observation]
      public void should_not_have_hidden_the_view()
      {
         A.CallTo(() => _journalPageEditorFormView.Hide()).MustNotHaveHappened();
      }
   }

   public class When_handling_a_page_deleted_event_and_page_is_currently_being_edited : concern_for_JournalPageEditorFormPresenter
   {
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage();

         A.CallTo(() => _journalPageEditorPresenter.AlreadyEditing(_journalPage)).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new JournalPageDeletedEvent(_journalPage));
      }

      [Observation]
      public void should_have_hidden_the_view()
      {
         A.CallTo(() => _journalPageEditorFormView.Hide()).MustHaveHappened();
      }
   }

   public class When_handling_project_saved_event : concern_for_JournalPageEditorFormPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ProjectSavedEvent(A.Fake<IProject>()));
      }

      [Observation]
      public void should_result_in_a_call_to_save_the_current_journal_page()
      {
         A.CallTo(() => _journalPageEditorPresenter.Save()).MustHaveHappened();
      }
   }

}