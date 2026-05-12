using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using OSPSuite.Assets;

namespace OSPSuite.Presentation.Extensions
{
   public static class ApplicationIconExtensions
   {
      private static readonly ConcurrentDictionary<ApplicationIcon, SvgImage> _svgCache = new();

      public static SvgImage ToSvgImage(this ApplicationIcon icon)
      {
         if (icon?.IconBytes == null)
            return null;

         return _svgCache.GetOrAdd(icon, key =>
         {
            using (var ms = new MemoryStream(key.IconBytes))
               return new SvgImage(ms);
         });
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