using System.Drawing;
using Northwoods.Go;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   public class SimpleNeighborhoodNode : ElementBaseNode, INeighborhoodNode
   {
      private NeighborLink _neighbor1Link;
      private NeighborLink _neighbor2Link;

      public SimpleNeighborhoodNode()
      {
         UserFlags = NodeLayoutType.NEIGHBORHOOD_NODE;
         NodeBaseSize = new SizeF(15F, 15F);
         NodeSize = NodeSize.Middle;

         Label = null;

         Port.IsValidTo = false;
         Port.IsValidFrom = false;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetBrushColor(diagramColors, diagramColors.NeighborhoodNode);
         if (FirstNeighborLink != null) FirstNeighborLink.SetColorFrom(diagramColors);
         if (SecondNeighborLink != null) SecondNeighborLink.SetColorFrom(diagramColors);
      }

      public override string Name { get; set; }

      public override NodeSize NodeSize
      {
         get { return _nodeSize; }
         set
         {
            _nodeSize = value;
            RefreshSize();
         }
      }

      public void Initialize(IContainerNode firstNeighborNode, IContainerNode secondNeighborNode)
      {
         GoLayer defaultLayer = Document.DefaultLayer;
         GoSubGraphBase.ReparentToCommonSubGraph(this, (GoObject)firstNeighborNode, (GoObject)secondNeighborNode, true, defaultLayer);

         FirstNeighbor = firstNeighborNode;
         SecondNeighbor = secondNeighborNode;
         if (FirstNeighbor == null || SecondNeighbor == null)
            throw new OSPSuiteException();

         //Refresh Ports names and postions after connecting both neighbors, if MultiPortContainerNodes are uses
         var multiPortFirstNeighbor = FirstNeighbor as MultiPortContainerNode;
         var multiPortSecondNeighbor = SecondNeighbor as MultiPortContainerNode;
         if (multiPortFirstNeighbor != null) multiPortFirstNeighbor.RefreshPort(_neighbor1Link);
         if (multiPortSecondNeighbor != null) multiPortSecondNeighbor.RefreshPort(_neighbor2Link);

         AdjustPosition();
      }

      public IContainerNode FirstNeighbor
      {
         get
         {
            if (_neighbor1Link == null) return null;
            return _neighbor1Link.ContainerNode;
         }
         private set
         {
            if (_neighbor1Link == null)
            {
               _neighbor1Link = new NeighborLink(this, value);
               if (FirstNeighbor == null)
                  throw new OSPSuiteException();
            }
            else if (!FirstNeighbor.Equals(value))
               throw new OSPSuiteException("Conflicting Neigborhood definition for first Neighbor : " + value.Name + " != " + FirstNeighbor.Name);
         }
      }

      public IContainerNode SecondNeighbor
      {
         get
         {
            if (_neighbor2Link == null) return null;
            return _neighbor2Link.ContainerNode;
         }
         private set
         {
            if (_neighbor2Link == null)
            {
               _neighbor2Link = new NeighborLink(this, value);
               if (SecondNeighbor == null) throw new OSPSuiteException();
            }
            else if (!SecondNeighbor.Equals(value))
               throw new OSPSuiteException
                  ("Conflicting Neigborhood definition for second Neighbor : " + value.Name + " != " + SecondNeighbor.Name);
         }
      }

      public IBaseLink FirstNeighborLink
      {
         get { return _neighbor1Link; }
      }

      public IBaseLink SecondNeighborLink
      {
         get { return _neighbor2Link; }
      }

      public IContainerNode GetOtherContainerNode(IContainerNode node)
      {
         if (node.Equals(FirstNeighbor)) return SecondNeighbor;
         if (node.Equals(SecondNeighbor)) return FirstNeighbor;
         throw new OSPSuiteException("ContainerNode " + node.Name + "(" + node.Id + ") is not linked to NeighborhoodNode " + Name + "(" + Id + ")");
      }

      public void AdjustPosition()
      {
         if (!checkAdjustabilityAndRefreshPorts()) return;

         Location = mean(_neighbor1Link.ToNode.GoObject.Center, _neighbor2Link.ToNode.GoObject.Center);
      }

      public void AdjustPositionForContainerInMove(IContainerNode node, SizeF offset)
      {
         if (!checkAdjustabilityAndRefreshPorts()) return;

         NeighborLink neighborLinkToContainerInMove = null;
         NeighborLink neighborLinkToOtherContainer = null;

         if (node.ContainsChildNode(FirstNeighbor, true))
         {
            neighborLinkToContainerInMove = _neighbor1Link;
            neighborLinkToOtherContainer = _neighbor2Link;
         }
         if (node.ContainsChildNode(SecondNeighbor, true))
         {
            neighborLinkToContainerInMove = _neighbor2Link;
            neighborLinkToOtherContainer = _neighbor1Link;
         }
         if (neighborLinkToContainerInMove == null) return;

         Location = mean(neighborLinkToOtherContainer.ToArrowEndPoint, neighborLinkToContainerInMove.ToArrowEndPoint + offset);
      }

      private bool checkAdjustabilityAndRefreshPorts()
      {
         if (_neighbor1Link == null || _neighbor2Link == null) return false;
         _neighbor1Link.UpdateRoute();
         _neighbor2Link.UpdateRoute();

         //Refresh Ports names and postions after connecting both neighbors, if MultiPortContainerNodes are used
         var multiPortFirstNeighbor = FirstNeighbor as MultiPortContainerNode;
         var multiPortSecondNeighbor = SecondNeighbor as MultiPortContainerNode;
         if (multiPortFirstNeighbor != null) multiPortFirstNeighbor.RefreshPort(_neighbor1Link);
         if (multiPortSecondNeighbor != null) multiPortSecondNeighbor.RefreshPort(_neighbor2Link);


         if (LocationFixed || FirstNeighbor.Hidden || SecondNeighbor.Hidden) return false;
         return true;
      }

      private PointF mean(PointF p1, PointF p2)
      {
         return new PointF((p1.X + p2.X) / 2F, (p1.Y + p2.Y) / 2F);
      }
   }
}
