using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   //transitional: removed together with GoDiagram once every diagram uses the UI-free model
   public class ReactionDiagramModelFactory : IReactionDiagramModelFactory
   {
      public IDiagramModel Create() => new DiagramModel();
   }
}
