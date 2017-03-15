using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   ///   Simple wrapper over repository item image combo box that ensure that 
   ///   1-the change event is fired as soon as the combo box selection is changed, and not only after the row loses the focus
   ///   2-The editor is read only (default for PKSim Combo Boxes)
   /// </summary>
   public class UxRepositoryItemImageComboBox : RepositoryItemImageComboBox
   {
      public UxRepositoryItemImageComboBox(BaseView view, IImageListRetriever imageListRetriever)
      {
         TextEditStyle = TextEditStyles.DisableTextEditor;
         EditValueChanged += (o, e) => view.PostEditor();
         SmallImages = imageListRetriever.AllImages16x16;
         LargeImages = imageListRetriever.AllImages32x32;
      }
   }
}