using System;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ContainerPortBase : CustomSubGraphPort
   {
      private Func<IBaseNode, IBaseNode, object, object, bool> _isValidLink;

      public ContainerPortBase()
      {
         FromSpot = GoObject.NoSpot;
         ToSpot = GoObject.NoSpot;
         IsValidSelfNode = false;
         IsValidDuplicateLinks = false;

         // Default: do not allow linking
         SetLinkValidator((n1, n2, o1, o2) => false);
      }

      public void SetLinkValidator(Func<IBaseNode, IBaseNode, object, object, bool> isValidLink)
      {
         _isValidLink = isValidLink;
      }

      public override bool IsValidLink(IGoPort toPort)
      {
         var thisNode = Node as IBaseNode;
         var toPortNode = toPort.Node as IBaseNode;
         if (thisNode == null || toPortNode == null || thisNode == toPortNode) return false; // do not allow self links
         return _isValidLink(thisNode, toPortNode, UserObject, toPort.UserObject);
      }

      public override string GetToolTip(GoView view)
      {
         if (Node.IsAnImplementationOf<SimpleContainerNode>() && !Node.IsAnImplementationOf<MultiPortContainerNode>())
            return ToolTips.BuildingBlockSpatialStructure.HowToCreateNeighborhood;

         return base.GetToolTip(view);
      }

      public override bool ContainsPoint(PointF p)
      {
         ContainerNodeBase containerNode = Parent as ContainerNodeBase;
         if (containerNode == null) return true;
         if (containerNode.IsLogical) return false;
         if (containerNode.IsAnImplementationOf<MultiPortContainerNode>()) return false;

         float portBorderWidth = 12;
         float portBorderHeight = 6;
         RectangleF rect = Bounds;
         if (!rect.Contains(p)) return false;

         rect.X += portBorderWidth;
         rect.Y += portBorderHeight;
         rect.Width -= 2 * portBorderWidth;
         rect.Height -= 2 * portBorderHeight;
         return !rect.Contains(p);
      }
   }
}