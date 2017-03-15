using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class JournalPageLink : BaseLink, IJournalPageLink
   {
      public JournalPageLink()
      {
         Style = GoStrokeStyle.Bezier;
      }

      public override void Initialize(IBaseNode fromNode, IBaseNode toNode)
      {
         base.Initialize(fromNode, toNode);
         this.ToBack();
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         PenColor = diagramColors.JournalPageLink;
      }
   }
}
