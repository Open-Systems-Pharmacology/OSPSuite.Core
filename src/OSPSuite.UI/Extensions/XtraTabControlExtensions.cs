using System.Windows.Forms;
using DevExpress.XtraTab;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Extensions
{
   public static class XtraTabControlExtensions
   {
      public static XtraTabPage AddPageFrom(this XtraTabControl tabControl, IView viewToAdd, int pageIndex)
      {
         while (tabControl.TabPages.Count <= pageIndex)
         {
            var page = new XtraTabPage { Padding = new Padding(0) };
            page.BorderStyle = BorderStyle.None;
            tabControl.TabPages.Add(page);
         }

         tabControl.TabPages[pageIndex].InitializeFrom(viewToAdd);
         return tabControl.TabPages[pageIndex];
      }
   }
}