using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Serialization.Journal.Queries;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class CreateJournalPage : JournalPagePayload
   {
   }

   public class CreateJournalItemCommand : JournalDatabaseCommand<CreateJournalPage>
   {
      private readonly IIdGenerator _idGenerator;
      private readonly IDatabaseMediator _databaseMediator;

      public CreateJournalItemCommand(IJournalSession journalSession, IIdGenerator idGenerator, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _idGenerator = idGenerator;
         _databaseMediator = databaseMediator;
      }

      public override void Execute(CreateJournalPage payload)
      {
         var journalPage = payload.JournalPage;
         journalPage.RunUpdate();

         if (!journalPage.IsTransient)
            throw new CannotCreateNewItemForPersitedEntity(journalPage.Id);

         _databaseMediator.ExecuteCommand(new AddItemContent { ItemWithContent = journalPage });

         journalPage.Id = _idGenerator.NewId();
         journalPage.UniqueIndex = _databaseMediator.ExecuteQuery(new NextAvailableJournalPageIndex());

         Db.JournalPages.Insert(new
         {
            journalPage.Id,
            journalPage.OriginId,
            journalPage.UniqueIndex,
            journalPage.Title,
            journalPage.Description,
            journalPage.CreatedAt,
            journalPage.CreatedBy,
            journalPage.UpdatedAt,
            journalPage.UpdatedBy,
            journalPage.FullText,
            ContentId = journalPage.Content.Id
         });
      }
   }
}