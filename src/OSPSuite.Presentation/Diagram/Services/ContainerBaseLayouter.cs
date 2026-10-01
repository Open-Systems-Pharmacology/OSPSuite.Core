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

      public IForceLayoutConfiguration ForceLayoutConfiguration { get; set; }

      public void DoForceLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes, int levelDepth)
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
            var visibleNodes = containerBase.GetAllChildren<IBaseNode>().Where(node => node.Visible).ToList();
            freeNodes.Each(freeNode => moveUntilFree(freeNode, visibleNodes.Where(node => !ReferenceEquals(node, freeNode)).ToList()));
         }
         finally
         {
            diagramModel.EndUpdate();
            diagramModel.FinishTransaction("PlaceFreeNodes");
         }
      }

      private static void moveUntilFree(IHasLayoutInfo freeNode, IReadOnlyList<IBaseNode> otherNodes)
      {
         for (var i = 0; i < MAX_FREE_NODE_MOVES && otherNodes.Any(node => node.Bounds.IntersectsWith(freeNode.Bounds)); i++)
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
