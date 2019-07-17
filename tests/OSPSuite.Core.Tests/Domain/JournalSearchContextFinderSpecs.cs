using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_JournalSearchContextFinder : ContextSpecification<IJournalSearchContextFinder>
   {
      protected JournalPage _journalPage;
      protected JournalSearch _journalSearch;
      protected string _searchContext;

      protected override void Context()
      {
         sut = new JournalSearchContextFinder();
         _journalPage = new JournalPage { FullText = "A very small little house in the green tree" };
         _journalSearch = new JournalSearch();
      }

      protected override void Because()
      {
         _searchContext = sut.ContextFor(_journalPage, _journalSearch);
      }
   }

   public class When_returning_the_context_of_a_full_text_search : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalSearch.Search = "house";
      }

      [Observation]
      public void should_return_the_text_before_and_after_with_the_actual_search_in_bold()
      {
         _searchContext.ShouldBeEqualTo("...very small little <b>house</b> in the green...");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_case_sensitive_that_is_not_found : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalSearch.Search = "House";
         _journalSearch.MatchCase = true;
      }

      [Observation]
      public void should_return_an_empty_context()
      {
         _searchContext.ShouldBeEqualTo("");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_cutting_a_word_in_two : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalSearch.Search = "hou";
      }

      [Observation]
      public void should_return_the_text_before_and_after_with_the_actual_search_in_bold()
      {
         _searchContext.ShouldBeEqualTo("...very small little <b>hou</b>se in the green...");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_where_not_enough_context_is_available : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalSearch.Search = "very small little";
         _journalSearch.MatchWholePhrase = true;
      }

      [Observation]
      public void should_return_the_text_before_and_after_with_the_actual_search_in_bold()
      {
         _searchContext.ShouldBeEqualTo("...A <b>very small little</b> house in the...");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_matching_any_words : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalSearch.Search = "very green";
         _journalSearch.MatchAny = true;
      }

      [Observation]
      public void should_return_the_text_before_and_after_with_the_actual_search_in_bold()
      {
         _searchContext.Contains("...A <b>very</b> small little house...").ShouldBeTrue();
         _searchContext.Contains("...house in the <b>green</b> tree...").ShouldBeTrue();
      }
   }

   public class When_returning_the_context_of_a_full_text_matching_exactly_the_search_term : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.FullText = "very small little";
         _journalSearch.Search = "very small little";
         _journalSearch.MatchWholePhrase = true;
      }

      [Observation]
      public void should_return_the_exact_search_term_in_bold()
      {
         _searchContext.ShouldBeEqualTo("<b>very small little</b>");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_with_multiple_match_still_in_context : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.FullText = "a very small house very pretty";
         _journalSearch.Search = "very";
      }

      [Observation]
      public void should_return_the_first_match()
      {
         _searchContext.ShouldBeEqualTo("...a <b>very</b> small house very...");
      }
   }

   public class When_returning_the_context_of_a_full_text_search_with_multiple_match : concern_for_JournalSearchContextFinder
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.FullText = "a very small pretty house on a very small mountain";
         _journalSearch.Search = "very";
      }

      [Observation]
      public void should_return_the_first_match()
      {
         _searchContext.Contains("...a <b>very</b> small pretty house...").ShouldBeTrue();
         _searchContext.Contains("...on a <b>very</b> small mountain...").ShouldBeTrue();
      }
   }

}	