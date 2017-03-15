using System;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Services
{
   internal class CustomRubberBanding : GoToolRubberBanding
   {
      public CustomRubberBanding(GoView v): base(v)
      { }

      public override bool CanStart()
      {
         bool pRetVal = false;
         GoInputEventArgs e = LastInput;
         pRetVal = (e.Buttons == MouseButtons.Left) && e.Control
                   && (Math.Abs(e.ViewPoint.X - FirstInput.ViewPoint.X) > 1
                       || Math.Abs(e.ViewPoint.Y - FirstInput.ViewPoint.Y) > 1);
         return pRetVal;
      }

      public override void DoMouseMove()
      {
         if (!Active)
         {
            if (Modal)
               return;
            else
            {
               if (View.Selection.Count == 1 && View.Selection.Primary.Bounds.Contains(FirstInput.ViewPoint.X, FirstInput.ViewPoint.Y))
                  View.Selection.Clear();

               Active = true;
               Box = new Rectangle(FirstInput.ViewPoint.X, FirstInput.ViewPoint.Y, 0, 0);
               View.Refresh();
            }
         }
         else
         {
            base.DoMouseMove();
         }
      }
   }
}