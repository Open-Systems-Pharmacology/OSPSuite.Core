using System.Collections.Generic;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IMenuBarSubMenu : IMenuBarItem
   {
      /// <summary>
      /// All bar items defined in that sub menu
      /// </summary>
      IEnumerable<IMenuBarItem> AllItems();

      /// <summary>
      /// Add a menu bar item as child from this menu
      /// </summary>
      /// <param name="menuBarItem"></param>
      void AddItem(IMenuBarItem menuBarItem);
   }

   public class MenuBarSubMenu : MenuBarItem, IMenuBarSubMenu
   {
      private readonly IList<IMenuBarItem> _addItem;

      public MenuBarSubMenu()
      {
         _addItem = new List<IMenuBarItem>();
      }

      public IEnumerable<IMenuBarItem> AllItems()
      {
         return _addItem;
      }

      public void AddItem(IMenuBarItem menuBarItem)
      {
         _addItem.Add(menuBarItem);
      }
   }
}