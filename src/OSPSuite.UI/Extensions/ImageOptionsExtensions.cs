using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using OSPSuite.Assets;

namespace OSPSuite.UI.Extensions
{
   public static class ImageOptionsExtensions
   {
      public static void SetImage(this XtraMdiTabPage owner, ApplicationIcon icon, IconSize iconSize = null) => 
         SetImage(owner.ImageOptions, icon, iconSize);

      public static void SetImage(this EditorButton owner, ApplicationIcon icon, IconSize iconSize = null) => 
         SetImage(owner.ImageOptions, icon, iconSize);

      public static void SetImage(this XtraTabPage owner, ApplicationIcon icon, IconSize iconSize = null) =>
         SetImage(owner.ImageOptions, icon, iconSize);
      

      public static void SetImage(this ImageCollectionImageOptions imageOptions, ApplicationIcon icon, IconSize iconSize = null)
      {
         imageOptions.SvgImage = icon;
         imageOptions.SvgImageSize = iconSize ?? IconSizes.Size16x16;
      }
   }
}