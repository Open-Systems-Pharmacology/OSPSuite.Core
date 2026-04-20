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

         // .NET 8 workaround: replace the default GoToolContext with a subclass
         // whose Start() doesn't JIT a local of the removed
         // System.Windows.Forms.ContextMenu type.
         var contextTool = new SafeGoToolContext(this) { SingleSelection = false };
         ReplaceMouseTool(typeof(GoToolContext), contextTool);

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

      // .NET 8 workaround: GoView.DoContextClick references the removed
      // System.Windows.Forms.ContextMenu type, so JITing it throws TypeLoadException.
      // Replicate the base logic while omitting the legacy ContextMenu branch
      // (nothing in OSPSuite/MoBi/PK-Sim uses GoObject.GetContextMenu — only ContextMenuStrip).
      public override bool DoContextClick(GoInputEventArgs evt)
      {
         var obj = PickObject(true, false, evt.DocPoint, false);
         if (obj == null)
         {
            RaiseBackgroundContextClicked(evt);
            return false;
         }

         RaiseObjectContextClicked(obj, evt);
         while (obj != null)
         {
            var strip = obj.GetContextMenuStrip(this);
            if (strip != null)
            {
               strip.Show(this, evt.ViewPoint);
               return true;
            }

            if (obj.OnContextClick(evt, this))
               return true;

            obj = obj.Parent;
         }

         return false;
      }
   }

   // .NET 8 workaround: GoToolContext.Start() declares a local of the removed
   // System.Windows.Forms.ContextMenu type, so JITing it throws TypeLoadException.
   // Override it to replicate the essential behaviour (CurrentObject pickup) while
   // skipping the legacy native-menu suppression path (OSPSuite never assigns
   // Control.ContextMenu or ContextMenuStrip on the GoView anyway).
   internal class SafeGoToolContext : GoToolContext
   {
      public SafeGoToolContext(GoView view) : base(view)
      {
      }

      public override void Start()
      {
         var view = View;
         if (view == null)
            return;

         CurrentObject = view.PickObject(true, false, LastInput.DocPoint, false);
      }

      public override void Stop()
      {
         // Base.Stop() reads the private myBackgroundContextMenu field (typed as
         // the removed System.Windows.Forms.ContextMenu), which triggers
         // TypeLoadException on JIT. Our Start() override never populates that
         // field or its ContextMenuStrip sibling, so there is nothing to restore —
         // just clear CurrentObject.
         CurrentObject = null;
      }
   }
}