using OSPSuite.Core.Diagram;

namespace OSPSuite.Core.Journal
{
   public interface IJournalDiagramManagerFactory
   {
      IDiagramManager<JournalDiagram> Create();
   }
}