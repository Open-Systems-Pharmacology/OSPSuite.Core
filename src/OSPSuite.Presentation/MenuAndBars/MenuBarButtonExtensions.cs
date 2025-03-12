namespace OSPSuite.Presentation.MenuAndBars
{
   public static class MenuBarButtonExtensions
   {
      public static T WithCommand<T>(this T menuBarItem, IUICommand command) where T : IMenuBarButton
      {
         menuBarItem.Command = command;
         return menuBarItem;
      }

      public static T AsChecked<T>(this T menuBarItem) where T : IMenuBarCheckItem
      {
         return menuBarItem.WithChecked(true);
      }

      public static T AsUnchecked<T>(this T menuBarItem) where T : IMenuBarCheckItem
      {
         return menuBarItem.WithChecked(false);
      }

      public static T WithChecked<T>(this T menuBarItem, bool isChecked) where T : IMenuBarCheckItem
      {
         menuBarItem.Checked = isChecked;
         return menuBarItem;
      }
   }
}