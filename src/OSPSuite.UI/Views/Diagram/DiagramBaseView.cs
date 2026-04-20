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

      public override bool DoContextClick(GoInputEventArgs evt)
      {
         return DiagramContextClick.Handle(
            this,
            evt,
            RaiseObjectContextClicked,
            RaiseBackgroundContextClicked);
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
         // Mirror the base Start() semantics minus the legacy ContextMenu branch:
         // only latch CurrentObject when the View has a ContextMenuStrip set.
         // OSPSuite never sets one, so in practice this is a no-op — which
         // matches the base method's observable behaviour and means right-clicks
         // do not alter selection state implicitly.
         var view = View;
         if (view?.ContextMenuStrip == null)
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