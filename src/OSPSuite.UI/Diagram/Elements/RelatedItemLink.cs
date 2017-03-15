using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class RelatedItemLink : BaseLink
   {
      public RelatedItemLink()
      {
         Style = GoStrokeStyle.Line;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         PenColor = diagramColors.RelatedItemLink;
      }

      public override void Initialize(IBaseNode fromNode, IBaseNode toNode)
      {
         base.Initialize(fromNode, toNode);
         this.ToBack();
      }
   }
}
