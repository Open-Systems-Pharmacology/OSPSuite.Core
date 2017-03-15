using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.MenuAndBars
{
   public static class CreateRibbonButton
   {
      public static IRibbonBarItem From(IMenuBarItem menuBarItem)
      {
         return new RibbonBarItem(menuBarItem);
      }
   }

   public static class RibbonBarItemExtensions
   {
      public static T WithLock<T>(this T uiElement, bool locked) where T : IRibbonBarItem
      {
         uiElement.MenuBarItem.Locked = locked;
         uiElement.AllMenuItems().Each(item => item.Locked = locked);
         return uiElement;
      }

      public static IRibbonBarItem WithCaption(this IRibbonBarItem ribbonBarItem, string caption)
      {
         ribbonBarItem.Caption = caption;
         return ribbonBarItem;
      }

      public static IRibbonBarItem WithIcon(this IRibbonBarItem ribbonBarItem, ApplicationIcon icon)
      {
         ribbonBarItem.Icon = icon;
         return ribbonBarItem;
      }

      public static IRibbonBarItem WithSubItem(this IRibbonBarItem ribbonBarItem, IMenuBarItem subElement)
      {
         ribbonBarItem.AddMenuElement(subElement);
         return ribbonBarItem;
      }

      public static IRibbonBarItem WithStyle(this IRibbonBarItem ribbonBarItem, ItemStyle itemStyle)
      {
         ribbonBarItem.ItemStyle = itemStyle;
         return ribbonBarItem;
      }

      public static IRibbonBarItem AsGroupStarter(this IRibbonBarItem ribbonBarItem)
      {
         ribbonBarItem.BeginGroup = true;
         return ribbonBarItem;
      }
   }
}