using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Services
{
   internal class CustomLinkingTool : GoToolLinkingNew
   {
      public CustomLinkingTool(GoView v) : base(v) { }

      public override void DoMouseMove()
      {
         base.DoMouseMove();
         // try to find valid port near to position
         IGoPort snapPort = PickNearestPort(this.LastInput.DocPoint);

         if (snapPort != null)
         {
            View.Cursor = Cursors.Hand;
            setLinkStyle(Link,Color.Black,3);
         }
         else
         {
            View.Cursor = Cursors.No;
            setLinkStyle(Link, Color.Red, 1);
         }
      }

      private void setLinkStyle(IGoLink Ilink, Color color, int width)
      {
         var link = Ilink as GoLink;
         if (link != null)
         {
            link.PenColor = color;
            link.Pen.Width = width;
         }       
      }

   }
}
