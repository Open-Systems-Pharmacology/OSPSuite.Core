using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ReactionNode : ReactionDiagramNode, IReactionNode
   {
      private bool _displayEductsRight;

      public Color EductPortColor { get; private set; }
      public Color ProductPortColor { get; private set; }
      public Color ModifierPortColor { get; private set; }

      public ReactionNode()
      {
         UserFlags = NodeLayoutType.REACTION_NODE;
         NodeBaseSize = new SizeF(30, 20);
         NodeSize = NodeSize.Middle;
      }

      public bool DisplayEductsRight
      {
         get => _displayEductsRight;
         set => SetField(ref _displayEductsRight, value);
      }

      public IEnumerable<ReactionLink> ReactionLinks => Links.OfType<ReactionLink>().ToList();

      public void ClearLinks()
      {
         ReactionLinks.Each(reactionLink => reactionLink.Unlink());
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetFillColorFrom(diagramColors, diagramColors.ReactionNode);

         var alpha = Alpha(diagramColors.NodeSizeOpacity);
         EductPortColor = Color.FromArgb(alpha, diagramColors.ReactionPortEduct);
         ProductPortColor = Color.FromArgb(alpha, diagramColors.ReactionPortProduct);
         ModifierPortColor = Color.FromArgb(alpha, diagramColors.ReactionPortModifier);

         ReactionLinks.Each(reactionLink => reactionLink.SetColorFrom(diagramColors));
      }

      public override void CopyLayoutInfoFrom(IBaseNode baseNode, PointF parentLocation)
      {
         base.CopyLayoutInfoFrom(baseNode, parentLocation);
         if (baseNode is ReactionNode node)
            DisplayEductsRight = node.DisplayEductsRight;
      }

      protected override ElementBaseNode CreateInstance() => new ReactionNode();

      protected override void CopyPropertiesTo(ElementBaseNode target)
      {
         base.CopyPropertiesTo(target);
         ((ReactionNode) target).DisplayEductsRight = DisplayEductsRight;
      }
   }
}
