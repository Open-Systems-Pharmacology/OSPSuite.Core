using System.Windows.Forms;
using Northwoods.Go;
using OSPSuite.UI.Diagram.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Diagram
{
   public class DiagramBaseView : GoView
   {
      public DiagramBaseView()
      {
         GridCellSize = Assets.Diagram.Base.GridCellSize;
         GridStyle = GoViewGridStyle.Dot;
         GridSnapDrag = GoViewSnapStyle.Jump;

         AllowCopy = false;
         
         NewLinkPrototype = new NewLink();
         PortGravity = Assets.Diagram.Base.PortGravity; // controls the distance within a port to be linked is snapped

         GoToolContext tool = FindMouseTool(typeof(GoToolContext)) as GoToolContext;
         tool.SingleSelection = false;

         MouseMoveTools.Insert(0, new CustomZooming(this));
         MouseMoveTools.Insert(0, new CustomRubberBanding(this));

         ReplaceMouseTool(typeof(GoToolLinkingNew), new CustomLinkingTool(this));


         ScrollBar hScrollBar = HorizontalScrollBar;
         ScrollBar vScrollBar = VerticalScrollBar;
         hScrollBar.Scroll += (o,e) => this.DoWithinExceptionHandler(() => onScroll(o,e));
         vScrollBar.Scroll += (o, e) => this.DoWithinExceptionHandler(() => onScroll(o, e));
      }

      private void onScroll(object sender, ScrollEventArgs e)
      {
         var scrollBar = sender as ScrollBar;
         if (scrollBar == null) return;
         if (e.NewValue < scrollBar.Minimum) e.NewValue = scrollBar.Minimum;
         if (e.NewValue > scrollBar.Maximum) e.NewValue = scrollBar.Maximum;
      }

      public override float LimitDocScale(float s)
      {
         if (s < Assets.Diagram.Base.MinLimitDocScale) 
            return Assets.Diagram.Base.MinLimitDocScale;

         if (s > Assets.Diagram.Base.MaxLimitDocScale) 
            return Assets.Diagram.Base.MaxLimitDocScale;

         return s;
      }

   }
}