using System;
using DevExpress.XtraLayout;

namespace OSPSuite.UI.Extensions
{
   public static class LayoutControlExtensions
   {
      public static void DoInBatch(this LayoutControl layoutControl, Action action)
      {
         try
         {
            layoutControl.BeginUpdate();
            action();
         }
         finally
         {
            layoutControl.EndUpdate();
         }
      }
   }
}