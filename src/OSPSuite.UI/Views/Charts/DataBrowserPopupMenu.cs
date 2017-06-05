using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.UI.Views.Charts
{
   internal class DataBrowserPopupMenu : GridViewMenu
   {
      private readonly IDataBrowserPresenter _presenter;
      private readonly DXMenuItem _selectItem;
      private readonly DXMenuItem _deselectItem;
      public int GroupRowHandle { set; private get; }

      public DataBrowserPopupMenu(GridView view, IDataBrowserPresenter presenter)
         : base(view)
      {
         _presenter = presenter;
         _selectItem = new DXMenuItem("Select all", onItemClick);
         _deselectItem = new DXMenuItem("Deselect all", onItemClick);
         Items.Add(_selectItem);
         Items.Add(_deselectItem);
      }

      private void onItemClick(object sender, EventArgs e)
      {
         bool used = (sender == _selectItem);
         if (View.IsDataRow(GroupRowHandle)) return;

         var ids = new List<string>();
         getChildRowIds(View, GroupRowHandle, ids);
         View.GridControl.Cursor = Cursors.WaitCursor;
         try
         {
            _presenter.RaiseUsedChanged(ids, used);
         }
         finally
         {
            View.GridControl.Cursor = Cursors.Default;
         }
      }

      private void getChildRowIds(GridView view, int groupRowHandle, List<string> ids)
      {
         if (!view.IsGroupRow(groupRowHandle)) return;
         //Get the number of immediate children
         int childCount = view.GetChildRowCount(groupRowHandle);
         for (int i = 0; i < childCount; i++)
         {
            //Get the handle of a child row with the required index
            int childHandle = view.GetChildRowHandle(groupRowHandle, i);
            //If the child is a group row, then add its children to the list
            if (view.IsGroupRow(childHandle))
               getChildRowIds(view, childHandle, ids);
            else
            {
               string id = view.GetRowCellValue(childHandle, BrowserColumns.ColumnId.ToString()) as string;
               if (!ids.Contains(id)) ids.Add(id);
            }
         }
      }
   }
}