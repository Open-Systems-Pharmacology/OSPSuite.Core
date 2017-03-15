using System;
using System.Drawing;
using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Elements
{
   [Serializable]
   public class CustomSubGraph : GoSubGraph
   {
      public GoView _view;

      public CustomSubGraph()
      {
         Shadowed = true;
         BorderPenColor = Color.Blue;

         Text = "";
         Corner = new SizeF(15, 15);
         CollapsedCorner = new SizeF(5, 5);
         TopLeftMargin = new SizeF(13, 0);
         CollapsedTopLeftMargin = new SizeF(10, 0); //10 0
      }

      protected override GoPort CreatePort()
      {
         return new CustomSubGraphPort();
      }

      public override void LayoutLabel()
      {
         if (Label == null) return;
         if (IsExpanded)
         {
            // don't move label if it's the only thing left (besides the standard children)
            int numchildren = 0;
            if (this.Handle != null) numchildren++;
            if (this.Label != null) numchildren++;
            if (this.Port != null) numchildren++;
            if (this.CollapsedObject != null) numchildren++;

            // if there is a/one invisible child (e.g. MoleculeProperties), ignore it as well.
            if (numchildren == Count - 1)
               foreach (var child in this) if (!child.Visible) numchildren++;

            // to avoid moving Container by collapse/expand/...  for empty or "almost empty (containing one invisible container)" subgraphs
            if (numchildren == this.Count)
            {
               print("- Layout Label no");
               return;
            }
            var boundsOld = Label.Bounds;
            base.LayoutLabel();
            print("- Layout Label base " + boundsOld.Y + " -> " + Label.Bounds.Y + "     Count = " + Count + " != " + numchildren);
         }
         else
         {
            // Label.Position.Y instead of Handle.Position.Y to avoid vertical movement by collapse/expand/... 
            Label.Position = new PointF(Handle.Position.X + 20, Label.Position.Y); 
            print("- Layout Label pos");
         }
      }

      public override void LayoutPort()
      {
         if (Port != null && Port.CanView())
         {
            var boundsOld = Port.Bounds;
            Port.Bounds = ComputeBorder();
            print("- Layout Port " + boundsOld.Y + " -> " + Port.Bounds.Y);
         }
      }

      public override void LayoutHandle()
      {
         if (!IsExpanded) return;

         GoSubGraphHandle h = Handle;
         if (h != null)
         {
            RectangleF b = ComputeBorder();
            var boundsOld = h.Bounds;
            h.Position = new PointF(b.X - 5, b.Y);
            print("- Layout Handle " + boundsOld.Y + " -> " + h.Bounds.Y);
         }
      }

      public override void DoResize(GoView view, RectangleF origRect, PointF newPoint,
                                    int whichHandle, GoInputState evttype, SizeF min, SizeF max)
      {
         base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
         LayoutPort();
      }

      private static bool debug = false;

      private void print(string line)
      {
         if (debug) Console.WriteLine(line);
      }

      private void printLabelBounds(string text)
      {
         if (Label != null) text += " " + Label.Text;
         text += " : " + Bounds.Y;
         print(text);
      }

      protected override RectangleF ComputeBounds()
      {
         printLabelBounds("< ComputeBounds ");
         var newBounds = base.ComputeBounds();
         print("> ComputeBounds  : " + newBounds.Y);
         return newBounds;
      }

      // making collapsed labels shorter
      protected override void PrepareCollapse()
      {
         print("------------------------------------------------------");
         printLabelBounds("< PrepareCollapse ");
         base.PrepareCollapse();
      }

      protected override void FinishCollapse(RectangleF sgrect)
      {
         base.FinishCollapse(sgrect);
         printLabelBounds("> FinishCollapse ");
      }

      protected override void PrepareExpand()
      {
         print("------------------------------------------------------");
         printLabelBounds("< PrepareExpand ");
         base.PrepareExpand();
      }

      protected override void FinishExpand(PointF hpos)
      {
         base.FinishExpand(hpos);
         printLabelBounds("> FinishExpand ");
      }
   }

   [Serializable]
   public class CustomSubGraphPort : GoBoxPort
   {
      public CustomSubGraphPort()
      {
         IsValidSelfNode = true;
         Style = GoPortStyle.None;
         LinkPointsSpread = true;
      }

      // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
      public override float GetFromLinkDir(IGoLink link)
      {
         float result = base.GetFromLinkDir(link);
         if (link.ToPort != null &&
             link.ToPort.GoObject != null &&
             link.ToPort.GoObject.IsChildOf(Parent))
         {
            result += 180;
            if (result > 360)
               result -= 360;
         }
         return result;
      }

      // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
      public override float GetToLinkDir(IGoLink link)
      {
         float result = base.GetToLinkDir(link);
         if (link.FromPort != null &&
             link.FromPort.GoObject != null &&
             link.FromPort.GoObject.IsChildOf(Parent))
         {
            result += 180;
            if (result > 360)
               result -= 360;
         }
         return result;
      }

      // sorting links by angle shouldn't be confused by whether it's
      // inside or outside of the CustomSubGraph
      public override float GetDirection(IGoLink link)
      {
         if (link == null) return 0;
         if (link.FromPort == this)
         {
            return base.GetFromLinkDir(link);
         }
         else
         {
            return base.GetToLinkDir(link);
         }
      }

   }
}