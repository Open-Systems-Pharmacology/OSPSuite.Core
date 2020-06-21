using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Mappers
{
   public abstract class MenuBarItemToBarItemMapperBase
   {
      protected readonly IStartOptions _startOptions;
      private readonly IKeysToWindowsKeysMapper _keysMapper;

      protected MenuBarItemToBarItemMapperBase(IStartOptions startOptions, IKeysToWindowsKeysMapper keysMapper)
      {
         _startOptions = startOptions;
         _keysMapper = keysMapper;
      }

      protected void UpdateBarButtonItem(IMenuBarItem menuBarItem, BarItem barItem)
      {
         barItem.Id = menuBarItem.Id;
         barItem.Name = menuBarItem.Name;
         barItem.Description = menuBarItem.Description;
         barItem.Hint = menuBarItem.Description;
         barItem.Enabled = menuBarItem.Enabled;
         barItem.UpdateIcon(menuBarItem.Icon);

         barItem.ItemShortcut = new BarShortcut(_keysMapper.MapFrom(menuBarItem.Shortcut));
         menuBarItem.EnabledChanged += (value => barItem.Enabled = value);

         menuBarItem.VisibilityChanged += (value => barItem.Visibility = value ? BarItemVisibility.Always : BarItemVisibility.Never);
      }
   }

   public abstract class MenuBarButtonToBarItemMapperBase : MenuBarItemToBarItemMapperBase
   {
      protected MenuBarButtonToBarItemMapperBase(IStartOptions startOptions, IKeysToWindowsKeysMapper keysMapper) : base(startOptions, keysMapper)
      {
      }

      protected void UpdateBarButtonItem(IMenuBarButton menuBarItem, BarItem buttonItem)
      {
         base.UpdateBarButtonItem(menuBarItem, buttonItem);
         buttonItem.ItemClick += (o, e) =>
         {
            if (menuBarItem.Locked)
               return;

            menuBarItem.Click();
         };
      }
   }
}