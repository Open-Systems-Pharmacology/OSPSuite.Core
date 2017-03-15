using System.Drawing;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   internal class ReactionPort : BasePort
   {
      public ReactionLinkType LinkType { get; }

      public ReactionPort(ReactionLinkType reactionLinkType)
      {
         LinkType = reactionLinkType;
         UserObject = LinkType;
         Style = GoPortStyle.Ellipse;
         Size = new SizeF(8, 8);
         AutoRescales = false;
      }

      public void RefreshColor(IDiagramColors diagramColors, Color color)
      {
         var reactionNode = Node as ReactionNode;
         if (reactionNode == null) return;
         BrushColor = Color.FromArgb(reactionNode.Alpha(diagramColors.NodeSizeOpacity), color);
      }
   }
}