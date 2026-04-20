using Northwoods.Go;

namespace OSPSuite.UI.Views.Diagram
{
   // .NET 8 workaround: GoOverview inherits the broken GoView.DoContextClick and
   // the default GoToolContext in its mouse tools. Apply the same overrides so
   // right-clicks inside an overview don't trigger TypeLoadException.
   public class BaseDiagramOverview : GoOverview
   {
      public BaseDiagramOverview()
      {
         var contextTool = new SafeGoToolContext(this) { SingleSelection = false };
         ReplaceMouseTool(typeof(GoToolContext), contextTool);
      }

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
}
