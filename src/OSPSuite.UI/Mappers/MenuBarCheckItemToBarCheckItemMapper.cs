using OSPSuite.Utility;
using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   public interface IMenuBarCheckItemToBarCheckItemMapper : IMapper<IMenuBarCheckItem, BarCheckItem>
   {
      BarCheckItem MapFrom(BarManager barManager, IMenuBarCheckItem menuBarCheckItem);
   }

   public class MenuBarCheckItemToBarCheckItemMapper : MenuBarButtonToBarItemMapperBase, IMenuBarCheckItemToBarCheckItemMapper
   {
      private readonly BarManager _barManager;

      public MenuBarCheckItemToBarCheckItemMapper(BarManager barManager, IStartOptions startOptions): base(startOptions)
      {
         _barManager = barManager;
      }

      public BarCheckItem MapFrom(IMenuBarCheckItem menuBarCheckItem)
      {
         return MapFrom(_barManager, menuBarCheckItem);
      }

      public BarCheckItem MapFrom(BarManager barManager, IMenuBarCheckItem menuBarCheckItem)
      {
         var barCheckItem = new BarCheckItem(barManager, menuBarCheckItem.Checked) { Caption = menuBarCheckItem.Caption };
         UpdateBarButtonItem(menuBarCheckItem, barCheckItem);
         menuBarCheckItem.CheckedChanged += (value => barCheckItem.Checked = value);
         return barCheckItem;
      }
   }
}