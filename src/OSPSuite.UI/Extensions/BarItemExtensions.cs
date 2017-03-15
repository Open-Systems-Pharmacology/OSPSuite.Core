using System.Drawing;
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

         barItem.Glyph = transparentImage(icon, IconSizes.Size16x16);
         barItem.LargeGlyph = transparentImage(icon, IconSizes.Size32x32);

         return barItem;
      }

      private static Image transparentImage(ApplicationIcon icon, IconSize iconSize)
      {
         return icon.ToImage(iconSize);
      }
   }
}