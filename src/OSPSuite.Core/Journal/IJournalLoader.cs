namespace OSPSuite.Core.Journal
{
   public interface IJournalLoader
   {
      /// <summary>
      ///    Load the journal path located at <paramref name="journalPath" /> using the <paramref name="projectFullPath" /> to
      ///    resolve relative path if required
      /// </summary>
      /// <param name="journalPath">Absolute path of journal or relative path to <paramref name="projectFullPath" /></param>
      /// <param name="projectFullPath">
      ///    Project full path. Only required if the <paramref name="journalPath" /> is a relative
      ///    path
      /// </param>
      Journal Load(string journalPath, string projectFullPath = null);

      Journal LoadCurrent();
   }
}