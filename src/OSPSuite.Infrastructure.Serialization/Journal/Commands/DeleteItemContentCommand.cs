using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class DeleteItemContent
   {
      public ItemWithContent ItemWithContent { get; set; }
   }

   public class DeleteItemContentCommand : JournalDatabaseCommand<DeleteItemContent>
   {
      public DeleteItemContentCommand(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override void Execute(DeleteItemContent payload)
      {
         var itemWithContent = payload.ItemWithContent;
         Db.Contents.Delete(itemWithContent.ContentId);
      }
   }
}