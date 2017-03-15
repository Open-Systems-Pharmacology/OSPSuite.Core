using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   /// <summary>  SimpleContainerNode is ContainerBaseNode with Neighborhood nodes  </summary>
   public class SimpleContainerNode : ContainerNodeBase
   {
      public SimpleContainerNode()
      {
         UserFlags = NodeLayoutType.CONTAINER_NODE;
         GetPort().SetLinkValidator(isValidLink);
      }

      private bool isValidLink(IBaseNode node1, IBaseNode node2, object obj1, object obj2)
      {
         var fromNode = node1 as SimpleContainerNode;
         var toNode = node2 as SimpleContainerNode;
         if (toNode == null || fromNode == null) return false;

         // do not allow neighborhoods from/to logical container
         if (toNode.IsLogical || fromNode.IsLogical) return false;

         // do not allow neighborhoods from/to invisible container
         if (!toNode.Visible || !fromNode.Visible) return false;

         // do not allow duplicate neighborhoods
         if (fromNode != null)
            foreach (var neighborContainerNode in fromNode.getNeighboredContainerNodes())
               if (toNode == neighborContainerNode) return false;

         return true;
      }

      private IEnumerable<IContainerNode> getNeighboredContainerNodes()
      {
         IList<IContainerNode> neighboredContainerNodes = new List<IContainerNode>();
         foreach (var neighborhoodNode in GetLinkedNodes<INeighborhoodNode>())
         {
            IContainerNode neighboredContainerNode = neighborhoodNode.GetOtherContainerNode(this);
            if (neighboredContainerNode != null) neighboredContainerNodes.Add(neighboredContainerNode);
         }
         return neighboredContainerNodes;
      }

      public override void DoMove(GoView view, PointF origLoc, PointF newLoc)
      {
         try
         {
            base.DoMove(view, origLoc, newLoc);
            view.Refresh();
            view.BeginUpdate();
            AdjustPositions(this, new SizeF(newLoc - new SizeF(origLoc)));
            view.EndUpdate();
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }
      }

      protected void AdjustPositions(IContainerNode containerNode, SizeF offset)
      {
         var oldLocation = containerNode.Location;
         foreach (var neighborhoodNode in containerNode.GetLinkedNodes<INeighborhoodNode>(true))
            neighborhoodNode.AdjustPositionForContainerInMove(containerNode, offset);
         containerNode.Location = oldLocation;
      }

      public virtual void AddTransportLink(TransportLink link)
      {
         Port.AddDestinationLink(link);
      }
   }
}