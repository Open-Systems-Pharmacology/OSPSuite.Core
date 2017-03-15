using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public interface IWorkspace
   {
      /// <summary>
      ///    Clear all active session from workspace
      /// </summary>
      void Clear();

      /// <summary>
      ///    Retrieves or sets the Journal associated with the workspace
      /// </summary>
      Journal.Journal Journal { get; set; }

      /// <summary>
      ///    Tries to access and lock the given file.
      /// </summary>
      /// <param name="fullPath">Path to access</param>
      /// <exception cref="CannotLockFileException">is thrown if the file cannot be locked</exception>
      void LockFile(string fullPath);

      /// <summary>
      ///    Set to true, the project was open as readonly
      /// </summary>
      bool ProjectIsReadOnly { get; set; }

      /// <summary>
      ///    Tries to acccess the file <paramref name="fileFullPath" />
      /// </summary>
      void AccessFile(string fileFullPath);

      /// <summary>
      ///    Updates the path of the underlying working journal to use a relative path if possible
      /// </summary>
      /// <param name="projectFileFullPath">Full path of the project file associated with the working journal</param>
      void UpdateJournalPathRelativeTo(string projectFileFullPath);
   }
}