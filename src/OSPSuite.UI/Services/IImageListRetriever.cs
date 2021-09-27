using OSPSuite.Utility;
using DevExpress.Utils;
using OSPSuite.Assets;

namespace OSPSuite.UI.Services
{
   public interface IImageListRetriever : IStartable
   {
      SvgImageCollection AllImages16x16 { get; }
      SvgImageCollection AllImages24x24 { get; }
      SvgImageCollection AllImages32x32 { get; }
      SvgImageCollection AllImagesForContextMenu { get; }
      SvgImageCollection AllImagesForTreeView { get; }
      SvgImageCollection AllImagesForTabs { get; }
      int ImageIndex(string imageName);
      int ImageIndex(ApplicationIcon icon);
   }
}