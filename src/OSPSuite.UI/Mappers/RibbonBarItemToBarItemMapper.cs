using System;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Mappers
{
   public interface IRibbonBarItemToBarItemMapper : IMapper<IRibbonBarItem, BarItem>
   {
   }

   public class RibbonBarItemToBarItemMapper : IRibbonBarItemToBarItemMapper
   {
      private readonly RibbonBarManager _ribbonBarManager;
      private readonly IMenuBarItemToBarItemMapper _barItemMapper;

      public RibbonBarItemToBarItemMapper(RibbonBarManager ribbonBarManager, IMenuBarItemToBarItemMapper barItemMapper)
      {
         _ribbonBarManager = ribbonBarManager;
         _barItemMapper = barItemMapper;
      }

      public BarItem MapFrom(IRibbonBarItem ribbonBarItem)
      {
         var barItem = _barItemMapper.MapFrom(_ribbonBarManager, ribbonBarItem.MenuBarItem);
         barItem.Caption = ribbonBarItem.Caption;
         var barButtonItem = barItem as BarButtonItem;
         if (barButtonItem != null)
            addSubMenusToBarButtonItem(barButtonItem, ribbonBarItem);
         else
            addSubMenusToBarSubItem(barItem.DowncastTo<BarSubItem>(), ribbonBarItem);

         barItem.RibbonStyle = mapStyleFrom(ribbonBarItem.ItemStyle);
         barItem.UpdateIcon(ribbonBarItem.Icon);

         return barItem;
      }

      private void addSubMenusToBarSubItem(BarSubItem barSubItem, IRibbonBarItem ribbonBarItem)
      {
         if (barSubItem.ItemLinks.Count == ribbonBarItem.AllMenuItems().Count()) return;

         foreach (var menuElement in ribbonBarItem.AllMenuItems())
         {
            barSubItem.ItemLinks.Add(_barItemMapper.MapFrom(_ribbonBarManager, menuElement));
         }
      }

      private void addSubMenusToBarButtonItem(BarButtonItem barButtonItem, IRibbonBarItem ribbonBarItem)
      {
         if (!ribbonBarItem.AllMenuItems().Any()) return;
         //we have a popup element to create for the given baritem
         try
         {
            _ribbonBarManager.BeginUpdate();
            var popupMenu = new PopupMenu(_ribbonBarManager);
            barButtonItem.DropDownControl = popupMenu;
            foreach (var menuElement in ribbonBarItem.AllMenuItems())
            {
               popupMenu.ItemLinks.Add(_barItemMapper.MapFrom(_ribbonBarManager, menuElement));
            }
            barButtonItem.ButtonStyle = BarButtonStyle.DropDown;
            popupMenu.Ribbon = _ribbonBarManager.Ribbon;
         }
         finally
         {
            _ribbonBarManager.EndUpdate();
         }
      }

      private RibbonItemStyles mapStyleFrom(ItemStyle itemStyle)
      {
         switch (itemStyle)
         {
            case ItemStyle.Large:
               return RibbonItemStyles.Large;
            case ItemStyle.Small:
               return RibbonItemStyles.SmallWithText | RibbonItemStyles.SmallWithoutText;
            case ItemStyle.All:
               return RibbonItemStyles.All;
            default:
               throw new ArgumentOutOfRangeException("itemStyle");
         }
      }
   }
}