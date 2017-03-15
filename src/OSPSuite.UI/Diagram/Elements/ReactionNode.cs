using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ReactionNode : ReactionDiagramNode, IReactionNode
   {
      private GoTriangle _triangle;
      private ReactionPort _eductsPort, _productsPort, _modifiersPort;

      private bool _displayEductsRight;

      public ReactionNode()
      {
         Port = null;
         _modifiersPort = new ReactionPort(ReactionLinkType.Modifier);
         _eductsPort = new ReactionPort(ReactionLinkType.Educt);
         _productsPort = new ReactionPort(ReactionLinkType.Product);

         Add(_eductsPort);
         AddChildName("EductsPort", _eductsPort);
         Add(_productsPort);
         AddChildName("ProductsPort", _productsPort);
         Add(_modifiersPort);
         AddChildName("ModulatorsPort", _modifiersPort);

         UserFlags = NodeLayoutType.REACTION_NODE;

         _triangle = new GoTriangle
         {
            A = new PointF(0, 0),
            B = new PointF(-10F, 10F),
            C = new PointF(10F, 10F)
         };

         Shape = _triangle;
         NodeBaseSize = new SizeF(30, 20);
         NodeSize = NodeSize.Middle;

         DisplayEductsRight = false;

         _eductsPort.SetLinkValidator(isValidLinkFromReactionNode);
         _productsPort.SetLinkValidator(isValidLinkFromReactionNode);
         _modifiersPort.SetLinkValidator(isValidLinkFromReactionNode);
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetBrushColor(diagramColors, diagramColors.ReactionNode);

         _eductsPort?.RefreshColor(diagramColors, diagramColors.ReactionPortEduct);
         _productsPort?.RefreshColor(diagramColors, diagramColors.ReactionPortProduct);
         _modifiersPort?.RefreshColor(diagramColors, diagramColors.ReactionPortModifier);

         foreach (var reactionLink in GetLinks<ReactionLink>())
         {
            reactionLink.SetColorFrom(diagramColors);
         }
      }

      private bool isValidLinkFromReactionNode(IBaseNode node1, IBaseNode node2, object obj1, object obj2)
      {
         return node2.IsAnImplementationOf<MoleculeNode>();
      }

      //Called during deserialization to create new objects instead of constructor
      //Care for reference members in CopyObject, because of object copy process in deserialization
      public override GoObject CopyObject(GoCopyDictionary env)
      {
         var copy = (ReactionNode) base.CopyObject(env);
         copy._triangle = (GoTriangle) copy.Shape;
         copy._eductsPort = (ReactionPort) copy.FindChild("EductsPort");
         copy._productsPort = (ReactionPort) copy.FindChild("ProductsPort");
         copy._modifiersPort = (ReactionPort) copy.FindChild("ModulatorsPort");
         return copy;
      }

      public override void CopyLayoutInfoFrom(IBaseNode baseNode, PointF parentLocation)
      {
         base.CopyLayoutInfoFrom(baseNode, parentLocation);
         var node = baseNode as ReactionNode;
         if (node == null) return;

         DisplayEductsRight = node.DisplayEductsRight;
      }

      public override bool CanLink
      {
         get { return ((GoPort) Ports.First).IsValidFrom; }
         set
         {
            foreach (var iPort in Ports)
            {
               var port = (GoPort) iPort;
               port.IsValidFrom = value;
               port.IsValidTo = value;
            }
         }
      }

      public override NodeSize NodeSize
      {
         get { return _nodeSize; }
         set
         {
            _nodeSize = value;
            if (_triangle == null) return;
            Shape.Size = new SizeF(_nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Width, _nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Height);
            refreshPortsLocation();
            RefreshLabel();

            if (_nodeSize == NodeSize.Middle)
               LabelSpot = MiddleBottom;
         }
      }

      public bool DisplayEductsRight
      {
         get { return _displayEductsRight; }
         set
         {
            _displayEductsRight = value;
            refreshPortsLocation();
         }
      }

      private void refreshPortsLocation()
      {
         PointF eductsPoint, productsPoint;
         if (_displayEductsRight)
         {
            eductsPoint = _triangle.C;
            productsPoint = _triangle.B;
         }
         else
         {
            eductsPoint = _triangle.B;
            productsPoint = _triangle.C;
         }
         _eductsPort.SetSpotLocation(Middle, eductsPoint);
         _productsPort.SetSpotLocation(Middle, productsPoint);
         _modifiersPort.SetSpotLocation(Middle, _triangle.A);
      }

      public new IEnumerable<ReactionLink> Links
      {
         get
         {
            var allLinks = new List<ReactionLink>();
            allLinks.AddRange(_eductsPort.Links.Cast<ReactionLink>());
            allLinks.AddRange(_modifiersPort.Links.Cast<ReactionLink>());
            allLinks.AddRange(_modifiersPort.Links.Cast<ReactionLink>());
            return allLinks;
         }
      }

      public override void AddLinkFrom(IBaseLink link)
      {
         _productsPort.AddDestinationLink((ReactionLink) link);
      }

      public override void AddLinkTo(IBaseLink link)
      {
         var reactionLink = (ReactionLink) link;
         switch (reactionLink.Type)
         {
            case ReactionLinkType.Educt:
               _eductsPort.AddSourceLink(reactionLink);
               break;
            case ReactionLinkType.Modifier:
               _modifiersPort.AddSourceLink(reactionLink);
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + reactionLink.Type);
         }
      }

      public void RemoveLink(ReactionLink reactionLink)
      {
         var rbl = (ReactionLink) reactionLink;
         switch (reactionLink.Type)
         {
            case ReactionLinkType.Educt:
               _eductsPort.RemoveLink(rbl);
               break;
            case ReactionLinkType.Product:
               _productsPort.RemoveLink(rbl);
               break;
            case ReactionLinkType.Modifier:
               _modifiersPort.RemoveLink(rbl);
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + reactionLink.Type);
         }
      }

      public void ReplaceReactionLink(MoleculeNode moleculeDefaultNode, MoleculeNode moleculeNode)
      {
         foreach (var link in Links.Where(x => x.MoleculeNode == moleculeDefaultNode))
         {
            link.MoleculeNode = moleculeNode;
         }
      }

      public void ClearLinks()
      {
         _eductsPort.ClearLinks();
         _productsPort.ClearLinks();
         _modifiersPort.ClearLinks();
      }
   }
}