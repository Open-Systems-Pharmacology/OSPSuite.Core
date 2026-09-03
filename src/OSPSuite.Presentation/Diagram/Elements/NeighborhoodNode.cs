using System.Drawing;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class NeighborLink : BaseLink
   {
      public IContainerNode ContainerNode => GetToNode() as IContainerNode;

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         Color = diagramColors.NeighborhoodLink;
         NotifyChanged();
      }
   }

   public class NeighborhoodNode : ElementBaseNode, INeighborhoodNode
   {
      private NeighborLink _firstNeighborLink;
      private NeighborLink _secondNeighborLink;

      public NeighborhoodNode()
      {
         UserFlags = NodeLayoutType.NEIGHBORHOOD_NODE;
         NodeBaseSize = new SizeF(15F, 15F);
         NodeSize = NodeSize.Middle;
         CanLink = false;
      }

      public override bool LabelVisible => false;

      public void Initialize(IContainerNode firstNeighborNode, IContainerNode secondNeighborNode)
      {
         if (firstNeighborNode == null || secondNeighborNode == null)
            throw new OSPSuiteException();

         _firstNeighborLink?.Unlink();
         _secondNeighborLink?.Unlink();
         _firstNeighborLink = createLinkTo(firstNeighborNode);
         _secondNeighborLink = createLinkTo(secondNeighborNode);
         AdjustPosition();
      }

      private NeighborLink createLinkTo(IContainerNode containerNode)
      {
         var link = new NeighborLink();
         link.Initialize(this, containerNode);
         return link;
      }

      public IContainerNode FirstNeighbor => _firstNeighborLink?.ContainerNode;

      public IContainerNode SecondNeighbor => _secondNeighborLink?.ContainerNode;

      public IBaseLink FirstNeighborLink => _firstNeighborLink;

      public IBaseLink SecondNeighborLink => _secondNeighborLink;

      public IContainerNode GetOtherContainerNode(IContainerNode node)
      {
         if (node == FirstNeighbor) return SecondNeighbor;
         if (node == SecondNeighbor) return FirstNeighbor;
         return null;
      }

      public void AdjustPosition()
      {
         if (FirstNeighbor == null || SecondNeighbor == null) return;
         var first = FirstNeighbor.Center;
         var second = SecondNeighbor.Center;
         Location = new PointF((first.X + second.X) / 2, (first.Y + second.Y) / 2);
      }

      public void AdjustPositionForContainerInMove(IContainerNode node, SizeF offset)
      {
         Location = Location.Plus(new PointF(offset.Width / 2, offset.Height / 2));
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetFillColorFrom(diagramColors, diagramColors.NeighborhoodNode);
         _firstNeighborLink?.SetColorFrom(diagramColors);
         _secondNeighborLink?.SetColorFrom(diagramColors);
      }

      protected override ElementBaseNode CreateInstance() => new NeighborhoodNode();
   }
}
