using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Journal
{
   public interface IJournalTask
   {
      /// <summary>
      ///    Creates and save a new <see cref="JournalPage" />. If no working journal was attached to the current project,
      ///    asks the user to select one.
      /// </summary>
      /// <param name="showEditor">
      ///    if set to <c>true</c>, the new <see cref="JournalPage" /> will be edited in the editor. Default is
      ///    <c>false</c>
      /// </param>
      JournalPage CreateJournalPage(bool showEditor = false);

      /// <summary>
      ///    Allows the user to select an existing working journal
      /// </summary>
      void SelectJournal();

      /// <summary>
      ///    Loads the journal defined at <paramref name="journalFilePath" />. If <paramref name="showJournal" /> is <c>true</c>,
      ///    journal will be shown
      /// </summary>
      void LoadJournal(string journalFilePath, bool showJournal);

      /// <summary>
      ///    Saves/updates the given working journal item to the working journal
      /// </summary>
      void SaveJournalPage(JournalPage journalPage);

      /// <summary>
      ///    Adds the <paramref name="relatedItems" /> to the activate <see cref="JournalPage" /> if available
      /// </summary>
      void AddAsRelatedItemsToJournal(IReadOnlyList<IObjectBase> relatedItems);

      /// <summary>
      ///    Deletes the <paramref name="journalPage" />
      /// </summary>
      void DeleteJournalPage(JournalPage journalPage);

      /// <summary>
      ///    Deletes a list of journal pages
      /// </summary>
      void DeleteJournalPages(IReadOnlyList<JournalPage> journalPages);

      /// <summary>
      ///    Gets the  journal page currently being edited
      /// </summary>
      JournalPage JournalPageCurrentlyEdited { get; }

      /// <summary>
      ///    Starts the edit workflow for the <paramref name="journalPage" />.
      ///    if <paramref name="showEditor" /> is set to <c>true</c>, the editor will be shown.
      ///    if <paramref name="journalSearch" /> is defined and <paramref name="showEditor" /> is <c>true</c>, the search will
      ///    be instantiated in the editor
      /// </summary>
      void Edit(JournalPage journalPage, bool showEditor, JournalSearch journalSearch = null);

      /// <summary>
      /// Returns all tags available in the journal
      /// </summary>
      IEnumerable<string> AllKnownTags { get; }

      /// <summary>
      ///    Updates the <paramref name="journalDiagram" />
      /// </summary>
      void SaveJournalDiagram(JournalDiagram journalDiagram);

      /// <summary>
      ///    Creates a summary description from the <paramref name="documentContent" />
      /// </summary>
      /// <returns>A short summary of the content</returns>
      string CreateItemDescriptionFromContent(string documentContent);

      /// <summary>
      ///    Reloads the journal currently open
      /// </summary>
      void ReloadJournal();

      /// <summary>
      /// Shows the current journal if defined
      /// </summary>
      void ShowJournal();
   }
}