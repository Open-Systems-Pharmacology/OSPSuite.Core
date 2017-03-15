using System.Drawing.Drawing2D;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   public class NeighborLink : BaseLink
   {
      public NeighborLink(INeighborhoodNode neighborhoodNode, IContainerNode containerNode): base(neighborhoodNode, containerNode)
      {
         Deletable = false;
      }

      public IContainerNode ContainerNode
      {
         get
         {
            IContainerNode containerNode = GetToNode() as IContainerNode;
            if (containerNode == null && ToNode != null)
            {
               var portNode = ToNode as PortNode;
               if (portNode != null) containerNode = portNode.GetParent();
            }
            return containerNode;
         }
      }

      public INeighborhoodNode NeighborhoodNode
      {
         get { return GetFromNode() as INeighborhoodNode; }
      }

      public override bool Visible
      {
         get
         { 
            var neighborhoodNode = NeighborhoodNode;
            var containerNode = ContainerNode;

            bool neighborhoodNodeVisible = true;
            if (neighborhoodNode != null) neighborhoodNodeVisible = neighborhoodNode.Visible;
            bool containerNodeVisible = true;
            if (containerNode != null) containerNodeVisible = !containerNode.Hidden && containerNode.IsVisible;

            return IsVisible && neighborhoodNodeVisible && containerNodeVisible;
         }
         set { base.Visible = value; }
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         PenColor = diagramColors.NeighborhoodLink;
         Pen.DashStyle = DashStyle.Solid;
         Pen.Width = 4;
      }
   }
}