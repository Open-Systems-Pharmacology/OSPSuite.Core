using System;
using System.Drawing;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class BasePort : GoPort
   {
      private readonly bool _inverseShape;
      private  Func<IBaseNode, IBaseNode, object, object, bool> _isValidLink;

      public BasePort()
      {
         _inverseShape = false;
         FromSpot = NoSpot;
         ToSpot = NoSpot;
         Brush = null;
         BrushColor = Color.Yellow;
         IsValidSelfNode = false;
         IsValidDuplicateLinks = false;

         // Default: do not allow linking
         SetLinkValidator((n1, n2, o1, o2) => false);
      }

      public BasePort(GoBasicNode node, bool inverseShape) : this()
      {
         _inverseShape = inverseShape;
         BrushColor = Color.FromArgb(128, node.Shape.BrushColor);
      }

      public void SetLinkValidator(Func<IBaseNode, IBaseNode, object, object, bool> isValidLink)
      {
         _isValidLink = isValidLink;
      }

      public override bool IsValidLink(IGoPort toPort)
      {
         if (!base.IsValidLink(toPort)) return false; //checks for example for IsValidDuplicateLinks //does not allow two links into the same direction
         if (IsLinked(toPort) || ((GoPort)toPort).IsLinked(this)) return false; //do not allow double links
         var thisNode = Node as IBaseNode;
         var toPortNode = toPort.Node as IBaseNode;
         if (thisNode == null || toPortNode == null) return false;
         return _isValidLink(thisNode, toPortNode, UserObject, toPort.UserObject);
      }

      public override bool ContainsPoint(PointF p)
      {
         var goBasicNode = Node as GoBasicNode;
         if (!_inverseShape || goBasicNode == null) return base.ContainsPoint(p);

         if (!goBasicNode.Shape.ContainsPoint(p)) return false;
         if (base.ContainsPoint(p)) return false;
         return true;
      }
   }
}