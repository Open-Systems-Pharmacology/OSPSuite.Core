using System;
using OSPSuite.Utility;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   public interface IStatusBarElementToBarItemMapper : IMapper<StatusBarElement, BarItem>
   {
   }

   public class StatusBarElementToBarItemMapper : IStatusBarElementToBarItemMapper
   {

      public BarItem MapFrom(StatusBarElement element)
      {
         switch (element.ElementType)
         {
            case StatusBarElementType.Text:
               var barStaticItem = new BarStaticItem();
               barStaticItem.AutoSize = mapFrom(element.AutoSize);
               barStaticItem.Alignment = mapFrom(element.Alignment);
               barStaticItem.Width = element.Width;
               barStaticItem.TextAlignment = element.TextAlignment;
               barStaticItem.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;
               return barStaticItem;
            case StatusBarElementType.ProgressBar:
               var repositoryItemProgressBar = new RepositoryItemProgressBar { Minimum = 0, Maximum = 100 };
               var barEditItem = new BarEditItem { Edit = repositoryItemProgressBar };
               barEditItem.Width = element.Width;
               barEditItem.Alignment = mapFrom(element.Alignment);
               return barEditItem;
            default:
               throw new ArgumentOutOfRangeException("element");
         }
      }

      private BarItemLinkAlignment mapFrom(StatusBarElementAlignment alignment)
      {
         switch (alignment)
         {
            case StatusBarElementAlignment.Left:
               return BarItemLinkAlignment.Left;
            case StatusBarElementAlignment.Right:
               return BarItemLinkAlignment.Right;
            default:
               throw new ArgumentOutOfRangeException("alignment");
         }
      }

      private BarStaticItemSize mapFrom(StatusBarElementSize statusBarElementSize)
      {
         switch (statusBarElementSize)
         {
            case StatusBarElementSize.None:
               return BarStaticItemSize.None;
            case StatusBarElementSize.Spring:
               return BarStaticItemSize.Spring;
            case StatusBarElementSize.Content:
               return BarStaticItemSize.Content;
            default:
               throw new ArgumentOutOfRangeException("statusBarElementSize");
         }
      }
   }
}