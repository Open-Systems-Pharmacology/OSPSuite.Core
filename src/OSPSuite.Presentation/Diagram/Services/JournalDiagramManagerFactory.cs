using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;

namespace OSPSuite.Presentation.Diagram.Services
{
   public class JournalDiagramManagerFactory : IJournalDiagramManagerFactory
   {
      private readonly IDiagramToolTipCreator _toolTipCreator;

      public JournalDiagramManagerFactory(IDiagramToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
      }

      public IDiagramManager<JournalDiagram> Create() => new JournalDiagramManager(_toolTipCreator);
   }
}
