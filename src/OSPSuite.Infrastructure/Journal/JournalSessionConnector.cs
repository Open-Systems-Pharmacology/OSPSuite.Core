using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Journal
{
   public interface IJournalSessionConnector
   {
      Core.Journal.Journal ConnectToJournal(string journalFullPath);
      Core.Journal.Journal SelectJournal();
      bool IsConnected { get; }
   }

   public class JournalSessionConnector : IJournalSessionConnector
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IJournalLoader _journalLoader;
      private readonly IJournalSession _journalSession;
      private readonly IProjectRetriever _projectRetriever;

      public JournalSessionConnector(IDialogCreator dialogCreator, IJournalLoader journalLoader, IJournalSession journalSession, IProjectRetriever projectRetriever)
      {
         _dialogCreator = dialogCreator;
         _journalLoader = journalLoader;
         _journalSession = journalSession;
         _projectRetriever = projectRetriever;
      }

      public Core.Journal.Journal ConnectToJournal(string journalFullPath)
      {
         if (!string.IsNullOrEmpty(journalFullPath))
            return openExistingJournal(journalFullPath: journalFullPath);

         return createOrOpenJournal(defaultDirectory: FileHelper.FolderFromFileFullPath(_projectRetriever.ProjectFullPath));
      }

      public Core.Journal.Journal SelectJournal()
      {
         var defaultFileName = FileHelper.FileNameFromFileFullPath(_journalSession.CurrentJournalPath);
         var defaultFilePath = _journalSession.CurrentJournalPath;
         if (string.IsNullOrEmpty(defaultFilePath))
            defaultFilePath = _projectRetriever.ProjectFullPath;

         return openExistingJournal(defaultFileName: defaultFileName, defaultDirectory: FileHelper.FolderFromFileFullPath(defaultFilePath));
      }

      public bool IsConnected => _journalSession.IsOpen;

      private Core.Journal.Journal createOrOpenJournal(string defaultDirectory)
      {
         var viewResult = _dialogCreator.MessageBoxYesNoCancel(Captions.Journal.NoJournalAssociatedWithProject, Captions.Journal.OpenJournalButton, Captions.Journal.CreateJournalButton, Captions.Journal.CancelJournalButton);
         switch (viewResult)
         {
            case ViewResult.Yes:
               return openExistingJournal(defaultDirectory: defaultDirectory);
            case ViewResult.No:
               return createNewJournal(defaultDirectory);
            default:
               return null;
         }
      }

      private Core.Journal.Journal createNewJournal(string defaultDirectory)
      {
         var workingJournalPath = _dialogCreator.AskForFileToSave(Captions.Journal.CreateJournal, Constants.Filter.JOURNAL_FILE_FILTER, Constants.DirectoryKey.PROJECT, defaultDirectory: defaultDirectory);
         if (string.IsNullOrEmpty(workingJournalPath))
            return null;

         _journalSession.Create(workingJournalPath);
         return _journalLoader.LoadCurrent();
      }

      private Core.Journal.Journal openExistingJournal(string journalFullPath = null, string defaultFileName = null, string defaultDirectory = null)
      {
         if (!FileHelper.FileExists(journalFullPath))
         {
            journalFullPath = _dialogCreator.AskForFileToOpen(Captions.Journal.OpenJournal, Constants.Filter.JOURNAL_FILE_FILTER, Constants.DirectoryKey.PROJECT, defaultFileName: defaultFileName, defaultDirectory: defaultDirectory);
            if (string.IsNullOrEmpty(journalFullPath))
               return null;
         }

         return _journalLoader.Load(journalFullPath);
      }
   }
}