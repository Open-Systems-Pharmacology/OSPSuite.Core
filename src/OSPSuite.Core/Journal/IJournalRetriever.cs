namespace OSPSuite.Core.Journal
{
   public interface IJournalRetriever
   {
      Journal Current { get; set; }

      /// <summary>
      ///    Returns the full path of the current working journal or an empty string if no journal is availble
      /// </summary>
      string JournalFullPath { get; }
   }

   public class JournalRetriever : IJournalRetriever
   {
      private readonly IWorkspace _workspace;

      public JournalRetriever(IWorkspace workspace)
      {
         _workspace = workspace;
      }

      public Journal Current
      {
         get { return _workspace.Journal; }
         set { _workspace.Journal = value; }
      }

      public string JournalFullPath => Current?.FullPath ?? string.Empty;
   }
}