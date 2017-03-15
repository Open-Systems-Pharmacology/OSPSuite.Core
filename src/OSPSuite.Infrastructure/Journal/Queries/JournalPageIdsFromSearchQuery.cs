using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class JournalPageIdsFromSearch : IQuery<IEnumerable<string>>
   {
      public JournalSearch Search { get; set; }
   }

   public class JournalItemIdsFromSearchQuery : JournalDatabaseQuery<JournalPageIdsFromSearch, IEnumerable<string>>
   {
      public JournalItemIdsFromSearchQuery(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override IEnumerable<string> Query(JournalPageIdsFromSearch query)
      {
         var search = query.Search;
         var searchTerms = search.SearchTerms;
         if (!searchTerms.Any())
            return Enumerable.Empty<string>();

         var queryOperator = search.MatchAny ? "OR" : "AND";
         var likeOperator = search.MatchCase ? "GLOB" : "LIKE";
         var wildcard = search.MatchCase ? "*" : "%";
         var sql = string.Format("SELECT jp.Id FROM JOURNAL_PAGES jp {0}", whereCondition(searchTerms, queryOperator, likeOperator, wildcard));
         return Db.Query<string>(sql);
      }

      private string whereCondition(IReadOnlyList<string> searchTerms, string queryOperator, string likeOperator, string wildcard)
      {
         var sb = new StringBuilder();
         searchTerms.Each((term, index) => { sb.AppendLine(queryCondition(index == 0 ? "WHERE" : queryOperator, term, likeOperator, wildcard)); });
         return sb.ToString();
      }

      private string queryCondition(string queryOperator, string searchTerm, string likeOperator, string wildcard)
      {
         return string.Format("{0} jp.FullText {1} '{3}{2}{3}'", queryOperator, likeOperator, searchTerm, wildcard);
      }
   }
}