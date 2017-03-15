using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ReactionDiagramNode : ElementBaseNode
   {
      protected override void RefreshLabel()
      {
         base.RefreshLabel();
         if (Label == null || _nodeSize != NodeSize.Small) return;

         Label.Visible = true;
         Label.FontSize = 7;
         Label.TextColor = Color.Black;
      }
   }
}