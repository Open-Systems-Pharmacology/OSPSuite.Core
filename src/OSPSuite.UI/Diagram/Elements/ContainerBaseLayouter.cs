using System.Collections.Generic;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ContainerBaseLayouter : IContainerBaseLayouter
   {
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
         var doc = containerBase as GoDocument;
         if (doc == null) doc = ((GoObject)containerBase).Document;

         _simpleForceLayouter.Document = doc;

         doc.StartTransaction();
         doc.Bounds = doc.ComputeBounds();
         DoLayoutForContainerBase(_simpleForceLayouter, containerBase, freeNodes, levelDepth);
         doc.Bounds = doc.ComputeBounds();
         doc.FinishTransaction("DoCompleteForceLayout");
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