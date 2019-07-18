using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Assets
{
   public class ApplicationIcon
   {
      private readonly Icon _masterIcon;
      private IconHeader _iconHeader;
      private MemoryStream _icoStream;
      private readonly IList<IconEntry> _iconEntries = new List<IconEntry>();
      private readonly ICache<IconSize, Icon> _iconCache = new Cache<IconSize, Icon>();
      private readonly ICache<IconSize, Bitmap> _imageCache = new Cache<IconSize, Bitmap>();

      public string IconName { get; set; }
      public IconSize IconSize { get; set; }
      public int Index { get; set; }

      public ApplicationIcon(byte[] bytes): this(bytesToIcon(bytes))
      {
      }

      public ApplicationIcon(Icon icon)
      {
         _masterIcon = icon;
         Index = -1;
         IconSize = IconSizes.Size16x16;
         createMultipleIconList();
      }

      private static Icon bytesToIcon(byte[] bytes)
      {
         using (MemoryStream ms = new MemoryStream(bytes))
         {
            return new Icon(ms);
         }
      }

      private void createMultipleIconList()
      {
         if (_masterIcon == null) return;
         _icoStream = _masterIcon.ToMemoryStream();
         _iconHeader = new IconHeader(_icoStream);

         // Read the icons
         var tempList = new List<IconEntry>();
         for (int counter = 0; counter < _iconHeader.Count; counter++)
         {
            tempList.Add(new IconEntry(_icoStream));
         }

         //Order by width and bit count so that we retrieve always with icon with the higher bit count
         foreach (var iconEntry in tempList.OrderBy(item => item.Width).ThenByDescending(item => item.BitCount))
         {
            _iconEntries.Add(iconEntry);
         }
      }

      public static implicit operator Icon(ApplicationIcon icon)
      {
         return icon.ToIcon();
      }

      public static implicit operator Image(ApplicationIcon icon)
      {
         return icon.ToImage();
      }

      public virtual Bitmap ToImage()
      {
         return ToImage(IconSize);
      }

      public virtual Bitmap ToImage(IconSize imageSize)
      {
         if (!_imageCache.Contains(imageSize))
         {
            var icon = WithSize(imageSize);
            var image = icon?.ToBitmap() ?? new Bitmap(imageSize.Width, imageSize.Height);
            _imageCache.Add(imageSize, image);
         }
         return _imageCache[imageSize];
      }

      public virtual Icon ToIcon()
      {
         return WithSize(IconSize);
      }

      public virtual Icon WithSize(IconSize iconSize)
      {
         try
         {
            if (!_iconCache.Contains(iconSize))
               _iconCache.Add(iconSize, retrieveIconBySize(iconSize));

            return _iconCache[iconSize];
         }
         catch (Exception)
         {
            return _masterIcon;
         }
      }

      private Icon retrieveIconBySize(IconSize iconSize)
      {
         foreach (var iconEntry in _iconEntries)
         {
            if (iconEntry.FitIn(iconSize))
               return buildIcon(iconEntry);
         }

         //we did not find any icon with the accurate size. We return the master icon
         return _masterIcon;
      }

      private Icon buildIcon(IconEntry icon)
      {
         var newIcon = new MemoryStream();

         // New Values
         Int16 newNumber = 1;
         Int32 newOffset = 22;

         // Write it
         var writer = new BinaryWriter(newIcon);
         writer.Write(_iconHeader.Reserved);
         writer.Write(_iconHeader.Type);
         writer.Write(newNumber);
         writer.Write(icon.Width);
         writer.Write(icon.Height);
         writer.Write(icon.ColorCount);
         writer.Write(icon.Reserved);
         writer.Write(icon.Planes);
         writer.Write(icon.BitCount);
         writer.Write(icon.BytesInRes);
         writer.Write(newOffset);

         // Grab the icon
         byte[] tmpBuffer = new byte[icon.BytesInRes];
         _icoStream.Position = icon.ImageOffset;
         _icoStream.Read(tmpBuffer, 0, icon.BytesInRes);
         writer.Write(tmpBuffer);

         // Finish up
         writer.Flush();
         newIcon.Position = 0;
         return new Icon(newIcon, icon.Width, icon.Height);
      }
   }
}