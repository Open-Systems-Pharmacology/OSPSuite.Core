using System.Collections.Generic;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;

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
         var imageList = new SvgImageCollection { ImageSize = iconSize.ToDrawingSize() };
         foreach (var icon in listOfIcons)
         {
            imageList.Add(icon.IconName, icon.ToSvgImage());
         }
         return imageList;
      }
   }
}
