using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Assets
{
   public class IconSizes
   {
      private static readonly ICache<string, IconSize> _allIconSizes = new Cache<string, IconSize>(iconSize => iconSize.Id, onMissingKey);

      public static IconSize Size16x16 = createIconSize(16, 16);
      public static IconSize Size24x24 = createIconSize(24, 24);
      public static IconSize Size32x32 = createIconSize(32, 32);
      public static IconSize Size48x48 = createIconSize(48, 48);

      private static IconSize createIconSize(int width, int height)
      {
         var iconSize = new IconSize(width, height);
         _allIconSizes.Add(iconSize);
         return iconSize;
      }

      public static IconSize ById(string iconSizeId)
      {
         return _allIconSizes[iconSizeId];
      }

      private static IconSize onMissingKey(string key)
      {
         return Size16x16;
      }

      public static IEnumerable<IconSize> All()
      {
         return _allIconSizes;
      }
   }
}