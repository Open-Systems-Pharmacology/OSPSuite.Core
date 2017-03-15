using System.Drawing.Drawing2D;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   public class TransportLink : GoLink, IWithColor
   {
      private NeighborLink _neighborLink;
      private MoleculeNode _moleculeNode;

      public TransportLink()
      {
         Style = GoStrokeStyle.Bezier;
      }

      public void SetColorFrom(IDiagramColors diagramColors)
      {
         PenColor = diagramColors.TransportLink;
         Pen.DashStyle = DashStyle.Solid;
      }

      public void Initialize(NeighborLink neighborLink, MoleculeNode moleculeNode)
      {
         _neighborLink = neighborLink;
         _moleculeNode = moleculeNode;
         _moleculeNode.AddLink(this);

          var simpleContainerNode = (SimpleContainerNode) ContainerNode;
         simpleContainerNode.AddTransportLink(this);
         simpleContainerNode.Add(this);
      }

      public INeighborhoodNode NeighborhoodNode
      {
         get { return _neighborLink.NeighborhoodNode; }
      }

      public IContainerNode ContainerNode
      {
         get { return _neighborLink.ContainerNode; }
      }

      public NeighborLink NeighborLink
      {
         get { return _neighborLink; }
      }

      public MoleculeNode MoleculeNode
      {
         get { return _moleculeNode; }
      }

      public IElementBaseNode GetOtherNode(IElementBaseNode node)
      {
         return (IElementBaseNode) GetOtherNode((GoNode) node);
      }

      public override bool Visible
      {
         get
         {
            var fromNode = base.FromNode as IHasLayoutInfo;
            var toNode = base.ToNode as IHasLayoutInfo;
            if (fromNode != null && toNode != null) return base.Visible  && fromNode.Visible && toNode.Visible;
            return true;
         }
         set { base.Visible = value; }
      }

   }
}