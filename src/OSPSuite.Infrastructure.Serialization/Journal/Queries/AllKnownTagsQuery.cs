using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class AllKnownTags : IQuery<IEnumerable<string>>
   {
   }

   public class AllKnownTagsQuery : JournalDatabaseQuery<AllKnownTags, IEnumerable<string>>
   {
      public AllKnownTagsQuery(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override IEnumerable<string> Query(AllKnownTags query)
      {
         return Db.Tags.All().Select(x => x.Id);
      }
   }
}