using System;
using System.Drawing;
using DevExpress.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit;

namespace OSPSuite.UI.Controls
{
   public class UxHtmlLabel : UxRichEditControl, IXtraResizableControl
   {
      private static readonly object _layoutInfoChanged = new object();
      private static string _fontFamily;
      private static double _fontSize;

      public UxHtmlLabel(string fontFamily, double fontSize)
      {
         _fontSize = fontSize;
         _fontFamily = fontFamily;

         AutoSizeMode = AutoSizeMode.Vertical;
         Options.VerticalScrollbar.Visibility = RichEditScrollbarVisibility.Hidden;
         ActiveView.BackColor = BackColor;
         BorderStyle = BorderStyles.NoBorder;
         PopupMenuShowing += (o, e) => e.Menu.Items.Clear();
         Enabled = false;
         Views.SimpleView.Padding = new System.Windows.Forms.Padding(0);
         ActiveViewType = RichEditViewType.Simple;
      }

      public UxHtmlLabel() : this("Tahoma", 8.25)
      {

      }

      event EventHandler IXtraResizableControl.Changed
      {
         add
         {
            Events.AddHandler(_layoutInfoChanged, value);
         }
         remove
         {
            Events.RemoveHandler(_layoutInfoChanged, value);
         }
      }
      protected void RaiseChanged()
      {
         var changed = (EventHandler)Events[_layoutInfoChanged];
         changed?.Invoke(this, EventArgs.Empty);
      }

      bool IXtraResizableControl.IsCaptionVisible => false;

      Size IXtraResizableControl.MaxSize => Size;

      Size IXtraResizableControl.MinSize => Size;

      protected override void OnSizeChanged(EventArgs e)
      {
         base.OnSizeChanged(e);
         RaiseChanged();
      }

      public void Caption(string caption, string fontFamily = "", double? fontSize = 8.25)
      {
          Document.HtmlText = stylizeHtmlText(fontFamily, fontSize) + $"<div>{caption}</div>";
      }

      private static string stylizeHtmlText(string fontFamily, double? fontSize)
      {
         return $@"
            <style>
               div {{
                   font-family: '{(string.IsNullOrEmpty(fontFamily) ? _fontFamily : fontFamily)}';
                  font-size: '{fontSize ?? _fontSize}';
               }}
               p {{
                  padding: 0px;
                  margin: 5px;
               }}
            </style>";
      }
   }
}