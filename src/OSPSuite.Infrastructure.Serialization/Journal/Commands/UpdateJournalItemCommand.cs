namespace OSPSuite.Infrastructure.Journal.Commands
{
   public class UpdateJournalPage : JournalPagePayload
   {
   }

   public class UpdateJournalItemCommand : JournalDatabaseCommand<UpdateJournalPage>
   {
      private readonly IDatabaseMediator _databaseMediator;

      public UpdateJournalItemCommand(IJournalSession journalSession, IDatabaseMediator databaseMediator) : base(journalSession)
      {
         _databaseMediator = databaseMediator;
      }

      public override void Execute(UpdateJournalPage payload)
      {
         var journalPage = payload.JournalPage;
         journalPage.RunUpdate();

         Db.JournalPages.Update(journalPage.Id, new
         {
            journalPage.Title,
            journalPage.OriginId,
            journalPage.Description,
            journalPage.FullText,
            journalPage.UpdatedAt,
            journalPage.UpdatedBy,
            journalPage.ParentId,
         });

         _databaseMediator.ExecuteCommand(new UpdateJournalPageTags {JournalPage = journalPage});

         if (!journalPage.IsLoaded)
            return;

         _databaseMediator.ExecuteCommand(new UpdateItemContent {ItemWithContent = journalPage});
      }
   }
}