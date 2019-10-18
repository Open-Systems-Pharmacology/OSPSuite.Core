using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Commands
{
   public class AddItemContent
   {
      public ItemWithContent ItemWithContent { get; set; }
   }

   public class AddItemContentCommand : JournalDatabaseCommand<AddItemContent>
   {
      private readonly IIdGenerator _idGenerator;

      public AddItemContentCommand(IJournalSession journalSession, IIdGenerator idGenerator) : base(journalSession)
      {
         _idGenerator = idGenerator;
      }

      public override void Execute(AddItemContent payload)
      {
         var itemWithContent = payload.ItemWithContent;
         var content = itemWithContent.Content;
         if (!content.IsTransient)
            throw new CannotCreateNewItemForPersitedEntity(content.Id);

         content.Id = _idGenerator.NewId();
         itemWithContent.ContentId = content.Id;

         Db.Contents.Insert(
            new
            {
               content.Id,
               content.Data
            });
      }
   }
}