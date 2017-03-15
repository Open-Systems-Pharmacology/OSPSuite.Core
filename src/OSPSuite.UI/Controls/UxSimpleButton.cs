using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Controls
{
   /// <summary>
   ///    Defines a simple button with shortcut ability
   /// </summary>
   public class UxSimpleButton : SimpleButton
   {
      public BarManager Manager { get; set; }
      public Keys Shortcut { get; set; }

      protected override void OnHandleCreated(EventArgs eventArgs)
      {
         base.OnHandleCreated(eventArgs);
         if (Manager == null || Shortcut == Keys.None) return;

         int item = Manager.Items.Add(new BarButtonItem(Manager, string.Concat("bbi_", Name), -1, new BarShortcut(Shortcut)));
         Manager.Items[item].ItemClick += (o, e) => PerformClick();
      }
   }
}