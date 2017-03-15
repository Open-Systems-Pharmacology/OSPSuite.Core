using System;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Extensions
{
   public static class WinFormExtensions
   {
      public static void DoWithinWaitCursor(this Form form, Action actionToPerform)
      {
         var currentCursor = form.Cursor;
         try
         {
            if (currentCursor != Cursors.WaitCursor)
               form.Cursor = Cursors.WaitCursor;

            form.DoWithinExceptionHandler(actionToPerform);
         }
         finally
         {
            if (currentCursor != Cursors.WaitCursor)
               form.Cursor = currentCursor;
         }
      }
   }
}