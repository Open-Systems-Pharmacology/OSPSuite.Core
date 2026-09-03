using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ReactionDiagramModelFactory : IReactionDiagramModelFactory
   {
      public IDiagramModel Create() => new DiagramModel();
   }
}
