using System;
using System.Windows.Forms;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Starter
{
   public class SimpleExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         string text = ex.Message + '\n' + ex.StackTrace;
         if (ex.InnerException != null)
            text += "\n\nInner Exception:\n" + ex.InnerException.Message + '\n' + ex.StackTrace;
         MessageBox.Show(text, "DataChart Test: Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
   }
}