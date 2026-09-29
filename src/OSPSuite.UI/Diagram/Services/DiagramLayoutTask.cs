using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Utility.Extensions;
using DiagramModel = OSPSuite.Presentation.Diagram.Elements.DiagramModel;
using ReactionNode = OSPSuite.UI.Diagram.Elements.ReactionNode;

namespace OSPSuite.UI.Diagram.Services
{
   public class DiagramLayoutTask : IDiagramLayoutTask
   {
      private readonly ILayerLayouter _layerLayouter;
      private readonly LayeredReactionLayouter _layeredReactionLayouter = new LayeredReactionLayouter();

      public DiagramLayoutTask(ILayerLayouter layerLayouter)
      {
         _layerLayouter = layerLayouter;
      }

      public void LayoutReactionDiagram(IContainerBase containerBase)
      {
         //transitional: removed together with GoDiagram once every diagram uses the UI-free model
         var reactionDiagramModel = reactionDiagramModelFor(containerBase);
         if (reactionDiagramModel != null)
         {
            layoutReactionDiagram(containerBase, reactionDiagramModel);
            return;
         }

         var diagramModel = containerBase as IDiagramModel;

         foreach (var reactionNode in containerBase.GetAllChildren<ReactionNode>())
            reactionNode.DisplayEductsRight = false;

         _layerLayouter.PerformLayout(containerBase, null);

         if(diagramModel != null)
            diagramModel.IsLayouted = true;
      }

      private void layoutReactionDiagram(IContainerBase containerBase, DiagramModel diagramModel)
      {
         diagramModel.StartTransaction();
         try
         {
            diagramModel.BeginUpdate();
            containerBase.GetAllChildren<OSPSuite.Presentation.Diagram.Elements.ReactionNode>().Each(reactionNode => reactionNode.DisplayEductsRight = false);
            _layeredReactionLayouter.Layout(containerBase);
            diagramModel.IsLayouted = true;
         }
         finally
         {
            diagramModel.EndUpdate();
            diagramModel.FinishTransaction("LayoutReactionDiagram");
         }
      }

      private static DiagramModel reactionDiagramModelFor(IContainerBase containerBase)
      {
         var current = containerBase;
         while (current is IBaseNode node)
            current = node.GetParent();

         return current as DiagramModel;
      }
   }
}
