using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   /// <summary>
   /// Convert a menu bar item to a bar item
   /// </summary>
   public interface IMenuBarButtonToBarItemMapper
   {
      BarItem MapFrom(BarManager barManager, IMenuBarButton menuBarItem);
   }

   public class MenuBarButtonToBarItemMapper : MenuBarButtonToBarItemMapperBase, IMenuBarButtonToBarItemMapper
   {
      private readonly IMenuBarCheckItemToBarCheckItemMapper _mapper;

      public MenuBarButtonToBarItemMapper(IMenuBarCheckItemToBarCheckItemMapper mapper, IStartOptions startOptions): base(startOptions)
      {
         _mapper = mapper;
      }

      public BarItem MapFrom(BarManager barManager, IMenuBarButton menuBarItem)
      {
         if (menuBarItem.IsForDeveloper && !_startOptions.IsDeveloperMode)
            return new BarButtonItem { Visibility = BarItemVisibility.Never };

         var existingItem = barManager.Items[menuBarItem.Name];
         if (existingItem != null)
            return existingItem;

         var menuBarCheckItem = menuBarItem as IMenuBarCheckItem;
         if (menuBarCheckItem != null)
            return _mapper.MapFrom(barManager, menuBarCheckItem);

         var buttonItem = new BarButtonItem(barManager, menuBarItem.Caption);

         UpdateBarButtonItem(menuBarItem, buttonItem);

         return buttonItem;
      }
   }
}