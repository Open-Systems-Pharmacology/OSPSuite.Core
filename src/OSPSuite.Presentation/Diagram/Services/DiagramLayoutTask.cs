using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Services
{
   public class DiagramLayoutTask : IDiagramLayoutTask
   {
      private readonly LayeredReactionLayouter _layeredReactionLayouter = new LayeredReactionLayouter();

      public void LayoutReactionDiagram(IContainerBase containerBase)
      {
         var diagramModel = diagramModelFor(containerBase);
         if (diagramModel == null)
            return;

         diagramModel.StartTransaction();
         try
         {
            diagramModel.BeginUpdate();
            containerBase.GetAllChildren<ReactionNode>().Each(reactionNode => reactionNode.DisplayEductsRight = false);
            _layeredReactionLayouter.Layout(containerBase);
            diagramModel.IsLayouted = true;
         }
         finally
         {
            diagramModel.EndUpdate();
            diagramModel.FinishTransaction("LayoutReactionDiagram");
         }
      }

      private static DiagramModel diagramModelFor(IContainerBase containerBase)
      {
         var current = containerBase;
         while (current is IBaseNode node)
            current = node.GetParent();

         return current as DiagramModel;
      }
   }
}
