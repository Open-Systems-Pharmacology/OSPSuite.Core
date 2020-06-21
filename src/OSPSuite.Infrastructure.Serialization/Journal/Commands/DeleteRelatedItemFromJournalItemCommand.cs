using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class DeleteRelatedItemFromJournalPage
   {
      public RelatedItem RelatedItem { get; set; }
   }

   public class DeleteRelatedItemFromJournalItemCommand : JournalDatabaseCommand<DeleteRelatedItemFromJournalPage>
   {
      private readonly IDatabaseMediator _databaseMediator;

      public DeleteRelatedItemFromJournalItemCommand(IJournalSession journalSession, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _databaseMediator = databaseMediator;
      }

      public override void Execute(DeleteRelatedItemFromJournalPage payload)
      {
         var relatedItem = payload.RelatedItem;
         Db.RelatedItems.Delete(relatedItem.Id);
         _databaseMediator.ExecuteCommand(new DeleteItemContent { ItemWithContent = relatedItem });
      }
   }
}