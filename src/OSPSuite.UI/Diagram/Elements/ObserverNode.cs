using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ObserverNode : ElementBaseNode
   {
      public ObserverNode()
      {
         UserFlags = NodeLayoutType.OBSERVER_NODE;
         LabelSpot = MiddleRight;
         NodeBaseSize = new SizeF(15F, 15F);
         NodeSize = NodeSize.Middle;
         Text = string.Empty;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetBrushColor(diagramColors, diagramColors.ObserverNode);
         foreach (var observerLink in GetLinks<ObserverLink>()) observerLink.SetColorFrom(diagramColors);
      }

      public void AddLink(ObserverLink link)
      {
         base.AddLinkFrom(link);
      }

      public void ClearLinks()
      {
         Port.ClearLinks();
      }
   }
}