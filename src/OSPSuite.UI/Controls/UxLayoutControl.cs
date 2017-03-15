using System;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.XtraLayout;

namespace OSPSuite.UI.Controls
{
   public class UxLayoutControl : LayoutControl
   {
      public UxLayoutControl()
      {
         AllowCustomization = false;
      }

      public UxLayoutControl(bool fAllowUseSplitters, bool fAllowUseTabbedGroup)
         : base(fAllowUseSplitters, fAllowUseTabbedGroup)
      {
         AllowCustomization = false;
      }

      public UxLayoutControl(bool createFast)
         : base(createFast)
      {
         AllowCustomization = false;
      }

      protected override void OnPaintBackground(PaintEventArgs e)
      {
         // Do not allow app to crash when components are painting
         try
         {
            base.OnPaintBackground(e);
         }
         catch (Exception ex)
         {
            Debug.Print(ex.ToString());
         }
      }

      protected override LayoutControlImplementor CreateILayoutControlImplementorCore()
      {
         return new LayoutControlImplementorWithoutTimer(this);
      }
   }

   public class LayoutControlImplementorWithoutTimer : LayoutControlImplementor
   {
      public LayoutControlImplementorWithoutTimer(ILayoutControlOwner owner) : base(owner)
      {
      }

      protected override bool AllowTimer => false;
   }
}