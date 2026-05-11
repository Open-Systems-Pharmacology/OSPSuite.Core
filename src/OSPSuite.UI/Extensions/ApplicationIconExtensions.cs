using System.Drawing;
using System.IO;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.UI.Extensions
{
   public static class ApplicationIconExtensions
   {
      private static readonly Cache<ApplicationIcon, SvgImage> _svgCache = new();

      public static SvgImage ToSvgImage(this ApplicationIcon icon)
      {
         if (icon?.IconBytes == null)
            return null;

         if (!_svgCache.Contains(icon))
         {
            using var ms = new MemoryStream(icon.IconBytes);
            _svgCache[icon] = new SvgImage(ms);
         }

         return _svgCache[icon];
      }

      public static Image ToImage(this ApplicationIcon icon) => icon.ToImage(IconSizes.Size16x16);

      public static Image ToImage(this ApplicationIcon icon, IconSize imageSize)
      {
         var svg = icon.ToSvgImage();
         return svg?.Render(imageSize.ToDrawingSize(), paletteProvider: null, useHighSpeedRendering: DefaultBoolean.Default, allowCache: DefaultBoolean.Default)
                ?? new Bitmap(imageSize.Width, imageSize.Height);
      }

      public static Size ToDrawingSize(this IconSize iconSize) => new(iconSize.Width, iconSize.Height);
   }
}