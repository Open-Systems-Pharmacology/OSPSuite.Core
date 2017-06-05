using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class DataBrowserView : BaseUserControlWithColumnSettings, IDataBrowserView
   {
      private GridHitInfo _downHitInfo;
      private IDataBrowserPresenter _presenter;

      public DataBrowserView()
      {
         InitializeComponent();

         gridView.AllowsFiltering = true;
         gridView.ShowColumnChooser = true;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsView.ShowGroupPanel = true;
         gridView.MultiSelect = true; 
         gridView.MouseDown += (o, e) => OnEvent(viewMouseDown, e);
         gridView.MouseMove += (o, e) => OnEvent(viewMouseMove, e);
         gridView.CellValueChanged += (o, e) => OnEvent(viewCellValueChanged, e);
         gridView.PopupMenuShowing += (o, e) => OnEvent(viewPopupMenuShowing, e);
         gridView.SelectionChanged += (o, e) => OnEvent(onGridViewSelectionChanged);
         InitializeWith(gridView);
         ActivateGridColumnChangedEventHandlers();
      }

      private void onGridViewSelectionChanged()
      {
         _presenter.UpdateDataSelection(SelectedDescendentDataRepositoryColumnIds);
      }

      public IReadOnlyList<string> SelectedDescendentDataRepositoryColumnIds => selectDescendentDataRows(gridView.GetSelectedRows()).Select(getDataRepositoryColumnIdForRowHandle).ToList();

      private IEnumerable<int> selectDescendentDataRows(IEnumerable<int> selectedRowHandles)
      {
         foreach (var rowHandle in selectedRowHandles)
         {
            if (gridView.IsGroupRow(rowHandle))
            {
               foreach (var descendentDataRow in selectDescendentDataRows(getImmediateChildrenRows(rowHandle)))
               {
                  yield return descendentDataRow;
               }
            }
            else if(gridView.IsDataRow(rowHandle))
            {
               yield return rowHandle;
            }
         }
      }

      private int[] getImmediateChildrenRows(int row)
      {
         var childRows = new int[gridView.GetChildRowCount(row)];
         for (var i = 0; i < gridView.GetChildRowCount(row); i++)
         {
            childRows[i] = (gridView.GetChildRowHandle(row, i));
         }
         return childRows;
      }

      public override GridColumn FindColumnByName(string columnName)
      {
         return gridView.Columns[columnName];
      }

      public void AttachPresenter(IDataBrowserPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _presenter = presenter;
      }

      public void SetDataSource(DataColumnsDTO dataColumnsDTO)
      {
         gridControl.DataSource = dataColumnsDTO;
         // use special checkEdit to change value without leaving checkbox
         gridView.Columns.ColumnByFieldName(BrowserColumns.Used.ToString()).ColumnEdit = new UxRepositoryItemCheckEdit(gridView);
         ApplyAllColumnSettings(); // data columns are available first after DataSource is set.
         disableColumnEditing();
         enableColumnFilterCheckBoxes();
      }

      private void disableColumnEditing()
      {
         foreach (GridColumn gridColumn in gridView.Columns)
            if (gridColumn.FieldName != BrowserColumns.Used.ToString())
               gridColumn.OptionsColumn.AllowEdit = false;
      }

      private void enableColumnFilterCheckBoxes()
      {
         foreach (GridColumn gridColumn in gridView.Columns)
            gridColumn.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
      }

      public IReadOnlyList<string> SelectedDataColumnIds => gridView.GetSelectedRows().Select(getDataRepositoryColumnIdForRowHandle).ToList();

      private string getDataRepositoryColumnIdForRowHandle(int rowHandle)
      {
         return gridView.GetDataRow(rowHandle)[BrowserColumns.ColumnId.ToString()] as string;
      }

      private void viewCellValueChanged(CellValueChangedEventArgs e)
      {
         if (e.Column.FieldName == BrowserColumns.Used.ToString())
         {
            var columnId = gridView.GetDataRow(e.RowHandle)[BrowserColumns.ColumnId.ToString()] as string;
            if (columnId == null) return;
            if (e.Value.GetType() != typeof(bool)) return;
            _presenter.RaiseUsedChanged(new[] { columnId }, (bool)e.Value);
            gridView.CloseEditor();
            _presenter.UpdateDataSelection(SelectedDataColumnIds);
         }
      }

      private void viewPopupMenuShowing(PopupMenuShowingEventArgs e)
      {
         if (e.MenuType == GridMenuType.Row && e.HitInfo.InRow && !gridView.IsDataRow(e.HitInfo.RowHandle))
         {
            var popupMenu = new DataBrowserPopupMenu(gridView, _presenter)
            {
               GroupRowHandle = e.HitInfo.RowHandle
            };
            popupMenu.Show(e.HitInfo.HitPoint);
         }
      }

      private void viewMouseDown(MouseEventArgs e)
      {
         _downHitInfo = null;
         GridHitInfo hitInfo = gridView.CalcHitInfo(new Point(e.X, e.Y));
         if (ModifierKeys != Keys.None) return;
         if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
            _downHitInfo = hitInfo;
      }

      private void viewMouseMove(MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left && _downHitInfo != null)
         {
            Size dragSize = SystemInformation.DragSize;
            var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
               _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
               var data = _presenter.SelectedDataColumns;
               gridControl.DoDragDrop(data, DragDropEffects.Move);
               _downHitInfo = null;
               DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
         }
      }
   }
}