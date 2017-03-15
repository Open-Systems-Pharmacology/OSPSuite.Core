using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   /// Simple wrapper over repository item combo box that ensure that 
   /// 1-the change event is fired as soon as the combo box selection is changed, and not only after the row loses the focus
   /// 2-The editor is read only (default for PKSim Combo Boxes)
   /// </summary>
   public class UxRepositoryItemComboBox : RepositoryItemComboBox
   {
      public UxRepositoryItemComboBox(DevExpress.XtraGrid.Views.Base.BaseView view)
      {
         TextEditStyle = TextEditStyles.DisableTextEditor;
         EditValueChanged += (o, e) => view.PostEditor();
      }
   }
}