using System;
using Northwoods.Go;

namespace OSPSuite.UI.Views.Diagram
{
   // Shared body for DoContextClick overrides used by DiagramBaseView and
   // BaseDiagramOverview. The base GoView.DoContextClick references the removed
   // System.Windows.Forms.ContextMenu type, so JITing it throws
   // TypeLoadException on .NET 8. This helper replicates the base logic while
   // omitting the legacy ContextMenu branch.
   internal static class DiagramContextClick
   {
      public static bool Handle(
         GoView view,
         GoInputEventArgs evt,
         Action<GoObject, GoInputEventArgs> raiseObjectContextClicked,
         Action<GoInputEventArgs> raiseBackgroundContextClicked)
      {
         var obj = view.PickObject(true, false, evt.DocPoint, false);
         if (obj == null)
         {
            raiseBackgroundContextClicked(evt);
            return false;
         }

         raiseObjectContextClicked(obj, evt);
         while (obj != null)
         {
            var strip = obj.GetContextMenuStrip(view);
            if (strip != null)
            {
               strip.Show(view, evt.ViewPoint);
               return true;
            }

            if (obj.OnContextClick(evt, view))
               return true;

            obj = obj.Parent;
         }

         return false;
      }
   }
}
