using System.Drawing;
using System.IO;
using DevExpress.Utils;
using DevExpress.Utils.Svg;

namespace OSPSuite.Assets
{
   public class ApplicationIcon
   {
      private readonly SvgImage _image;

      public string IconName { get; set; }
      public int Index { get; set; }

      public ApplicationIcon(byte[] bytes) : this(bytesToImage(bytes))
      {
      }

      public ApplicationIcon(SvgImage image)
      {
         _image = image;
         Index = -1;
      }

      private static SvgImage bytesToImage(byte[] bytes)
      {
         using (var ms = new MemoryStream(bytes))
            return new SvgImage(ms);
      }

      public static implicit operator SvgImage(ApplicationIcon icon) => icon.ToSvgImage();

      public static implicit operator Image(ApplicationIcon icon) => icon.ToImage();

      public virtual Image ToImage() => ToImage(IconSizes.Size16x16);

      public virtual Image ToImage(IconSize imageSize)
      {
         return _image?.Render(imageSize, paletteProvider: null, useHighSpeedRendering: DefaultBoolean.Default, allowCache: DefaultBoolean.Default)
                ?? new Bitmap(imageSize.Width, imageSize.Height);
      }

      public virtual SvgImage ToSvgImage() => _image;
   }
}