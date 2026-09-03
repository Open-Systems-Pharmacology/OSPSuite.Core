using OSPSuite.Core.Diagram;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class ReactionDiagramModelFactory : IReactionDiagramModelFactory
   {
      public IDiagramModel Create()
      {
         return new DiagramModel();
      }
   }
}
