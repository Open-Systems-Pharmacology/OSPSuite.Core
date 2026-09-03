using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ReactionDiagramNode : ElementBaseNode
   {
      public override bool LabelVisible => true;

      public override float LabelFontSize => NodeSize == NodeSize.Small ? 7 : base.LabelFontSize;

      public override Color LabelColor => NodeSize == NodeSize.Small ? Color.Black : base.LabelColor;
   }
}
