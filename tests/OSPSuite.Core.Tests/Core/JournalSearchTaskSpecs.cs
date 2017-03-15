using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Journal;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalSearchTask : ContextSpecification<IJournalSearchTask>
   {
      protected IDatabaseMediator _databaseMediator;
      protected IJournalRetriever _journalRetriever;
      protected IEventPublisher _eventPublisher;
      protected IJournalSearchContextFinder _searchContextFinder;
      protected JournalSearch _journalSearch;
      protected List<string> _journalIds;

      protected override void Context()
      {
         _databaseMediator = A.Fake<IDatabaseMediator>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _searchContextFinder = A.Fake<IJournalSearchContextFinder>();
         sut = new JournalSearchTask(_databaseMediator, _journalRetriever, _eventPublisher, _searchContextFinder);

         _journalSearch = new JournalSearch();
         _journalIds = new List<string>();
      }
   }

   public class When_performing_a_journal_search_using_the_given_search_criteria : concern_for_JournalSearchTask
   {
      private JournalPageIdsFromSearch _payload;
      private JournalSearchPerformedEvent _event;
      private JournalPage _journalItem1;
      private JournalPage _journalItem2;

      protected override void Context()
      {
         base.Context();
         _journalIds.Add("id1");
         _journalIds.Add("not found");
         _journalIds.Add("id2");

         A.CallTo(() => _databaseMediator.ExecuteQuery(A<JournalPageIdsFromSearch>._))
            .Invokes(x => _payload = x.GetArgument<JournalPageIdsFromSearch>(0))
            .Returns(_journalIds);

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalSearchPerformedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalSearchPerformedEvent>(0));

         _journalItem1 = new JournalPage();
         _journalItem2 = new JournalPage();

         A.CallTo(() => _journalRetriever.Current.JournalPageById("id1")).Returns(_journalItem1);
         A.CallTo(() => _journalRetriever.Current.JournalPageById("id2")).Returns(_journalItem2);
         A.CallTo(() => _journalRetriever.Current.JournalPageById("not found")).Returns(null);

         A.CallTo(() => _searchContextFinder.ContextFor(_journalItem1, _journalSearch)).Returns("Context1");
         A.CallTo(() => _searchContextFinder.ContextFor(_journalItem2, _journalSearch)).Returns("Context2");
      }

      protected override void Because()
      {
         sut.PerformSearch(_journalSearch);
      }

      [Observation]
      public void should_retrieve_the_matching_journal_ids_from_the_database()
      {
         _payload.Search.ShouldBeEqualTo(_journalSearch);
      }

      [Observation]
      public void should_publish_the_working_search_performed_event_with_the_corresponding_journal_search_results()
      {
         _event.SearchResults.Select(x => x.JournalPage).ShouldOnlyContain(_journalItem1, _journalItem2);
         _event.SearchResults.Select(x => x.Context).ShouldOnlyContain("Context1", "Context2");
      }

      [Observation]
      public void should_publish_the_working_search_performed_event_with_the_corresponding_journal_search_critieria()
      {
         _event.JournalSearch.ShouldBeEqualTo(_journalSearch);
      }
   }

   public class When_performing_a_journal_search_and_no_journal_is_connected_to_the_current_project : concern_for_JournalSearchTask
   {
      private JournalPageIdsFromSearch _payload;
      private JournalSearchPerformedEvent _event;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _databaseMediator.ExecuteQuery(A<JournalPageIdsFromSearch>._))
            .Invokes(x => _payload = x.GetArgument<JournalPageIdsFromSearch>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalSearchPerformedEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalSearchPerformedEvent>(0));

         _journalRetriever.Current = null;
      }

      protected override void Because()
      {
         sut.PerformSearch(_journalSearch);
      }

      [Observation]
      public void should_not_perform_the_search()
      {
         _payload.ShouldBeNull();
         _event.ShouldBeNull();
      }
   }

   public class When_clearing_the_journal_search : concern_for_JournalSearchTask
   {
      private JournalClearSearchEvent _event;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalClearSearchEvent>._))
            .Invokes(x => _event = x.GetArgument<JournalClearSearchEvent>(0));
      }

      protected override void Because()
      {
         sut.ClearSearch();
      }

      [Observation]
      public void should_publish_the_event_journal_clear_search()
      {
         _event.ShouldNotBeNull();
      }
   }

}	