using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.UI.Mappers;

namespace OSPSuite.UI.Views
{
   public class ContextMenuView : IContextMenuView
   {
      private readonly IMenuBarItemToBarItemMapper _mapper;
      private readonly IList<IMenuBarItem> _allMenuItems;

      public IEnumerable<IMenuBarItem> AllMenuItems
      {
         get { return _allMenuItems; }
      }

      public ContextMenuView(IMenuBarItemToBarItemMapper mapper)
      {
         _mapper = mapper;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public void ActivateMenu(IMenuBarItem menuBarItem)
      {
         var menuBarButton = menuBarItem as MenuBarButton;
         if (menuBarButton == null) return;
         var barItem = _mapper.MapFrom(new BarManager(), menuBarButton);
         if (!barItem.Enabled) return;
         menuBarButton.Click();
      }

      public void AddMenuItem(IMenuBarItem menuItem)
      {
         _allMenuItems.Add(menuItem);
      }

      public void Display(IView view, Point location)
      {
         var viewWithPopup = view as IViewWithPopup;
         if (viewWithPopup == null)
            return;

         var control = view.DowncastTo<Control>();
         var barManager = viewWithPopup.PopupBarManager;
         var contextMenu = new PopupMenu(barManager);
         contextMenu.CloseUp += contextMenuCloseUp;

         foreach (var menu in _allMenuItems)
         {
            var barItem = _mapper.MapFrom(barManager, menu);
            contextMenu.ItemLinks.Add(barItem, menu.BeginGroup);
         }

         contextMenu.ShowPopup(control.PointToScreen(location));
      }

      public void RemoveMenuItem(string caption)
      {
         var menu = _allMenuItems.FirstOrDefault(x => string.Equals(x.Caption, caption));
         if (menu == null)
            return;

         _allMenuItems.Remove(menu);
      }

      private void contextMenuCloseUp(object sender, EventArgs e)
      {
         var popupMenu = sender as PopupMenu;
         if (popupMenu == null) return;
         popupMenu.Manager.Items.Clear();
         popupMenu.CloseUp -= contextMenuCloseUp;
         _allMenuItems.Clear();
      }
   }
}