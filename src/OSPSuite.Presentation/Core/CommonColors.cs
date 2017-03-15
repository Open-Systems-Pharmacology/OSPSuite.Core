using System.Collections.Generic;
using System.Drawing;

namespace OSPSuite.Presentation.Core
{
   public static class CommonColors
   {
      private static readonly List<Color> _allColors = new List<Color>();

      // The order here is the same as inclusion in the color picker.
      // This is because it's easiest to make edits to the order when 
      // the color is being shown by Visual Studio
      public static Color Grey = addColor(Color.FromArgb(-8355712));
      public static Color DarkBrown = addColor(Color.FromArgb(-8372160));
      public static Color Brown = addColor(Color.FromArgb(-4177920));
      public static Color DarkRed = addColor(Color.FromArgb(-4194304));
      public static Color Red = addColor(Color.FromArgb(-65536));
      public static Color PalePink = addColor(Color.FromArgb(-32640));
      public static Color DarkViolet = addColor(Color.FromArgb(-8388480));
      public static Color Orchid = addColor(Color.FromArgb(-4194112));
      public static Color Magenta = addColor(Color.FromArgb(-65281));
      public static Color Orange = addColor(Color.FromArgb(-32768));
      public static Color DarkBlue = addColor(Color.FromArgb(-16777024));
      public static Color Blue = addColor(Color.FromArgb(-16776961));
      public static Color Violet = addColor(Color.FromArgb(-8355585));
      public static Color SeaGreen = addColor(Color.FromArgb(-16744320));
      public static Color Green = addColor(Color.FromArgb(-16744448));
      public static Color MediumGreen = addColor(Color.FromArgb(-16728064));
      public static Color LightGreen = addColor(Color.FromArgb(-16711936));
      public static Color PaleGreen = addColor(Color.FromArgb(-8323200));
      public static Color DarkCyan = addColor(Color.FromArgb(-16727872));
      public static Color DarkOlive = addColor(Color.FromArgb(-8355840));
      public static Color Olive = addColor(Color.FromArgb(-4145152));
      public static Color DarkGrey = addColor(Color.FromArgb(-1256646));
      public static Color Yellow = addColor(Color.FromArgb(-256));
      

      private static Color addColor(Color color)
      {
         _allColors.Add(color);
         return color;
      }

      public static IEnumerable<Color> OrderedForCurves()
      {
         yield return Red;
         yield return Blue;
         yield return Green;
         yield return Magenta;
         yield return Orange;
         yield return DarkCyan;
         yield return Orchid;
         yield return Brown;
         yield return LightGreen;
         yield return Grey;
         yield return Olive;
         yield return Violet;
         yield return DarkBlue;
         yield return MediumGreen;
         yield return DarkRed;
         yield return Yellow;
         yield return SeaGreen;
         yield return DarkViolet;
         yield return DarkBrown;
         yield return PaleGreen;
         yield return DarkOlive;
         yield return PalePink;
         yield return DarkGrey;
      }

      public static IReadOnlyList<Color> OrderedForPicker()
      {
         return _allColors;
      }
   }
}