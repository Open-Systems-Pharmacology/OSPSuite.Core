using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Queries
{
   public class JournalPageById : IQuery<JournalPage>
   {
      public string Id { get; set; }
   }

   public class JournalPageByIdQuery : JournalDatabaseQuery<JournalPageById, JournalPage>
   {
      private readonly IDatabaseMediator _databaseMediator;

      public JournalPageByIdQuery(IJournalSession journalSession, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _databaseMediator = databaseMediator;
      }

      public override JournalPage Query(JournalPageById query)
      {
         var journalPage = Db.JournalPages.Get(query.Id);
         loadRelatedData(journalPage);
         return journalPage;
      }

      private void loadRelatedData(JournalPage journalPage)
      {
         var relatedData = _databaseMediator.ExecuteQuery(new JournalPageRelatedData {JournalPageId = journalPage.Id});
         journalPage.UpdateRelatedData(relatedData);
      }
   }
}