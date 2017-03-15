using System;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Core
{
   public static class CreateMenuButton
   {
      public static IMenuBarButton WithCaption(string caption)
      {
         return new MenuBarButton {Caption = caption};
      }
   }

   public static class CreateMenuCheckButton
   {
      public static IMenuBarCheckItem WithCaption(string caption)
      {
         return new MenuBarCheckButton {Caption = caption};
      }
   }

   public static class MenuBarItemExtensions
   {
      public static IMenuBarButton WithCommand<TCommand>(this IMenuBarButton menuBarItem) where TCommand : IUICommand
      {
         return menuBarItem.WithCommand(IoC.Resolve<TCommand>());
      }

      public static T WithActionCommand<T>(this T menuBarItem, Action actionToExecute) where T : IMenuBarButton
      {
         return menuBarItem.WithCommand(new ExecuteActionUICommand(actionToExecute));
      }

      public static IMenuBarButton WithCommandFor<TCommand, TObject>(this IMenuBarButton menuBarItem, TObject entity)
         where TCommand : IObjectUICommand<TObject>
         where TObject : class
      {
         return menuBarItem.WithCommand(IoC.Resolve<TCommand>().For(entity));
      }

      public static IMenuBarButton WithEnabled(this IMenuBarButton menuBarItem, bool enabled)
      {
         menuBarItem.Enabled = enabled;
         return menuBarItem;
      }

      public static T WithId<T>(this T menuBarItem, MenuBarItemId menuBarId) where T : IMenuBarItem
      {
         menuBarItem.Name = menuBarId.Name;
         menuBarItem.Id = menuBarId.Id;
         return menuBarItem;
      }

      public static T WithId<T>(this T buttonGroup, ButtonGroupId buttonGroupId) where T : IButtonGroup
      {
         buttonGroup.Id = buttonGroupId.Id;
         return buttonGroup;
      }

      public static IMenuBarButton AsDisabledIf(this IMenuBarButton menuBarItem, bool disableCondition)
      {
         menuBarItem.Enabled = !disableCondition;
         return menuBarItem;
      }

      public static T WithCheckedAction<T>(this T menuBarItem, Action<bool> checkAction) where T : IMenuBarCheckItem
      {
         menuBarItem.CheckedChanged += checkAction;
         return menuBarItem;
      }
   }
}