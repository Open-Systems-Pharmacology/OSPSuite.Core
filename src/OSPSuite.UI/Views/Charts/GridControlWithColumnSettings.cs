using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Dragging;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class GridControlWithColumnSettings : UxGridControl
   {
      protected GridView GridView { get; private set; }

      internal GridControlWithColumnSettings()
      {
         InitializeComponent();

         GridView.MouseUp += onMouseUp;
         GridView.GridMenuItemClick += onGridMenuItemClick;
         GridView.DragObjectDrop += onDragObjectDrop;
      }

      private void onMouseUp(object sender, MouseEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Button != MouseButtons.Left || e.Clicks > 1 || view == null) return;
         Point p = view.GridControl.PointToClient(MousePosition);
         GridHitInfo info = view.CalcHitInfo(p);
         if (info.HitTest == GridHitTest.Column)
            info.Column.FieldNameSortGroup = string.Empty;
      }

      private void onGridMenuItemClick(object sender, GridMenuItemClickEventArgs e)
      {
         if (e.MenuType != GridMenuType.Column) return;
         GridStringId gridStringId;
         try
         {
            gridStringId = (GridStringId) e.DXMenuItem.Tag;
         }
         catch
         {
            return;
         }
         if (gridStringId == GridStringId.MenuColumnSortAscending
             || gridStringId == GridStringId.MenuColumnSortDescending
             || gridStringId == GridStringId.MenuColumnGroup)
            e.Column.FieldNameSortGroup = string.Empty;
      }

      private void onDragObjectDrop(object sender, DragObjectDropEventArgs e)
      {
         if (e.DragObject is GridColumn
             && e.DropInfo is ColumnPositionInfo
             && (e.DropInfo as ColumnPositionInfo).InGroupPanel)
            (e.DragObject as GridColumn).FieldNameSortGroup = string.Empty;
      }
   }
}