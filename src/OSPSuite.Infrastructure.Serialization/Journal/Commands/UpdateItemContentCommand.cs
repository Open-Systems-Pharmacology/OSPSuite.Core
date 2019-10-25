using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class UpdateItemContent
   {
      public ItemWithContent ItemWithContent { get; set; }
   }

   public class UpdateItemContentCommand : JournalDatabaseCommand<UpdateItemContent>
   {
      public UpdateItemContentCommand(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override void Execute(UpdateItemContent payload)
      {
         var content = payload.ItemWithContent.Content;
         Db.Contents.Update(content.Id, new {content.Data});
      }
   }
}