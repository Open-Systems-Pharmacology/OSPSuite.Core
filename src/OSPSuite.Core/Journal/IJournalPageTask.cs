using System.Collections.Generic;

namespace OSPSuite.Core.Journal
{
   public interface IJournalPageTask
   {
      /// <summary>
      /// Saves the tags defined in <paramref name="journalPage"/> into the database
      /// </summary>
      void UpdateTagsIn(JournalPage journalPage);

      /// <summary>
      /// Sets <paramref name="tags"/> into the <paramref name="journalPage"/> and update the <paramref name="journalPage"/> in the database
      /// </summary>
      void UpdateTags(JournalPage journalPage, IEnumerable<string> tags);

      /// <summary>
      ///    Adds the <paramref name="relatedItem" /> to the <paramref name="journalPage" />
      /// </summary>
      void AddRelatedItemTo(JournalPage journalPage, RelatedItem relatedItem);

      /// <summary>
      /// Adds a new related item to the <paramref name="journalPage" /> from a file selected by the user.
      /// </summary>
      void AddRelatedItemFromFile(JournalPage journalPage);

      /// <summary>
      ///    Deletes the <paramref name="relatedItem" /> from the <paramref name="journal" />
      /// </summary>
      void DeleteRelatedItemFrom(Journal journal, RelatedItem relatedItem);

      /// <summary>
      ///   Deletes a list of related items from the <paramref name="journal"/>
      /// </summary>
      void DeleteRelatedItemsFrom(Journal journal, IReadOnlyList<RelatedItem> relatedItems);

      /// <summary>
      ///    Sets <paramref name="parentJournalPage" /> as parent of <paramref name="journalPage" />
      /// </summary>
      void AddAsParentTo(JournalPage journalPage, JournalPage parentJournalPage);

      /// <summary>
      ///    Deletes any existing parent relationship from <paramref name="journalPage" />
      /// </summary>
      /// <param name="journalPage"></param>
      void DeleteParentFrom(JournalPage journalPage);

      /// <summary>
      ///    Updates the <paramref name="journalPage" /> in the database, assuming that it was already created
      /// </summary>
      void UpdateJournalPage(JournalPage journalPage);
   }
}