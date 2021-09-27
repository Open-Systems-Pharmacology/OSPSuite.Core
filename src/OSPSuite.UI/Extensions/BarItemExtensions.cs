using System.Drawing;
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

         //TODO This does not seeem to be supported yet
         barItem.ImageOptions.SvgImage = transparentImage(icon, IconSizes.Size16x16);
         // barItem.gl..LargeGlyph. = transparentImage(icon, IconSizes.Size32x32);

         return barItem;
      }

      private static SvgImage transparentImage(ApplicationIcon icon, IconSize iconSize)
      {
         return icon;
      }
   }
}