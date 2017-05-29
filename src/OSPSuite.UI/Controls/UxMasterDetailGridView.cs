using DevExpress.XtraGrid;

namespace OSPSuite.UI.Controls
{
   public class UxMasterDetailGridView : UxGridView
   {
      protected override string ViewName => "UxMasterDetailGridView";

      public UxMasterDetailGridView(GridControl gridControl) : base(gridControl)
      {
      }
   }
}