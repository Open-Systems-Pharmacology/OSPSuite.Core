using System.Collections.Generic;
using DevExpress.Utils;
using OSPSuite.Assets;

namespace OSPSuite.UI.Mappers
{
   public interface IApplicationIconsToImageCollectionMapper
   {
      SvgImageCollection MapFrom(IEnumerable<ApplicationIcon> listOfIcons, IconSize iconSize);
   }

   public class ApplicationIconsToImageCollectionMapper : IApplicationIconsToImageCollectionMapper
   {
      public SvgImageCollection MapFrom(IEnumerable<ApplicationIcon> listOfIcons, IconSize iconSize)
      {
         var imageList = new SvgImageCollection { ImageSize = iconSize};
         foreach (var icon in listOfIcons)
         {
            imageList.Add(icon.IconName,  icon);
         }
         return imageList;
      }
   }
}