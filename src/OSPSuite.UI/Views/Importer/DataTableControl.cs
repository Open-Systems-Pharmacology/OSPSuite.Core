using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class DataTableControl : XtraUserControl
   {
      private UxGridControl _grid;

      public DataTableControl(DataTable table)
      {
         InitializeComponent();
         Table = table;
         buildControl();
      }

      public DataTable Table { get; private set; }

      private void buildControl()
      {
         this.DoWithinWaitCursor(() =>
         {
            Controls.Clear();
            _grid = new UxGridControl { DataSource = Table};
            Controls.Add(_grid);
            _grid.Dock = DockStyle.Fill;

            var mv = new GridView(_grid);
            _grid.MainView = mv;
            mv.OptionsView.ShowGroupPanel = false;
            mv.OptionsView.ColumnAutoWidth = false;
            mv.OptionsCustomization.AllowSort = false;
            mv.OptionsCustomization.AllowFilter = false;
            mv.OptionsCustomization.AllowQuickHideColumns = false;
            mv.PopupMenuShowing += onPopUpMenuShowing;
            mv.BestFitColumns();

            foreach (GridColumn col in mv.Columns)
               col.OptionsColumn.AllowEdit = false;

            //tooltips
            _grid.ToolTipController = new ToolTipController();
            _grid.ToolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;

            PerformLayout();
         });
      }

      /// <summary>
      ///    This event handler is used to react when a context menu on a grid column is requested.
      /// </summary>
      private void onPopUpMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.MenuType != GridMenuType.Column) return;

         var menu = e.Menu as GridViewColumnMenu;
         if (menu == null) return;
         if (menu.Column == null) return;

         var view = sender as GridView;
         if (view == null) return;

         foreach (DXMenuItem item in menu.Items)
         {
            if (item.Tag.ToString() != GridStringId.MenuColumnRemoveColumn.ToString() &&
                item.Tag.ToString() != GridStringId.MenuColumnColumnCustomization.ToString()) continue;
            item.Enabled = false;
         }
      }

      /// <summary>
      ///    Method reacting on mouse movements for showing context sensitive tool tips.
      /// </summary>
      private static void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var grid = e.SelectedControl as UxGridControl;
         if (grid == null) return;

         var view = grid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;

         var hi = view.CalcHitInfo(e.ControlMousePosition);
         if (hi.HitTest != GridHitTest.Column) return;

         var dv = view.DataSource as DataView;
         if (dv == null) return;
         var table = dv.Table;
         if (table == null) return;

         e.Info = new ToolTipControlInfo(hi.Column, "")
         {
            SuperTip = createSuperToolTipFor(table.Columns[hi.Column.FieldName]),
            ToolTipType = ToolTipType.SuperTip
         };
      }

      /// <summary>
      ///    Creates a SuperToolTip for given column.
      /// </summary>
      private static SuperToolTip createSuperToolTipFor(DataColumn column)
      {
         var superTip = new SuperToolTip();
         superTip.Items.AddTitle(column.ColumnName);
         superTip.Items.Add(String.Format("Data Type = {0}", column.DataType));
         foreach (string prop in column.ExtendedProperties.Keys)
         {
            if (String.IsNullOrEmpty(column.ExtendedProperties[prop].ToString())) continue;
            superTip.Items.Add(String.Format("{0} = {1}", prop,
               column.ExtendedProperties[prop]));
         }
         return superTip;
      }

      private void cleanMemory()
      {
         if (_grid != null)
         {
            CleanUpHelper.ReleaseControls(_grid.Controls);
            _grid.DataSource = null;
            _grid.Dispose();
         }
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }
   }
}