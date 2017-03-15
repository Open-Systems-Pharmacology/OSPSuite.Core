using System;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraTab.ViewInfo;

namespace OSPSuite.UI.Controls
{
   public class UxTabControl : XtraTabControl
   {
      private BarManager _barManager;

      public void EnableTabManagementInHeader(BarManager barManager)
      {
         var customButton = new CustomHeaderButton(ButtonPredefines.Combo) { ToolTip = "Select Page" };
         CustomHeaderButtons.Add(customButton);
         CustomHeaderButtonClick +=  onTabControlCustomHeaderButtonClick;
         _barManager = barManager;
      }

      /// <summary>
      /// Present a popup menu with all pages to select a page to move to.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void onTabControlCustomHeaderButtonClick(object sender, CustomHeaderButtonEventArgs e)
      {
         var popupMenu = new DXPopupMenu { MenuViewType = MenuViewType.Menu };

         foreach (XtraTabPage page in TabPages)
         {
            var menuitem = new DXMenuItem(page.Text);
            menuitem.Click += onPageListMenuItemClick;
            menuitem.Tag = popupMenu;
            if (page.Image != null)
               menuitem.Image = page.Image;

            popupMenu.Items.Add(menuitem);
         }
         var menuPos = PointToClient(MousePosition);
         MenuManagerHelper.ShowMenu(popupMenu, LookAndFeel, _barManager, this, menuPos);
      }

      /// <summary>
      /// Handle page selection in page list popup menu.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void onPageListMenuItemClick(object sender, EventArgs e)
      {
         var menuItem = sender as DXMenuItem;
         if (menuItem == null) return;
         foreach (XtraTabPage page in TabPages)
         {
            if (page.Text != menuItem.Caption) 
               continue;

            SelectedTabPage = page;
            MakePageVisible(page);
            break;
         }
         //dispose dynamically created objects.
         var popupMenu = menuItem.Tag as DXPopupMenu;
         if (popupMenu == null) return;
         foreach (DXMenuItem item in popupMenu.Items)
         {
            item.Click -= onPageListMenuItemClick;
            item.Dispose();
         }
         popupMenu.Items.Clear();
         popupMenu.Dispose();
      }
   }
}