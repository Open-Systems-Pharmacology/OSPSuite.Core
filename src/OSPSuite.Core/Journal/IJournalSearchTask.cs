namespace OSPSuite.Core.Journal
{
   public interface IJournalSearchTask
   {
      /// <summary>
      ///    Performs the searach according to search criteria defined in <paramref name="journalSearch" />
      /// </summary>
      void PerformSearch(JournalSearch journalSearch);

      /// <summary>
      ///    Clears the search results
      /// </summary>
      void ClearSearch();
   }
}