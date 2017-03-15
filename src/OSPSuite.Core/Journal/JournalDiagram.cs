using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Journal
{
   public class JournalDiagram : JournalEntity, IWithName, IWithDiagramFor<JournalDiagram>
   {
      public string Name { get; set; }
      public byte[] Data { get; set; }
      public IDiagramModel DiagramModel { get; set; }
      public IDiagramManager<JournalDiagram> DiagramManager { get; set; }

      /// <summary>
      /// Reference to containing journal
      /// </summary>
      public Journal Journal { get; set; }
   }
}