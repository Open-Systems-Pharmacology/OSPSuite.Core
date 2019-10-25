using OSPSuite.Core.Journal;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class DeleteJournalPage : JournalPagePayload
   {
   }

   public class DeleteJournalPageCommand : JournalDatabaseCommand<DeleteJournalPage>
   {
      private readonly IDatabaseMediator _databaseMediator;

      public DeleteJournalPageCommand(IJournalSession journalSession, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _databaseMediator = databaseMediator;
      }

      public override void Execute(DeleteJournalPage payload)
      {
         var journalPage = payload.JournalPage;
         journalPage.RelatedItems.Each(delete);
         Db.JournalPages.Delete(journalPage.Id);
         _databaseMediator.ExecuteCommand(new DeleteItemContent { ItemWithContent = journalPage });
      }

      private void delete(RelatedItem relatedItem)
      {
         _databaseMediator.ExecuteCommand(new DeleteRelatedItemFromJournalPage {RelatedItem = relatedItem});
      }
   }
}