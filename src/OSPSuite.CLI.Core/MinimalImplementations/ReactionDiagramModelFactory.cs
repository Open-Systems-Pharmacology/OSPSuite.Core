using OSPSuite.Core.Diagram;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   //transitional: removed together with GoDiagram once every diagram uses the UI-free model
   public class ReactionDiagramModelFactory : IReactionDiagramModelFactory
   {
      public IDiagramModel Create()
      {
         return new DiagramModel();
      }
   }
}
