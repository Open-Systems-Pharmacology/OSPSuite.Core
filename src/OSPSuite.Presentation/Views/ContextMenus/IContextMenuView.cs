using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.Views.ContextMenus
{
   public interface IContextMenuView
   {
      void AddMenuItem(IMenuBarItem menuBarItem);
      void Display(IView view, Point location);
      IEnumerable<IMenuBarItem> AllMenuItems { get; }
      void ActivateMenu(IMenuBarItem menuBarItem);
      void RemoveMenuItem(string caption);
   }
}