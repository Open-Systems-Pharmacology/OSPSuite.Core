using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class AllJournalDiagrams : IQuery<IEnumerable<JournalDiagram>>
   {
   }

   public class AllJournalDiagramsQuery : JournalDatabaseQuery<AllJournalDiagrams, IEnumerable<JournalDiagram>>
   {
      private readonly IOSPSuiteExecutionContext _context;
      private readonly IJournalDiagramManagerFactory _journalDiagramManagerFactory;

      public AllJournalDiagramsQuery(IJournalSession journalSession, IOSPSuiteExecutionContext context,IJournalDiagramManagerFactory journalDiagramManagerFactory)
         : base(journalSession)
      {
         _context = context;
         _journalDiagramManagerFactory = journalDiagramManagerFactory;
      }

      public override IEnumerable<JournalDiagram> Query(AllJournalDiagrams query)
      {
         var journalDiagrams = new List<JournalDiagram>();
         journalDiagrams.AddRange(Db.Diagrams.All());
         journalDiagrams.Each(deserializeDiagram);
         return journalDiagrams;
      }

      private void deserializeDiagram(JournalDiagram journalDiagram)
      {
         journalDiagram.DiagramModel = _context.Deserialize<IDiagramModel>(journalDiagram.Data);
         journalDiagram.DiagramManager = _journalDiagramManagerFactory.Create();
      }
   }
}