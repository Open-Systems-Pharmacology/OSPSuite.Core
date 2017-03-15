using System.Drawing;

namespace OSPSuite.UI.Extensions
{
   public static class ColorExtensions
   {
      public static bool IsEqualTo(this Color color, Color secondColor)
      {
         return secondColor.ToArgb().Equals(color.ToArgb());
      }
   }
}
