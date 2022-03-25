using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using OSPSuite.Assets;

namespace OSPSuite.UI.Extensions
{
   public static class BarItemExtensions
   {
      public static BarItem UpdateIcon(this BarItem barItem, ApplicationIcon icon)
      {
         if (icon == null)
            return barItem;

         barItem.ImageOptions.SvgImage = transparentImage(icon, IconSizes.Size16x16);

         return barItem;
      }

      private static SvgImage transparentImage(ApplicationIcon icon, IconSize iconSize)
      {
         return icon;
      }
   }
}