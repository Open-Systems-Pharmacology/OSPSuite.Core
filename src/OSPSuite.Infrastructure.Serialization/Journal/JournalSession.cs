using System;
using OSPSuite.Assets;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.Journal
{
   public interface IJournalSession
   {
      /// <summary>
      ///    Opens the working journal located at path <paramref name="journalDatabasePath" />
      /// </summary>
      /// <exception cref="InvalidOperationException">
      ///    Is thrown if the file <paramref name="journalDatabasePath" /> does not exist or if
      ///    a working journal was already open
      /// </exception>
      void Open(string journalDatabasePath);

      /// <summary>
      ///    Tries to open the working journal located at <paramref name="journalDatabasePath" /> and returns <c>true</c>
      ///    if the working journal could be open otherwise <c>false</c>
      /// </summary>
      bool TryOpen(string journalDatabasePath);

      /// <summary>
      ///    Create a new working journal located at path <paramref name="journalDatabasePath" />
      /// </summary>
      void Create(string journalDatabasePath);

      /// <summary>
      ///    Close the current  journal if open. Do nothing otherwise
      /// </summary>
      void Close();

      /// <summary>
      ///    Return the IJournalDatabase that was last opened with Open.
      /// </summary>
      /// <exception cref="InvalidOperationException">Is thrown if the WorkingJournal is not opened.</exception>
      JournalDatabase Current { get; }

      /// <summary>
      ///    Returns whether a journal is available or not
      /// </summary>
      bool IsOpen { get; }

      /// <summary>
      /// Returns the full path of the currently open <see cref="JournalDatabase"/> or an empty string if no <see cref="JournalDatabase"/> is opened
      /// </summary>
      string CurrentJournalPath { get; }
   }

   public class JournalSession : IJournalSession
   {
      private readonly IConnectionProvider _connectionProvider;
      private JournalDatabase _journalDatabase;
      public string CurrentJournalPath { get; private set; }

      public JournalSession(IConnectionProvider connectionProvider)
      {
         _connectionProvider = connectionProvider;
      }

      public void Open(string journalDatabasePath)
      {
         Close();
         _journalDatabase = JournalDatabase.Init(_connectionProvider, journalDatabasePath);
         CurrentJournalPath = journalDatabasePath;
      }

      public bool TryOpen(string journalDatabasePath)
      {
         try
         {
            if (!FileHelper.FileExists(journalDatabasePath))
               return false;

            Open(journalDatabasePath);
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      public void Create(string journalDatabasePath)
      {
         //Project file is already open for the same filename
         if (sessionIsAlreadyOpenFor(journalDatabasePath))
            return;

         //new working journal from scratch from scratch. save file
         FileHelper.DeleteFile(journalDatabasePath);
         Open(journalDatabasePath);
         Current.Create();
      }

      public void Close()
      {
         _journalDatabase = null;
         CurrentJournalPath = string.Empty;
      }

      public JournalDatabase Current
      {
         get
         {
            if (_journalDatabase == null)
               throw new InvalidOperationException(Error.JournalNotOpen);

            return _journalDatabase;
         }
      }

      public bool IsOpen => _journalDatabase != null;

      private bool sessionIsAlreadyOpenFor(string fileName)
      {
         return string.Equals(CurrentJournalPath, fileName) && IsOpen;
      }
   }
}