using System.Linq;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class JournalPageRelatedData : IQuery<JournalPageData>
   {
      public string JournalPageId { get; set; }
   }

   public class JournalItemRelatedDataQuery : JournalDatabaseQuery<JournalPageRelatedData, JournalPageData>
   {
      public JournalItemRelatedDataQuery(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override JournalPageData Query(JournalPageRelatedData query)
      {
         var sql =
            @"
               SELECT tags.* FROM TAGS tags, JOURNAL_PAGE_TAGS jpt WHERE jpt.JournalPageId= @JournalPageId AND jpt.TagId= tags.Id;     
               SELECT * from RELATED_ITEMS where JournalPageId = @JournalPageId;";

         using (var multi = Db.QueryMultiple(sql, new {query.JournalPageId }))
         {
            var tags = multi.Read<Tag>().ToList();
            var relatedItem = multi.Read<RelatedItem>().ToList();
            return new JournalPageData(tags, relatedItem);
         }
      }
   }
}