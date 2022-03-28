using System.Windows.Forms;
using DevExpress.XtraTab;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Extensions
{
   public static class XtraTabPageExtensions
   {
      public static void InitializeFrom(this XtraTabPage page, IView viewToAdd)
      {
         var control = viewToAdd as Control;
         if (control == null) return;
         page.FillWith(viewToAdd);
         page.Text = viewToAdd.Caption;
         page.SetImage(viewToAdd.ApplicationIcon, UIConstants.ICON_SIZE_TAB);
      }
   }
}