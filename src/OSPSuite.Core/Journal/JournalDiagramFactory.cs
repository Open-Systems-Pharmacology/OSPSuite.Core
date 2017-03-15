using OSPSuite.Assets;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Core.Journal
{
   public interface IJournalDiagramFactory
   {
      JournalDiagram Create(string diagramName);
      JournalDiagram CreateDefault();
   }

   public class JournalDiagramFactory : IJournalDiagramFactory
   {
      private readonly IDiagramModelFactory _diagramModelFactory;
      private readonly IJournalDiagramManagerFactory _journalDiagramManagerFactory;

      public JournalDiagramFactory(IDiagramModelFactory diagramModelFactory, IJournalDiagramManagerFactory journalDiagramManagerFactory)
      {
         _diagramModelFactory = diagramModelFactory;
         _journalDiagramManagerFactory = journalDiagramManagerFactory;
      }

      public JournalDiagram Create(string diagramName)
      {
         return new JournalDiagram
         {
            Name = diagramName,
            DiagramModel = _diagramModelFactory.Create(),
            DiagramManager = _journalDiagramManagerFactory.Create()
         };
      }

      public JournalDiagram CreateDefault()
      {
         return Create(Captions.Journal.DefaultDiagramName);
      }
   }
}