using System.Drawing;
using System.IO;
using DevExpress.Utils.Svg;

namespace OSPSuite.Assets
{
   public class ApplicationIcon
   {
      private readonly SvgImage _image;
      private readonly SvgBitmap _bitmap;

      public string IconName { get; set; }
      public IconSize IconSize { get; set; }
      public int Index { get; set; }

      public ApplicationIcon(byte[] bytes) : this(bytesToImage(bytes))
      {
      }

      public ApplicationIcon(SvgImage image)
      {
         _image = image;
         _bitmap = image == null ? null : new SvgBitmap(image);
         Index = -1;
         IconSize = IconSizes.Size16x16;
      }

      private static SvgImage bytesToImage(byte[] bytes)
      {
         using (var ms = new MemoryStream(bytes))
            return new SvgImage(ms);
      }

      public static implicit operator SvgImage(ApplicationIcon icon) => icon.ToSvgImage();

      public virtual Image ToImage() => ToImage(IconSize);

      public virtual Image ToImage(IconSize imageSize)
      {
         return _bitmap?.Render(imageSize, null);
         /*Bitmap target = new Bitmap(
            (int)imageSize.Width,
            (int)imageSize.Height);
         using (Graphics g = Graphics.FromImage(target))
         {
            _bitmap.RenderToGraphics(g,
               SvgPaletteHelper.GetSvgPalette(LookAndFeel, ObjectState.Normal));
         }*/
      }

      public virtual SvgImage ToSvgImage()
      {
         return WithSize(IconSize);
      }

      public virtual SvgImage WithSize(IconSize iconSize)
      {
         return _image;
      }
   }
}