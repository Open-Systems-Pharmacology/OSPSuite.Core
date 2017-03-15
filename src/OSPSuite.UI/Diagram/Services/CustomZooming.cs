using System;
using System.Windows.Forms;
using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Services
{
   internal class CustomZooming : GoToolZooming
   {
      public CustomZooming(GoView v): base(v)
      { }

      public override bool CanStart()
      {
         GoInputEventArgs e = LastInput;
         return (e.Buttons == MouseButtons.Left) && e.Shift
                && (Math.Abs(e.ViewPoint.X - FirstInput.ViewPoint.X) > 1
                    || Math.Abs(e.ViewPoint.Y - FirstInput.ViewPoint.Y) > 1);
      }
   }
}