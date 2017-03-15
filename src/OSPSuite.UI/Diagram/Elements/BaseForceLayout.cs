using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using Northwoods.Go.Layout;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class BaseForceLayout : GoLayoutForceDirected, IBaseForceLayout
   {
      private IForceLayoutConfiguration _layoutConfiguration;
      public IForceLayoutConfiguration Config
      {
         get { return _layoutConfiguration; }
         set
         {
            _layoutConfiguration = value;
            MaxIterations = value.MaxIterations;
            Epsilon = value.Epsilon;
            InfinityDistance = value.InfinityDistance;
            ArrangementSpacing = new SizeF(value.ArrangementSpacingWidth, value.ArrangementSpacingHeight);

            NUMBER_OF_GROUPS = ForceLayoutConfiguration.NUMBER_OF_GROUPS;
         }
      }

      protected RectangleF ContainerBaseBounds { get; private set; }

      private int NUMBER_OF_GROUPS = 10;

      private GoLayoutForceDirectedNode _mpNode;

      public void PerformLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes)
      {
         string text = "DiagramModel";
         var containerBaseNode = containerBase as IContainerNode;
         if (containerBaseNode != null) text = containerBaseNode.GetLongName();
         if (Config.LogPositions) Console.WriteLine("PerformLayout for " + text + " with Bounds " + containerBase.Bounds);


         ContainerBaseBounds = containerBase.Bounds;
         var originalBounds = containerBase.Bounds;
         var originalLocation = containerBase.Location;

         var net = CreateNetwork();
         net.AddNodesAndLinksFromCollection(containerBase as IGoCollection, true);

         foreach (var n in containerBase.GetDirectChildren<IContainerNode>())
            if (n.Name == Captions.MoleculeProperties)
            {
               _mpNode = net.FindNode((GoObject)n);
               break;
            }

         if (Config.LogPositions) PrintNodes("<", net);
         if (Config.LogPositions) PrintLinks("<", net);

         if (containerBaseNode != null) insertExternalNeighborLinks(containerBaseNode, net);

         if (Config.LogPositions) PrintNodes("+extNeighborLinks ", net);
         if (Config.LogPositions) PrintLinks("+extNeighborLinks ", net);

         // delete invisible nodes before replacing inner neighborhoods by links, to keep "end" neighbor nodes as nodes
         deleteInvisibleNodes(net);

         if (Config.LogPositions) PrintNodes("-invisible ", net);
         if (Config.LogPositions) PrintLinks("-invisible ", net);

         // Remove neighborhood nodes and neighbor links and insert direct links between the nodes they connect (ContainerNodes or PortNodes)
         replaceInnerNeighborhoodNodesByDirectLinks(containerBase, net);

         if (Config.LogPositions) PrintNodes("+intNeighborLinks ", net);
         if (Config.LogPositions) PrintLinks("+intNeighborLinks ", net);

         setNodeType(net, freeNodes);

         Network = net;

         if (Config.LogPositions) PrintNodes("<", net);
         if (Config.LogPositions) PrintLinks("<", net);


         if (net.NodeCount > 0) base.PerformLayout();
         if (Config.LogPositions) PrintNodes(">", net);

         if (Config.LogPositions) Console.WriteLine(" Bounds " + containerBase.Bounds);
         var newBounds = containerBase.CalculateBounds();
         if (Config.LogPositions) Console.WriteLine(" After " + newBounds + "ComputeBounds: Bounds " + containerBase.Bounds);


         if (containerBaseNode != null && containerBaseNode.LocationFixed)
            containerBase.Location = originalLocation;

         PointF containerTranslation = containerBase.Bounds.Location.Minus(originalBounds.Location);
         foreach (var netNode in net.Nodes) netNode.GoObject.Position = netNode.GoObject.Position.Minus(containerTranslation);

         if (Config.LogPositions) Console.WriteLine(" Bounds " + containerBase.Bounds);
      }

      private void insertExternalNeighborLinks(IContainerNode parentContainerNode, GoLayoutForceDirectedNetwork net)
      {
         foreach (var childContainerNode in parentContainerNode.GetDirectChildren<IContainerNode>())
            foreach (var neighborhoodNode in childContainerNode.GetLinkedNodes<INeighborhoodNode>())
               if (!parentContainerNode.ContainsChildNode(neighborhoodNode, true))
               {
                  var otherContainerNode = neighborhoodNode.GetOtherContainerNode(childContainerNode);
                  var nodeOnBoundary = new GoLayoutForceDirectedNode();
                  var pos = neighborhoodNode.Location;
                  bool pointExists = GoObject.GetNearestIntersectionPoint(parentContainerNode.Bounds, otherContainerNode.Center, childContainerNode.Center, out pos);

                  if (!pointExists) continue;
                  
                  //to be not deleted due to invisible node, seems to set also the position
                  nodeOnBoundary.GoObject = neighborhoodNode as GoObject; 
                  nodeOnBoundary.IsFixed = true;
                  nodeOnBoundary.UserFlags = NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE;

                  //set Position after setting GoObject, because setting GoObject seems to set position
                  nodeOnBoundary.Position = pos;   
                  net.AddNode(nodeOnBoundary);
                  net.LinkNodes(net.FindNode(childContainerNode as GoObject), nodeOnBoundary, neighborhoodNode as GoObject);
               }
      }

      private void replaceInnerNeighborhoodNodesByDirectLinks(IContainerBase containerBase, GoLayoutForceDirectedNetwork net)
      {
         // replace neighborhood nodes and neighbor links by direct links between the nodes they connect (ContainerNodes or PortNodes)
         foreach (var neighborHoodNode in containerBase.GetDirectChildren<INeighborhoodNode>())
         {
            var fromLink = net.FindLink(neighborHoodNode.FirstNeighborLink as GoObject);
            var toLink = net.FindLink(neighborHoodNode.SecondNeighborLink as GoObject);
            if (fromLink == null || toLink == null) continue;

            var netLink = new GoLayoutForceDirectedLink { FromNode = fromLink.ToNode, ToNode = toLink.ToNode };
            netLink.GoObject = neighborHoodNode as GoObject;
            netLink.Length = Config.BaseSpringLength * Config.RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE].Value;
            netLink.Stiffness = Config.BaseSpringLength * Config.RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE].Value;
            net.DeleteNode(neighborHoodNode as GoObject);
            net.AddLink(netLink);
         }
      }

      private void deleteInvisibleNodes(GoLayoutForceDirectedNetwork net)
      {
         IList<GoLayoutForceDirectedNode> invisibleNodes = new List<GoLayoutForceDirectedNode>();
         foreach (GoLayoutForceDirectedNode netNode in net.Nodes)
            if (netNode.GoObject == null || !netNode.GoObject.Visible)
               invisibleNodes.Add(netNode);

         foreach (var node in invisibleNodes) net.DeleteNode(node);

         IList<GoLayoutForceDirectedLink> invisibleLinks = new List<GoLayoutForceDirectedLink>();
         foreach (GoLayoutForceDirectedLink netLink in net.Links)
            if (netLink.GoObject == null || !netLink.GoObject.Visible)
               invisibleLinks.Add(netLink);

         foreach (var Link in invisibleLinks) net.DeleteLink(Link);
      }

      private void setNodeType(GoLayoutForceDirectedNetwork net, IList<IHasLayoutInfo> freeNodes)
      {
         foreach (GoLayoutForceDirectedNode netNode in net.Nodes)
         {
            IHasLayoutInfo hasLayoutInfo = netNode.GoObject as IHasLayoutInfo;
            if (hasLayoutInfo != null && netNode.UserFlags == 0)
            {
               netNode.IsFixed = hasLayoutInfo.LocationFixed
                                 || hasLayoutInfo.IsAnImplementationOf<PortNode>()
                                 || (freeNodes != null && !freeNodes.Contains(hasLayoutInfo));
               netNode.UserFlags = hasLayoutInfo.UserFlags;

               // position linkless nodes by gravitational force (fix positioning seems not so easy because of moving bounds)
               if (netNode.LinksCount == 0) netNode.UserFlags = NodeLayoutType.LINKLESS_NODE; //try to position linkless nodes by grav force,
            }
         }
      }

      private void PrintNodes(string prefix, GoLayoutForceDirectedNetwork net)
      {
         string text = "";
         foreach (var netNode in net.Nodes) text += NodeText(netNode) + NodePosition(netNode) + "; ";
         Console.WriteLine(prefix + " " + text + "\n");
      }

      private void PrintLinks(string prefix, GoLayoutForceDirectedNetwork net)
      {
         string text = "";
         foreach (var netLink in net.Links) text += NodeText(netLink.FromNode) + "-" + NodeText(netLink.ToNode) + "; ";
         Console.WriteLine(prefix + " " + text + "\n");
      }

      private string NodeText(GoLayoutForceDirectedNode netNode)
      {
         string text = "";
         if (netNode == null)
            return "XXX";
         var goNode = netNode.GoObject as GoNode;
         if (goNode != null) text += goNode.Text;
         text += ":" + netNode.UserFlags + (netNode.IsFixed ? "F" : ""); // F = fixed
         text += ((netNode.GoObject == null || !netNode.GoObject.Visible) ? "I" : ""); // i = invisible
         return text;
      }

      private string NodePosition(GoLayoutForceDirectedNode netNode)
      {
         return "[" + Convert.ToInt32(netNode.Position.X) + "|" + Convert.ToInt32(netNode.Position.Y) + "]";
      }

      protected override bool UpdatePositions()
      {
         bool rc = base.UpdatePositions();
         if (Network.NodeCount <= 0) return rc;

         var node1 = Network.Nodes.First();
         RectangleF currentBounds = new RectangleF(node1.Bounds.X, node1.Bounds.Y, 0, 0);
         foreach (GoLayoutForceDirectedNode node in Network.Nodes)
         {
            var nodeBounds = node.Bounds;
            if (nodeBounds.X < currentBounds.X) currentBounds.X = nodeBounds.X;
            if (nodeBounds.Y < currentBounds.Y) currentBounds.Y = nodeBounds.Y;
            if (nodeBounds.X + nodeBounds.Width > currentBounds.X + currentBounds.Width) currentBounds.Width = nodeBounds.X + nodeBounds.Width - currentBounds.X;
            if (nodeBounds.Y + nodeBounds.Height > currentBounds.Y + currentBounds.Height) currentBounds.Height = nodeBounds.Y + nodeBounds.Height - currentBounds.Y;
         }
         ContainerBaseBounds = currentBounds;

         return rc;
      }

      protected override float ElectricalFieldX(PointF xy)
      {
         return 0;
      }

      protected override float ElectricalFieldY(PointF xy)
      {
         return 0;
      }

      protected override float GravitationalFieldX(PointF p)
      {
         float gravitationalCenterDistance = (ContainerBaseBounds.Left + ContainerBaseBounds.Center().X) / 2F - p.X;
         return 4F * gravitationalCenterDistance / ContainerBaseBounds.Width;
      }

      protected override float GravitationalFieldY(PointF p)
      {
         float gravitationalCenterDistance = (ContainerBaseBounds.Top + ContainerBaseBounds.Center().Y) / 2F - p.Y;
         return 4F * gravitationalCenterDistance / ContainerBaseBounds.Height;
      }

      protected override float ElectricalCharge(GoLayoutForceDirectedNode pNode)
      {
         int group = pNode.UserFlags;
         if (group >= 0 && group < NUMBER_OF_GROUPS && Config.RelativeElectricalChargeOf[group].HasValue)
         {
            float charge = Config.BaseElectricalCharge * Config.RelativeElectricalChargeOf[group].Value;
            float sizefactor = (float)(Math.Sqrt(pNode.Width / 150F * pNode.Height / 50F));
            if (sizefactor > 40) sizefactor = 40F;

            return charge * sizefactor;
         }
         return Config.BaseElectricalCharge;
      }

      protected override float GravitationalMass(GoLayoutForceDirectedNode pNode)
      {
         int group = pNode.UserFlags;
         if (group >= 0 && group < NUMBER_OF_GROUPS && Config.RelativeGravitationalMassOf[group].HasValue) return Config.BaseGravitationalMass * Config.RelativeGravitationalMassOf[group].Value;
         return Config.BaseGravitationalMass;
      }

      protected override float SpringLength(GoLayoutForceDirectedLink pLink)
      {
         GoLayoutForceDirectedNode pFromNode = pLink.FromNode;
         int fromGroup = pFromNode.UserFlags;
         GoLayoutForceDirectedNode pToNode = pLink.ToNode;
         int toGroup = pToNode.UserFlags;

         if (fromGroup >= 0 && fromGroup < NUMBER_OF_GROUPS && toGroup >= 0 && toGroup < NUMBER_OF_GROUPS
             && Config.RelativeSpringLengthOf[fromGroup, toGroup].HasValue)
         {
            float length = Config.BaseSpringLength * Config.RelativeSpringLengthOf[fromGroup, toGroup].Value;
            float fromSizefactor = pFromNode.Width / 150F * pFromNode.Height / 50F;
            float toSizefactor = pToNode.Width / 150F * pToNode.Height / 50F;
            float sizefactor = (float)(Math.Sqrt(fromSizefactor + toSizefactor));
            if (sizefactor > 40) sizefactor = 40F;

            return length * sizefactor;
         }
         return Config.BaseSpringLength;
      }

      protected override float SpringStiffness(GoLayoutForceDirectedLink pLink)
      {
         GoLayoutForceDirectedNode pFromNode = pLink.FromNode;
         int fromGroup = pFromNode.UserFlags;

         GoLayoutForceDirectedNode pToNode = pLink.ToNode;
         int toGroup = pToNode.UserFlags;
         if (fromGroup >= 0 && fromGroup < NUMBER_OF_GROUPS && toGroup >= 0 && toGroup < NUMBER_OF_GROUPS
             && Config.RelativeSpringStiffnessOf[fromGroup, toGroup].HasValue)
         {
            float stiffness = Config.BaseSpringStiffness * Config.RelativeSpringStiffnessOf[fromGroup, toGroup].Value;
            float fromSizefactor = pFromNode.Width / 150F * pFromNode.Height / 50F;
            float toSizefactor = pToNode.Width / 150F * pToNode.Height / 50F;
            float sizefactor = (float)(Math.Sqrt(fromSizefactor + toSizefactor));
            if (sizefactor > 10) sizefactor = 10F;

            return stiffness / sizefactor;
         }
         return Config.BaseSpringStiffness;
      }

   }
}