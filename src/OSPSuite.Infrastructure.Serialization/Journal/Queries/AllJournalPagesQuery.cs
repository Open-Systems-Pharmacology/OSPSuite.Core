using System.Collections.Generic;
using OSPSuite.Core.Journal;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class AllJournalPages : IQuery<IEnumerable<JournalPage>>
   {
   }

   public class AllJournalPagesQuery : JournalDatabaseQuery<AllJournalPages, IEnumerable<JournalPage>>
   {
      private readonly IDatabaseMediator _databaseMediator;

      public AllJournalPagesQuery(IJournalSession journalSession, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _databaseMediator = databaseMediator;
      }

      public override IEnumerable<JournalPage> Query(AllJournalPages query)
      {
         var journalPages = new List<JournalPage>();
         journalPages.AddRange(Db.JournalPages.All());
         journalPages.Each(loadRelatedData);
         return journalPages;
      }

      private void loadRelatedData(JournalPage journalPage)
      {
         var data = _databaseMediator.ExecuteQuery(new JournalPageRelatedData {JournalPageId = journalPage.Id});
         journalPage.UpdateRelatedData(data);
      }
   }
}