using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   public interface IMenuBarSubMenuToBarSubItemMapper
   {
      BarSubItem MapFrom(BarManager barManager, IMenuBarSubMenu menuBarSubMenu);
   }

   public class MenuBarSubMenuToBarSubItemMapper : MenuBarItemToBarItemMapperBase, IMenuBarSubMenuToBarSubItemMapper
   {
      private readonly IMenuBarButtonToBarItemMapper _mapper;

      public MenuBarSubMenuToBarSubItemMapper(IMenuBarButtonToBarItemMapper mapper, IStartOptions startOptions) : base(startOptions)
      {
         _mapper = mapper;
      }

      public BarSubItem MapFrom(BarManager barManager, IMenuBarSubMenu menuBarSubMenu)
      {
         try
         {
            barManager.BeginUpdate();

            var existingItem = barManager.Items[menuBarSubMenu.Name];
            if (existingItem != null)
               return existingItem as BarSubItem;

            var mainMenu = new BarSubItem(barManager, menuBarSubMenu.Caption);

            foreach (var menuElement in menuBarSubMenu.AllItems())
            {
               var barItem = menuElement.IsAnImplementationOf<IMenuBarButton>()
                  ? _mapper.MapFrom(barManager, menuElement as IMenuBarButton)
                  : MapFrom(barManager, menuElement as IMenuBarSubMenu);

               mainMenu.AddItem(barItem).BeginGroup = menuElement.BeginGroup;
               //this is required since sometime this information is not updated as expected
               mainMenu.LinksPersistInfo[mainMenu.LinksPersistInfo.Count - 1].BeginGroup = menuElement.BeginGroup;
            }

            UpdateBarButtonItem(menuBarSubMenu, mainMenu);
            return mainMenu;
         }
         finally
         {
            barManager.EndUpdate();
         }
      }
   }
}