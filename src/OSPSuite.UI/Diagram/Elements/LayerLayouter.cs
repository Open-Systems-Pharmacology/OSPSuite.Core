using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layout;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class LayerLayouter : GoLayoutLayeredDigraph, ILayerLayouter
   {
      private readonly IDialogCreator _dialogCreator;

      public LayerLayouter(IDialogCreator dialogCreator)
      {
         _dialogCreator = dialogCreator;
      }

      public void PerformLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes)
      {
         GoDocument doc = containerBase as GoDocument;
         if (doc == null && containerBase as GoObject == null)
         {
            _dialogCreator.MessageBoxInfo("Bad ContainerBase Type = " + containerBase.GetType().Name);
            return;
         }

         if (doc == null) doc = ((GoObject)containerBase).Document;
         Document = doc;

         doc.StartTransaction();

         string text = "DiagramModel";
         var containerBaseNode = containerBase as IContainerNode;
         if (containerBaseNode != null) text = containerBaseNode.GetLongName();

         LayerSpacing = 0F;
         ColumnSpacing = 0F;
         DirectionOption = GoLayoutDirection.Right;
         CycleRemoveOption = GoLayoutLayeredDigraphCycleRemove.DepthFirst;
         LayeringOption = GoLayoutLayeredDigraphLayering.OptimalLinkLength;
         InitializeOption = GoLayoutLayeredDigraphInitIndices.DepthFirstOut;
         Iterations = 4;
         AggressiveOption = GoLayoutLayeredDigraphAggressive.Less;
         PackOption = GoLayoutLayeredDigraphPack.Straighten;
         SetsPortSpots = false;


         var net = CreateNetwork();
         net.AddNodesAndLinksFromCollection(containerBase as IGoCollection, true);

         setNodeType(net, freeNodes);

         Network = net;

         if (net.NodeCount > 0) base.PerformLayout();
         doc.FinishTransaction("DoLayerLayout");

      }

      private void setNodeType(GoLayoutLayeredDigraphNetwork net, IList<IHasLayoutInfo> freeNodes)
      {
         IList<GoLayoutLayeredDigraphNode> fixedNodes = new List<GoLayoutLayeredDigraphNode>();
         foreach (GoLayoutLayeredDigraphNode netNode in net.Nodes)
         {
            IHasLayoutInfo hasLayoutInfo = netNode.GoObject as IHasLayoutInfo;
            if (hasLayoutInfo != null && hasLayoutInfo.LocationFixed) fixedNodes.Add(netNode);
         }
         foreach (GoLayoutLayeredDigraphNode netNode in fixedNodes) net.DeleteNode(netNode);
      }

      protected override float NodeMinLayerSpace(GoLayoutLayeredDigraphNode node, bool topleft)
      {
         return base.NodeMinLayerSpace(node, !topleft);
      }

      private void PrintNodes(string prefix, GoLayoutLayeredDigraphNetwork net)
      {
         string text = "";
         foreach (var netNode in net.Nodes) text += NodeText(netNode) + NodePosition(netNode) + "; ";
         Console.WriteLine(prefix + " " + text + "\n");
      }

      private void PrintLinks(string prefix, GoLayoutLayeredDigraphNetwork net)
      {
         string text = "";
         foreach (var netLink in net.Links) text += NodeText(netLink.FromNode) + "-" + NodeText(netLink.ToNode) + "; ";
         Console.WriteLine(prefix + " " + text + "\n");
      }

      private string NodeText(GoLayoutLayeredDigraphNode netNode)
      {
         string text = "";
         if (netNode == null)
            return "XXX";
         var goNode = netNode.GoObject as GoNode;
         if (goNode != null) text += goNode.Text;
         text += ":" + netNode.UserFlags;
         text += ((netNode.GoObject == null || !netNode.GoObject.Visible) ? "I" : ""); // i = invisible
         return text;
      }

      private string NodePosition(GoLayoutLayeredDigraphNode netNode)
      {
         return "[" + Convert.ToInt32(netNode.Position.X) + "|" + Convert.ToInt32(netNode.Position.Y) + "]";
      }
   }
}
