using DevExpress.XtraEditors;

namespace OSPSuite.UI.Controls
{
   public class UxMRUEdit : MRUEdit
   {
      public UxMRUEdit()
      {
         Properties.AllowRemoveMRUItems = false;
      }
   }
}