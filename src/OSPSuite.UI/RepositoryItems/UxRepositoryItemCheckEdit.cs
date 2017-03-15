using DevExpress.XtraEditors.Repository;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   /// Simple wrapper over RepositoryItemCheckEdit that ensure that 
   /// the change event is fired as soon as the selectionchanged, and not only after the row loses the focus
   /// </summary>
   public class UxRepositoryItemCheckEdit : RepositoryItemCheckEdit
   {
      public UxRepositoryItemCheckEdit(DevExpress.XtraGrid.Views.Base.BaseView view)
      {
         EditValueChanged += (o, e) => view.PostEditor();
      }
   }
}