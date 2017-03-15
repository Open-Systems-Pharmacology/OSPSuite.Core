using System.Collections.Generic;

namespace OSPSuite.Core.Journal
{
   public class JournalPageData
   {
      public IReadOnlyList<Tag> Tags { get; private set; }
      public IReadOnlyList<RelatedItem> RelatedItems { get; private set; }

      public JournalPageData(IReadOnlyList<Tag> tags, IReadOnlyList<RelatedItem> relatedItems)
      {
         Tags = tags;
         RelatedItems = relatedItems;
      }
   }
}