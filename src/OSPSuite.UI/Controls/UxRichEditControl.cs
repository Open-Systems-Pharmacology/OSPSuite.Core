using System.ComponentModel;
using DevExpress.XtraRichEdit;

namespace OSPSuite.UI.Controls
{
   [ToolboxItem(true)]
   public class UxRichEditControl : RichEditControl
   {
      public UxRichEditControl()
      {
         UnhandledException += onUnhandledException;
      }

      private void onUnhandledException(object sender, RichEditUnhandledExceptionEventArgs richEditUnhandledExceptionEventArgs)
      {
         richEditUnhandledExceptionEventArgs.Handled = true;
      }
   }
}
