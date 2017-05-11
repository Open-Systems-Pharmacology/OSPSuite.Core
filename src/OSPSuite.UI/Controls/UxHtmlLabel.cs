using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit;
using AutoSizeMode = DevExpress.XtraRichEdit.AutoSizeMode;

namespace OSPSuite.UI.Controls
{
   public class UxHtmlLabel : UxRichEditControl, IXtraResizableControl
   {
      private readonly object _layoutInfoChanged = new object();
      public string FontFamily { get; set; } = UIConstants.DEFAULT_HTML_FONT;
      public double FontSize { get; set; } = UIConstants.DEFAULT_HTML_FONT_SIZE;

      public UxHtmlLabel()
      {
         AutoSizeMode = AutoSizeMode.Vertical;
         Options.VerticalScrollbar.Visibility = RichEditScrollbarVisibility.Hidden;
         ActiveView.BackColor = BackColor;
         BorderStyle = BorderStyles.NoBorder;
         PopupMenuShowing += (o, e) => e.Menu.Items.Clear();
         Enabled = false;
         Views.SimpleView.Padding = new Padding(0);
         ActiveViewType = RichEditViewType.Simple;
      }

      event EventHandler IXtraResizableControl.Changed
      {
         add { Events.AddHandler(_layoutInfoChanged, value); }
         remove { Events.RemoveHandler(_layoutInfoChanged, value); }
      }

      protected void RaiseChanged()
      {
         var changed = (EventHandler) Events[_layoutInfoChanged];
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

      public string Caption
      {
         set => Document.HtmlText = $"{stylizeHtmlText}<div>{value}</div>";
      }

      private string stylizeHtmlText => $@"
            <style>
               div {{
                   font-family: '{FontFamily}';
                  font-size: '{FontSize}';
               }}
               p {{
                  padding: 0px;
                  margin: 5px;
               }}
            </style>";
   }
}