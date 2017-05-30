using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImportDataTableGridView : BaseUserControl, IImportDataTableGridView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IToolTipCreator _toolTipCreator;
      private IImportDataTableGridPresenter _presenter;

      public ImportDataTableGridView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _imageListRetriever = imageListRetriever;
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
      }

      /// <summary>
      /// This method opens an edit dialog for meta data.
      /// </summary>
      /// <param name="metaData">Meta data to be edited.</param>
      /// <param name="column">Grid column of column the meta data belong to.</param>
      private void openEditMetaDataForColumn(MetaDataTable metaData, GridColumn column)
      {
         var icon = (ParentForm == null) ? ApplicationIcons.EmptyIcon : ParentForm.Icon;
         var frm = new EditMetaDataView(metaData) { StartPosition = FormStartPosition.CenterParent, Icon = icon };
         frm.OnCopyMetaData += (o, e) => OnEvent(() => onCopyMetaDataColumnControl(metaData, column));
         if (frm.ShowDialog() == DialogResult.OK)
            _presenter.SetMetaDataForColumn(metaData, column.FieldName);
      }

      private void onCopyMetaDataColumnControl(MetaDataTable metaData, GridColumn column)
      {
         OnEvent(() => _presenter.RaiseCopyMetaDataColumnControlEvent(metaData, column.FieldName));
      }

      /// <summary>
      /// This event handler is used to react when a context menu on a grid column is requested.
      /// </summary>
      private void onPopUpMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.MenuType != GridMenuType.Column) return;

         var menu = e.Menu as GridViewColumnMenu;
         if (menu?.Column == null) return;
         var columnName = menu.Column.FieldName;

         var view = sender as GridView;

         var table = view?.GridControl.DataSource as ImportDataTable;
         if (table == null) return;
         if (!table.Columns.ContainsName(columnName)) return;

         var col = table.Columns.ItemByName(columnName);
         foreach (DXMenuItem item in menu.Items)
         {
            if (item.Tag.ToString() != DevExpress.XtraGrid.Localization.GridStringId.MenuColumnRemoveColumn.ToString() &&
               item.Tag.ToString() != DevExpress.XtraGrid.Localization.GridStringId.MenuColumnColumnCustomization.ToString()) continue;
            item.Enabled = false;
         }
         menu.Items[0].BeginGroup = true;
         menu.Items.Insert(0, new DXMenuItem("Enter Unit Information", onSetUnitClick)
         {
            Visible = col.Dimensions != null && col.HasRequiredInputParameters,
            Tag = menu.Column,
            BeginGroup = false,
            Image = ApplicationIcons.UnitInformation.ToImage()
         });
         menu.Items.Insert(0, new DXMenuItem("Set Unit", onSetUnitClick)
         {
            Visible = col.Dimensions != null && !col.HasRequiredInputParameters,
            Tag = menu.Column,
            BeginGroup = false,
            Image = ApplicationIcons.UnitInformation.ToImage()
         });
         menu.Items.Insert(0, new DXMenuItem("Edit Meta Data", onEditMetaDataClick)
         {
            Enabled = (col.MetaData != null),
            Tag = menu.Column,
            BeginGroup = false,
            Image = ApplicationIcons.MetaData.ToImage()
         });
      }

      private void onSetUnitClick(object sender, EventArgs e)
      {
         _presenter.RaiseSetUnitEvent(sender, e);
      }

      /// <summary>
      /// This event handler is used react on menu item edit meta data clicks.
      /// </summary>
      private void onEditMetaDataClick(object sender, EventArgs e)
      {
         var item = sender as DXMenuItem;

         var col = item?.Tag as GridColumn;

         var table = col?.View.GridControl.DataSource as ImportDataTable;
         if (table == null) return;
         if (!table.Columns.ContainsName(col.FieldName)) return;

         var metaData = table.Columns.ItemByName(col.FieldName).MetaData;
         if (metaData == null) return;
         openEditMetaDataForColumn(metaData, col);
      }

      /// <summary>
      /// This event handler is used to react on mouse double clicks on columns to open the edit dialog for meta data.
      /// </summary>
      private void onGridMouseDoubleClick(object sender, MouseEventArgs e)
      {
         var grid = sender as UxGridControl;

         var hitInfo = grid?.MainView.CalcHitInfo(e.Location) as GridHitInfo;
         if (hitInfo == null) return;

         if (hitInfo.InColumn)
         {
            var table = grid.DataSource as ImportDataTable;
            if (table == null) return;
            if (table.Columns.ContainsName(hitInfo.Column.FieldName))
            {
               var col = table.Columns.ItemByName(hitInfo.Column.FieldName);
               if (col.MetaData != null)
                  openEditMetaDataForColumn(col.MetaData, hitInfo.Column);
            }
         }
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         gridView.PopupMenuShowing += onPopUpMenuShowing;
         gridControl.MouseDoubleClick += onGridMouseDoubleClick;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         gridView.Images = _imageListRetriever.AllImages16x16;

         gridView.OptionsView.ShowGroupPanel = false;
         gridView.OptionsView.ColumnAutoWidth = false;
         gridView.OptionsCustomization.AllowSort = false;
         gridView.OptionsCustomization.AllowFilter = false;
         gridView.OptionsCustomization.AllowQuickHideColumns = false;

         gridView.RowStyle += (sender, args) => OnEvent(() => highlightRowsBelowLowerLimitOfQuantification(args));
      }

      private void highlightRowsBelowLowerLimitOfQuantification(RowStyleEventArgs args)
      {
         var sourceRow = gridView.GetDataSourceRowIndex(args.RowHandle);

         var table = gridControl.DataSource as DataTable;

         if (table == null) return;

         var color = _presenter.GetBackgroundColorForRow(sourceRow);
         if(color != _presenter.DefaultBackgroundColorForRow)
            gridView.AdjustAppearance(args, color);
      }

      public void SetInputParametersForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName)
      {
         if (!_presenter.SetParametersInTableForColumn(inputParameters, dimension, columnName)) return;

         var gridColumn = gridView.Columns.ColumnByFieldName(columnName);
         setColumnImage(gridColumn);
      }

      public void SetUnitInformationForColumn(Dimension dimension, Unit unit, string columnName)
      {
         if (!_presenter.SetUnitInTableForColumnName(dimension, unit, columnName)) return;

         foreach (GridColumn gridCol in gridView.Columns)
         {
            setColumnImage(gridCol);
            var tableCol = _presenter.TableColumnByName(gridCol.FieldName);
            gridCol.Caption = tableCol.GetCaptionForColumn();
            gridCol.BestFit();
         }
      }

      /// <summary>
      /// This event handler is used for creating tool tips for column headers.
      /// </summary>
      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var grid = e.SelectedControl as UxGridControl;

         var view = grid?.GetViewAt(e.ControlMousePosition) as GridView;

         var hi = view?.CalcHitInfo(e.ControlMousePosition);
         if (hi == null) return;

         if (hi.HitTest == GridHitTest.Column)
         {
            tooltipForColumn(e, hi);
         }
         if (hi.InDataRow)
         {
            tooltipForRow(e, hi.RowHandle);
         }
      }

      private void tooltipForRow(ToolTipControllerGetActiveObjectInfoEventArgs eventArgs, int rowHandle)
      {
         var table = gridControl.DataSource as ImportDataTable;
         if (table == null) return;

         var lowerLimitOfQuantificationToolTipText = _presenter.GetLowerLimitOfQuantificationToolTipTextForRow(gridView.GetDataSourceRowIndex(rowHandle));
         if (string.IsNullOrEmpty(lowerLimitOfQuantificationToolTipText)) return;

         eventArgs.Info = new ToolTipControlInfo(rowHandle, lowerLimitOfQuantificationToolTipText)
         {
            SuperTip = _toolTipCreator.CreateToolTip(lowerLimitOfQuantificationToolTipText, Captions.LLOQ),
            ToolTipType = ToolTipType.SuperTip
         };
      }

      private void tooltipForColumn(ToolTipControllerGetActiveObjectInfoEventArgs e, GridHitInfo hi)
      {
         var table = gridControl.DataSource as ImportDataTable;
         if (table == null) return;

         e.Info = new ToolTipControlInfo(hi.Column, "")
         {
            SuperTip =
               _presenter.GetToolTipForImportDataColumn(
                  table.Columns.ItemByIndex(hi.Column.ColumnHandle)),
            ToolTipType = ToolTipType.SuperTip
         };
      }

      public void AttachPresenter(IImportDataTableGridPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ImportDataTable table)
      {
         gridControl.DataSource = table;
         gridControl.BindingContext = new BindingContext();
         gridControl.ForceInitialize();
         foreach (GridColumn col in gridView.Columns)
         {
            col.OptionsColumn.AllowEdit = false;
            setColumnImage(col);
            var tableColumn = table.Columns.ItemByName(col.FieldName);
            col.Caption = tableColumn.GetCaptionForColumn();
            col.Visible = !string.IsNullOrEmpty(tableColumn.Source);
         }
         gridView.BestFitColumns();

         gridControl.ToolTipController = new ToolTipController();
         gridControl.ToolTipController.GetActiveObjectInfo += (o,e) => OnEvent(() => onToolTipControllerGetActiveObjectInfo(o,e));
      }

      /// <summary>
      /// This method sets the image of the given grid column depending on fill state and requirements.
      /// </summary>
      public void SetColumnImage(string columnName)
      {
         var column = gridView.Columns.ColumnByFieldName(columnName);
         setColumnImage(column);
      }

      private void setColumnImage(GridColumn column)
      {
         if (_presenter.TableColumnByName(column.FieldName) == null) return;

         column.ImageIndex = _presenter.GetImageIndexForColumnName(column.FieldName);
         column.ImageAlignment = StringAlignment.Far;
      }

      public void Clear()
      {
         gridControl.DataSource = null;
         CleanUpHelper.ReleaseControls(Controls);
         Dispose();
      }

      public void SetUnitForColumn(ImportDataTable table)
      {
         foreach (GridColumn gridCol in gridView.Columns)
         {
            setColumnImage(gridCol);
            gridCol.Caption = _presenter.GetCaptionForColumnByName(gridCol.FieldName);
            gridCol.BestFit();
         }
      }

      public void ReflectMetaDataChangesForColumn(ImportDataColumn column)
      {
         foreach (GridColumn gc in gridView.Columns)
         {
            if (_presenter.TableColumnByName(gc.FieldName) == null) continue;
            setColumnImage(gc);
            gc.Caption = _presenter.GetCaptionForColumnByName(gc.FieldName);
            gc.BestFit();
         }
      }
   }
}
