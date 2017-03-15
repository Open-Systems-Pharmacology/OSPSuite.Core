using System.Collections.Generic;
using DevExpress.Utils;
using OSPSuite.Assets;

namespace OSPSuite.UI.Mappers
{
   public interface IApplicationIconsToImageCollectionMapper
   {
      ImageCollection MapFrom(IEnumerable<ApplicationIcon> listOfIcons, IconSize iconSize);
   }

   public class ApplicationIconsToImageCollectionMapper : IApplicationIconsToImageCollectionMapper
   {
      public ImageCollection MapFrom(IEnumerable<ApplicationIcon> listOfIcons, IconSize iconSize)
      {
         var imageList = new ImageCollection {ImageSize = iconSize};
         foreach (var icon in listOfIcons)
         {
            imageList.AddImage(icon.ToImage(iconSize), icon.IconName);
         }
         return imageList;
      }
   }
}