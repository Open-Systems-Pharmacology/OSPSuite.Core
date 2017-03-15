using OSPSuite.Utility;
using DevExpress.Utils;
using OSPSuite.Assets;

namespace OSPSuite.UI.Services
{
   public interface IImageListRetriever : IStartable
   {
      ImageCollection AllImages16x16 { get; }
      ImageCollection AllImages24x24 { get; }
      ImageCollection AllImages32x32 { get; }
      ImageCollection AllImagesForContextMenu { get; }
      ImageCollection AllImagesForTreeView { get; }
      ImageCollection AllImagesForTabs { get; }
      int ImageIndex(string imageName);
      int ImageIndex(ApplicationIcon icon);
   }
}