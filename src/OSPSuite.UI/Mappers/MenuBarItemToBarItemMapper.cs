using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   public interface IMenuBarItemToBarItemMapper
   {
      BarItem MapFrom(BarManager barManager, IMenuBarItem menuBarItem);
   }

   public class MenuBarItemToBarItemMapper : IMenuBarItemToBarItemMapper
   {
      private readonly IMenuBarButtonToBarItemMapper _barItemMapper;
      private readonly IMenuBarSubMenuToBarSubItemMapper _barSubItemMapper;

      public MenuBarItemToBarItemMapper(IMenuBarButtonToBarItemMapper barItemMapper, IMenuBarSubMenuToBarSubItemMapper barSubItemMapper)
      {
         _barItemMapper = barItemMapper;
         _barSubItemMapper = barSubItemMapper;
      }

      public BarItem MapFrom(BarManager barManager, IMenuBarItem menuBarItem)
      {
         try
         {
            barManager.BeginUpdate();
            if(menuBarItem.IsAnImplementationOf<IMenuBarButton>())
               return _barItemMapper.MapFrom(barManager, menuBarItem as IMenuBarButton);

            return _barSubItemMapper.MapFrom(barManager, menuBarItem as IMenuBarSubMenu);
         }
         finally
         {
            barManager.EndUpdate();
         }
      }
   }
}