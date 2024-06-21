using OSPSuite.Presentation;
using System.Windows.Forms;
using OSPSuite.Presentation.Core;

namespace OSPSuite.UI.Extensions
{
   public static class DragDropKeyStateHelper
   {
      // Convert DragDropKeyState as integer to Enum DragDropKeyFlags 
      // This is because the DragEventArgs.KeyState is an integer
      // but on the events which are using it, it is being converted to DragDropKeyState for better readability
      public static DragDropKeyFlags ConvertKeyState(this DragEventArgs dragEventArgs,  int keyState) => 
         (DragDropKeyFlags)keyState;
   }
}
