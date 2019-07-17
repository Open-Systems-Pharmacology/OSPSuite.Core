using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.FileLocker;

namespace OSPSuite.Core
{
   public abstract class Workspace<TProject> : IWorkspace where TProject : class, IProject
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IFileLocker _fileLocker;
      private Journal.Journal _journal;
      protected TProject _project;
      public bool ProjectIsReadOnly { get; set; }

      protected Workspace(IEventPublisher eventPublisher, IFileLocker fileLocker)
      {
         _eventPublisher = eventPublisher;
         _fileLocker = fileLocker;
      }

      public virtual void Clear()
      {
         ProjectIsReadOnly = false;
         //TODO   _journalSession.Close();
         ReleaseLock();
         _project = null;
         Journal = null;
         ReleaseMemory();
      }

      public void AccessFile(string fileFullPath)
      {
         _fileLocker.AccessFile(fileFullPath);
      }

      public void UpdateJournalPathRelativeTo(string projectFileFullPath)
      {
         if (Journal == null)
            return;

         _project.JournalPath = FileHelper.CreateRelativePath(Journal.FullPath, FileHelper.FolderFromFileFullPath(projectFileFullPath));
      }

      protected void ReleaseMemory()
      {
         //This might free some C++ objects still  being held into memory
         this.GCCollectAndCompact();
      }

      protected void ReleaseLock()
      {
         _fileLocker.ReleaseFile();
      }

      public void LockFile(string fullPath)
      {
         try
         {
            //try to lock the file is the file exists
            if (FileHelper.FileExists(fullPath))
               _fileLocker.AccessFile(fullPath);
         }
         catch (Exception e)
         {
            throw new CannotLockFileException(e);
         }
      }

      public Journal.Journal Journal
      {
         get => _journal;
         set
         {
            _journal = value;
            if (_journal == null)
            {
               _eventPublisher.PublishEvent(new JournalClosedEvent());
               return;
            }

            if (_project == null) return;
            _project.JournalPath = _journal.FullPath;
            _eventPublisher.PublishEvent(new JournalLoadedEvent(_journal));
         }
      }
   }
}