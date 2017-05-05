using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.Utils.Controls;
using DevExpress.XtraRichEdit;

namespace OSPSuite.UI.Controls
{
   [ToolboxItem(true)]
   public class UxRichEditControl : RichEditControl, IXtraResizableControl
   {
      public UxRichEditControl()
      {
         UnhandledException += onUnhandledException;
      }

      private void onUnhandledException(object sender, RichEditUnhandledExceptionEventArgs richEditUnhandledExceptionEventArgs)
      {
         richEditUnhandledExceptionEventArgs.Handled = true;
      }

      private static readonly object _layoutInfoChanged = new object();

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

   }
}
