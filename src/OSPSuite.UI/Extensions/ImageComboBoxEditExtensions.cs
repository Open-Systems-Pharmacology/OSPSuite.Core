using DevExpress.XtraEditors;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Extensions
{
   public static class ImageComboBoxEditExtensions
   {
      public static void SetImages(this ImageComboBoxEdit comboBoxEdit, IImageListRetriever imageListRetriever)
      {
         comboBoxEdit.Properties.SmallImages = imageListRetriever.AllImages16x16;
         comboBoxEdit.Properties.LargeImages = imageListRetriever.AllImages32x32;
      }

      public static void ClearImages(this ImageComboBoxEdit comboBoxEdit)
      {
         comboBoxEdit.Properties.SmallImages = null;
         comboBoxEdit.Properties.LargeImages = null;
      }
   }
}