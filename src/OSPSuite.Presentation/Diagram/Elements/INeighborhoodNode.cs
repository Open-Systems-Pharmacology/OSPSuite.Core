using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface INeighborhoodNode : IElementBaseNode
   {
      void Initialize(IContainerNode firstNeighborNode, IContainerNode secondNeighborNode);
      IContainerNode FirstNeighbor { get; }
      IContainerNode SecondNeighbor { get; }
      IBaseLink FirstNeighborLink { get; }
      IBaseLink SecondNeighborLink { get; }
      IContainerNode GetOtherContainerNode(IContainerNode node);
      void AdjustPosition();
      void AdjustPositionForContainerInMove(IContainerNode node, SizeF offset);
   }
}