using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Journal.Commands
{
   public class UpdateJournalPageTags : JournalPagePayload
   {
   }

   public class UpdateJournalItemTagsCommand : JournalDatabaseCommand<UpdateJournalPageTags>
   {
      public UpdateJournalItemTagsCommand(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override void Execute(UpdateJournalPageTags payload)
      {
         var journalPage = payload.JournalPage;
         var journalPageId = journalPage.Id;

         //first delete all tags defined for the current working journal
         Db.JournalPageTags.DeleteWhere("JournalPageId=@journalPageId", new {journalPageId});

         //add new tags to tag list if not already available
         var tags = new List<dynamic>();
         var existingTags = Db.Tags.All().ToList();
         var newTags = new List<dynamic>();

         journalPage.Tags.Each(tag =>
         {
            if (!existingTags.ExistsById(tag))
               newTags.Add(new {Id = tag});

            tags.Add(new {JournalPageId = journalPageId, TagId = tag});
         });

         //add to db
         Db.Tags.Insert(newTags);
         Db.JournalPageTags.Insert(tags);
      }
   }
}