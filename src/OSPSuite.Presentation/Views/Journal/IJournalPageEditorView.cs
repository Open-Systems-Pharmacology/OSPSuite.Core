using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalPageEditorView : IView<IJournalPageEditorPresenter>
   {
      void BindTo(JournalPageDTO journalPageDTO);

      /// <summary>
      /// If any of the components of the view have changes returns true
      /// Otherwise returns false
      /// </summary>
      bool HasChanges();

      /// <summary>
      /// Saves the document, tags and title
      /// </summary>
      void SaveDocument();

      /// <summary>
      /// Enables the save button in the text editor
      /// </summary>
      void EnableSaveButton();

      /// <summary>
      /// Refresh tags to the tag control
      /// </summary>
      void RefreshTags();

      void DeleteBinding();

      /// <summary>
      /// Shows the seacch in the editor using the search criteria defined in <paramref name="journalSearch"/>
      /// </summary>
      void ShowSearch(JournalSearch journalSearch);

      void ApplyUserSettingsToRichEdit(JournalPageEditorSettings settings);

      /// <summary>
      /// Reads the current value of grid lines visibility
      /// </summary>
      /// <returns>True if grid lines are visible when the table lines are not visible, otherwise false</returns>
      bool GetGridLinesPreference();
   }
}