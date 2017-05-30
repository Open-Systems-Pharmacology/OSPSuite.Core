using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Journal
{
   public interface IJournalSearchContextFinder
   {
      /// <summary>
      ///    Returns the context of the search performed in <paramref name="journalPage" /> according to the search
      ///    <paramref name="journalSearch" />
      /// </summary>
      string ContextFor(JournalPage journalPage, JournalSearch journalSearch);
   }

   public class JournalSearchContextFinder : IJournalSearchContextFinder
   {
      private const int CONTEXT_WORD_COUNT = 3;

      public string ContextFor(JournalPage journalPage, JournalSearch journalSearch)
      {
         var searchTerms = journalSearch.SearchTerms;
         if (!searchTerms.Any())
            return string.Empty;

         if (journalSearch.MatchWholePhrase)
            return findContextFor(journalPage, journalSearch.Search, journalSearch.MatchCase);

         return multipleLinesContext(searchTerms.Select(term => findContextFor(journalPage, term, journalSearch.MatchCase))
            .Where(conxtext => !string.IsNullOrEmpty(conxtext)));
      }

      private string findContextFor(JournalPage journalPage, string searchPattern, bool matchCase)
      {
         var regex = new Regex(createSearchPattern(searchPattern), matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
         var contextBuilder = new List<string>();
         foreach (Match match in regex.Matches(journalPage.FullText))
         {
            var searchMatch = valueFor(match, "search");
            if (string.IsNullOrEmpty(searchMatch))
               return string.Empty;

            var context = lastFewWordsFrom(valueFor(match, "before")) +
                          "<b>" + searchMatch + "</b>" +
                          firstFewWordsFrom(valueFor(match, "after"));

            contextBuilder.Add(context.Trim());
         }
         return multipleLinesContext(contextBuilder);
      }

      private string multipleLinesContext(IEnumerable<string> contextBuilder)
      {
         return contextBuilder.ToString("<br>");
      }

      private string valueFor(Match match, string group)
      {
         return match.Groups[group].Value;
      }

      private string createSearchPattern(string searchPattern)
      {
         var escapedPattern = Regex.Escape(searchPattern);
         return "(?<before>(?:\\S+\\s){0," + CONTEXT_WORD_COUNT + "}\\S*)?" + //up to CONTEXT_WORD_COUNT words BEFORE the match
                $"(?<search>{escapedPattern})"+ //actual match
                "(?<after>\\S*(?:\\s\\S+){0," + CONTEXT_WORD_COUNT + "})?"; //up to CONTEXT_WORD_COUNT words AFTER the match
      }

      private string lastFewWordsFrom(string match)
      {
         if (string.IsNullOrEmpty(match))
            return match;

         return string.Format("...{0}", match);
      }

      private string firstFewWordsFrom(string match)
      {
         if (string.IsNullOrEmpty(match))
            return match;

         return string.Format("{0}...", match);
      }
   }
}