using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Journal.Commands;

namespace OSPSuite.Infrastructure.Journal
{
   public class JournalPageTask : IJournalPageTask
   {
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDialogCreator _dialogCreator;

      public JournalPageTask(IDatabaseMediator databaseMediator, IEventPublisher eventPublisher, IDialogCreator dialogCreator)
      {
         _databaseMediator = databaseMediator;
         _eventPublisher = eventPublisher;
         _dialogCreator = dialogCreator;
      }

      public void UpdateTagsIn(JournalPage journalPage)
      {
         _databaseMediator.ExecuteCommand(new UpdateJournalPageTags {JournalPage = journalPage});
         _eventPublisher.PublishEvent(new JournalPageUpdatedEvent(journalPage));
      }

      public void UpdateTags(JournalPage journalPage, IEnumerable<string> tags)
      {
         journalPage.UpdateTags(tags);
         UpdateTagsIn(journalPage);
      }

      public void AddRelatedItemTo(JournalPage journalPage, RelatedItem relatedItem)
      {
         _databaseMediator.ExecuteCommand(new AddRelatedPageToJournalPage {JournalPage = journalPage, RelatedItem = relatedItem});
         journalPage.AddRelatedItem(relatedItem);
         _eventPublisher.PublishEvent(new JournalPageUpdatedEvent(journalPage));
      }

      public void DeleteRelatedItemFrom(Core.Journal.Journal journal, RelatedItem relatedItem)
      {
         DeleteRelatedItemsFrom(journal, new[] {relatedItem});
      }

      private void deleteRelatedItemFrom(JournalPage journalPage, RelatedItem relatedItem)
      {
         _databaseMediator.ExecuteCommand(new DeleteRelatedItemFromJournalPage {RelatedItem = relatedItem});
         journalPage.RemoveRelatedItem(relatedItem);
         _eventPublisher.PublishEvent(new JournalPageUpdatedEvent(journalPage));
      }

      public void DeleteRelatedItemsFrom(Core.Journal.Journal journal, IReadOnlyList<RelatedItem> relatedItems)
      {
         if (!relatedItems.Any())
            return;

         var viewResult = _dialogCreator.MessageBoxYesNo(promptForDelete(relatedItems));
         if (viewResult != ViewResult.Yes) return;

         relatedItems.Each(item => deleteRelatedItemFrom(journal.JournalPageContaining(item), item));
      }

      private string promptForDelete(IReadOnlyList<RelatedItem> relatedItems)
      {
         return relatedItems.Count == 1 ? promptForSingleRelatedItemDelete(relatedItems.First()) : Captions.Journal.ReallyDeleteMultipleRelatedItems;
      }

      private string promptForSingleRelatedItemDelete(RelatedItem relatedItem)
      {
         return Captions.Journal.ReallyDeleteRelatedItem(relatedItem.Display);
      }

      public void AddAsParentTo(JournalPage journalPage, JournalPage parentJournalPage)
      {
         if (parentJournalPage == null) return;
         updateJournalPageParentId(journalPage, parentJournalPage.Id);
      }

      public void DeleteParentFrom(JournalPage journalPage)
      {
         updateJournalPageParentId(journalPage, null);
      }

      private void updateJournalPageParentId(JournalPage journalPage, string parentId)
      {
         journalPage.ParentId = parentId;
         UpdateJournalPage(journalPage);
      }

      public void UpdateJournalPage(JournalPage journalPage)
      {
         _databaseMediator.ExecuteCommand(new UpdateJournalPage {JournalPage = journalPage});
         _eventPublisher.PublishEvent(new JournalPageUpdatedEvent(journalPage));
      }
   }
}