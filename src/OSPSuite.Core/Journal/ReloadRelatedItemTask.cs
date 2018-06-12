using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Journal
{
   public interface IReloadRelatedItemTask
   {
      /// <summary>
      ///    Load the related item and import it either in the app if possible, or in the sister app (MoBi for PKSim) if
      ///    possible.
      ///    Lastly dump the content of the reported item into file otherwise
      /// </summary>
      /// <param name="relatedItem">Related item to load</param>
      void Load(RelatedItem relatedItem);

      /// <summary>
      ///    Import all related items into calling application. THat means that file based related item or sister application
      ///    related items won't be loaded
      /// </summary>
      void ImportAllIntoApplication(IEnumerable<RelatedItem> relatedItems);
   }

   public abstract class ReloadRelatedItemTask : IReloadRelatedItemTask
   {
      private readonly IApplicationConfiguration _applicationConfiguration;
      private readonly IContentLoader _contentLoader;
      private readonly IDialogCreator _dialogCreator;

      protected ReloadRelatedItemTask(IApplicationConfiguration applicationConfiguration, IContentLoader contentLoader, IDialogCreator dialogCreator)
      {
         _applicationConfiguration = applicationConfiguration;
         _contentLoader = contentLoader;
         _dialogCreator = dialogCreator;
      }

      public void Load(RelatedItem relatedItem)
      {
         _contentLoader.Load(relatedItem);

         if (RelatedItemBelongsToApplication(relatedItem))
            LoadOwnContent(relatedItem);

         else if (RelatedItemCanBeLaunchedBySisterApplication(relatedItem))
            LoadContentInSisterApplication(relatedItem);

         else
            DumpContentToUserDefinedLocation(relatedItem);
      }

      public void ImportAllIntoApplication(IEnumerable<RelatedItem> relatedItems)
      {
         relatedItems.Where(RelatedItemBelongsToApplication).Each(x =>
         {
            _contentLoader.Load(x);
            LoadOwnContent(x);
         });
      }

      protected void LoadContentInSisterApplication(RelatedItem relatedItem)
      {
         var tmpFileWithRelatedContent = saveRelatedItemToContentToTempFile(relatedItem);
         StartSisterApplicationWithContentFile(tmpFileWithRelatedContent);
      }

      private string saveRelatedItemToContentToTempFile(RelatedItem relatedItem)
      {
         var filePath = FileHelper.GenerateTemporaryFileName();
         exportedRelatedItemToFile(relatedItem, filePath);
         return filePath;
      }

      private static void exportedRelatedItemToFile(RelatedItem relatedItem, string file)
      {
         File.WriteAllBytes(file, relatedItem.Content.Data);
      }

      protected abstract void StartSisterApplicationWithContentFile(string contentFile);

      protected abstract void LoadOwnContent(RelatedItem relatedItem);

      protected void DumpContentToUserDefinedLocation(RelatedItem relatedItem)
      {
         var relatedItemPath = new FileInfo(relatedItem.FullPath);
         var defaultFileName = FileHelper.FileNameFromFileFullPath(relatedItem.FullPath);
         var relatedItemFilter = Captions.Journal.ExportRelatedItemToFileFilter(relatedItemPath.Extension);
         var filePath = _dialogCreator.AskForFileToSave(Captions.Journal.ExportRelatedItemToFile, relatedItemFilter, Constants.DirectoryKey.REPORT, defaultFileName: defaultFileName);

         if (string.IsNullOrEmpty(filePath))
            return;

         exportedRelatedItemToFile(relatedItem, filePath);
      }

      protected abstract bool RelatedItemCanBeLaunchedBySisterApplication(RelatedItem relatedItem);

      protected bool RelatedItemBelongsToApplication(RelatedItem relatedItem)
      {
         return _applicationConfiguration.Product == relatedItem.Origin;
      }
   }
}