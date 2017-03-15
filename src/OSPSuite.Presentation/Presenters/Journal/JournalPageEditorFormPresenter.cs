using System.Drawing;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalPageEditorFormPresenter : IPresenter<IJournalPageEditorFormView>,
      IMainViewItemPresenter,
      IListener<EditJournalPageStartedEvent>,
      IListener<ProjectSavedEvent>,
      IListener<ProjectClosingEvent>,
      IListener<JournalPageDeletedEvent>

   {
      void Display();
      void Edit(JournalPage journalPage);
      void FormClosing(Point location, Size size);
   }

   public class JournalPageEditorFormPresenter :
      AbstractCommandCollectorPresenter<IJournalPageEditorFormView, IJournalPageEditorFormPresenter>,
      IJournalPageEditorFormPresenter
   {
      private readonly IJournalPageEditorPresenter _journalPageEditorPresenter;
      private readonly IPresentationUserSettings _userSettings;

      public JournalPageEditorFormPresenter(IJournalPageEditorFormView view, IJournalPageEditorPresenter journalPageEditorPresenter, IPresentationUserSettings userSettings) : base(view)
      {
         _journalPageEditorPresenter = journalPageEditorPresenter;
         _userSettings = userSettings;
         view.AttachWorkingJournalItemEditorView(journalPageEditorPresenter.View);
         AddSubPresenters(_journalPageEditorPresenter);
      }

      public void Display()
      {
         var journalPageEditorSettings = _userSettings.JournalPageEditorSettings;
         _view.SetFormLayout(journalPageEditorSettings.Location, journalPageEditorSettings.Size);
         _view.Display();
      }

      public void FormClosing(Point location, Size size)
      {
         save();
         hideView();
         saveFormLayout(location, size);
      }

      private void saveFormLayout(Point location, Size size)
      {
         _userSettings.JournalPageEditorSettings.Location = location;
         _userSettings.JournalPageEditorSettings.Size= size;
      }

      private void hideView()
      {
         _journalPageEditorPresenter.UpdateUserPreferences();
         _view.Hide();
      }

      private void save()
      {
         _journalPageEditorPresenter.Save();
      }

      public void Edit(JournalPage journalPage)
      {
         _journalPageEditorPresenter.Edit(journalPage);
      }

      public void Handle(EditJournalPageStartedEvent eventToHandle)
      {
         if (!eventToHandle.ShowEditor) return;
         Display();
      }

      public void ToggleVisibility()
      {
         _view.ToggleVisibility();
      }

      public void Handle(ProjectSavedEvent eventToHandle)
      {
         save();
      }

      public void Handle(ProjectClosingEvent eventToHandle)
      {
         closeForm();
      }

      private void closeForm()
      {
         _journalPageEditorPresenter.Clear();
         hideView();
      }

      public void Handle(JournalPageDeletedEvent eventToHandle)
      {
         if (_journalPageEditorPresenter.AlreadyEditing(eventToHandle.JournalPage))
            closeForm();
      }
   }
}