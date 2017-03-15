using System;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.FileLocker;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Journal;

namespace OSPSuite.Infrastructure
{
   public abstract class Workspace<TProject> : IWorkspace where TProject : class, IProject
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IJournalSession _journalSession;
      private readonly IFileLocker _fileLocker;
      private Core.Journal.Journal _journal;
      protected TProject _project;
      public bool ProjectIsReadOnly { get; set; }

      protected Workspace(IEventPublisher eventPublisher, IJournalSession journalSession, IFileLocker fileLocker)
      {
         _eventPublisher = eventPublisher;
         _journalSession = journalSession;
         _fileLocker = fileLocker;
      }

      public virtual void Clear()
      {
         ProjectIsReadOnly = false;
         ReleaseLock();
         _journalSession.Close();
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

         _project.JournalPath = FileHelper.CreateRelativePath(Journal.FullPath, projectFileFullPath);
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

      public Core.Journal.Journal Journal
      {
         get { return _journal; }
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