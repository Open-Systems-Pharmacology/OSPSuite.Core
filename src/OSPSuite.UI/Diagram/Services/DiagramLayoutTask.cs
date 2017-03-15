using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Services
{
   public class DiagramLayoutTask : IDiagramLayoutTask
   {
      private readonly ILayerLayouter _layerLayouter;

      public DiagramLayoutTask(ILayerLayouter layerLayouter)
      {
         _layerLayouter = layerLayouter;
      }

      public void LayoutReactionDiagram(IContainerBase containerBase)
      {
         var diagramModel = containerBase as IDiagramModel;

         foreach (var reactionNode in containerBase.GetAllChildren<ReactionNode>())
            reactionNode.DisplayEductsRight = false;

         _layerLayouter.PerformLayout(containerBase, null);

         if(diagramModel != null)
            diagramModel.IsLayouted = true;
      }
   }
}
