using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Serialization.Journal.Commands;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.Infrastructure.Serialization.Journal
{
   public class JournalPageTask : IJournalPageTask
   {
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDialogCreator _dialogCreator;
      private readonly IRelatedItemFactory _relatedItemFactory;
      private readonly NumericFormatter<double> _fileSizeFormatter;

      public JournalPageTask(
         IDatabaseMediator databaseMediator,
         IEventPublisher eventPublisher,
         IDialogCreator dialogCreator,
         IRelatedItemFactory relatedItemFactory)
      {
         _databaseMediator = databaseMediator;
         _eventPublisher = eventPublisher;
         _dialogCreator = dialogCreator;
         _relatedItemFactory = relatedItemFactory;
         _fileSizeFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);
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
         if (relatedItem == null)
            return;

         _databaseMediator.ExecuteCommand(new AddRelatedPageToJournalPage {JournalPage = journalPage, RelatedItem = relatedItem});
         journalPage.AddRelatedItem(relatedItem);
         _eventPublisher.PublishEvent(new JournalPageUpdatedEvent(journalPage));
      }

      public void AddRelatedItemFromFile(JournalPage journalPage)
      {
         var allFilesFilter = Constants.Filter.FileFilter(Captions.Journal.RelatedItemFile, Constants.Filter.ANY_EXTENSION);
         var fileName = _dialogCreator.AskForFileToOpen(Captions.Journal.SelectedFileToLoadAsRelatedItem, allFilesFilter, Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(fileName))
            return;

         if (!fileIsTooLargeAndCannotBeAddedToJournal(fileName))
            return;

         var relatedItem = _relatedItemFactory.CreateFromFile(fileName);
         AddRelatedItemTo(journalPage, relatedItem);

         //unload data to ensure that we do no keep the reference in memory
         relatedItem.Content = null;
      }

      private bool fileIsTooLargeAndCannotBeAddedToJournal(string fileName)
      {
         var sizeInBytes = FileLength(fileName);
         var sizeInMegaBytes = sizeInMb(sizeInBytes);

         if (sizeInBytes >= Constants.RELATIVE_ITEM_MAX_FILE_SIZE_IN_BYTES)
            throw new OSPSuiteException(Error.FileSizeExceedsMaximumSize(sizeInMegaBytes, sizeInMb(Constants.RELATIVE_ITEM_MAX_FILE_SIZE_IN_BYTES)));

         if (sizeInBytes < Constants.RELATIVE_ITEM_FILE_SIZE_WARNING_THRESHOLD_IN_BYTES)
            return true;

         var thresholdSizeInMegaBytes = sizeInMb(Constants.RELATIVE_ITEM_FILE_SIZE_WARNING_THRESHOLD_IN_BYTES);
         var res = _dialogCreator.MessageBoxYesNo(Captions.Journal.ReallyLoadRelatedItemFileExceedingThreshold(sizeInMegaBytes, thresholdSizeInMegaBytes));

         return res == ViewResult.Yes;
      }

      private string sizeInMb(long sizeInByte) => _fileSizeFormatter.Format(sizeInByte * 1.0 / Constants.MB_TO_BYTES);

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

      //JUST FOR TESTING
      public Func<string, long> FileLength { get; set; } = (fileFullPath) => new FileInfo(fileFullPath).Length;
   }
}