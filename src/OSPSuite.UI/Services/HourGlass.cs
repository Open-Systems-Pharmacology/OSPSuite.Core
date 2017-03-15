using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OSPSuite.UI.Services
{
   /// <summary>
   ///   Wrapper over hour glass.
   ///   Taken from http://stackoverflow.com/questions/302663/cursor-current-vs-this-cursor-in-net-c
   /// </summary>
   public class HourGlass : IDisposable
   {
      public HourGlass()
      {
         Enabled = true;
      }

      public void Dispose()
      {
         Enabled = false;
      }

      public static bool Enabled
      {
         get { return Application.UseWaitCursor; }
         set
         {
            if (value == Application.UseWaitCursor) return;
            Application.UseWaitCursor = value;
            Form f = Form.ActiveForm;
            if (f != null) // Send WM_SETCURSOR
               SendMessage(f.Handle, 0x20, f.Handle, (IntPtr)1);
         }
      }

      [DllImport("user32.dll")]
      private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
   }
}