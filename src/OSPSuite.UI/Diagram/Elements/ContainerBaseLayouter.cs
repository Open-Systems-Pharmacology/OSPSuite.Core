using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Diagram.Services;
using OSPSuite.Utility.Extensions;
using ReactionDiagramModel = OSPSuite.Presentation.Diagram.Elements.DiagramModel;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ContainerBaseLayouter : IContainerBaseLayouter
   {
      private const int MAX_FREE_NODE_MOVES = 1000;
      private readonly LayeredReactionLayouter _layeredReactionLayouter = new LayeredReactionLayouter();
      protected BaseForceLayout _simpleForceLayouter;
      public IForceLayoutConfiguration ForceLayoutConfiguration
      {
         get { return _simpleForceLayouter.Config; }
         set { _simpleForceLayouter.Config = value; }
      }

      public ContainerBaseLayouter()
      {
         _simpleForceLayouter = new BaseForceLayout();
      }

      public void DoForceLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes, int levelDepth)
      {
         //transitional: removed together with GoDiagram once every diagram uses the UI-free model
         var reactionDiagramModel = reactionDiagramModelFor(containerBase);
         if (reactionDiagramModel != null)
         {
            doForceLayoutForReactionDiagram(containerBase, reactionDiagramModel, freeNodes);
            return;
         }

         var doc = containerBase as GoDocument;
         if (doc == null) doc = ((GoObject)containerBase).Document;

         _simpleForceLayouter.Document = doc;

         doc.StartTransaction();
         doc.Bounds = doc.ComputeBounds();
         DoLayoutForContainerBase(_simpleForceLayouter, containerBase, freeNodes, levelDepth);
         doc.Bounds = doc.ComputeBounds();
         doc.FinishTransaction("DoCompleteForceLayout");
      }

      private void doForceLayoutForReactionDiagram(IContainerBase containerBase, ReactionDiagramModel diagramModel, IList<IHasLayoutInfo> freeNodes)
      {
         diagramModel.StartTransaction();
         try
         {
            diagramModel.BeginUpdate();
            if (freeNodes == null || freeNodes.Count == 0)
            {
               _layeredReactionLayouter.Layout(containerBase);
               diagramModel.IsLayouted = true;
               return;
            }

            var visibleNodes = containerBase.GetAllChildren<IBaseNode>().Where(node => node.Visible).ToList();
            freeNodes.Each(freeNode => moveUntilFree(freeNode, visibleNodes.Where(node => !ReferenceEquals(node, freeNode)).ToList()));
         }
         finally
         {
            diagramModel.EndUpdate();
            diagramModel.FinishTransaction("DoCompleteForceLayout");
         }
      }

      private static void moveUntilFree(IHasLayoutInfo freeNode, IReadOnlyList<IBaseNode> otherNodes)
      {
         for (var i = 0; i < MAX_FREE_NODE_MOVES && otherNodes.Any(node => node.Bounds.IntersectsWith(freeNode.Bounds)); i++)
         {
            freeNode.Location = freeNode.Location.Plus(Assets.Diagram.Base.InsertLocationOffset);
         }
      }

      private static ReactionDiagramModel reactionDiagramModelFor(IContainerBase containerBase)
      {
         var current = containerBase;
         while (current is IBaseNode node)
            current = node.GetParent();

         return current as ReactionDiagramModel;
      }

      protected void DoLayoutForContainerBase(IBaseForceLayout layouter, IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes, int levelDepth)
      {
         // Layout each childcontainer to determine the right size
         for (int level = levelDepth; level > 0; level--)
         {
            foreach (var childContainer in getExpandedChildren(containerBase, level))
            {
               layouter.PerformLayout(childContainer, freeNodes);
               childContainer.PostLayoutStep();
            }
         }

         // Layout each childcontainer again to layout the content based on neighborhood ports
         for (int level = 0; level <= levelDepth; level++)
         {
            foreach (var childContainer in getExpandedChildren(containerBase, level))
            {
               layouter.PerformLayout(childContainer, freeNodes);
               childContainer.PostLayoutStep();
            }
         }
         containerBase.PostLayoutStep();

      }

      private IEnumerable<IContainerBase> getExpandedChildren(IContainerBase containerBase, int level)
      {
         var children = new List<IContainerBase>();
         if (level == 0) children.Add(containerBase);
         else
         {
            foreach (var childContainerNode in containerBase.GetDirectChildren<IContainerNode>())
               if (childContainerNode.IsExpanded) children.AddRange(getExpandedChildren(childContainerNode, level - 1));
         }
         return children;
      }

   }

}
