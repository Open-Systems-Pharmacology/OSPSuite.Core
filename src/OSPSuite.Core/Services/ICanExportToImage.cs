using System.Drawing.Imaging;

namespace OSPSuite.Core.Services
{
   public interface ICanExportToImage
   {
      void ExportToImage();
   }
   public interface ICanExportToImageWithWatermark
   {
      void ExportToImage(string filePath, ImageFormat imageFormat, string waterMark = "");
   }
}