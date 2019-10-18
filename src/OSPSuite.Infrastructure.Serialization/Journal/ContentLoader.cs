using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal
{
   public class ContentLoader : IContentLoader
   {
      private readonly IJournalSession _journalSession;

      public ContentLoader(IJournalSession journalSession)
      {
         _journalSession = journalSession;
      }

      public void Load(ItemWithContent itemWithContent)
      {
         var db = _journalSession.Current;
         var content = db.Contents.Get(itemWithContent.ContentId);
         itemWithContent.Content = content;
      }
   }
}