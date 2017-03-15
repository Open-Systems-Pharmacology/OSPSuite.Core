using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;

namespace OSPSuite.UI.Controls
{
   public class UxCheckEdit : CheckEdit
   {
      /// <summary>
      /// Enables and disables mouse clicks outside the actual rectangle of the checkbox.
      /// By default, you cannot activate the control outside the rectangle. 
      /// Set this true to allow activation anywhere in the label, checkbox, or empty space surrounding
      /// </summary>
      public bool AllowClicksOutsideControlArea { get; set; }

      public UxCheckEdit()
      {
         AllowClicksOutsideControlArea = false;
         MouseDown += supressCheckBoxClicksOutsideGlyph;
         Properties.AllowFocused = false;
      }

      private void supressCheckBoxClicksOutsideGlyph(object sender, MouseEventArgs e)
      {
         if (AllowClicksOutsideControlArea) return;

         if (!controlAreaContainsLocation(e.Location))
            ((DXMouseEventArgs)e).Handled = true;
      }

      private bool controlAreaContainsLocation(Point location)
      {
         var cInfo = (CheckEditViewInfo)GetViewInfo();
         return getControlExtents(cInfo).Contains(location);
      }

      private Rectangle getControlExtents(CheckEditViewInfo cInfo)
      {
         var labelRect = getLabelRectangle(cInfo);
         var glyphRect = getGlyphRectangle(cInfo);

         var left = Math.Min(labelRect.Left, glyphRect.Left);
         var right = Math.Max(labelRect.Right, glyphRect.Right);
         var bottom = Math.Max(labelRect.Bottom, glyphRect.Bottom);
         var top = Math.Min(labelRect.Top, glyphRect.Top);

         return new Rectangle(left, top, right - left, bottom - top);
      }
      
      private static Rectangle getLabelRectangle(CheckEditViewInfo cInfo)
      {
         return cInfo.CheckInfo.CaptionRect;
      }
      
      private static Rectangle getGlyphRectangle(CheckEditViewInfo cInfo)
      {
         return cInfo.CheckInfo.GlyphRect;
      }
   }
}
