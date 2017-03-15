using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure.Journal
{
   public class JournalSearchTask : IJournalSearchTask
   {
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IJournalRetriever _journalRetriever;
      private readonly IEventPublisher _eventPublisher;
      private readonly IJournalSearchContextFinder _searchContextFinder;

      public JournalSearchTask(IDatabaseMediator databaseMediator, IJournalRetriever journalRetriever,
         IEventPublisher eventPublisher, IJournalSearchContextFinder searchContextFinder)
      {
         _databaseMediator = databaseMediator;
         _journalRetriever = journalRetriever;
         _eventPublisher = eventPublisher;
         _searchContextFinder = searchContextFinder;
      }

      public void PerformSearch(JournalSearch journalSearch)
      {
         if (_journalRetriever.Current == null)
            return;

         var journalPageIds = _databaseMediator.ExecuteQuery(new JournalPageIdsFromSearch {Search = journalSearch});
         var searchResults = journalPageIds.Select(id => createSearchResult(id, journalSearch)).Where(x => x != null).ToList();
         _eventPublisher.PublishEvent(new JournalSearchPerformedEvent(searchResults, journalSearch));
      }

      public void ClearSearch()
      {
         _eventPublisher.PublishEvent(new JournalClearSearchEvent());
      }

      private JournalSearchItem createSearchResult(string workingJournalId, JournalSearch journalSearch)
      {
         var journalPage = _journalRetriever.Current.JournalPageById(workingJournalId);
         return journalPage == null
            ? null
            : new JournalSearchItem
            {
               JournalPage = journalPage,
               Context = _searchContextFinder.ContextFor(journalPage, journalSearch)
            };
      }
   }
}