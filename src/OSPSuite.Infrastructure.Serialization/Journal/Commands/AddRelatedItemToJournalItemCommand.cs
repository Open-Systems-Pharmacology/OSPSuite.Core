using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class AddRelatedPageToJournalPage : JournalPagePayload
   {
      public RelatedItem RelatedItem { get; set; }
   }

   public class AddRelatedItemToJournalItemCommand : JournalDatabaseCommand<AddRelatedPageToJournalPage>
   {
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IIdGenerator _idGenerator;

      public AddRelatedItemToJournalItemCommand(IJournalSession journalSession, IDatabaseMediator databaseMediator, IIdGenerator idGenerator)
         : base(journalSession)
      {
         _databaseMediator = databaseMediator;
         _idGenerator = idGenerator;
      }

      public override void Execute(AddRelatedPageToJournalPage payload)
      {
         var journalPage = payload.JournalPage;
         var relatedItem = payload.RelatedItem;

         if (!relatedItem.IsTransient)
            throw new CannotCreateNewItemForPersitedEntity(relatedItem.Id);

         relatedItem.Id = _idGenerator.NewId();

         _databaseMediator.ExecuteCommand(new AddItemContent { ItemWithContent = relatedItem });

         Db.RelatedItems.Insert(new
         {
            relatedItem.Id,
            JournalPageId = journalPage.Id,
            relatedItem.ItemType,
            relatedItem.Name,
            relatedItem.CreatedAt,
            relatedItem.Description,
            relatedItem.OriginId,
            relatedItem.Version,
            relatedItem.IconName,
            relatedItem.FullPath,
            relatedItem.Discriminator,
            ContentId = relatedItem.Content.Id
         });

         _databaseMediator.ExecuteCommand(new UpdateJournalPage { JournalPage = journalPage });
      }
   }
}