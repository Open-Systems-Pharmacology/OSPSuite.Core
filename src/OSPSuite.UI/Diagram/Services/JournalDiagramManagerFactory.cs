using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.UI.Diagram.Managers;

namespace OSPSuite.UI.Diagram.Services
{
   public class JournalDiagramManagerFactory : IJournalDiagramManagerFactory
   {
      private readonly IDiagramToolTipCreator _diagramToolTipCreator;

      public JournalDiagramManagerFactory(IDiagramToolTipCreator diagramToolTipCreator)
      {
         _diagramToolTipCreator = diagramToolTipCreator;
      }

      public IDiagramManager<JournalDiagram> Create()
      {
         return new JournalDiagramManager(_diagramToolTipCreator);
      }
   }
}