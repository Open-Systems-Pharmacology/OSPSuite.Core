using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalPageEditorPresenter : IPresenter<IJournalPageEditorView>,
      IListener<EditJournalPageStartedEvent>,
      IListener<ShowJournalSearchEvent>
   {
      void Edit(JournalPage journalPage);

      /// <summary>
      ///    Sets the content of the <paramref name="journalPage" /> to <paramref name="bytes" />
      /// </summary>
      void InitializeJournalPageContent(JournalPage journalPage, byte[] bytes);

      /// <summary>
      ///    Saves changes in <see cref="JournalPage" /> currently edited
      /// </summary>
      /// <returns>true if view should be closed, otherwise false</returns>
      void Save();

      IEnumerable<string> AllKnownTags { get; }

      /// <summary>
      ///    Changes the title of the working journal item to <paramref name="text" />
      /// </summary>
      void TitleChanged(string text);

      /// <summary>
      ///    Changes the tags of the working journal item to <paramref name="tags" />
      /// </summary>
      void TagsChanged(IEnumerable<string> tags);

      /// <summary>
      ///    Clear binding to <see cref="JournalPage" /> currently edited
      /// </summary>
      void Clear();

      IEnumerable<Origin> AllOrigins { get; }

      /// <summary>
      ///    True if the table should have gridlines visible when the table cell boundaries are not shown
      /// </summary>
      bool EnableTableGridLines { get; set; }

      /// <summary>
      ///    Called whenever the source was changed by the user
      /// </summary>
      void SourceChanged();

      /// <summary>
      ///    Returns <c>true</c> of the editor is already showing the content for <paramref name="journalPage" /> other wise
      ///    <c>false</c>
      /// </summary>
      bool AlreadyEditing(JournalPage journalPage);

      /// <summary>
      ///    Updates the user preferences from the current rich editor configuration
      /// </summary>
      void UpdateUserPreferences();

      /// <summary>
      ///    Displays the page that was defined before this one in the working journal
      /// </summary>
      void NavigateToPreviousPage();

      /// <summary>
      ///    Displays the page that was defined after this one in the working journal
      /// </summary>
      void NavigateToNextPage();
   }

   public class JournalPageEditorPresenter : AbstractPresenter<IJournalPageEditorView, IJournalPageEditorPresenter>,
      IJournalPageEditorPresenter
   {
      private readonly IContentLoader _contentLoader;
      private readonly IJournalPageToJournalPageDTOMapper _mapper;
      private readonly IJournalTask _journalTask;
      private readonly IJournalRetriever _journalRetriever;
      private JournalPageDTO _journalPageDTO;
      private readonly JournalPageEditorSettings _journalPageSettings;

      public JournalPageEditorPresenter(
         IJournalPageEditorView view,
         IContentLoader contentLoader,
         IJournalPageToJournalPageDTOMapper mapper,
         IJournalTask journalTask,
         IJournalRetriever journalRetriever,
         IPresentationUserSettings userSettings
      )
         : base(view)
      {
         _contentLoader = contentLoader;
         _mapper = mapper;
         _journalTask = journalTask;
         _journalRetriever = journalRetriever;
         _journalPageSettings = userSettings.JournalPageEditorSettings;
         _view.ApplyUserSettingsToRichEdit(_journalPageSettings);
      }

      public IEnumerable<string> AllKnownTags => _journalTask.AllKnownTags;

      public void TitleChanged(string text)
      {
         markJournalPageAsEdited();
         _journalPageDTO.Title = text;
         updateViewTitle();
      }

      private void updateViewTitle()
      {
         _view.Caption = _journalPageDTO.Caption;
      }

      private void markJournalPageAsEdited()
      {
         _journalPageDTO.Edited = true;
         enableSaveButton();
      }

      public void TagsChanged(IEnumerable<string> tags)
      {
         _journalPageDTO.Tags = tags;
         _journalPageDTO.Edited = true;
         enableSaveButton();
      }

      public void Clear()
      {
         _journalPageDTO = null;
         _view.DeleteBinding();
      }

      public IEnumerable<Origin> AllOrigins => Origins.All;

      public bool EnableTableGridLines
      {
         get => _journalPageSettings.ShowTableGridLines;
         set => _journalPageSettings.ShowTableGridLines = value;
      }

      public void SourceChanged()
      {
         markJournalPageAsEdited();
      }

      private void enableSaveButton()
      {
         _view.EnableSaveButton();
      }

      public void Edit(JournalPage journalPage)
      {
         _contentLoader.Load(journalPage);
         _journalPageDTO = _mapper.MapFrom(journalPage);
         _view.BindTo(_journalPageDTO);
         updateViewTitle();
      }

      public void InitializeJournalPageContent(JournalPage journalPage, byte[] bytes)
      {
         journalPage.Content.Data = bytes;
      }

      public void Save()
      {
         if (!_view.HasChanges()) return;
         _view.SaveDocument();
      }

      public void Handle(EditJournalPageStartedEvent eventToHandle)
      {
         if (AlreadyEditing(eventToHandle.JournalPage))
         {
            refreshTags(eventToHandle.JournalPage);
            return;
         }

         Save();
         UpdateUserPreferences();

         Edit(eventToHandle.JournalPage);
      }

      private void refreshTags(JournalPage journalPage)
      {
         //tags might have been edited outside the editor. Ensure that we are using the same tags before refreshing the view
         _journalPageDTO.Tags = journalPage.Tags;
         _view.RefreshTags();
      }

      private void showSearchIfRequired(JournalSearch journalSearch)
      {
         if (journalSearch == null) return;
         _view.ShowSearch(journalSearch);
      }

      public bool AlreadyEditing(JournalPage journalPage)
      {
         return _journalPageDTO != null && Equals(_journalPageDTO.JournalPage, journalPage);
      }

      public void UpdateUserPreferences()
      {
         _journalPageSettings.ShowTableGridLines = _view.GetGridLinesPreference();
      }

      public void NavigateToPreviousPage() => navigateToPage(-1);

      public void NavigateToNextPage() => navigateToPage(1);

      private void navigateToPage(int pageOffset)
      {
         var journalPage = _journalPageDTO?.JournalPage;
         if (journalPage == null)
            return;

         var journal = _journalRetriever.Current;
         var pageIndex = journal.JournalPageIndexFor(journalPage);
         var pageToNavigateTo = journal.JournalPageByIndex(pageIndex + pageOffset);

         if (pageToNavigateTo == null)
            return;

         _journalTask.Edit(pageToNavigateTo, showEditor: true);
      }

      public void Handle(ShowJournalSearchEvent eventToHandle)
      {
         showSearchIfRequired(eventToHandle.JournalSearch);
      }
   }
}