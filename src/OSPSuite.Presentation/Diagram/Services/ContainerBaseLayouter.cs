using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Services
{
   public class ContainerBaseLayouter : IContainerBaseLayouter
   {
      private const int MAX_FREE_NODE_MOVES = 1000;

      public void PlaceFreeNodes(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes)
      {
         if (freeNodes == null || freeNodes.Count == 0)
            return;

         var diagramModel = diagramModelFor(containerBase);
         if (diagramModel == null)
            return;

         diagramModel.StartTransaction();
         try
         {
            diagramModel.BeginUpdate();
            var visibleNodes = containerBase.GetAllChildren<DiagramNode>().Where(node => node.Visible).ToList();
            freeNodes.Cast<DiagramNode>().Each(freeNode => moveUntilFree(freeNode, visibleNodes.Where(node => !isPartOf(node, freeNode)).ToList()));
         }
         finally
         {
            diagramModel.EndUpdate();
            diagramModel.FinishTransaction("PlaceFreeNodes");
         }
      }

      private static bool isPartOf(IBaseNode node, IHasLayoutInfo freeNode)
      {
         return ReferenceEquals(node, freeNode) || (freeNode as IContainerNode)?.ContainsChildNode(node, recursive: true) == true;
      }

      private static void moveUntilFree(DiagramNode freeNode, IReadOnlyList<DiagramNode> otherNodes)
      {
         for (var i = 0; i < MAX_FREE_NODE_MOVES && otherNodes.Any(node => node.DrawnBounds.IntersectsWith(freeNode.DrawnBounds)); i++)
         {
            freeNode.Location = freeNode.Location.Plus(Assets.Diagram.Base.InsertLocationOffset);
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
