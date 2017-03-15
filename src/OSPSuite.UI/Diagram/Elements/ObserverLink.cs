using System.Drawing.Drawing2D;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ObserverLink : BaseLink
   {
      public ObserverNode ObserverNode
      {
         get { return GetFromNode() as ObserverNode; }
      }

      public MoleculeNode MoleculeNode
      {
         get { return GetToNode() as MoleculeNode; }
      }

      public ObserverLink()
      {
         Style = GoStrokeStyle.Bezier;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         PenColor = diagramColors.ObserverLink;
         Pen.DashStyle = DashStyle.Dash;
      }

      public override bool Visible
      {
         get
         {
            if (base.Visible)
               return ObserverNode.Visible;
            return false;
         }
         set { base.Visible = value; }
      }
   }
}