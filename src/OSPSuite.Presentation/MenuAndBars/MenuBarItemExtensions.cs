using OSPSuite.Assets;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.MenuAndBars
{
   public static class MenuBarItemExtensions
   {
      public static T WithDescription<T>(this T uiElement, string description) where T : IMenuBarItem
      {
         uiElement.Description = description;
         return uiElement;
      }

      public static T WithName<T>(this T uiElement, string name) where T : IMenuBarItem
      {
         uiElement.Name = name;
         return uiElement;
      }

      public static T WithCaption<T>(this T uiElement, string caption) where T : IMenuBarItem
      {
         uiElement.Caption = caption;
         return uiElement;
      }

      public static T WithIcon<T>(this T uiElement, ApplicationIcon icon) where T : IMenuBarItem
      {
         uiElement.Icon = icon;
         return uiElement;
      }

      public static T WithShortcut<T>(this T uiElement, Keys key) where T : IMenuBarItem
      {
         uiElement.Shortcut = key;
         return uiElement;
      }

      public static T ForDeveloper<T>(this T uiElement) where T : IMenuBarItem
      {
         uiElement.IsForDeveloper = true;
         return uiElement;
      }

      public static T AsGroupStarter<T>(this T uiElement) where T : IMenuBarItem
      {
         uiElement.BeginGroup = true;
         return uiElement;
      }

      public static T WithId<T>(this T uiElement, int id) where T : IMenuBarItem
      {
         uiElement.Id = id;
         return uiElement;
      }
   }
}