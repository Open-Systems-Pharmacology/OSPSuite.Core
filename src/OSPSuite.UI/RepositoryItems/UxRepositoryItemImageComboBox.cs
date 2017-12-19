using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   ///    Simple wrapper over repository item image combo box that ensure that
   ///    1-the change event is fired as soon as the combo box selection is changed, and not only after the row loses the
   ///    focus
   ///    2-The editor is read only (default for PKSim Combo Boxes)
   /// </summary>
   public class UxRepositoryItemImageComboBox : RepositoryItemImageComboBox
   {
      private readonly IImageListRetriever _imageListRetriever;

      public UxRepositoryItemImageComboBox(BaseView view, IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         TextEditStyle = TextEditStyles.DisableTextEditor;
         EditValueChanged += (o, e) => view.PostEditor();
         SmallImages = _imageListRetriever.AllImages16x16;
         LargeImages = _imageListRetriever.AllImages32x32;
      }

      public UxRepositoryItemImageComboBox AddItem(object value, ApplicationIcon icon, string display = null)
      {
         return AddItem(value, _imageListRetriever.ImageIndex(icon), display);
      }

      public UxRepositoryItemImageComboBox AddItem(object value, string iconName, string display = null)
      {
         return AddItem(value, _imageListRetriever.ImageIndex(iconName), display);
      }

      public UxRepositoryItemImageComboBox AddItem(object value, int iconIndex, string display = null)
      {
         return AddItem(new ImageComboBoxItem(display ?? value.ToString(), value, iconIndex));
      }

      public UxRepositoryItemImageComboBox AddItem(ImageComboBoxItem imageComboBoxItem)
      {
         Items.Add(imageComboBoxItem);
         return this;
      }
   }
}