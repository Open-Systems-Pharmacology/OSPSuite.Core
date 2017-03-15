namespace OSPSuite.Presentation.MenuAndBars
{
   public static class CreateSubMenu
   {
      public static IMenuBarSubMenu WithCaption(string caption)
      {
         return new MenuBarSubMenu {Caption = caption};
      }
   }

   public static class MenuBarSubMenuExtensions
   {
      public static IMenuBarSubMenu WithItem(this IMenuBarSubMenu barSubMenu, IMenuBarItem subElement)
      {
         barSubMenu.AddItem(subElement);
         return barSubMenu;
      }
   }
}