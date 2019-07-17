using System.IO;
using System.Linq;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.Journal
{
   public class JournalLoader : IJournalLoader
   {
      private readonly IJournalSession _journalSession;
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IJournalDiagramFactory _diagramFactory;

      public JournalLoader(IJournalSession journalSession, IDatabaseMediator databaseMediator, IJournalDiagramFactory diagramFactory)
      {
         _journalSession = journalSession;
         _databaseMediator = databaseMediator;
         _diagramFactory = diagramFactory;
      }

      public Core.Journal.Journal Load(string journalPath, string projectFullPath = null)
      {
         var journalFullPath = retrieveJournalAbsolutePath(journalPath, projectFullPath);
         var couldOpen = _journalSession.TryOpen(journalFullPath);
         return couldOpen ? LoadCurrent() : null;
      }

      private string retrieveJournalAbsolutePath(string journalPath, string projectFullPath)
      {
         if (FileHelper.FileExists(journalPath) || string.IsNullOrEmpty(projectFullPath) || string.IsNullOrEmpty(journalPath))
            return journalPath;

         return Path.GetFullPath(Path.Combine(FileHelper.FolderFromFileFullPath(projectFullPath), journalPath));
      }

      public Core.Journal.Journal LoadCurrent()
      {
         var journal = new Core.Journal.Journal {FullPath = _journalSession.CurrentJournalPath};
         journal.AddJournalPages(_databaseMediator.ExecuteQuery(new AllJournalPages()));
         journal.AddDiagrams(_databaseMediator.ExecuteQuery(new AllJournalDiagrams()));

         if (!journal.Diagrams.Any())
            createDefaultDiagram(journal);

         journal.Edited = journal.JournalPages.OrderByDescending(x => x.UpdatedAt).FirstOrDefault();
         return journal;
      }

      private void createDefaultDiagram(Core.Journal.Journal journal)
      {
         var journalDiagram = _diagramFactory.CreateDefault();
         journal.AddDiagram(journalDiagram);
         _databaseMediator.ExecuteCommand(new CreateJournalDiagram {Diagram = journalDiagram});
      }
   }
}