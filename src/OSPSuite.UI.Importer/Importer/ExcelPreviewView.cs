using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenter;
using OSPSuite.Presentation.View;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Importer
{
   public partial class ExcelPreviewView : BaseModalView, IExcelPreviewView
   {
      private IExcelPreviewPresenter _presenter;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.PreviewOriginData;

      public ExcelPreviewView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnExtra.Click += (sender, args) => setRange();
         btnOk.Click += (sender, args) => setRange();
         btnCancel.Text = Captions.Importer.Close;
         excelGridView.MouseDown += (o, e) => mouseDownEvent(e);
         btnExtra.Text = Captions.Importer.SetRange;

         setGridViewOptions();
         Caption = Captions.Importer.OriginalDataPreviewView;
         
         lblRangeSelectHint.Text = Captions.Importer.ToolTips.RangeSelect;
      }

      private void setGridViewOptions()
      {
         excelGridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         excelGridView.OptionsSelection.MultiSelect = true;
         excelGridView.OptionsBehavior.Editable = false;
         excelGridView.OptionsView.ShowGroupPanel = false;
         excelGridView.OptionsView.ShowIndicator = false;
         excelGridView.OptionsView.ShowColumnHeaders = false;
         excelGridView.OptionsView.ColumnAutoWidth = false;
      }

      private void mouseDownEvent(MouseEventArgs e)
      {
         var view = excelGridControl.GetViewAt(e.Location);
         var gridHitInfo = view.CalcHitInfo(e.Location) as GridHitInfo;
         if (gridHitInfo != null && gridHitInfo.HitTest == GridHitTest.EmptyRow)
            clearRange();
      }

      private void clearRange()
      {
         excelGridView.ClearSelection();
      }

      private void setRange()
      {
         _presenter.UpdateRange(getRange());
      }

      private Rectangle? getRange()
      {
         var largestSelectedColumnIndex = int.MinValue;
         var smallestSelectedColumnIndex = int.MaxValue;

         var largestSelectedRowIndex = int.MinValue;
         var smallestSelectedRowIndex = int.MaxValue;
         var cells = excelGridView.GetSelectedCells();
         if (!cells.Any())
            return null;

         cells.Each(cell =>
         {
            largestSelectedColumnIndex = Math.Max(cell.Column.AbsoluteIndex, largestSelectedColumnIndex);
            smallestSelectedColumnIndex = Math.Min(cell.Column.AbsoluteIndex, smallestSelectedColumnIndex);

            largestSelectedRowIndex = Math.Max(excelGridView.GetDataSourceRowIndex(cell.RowHandle), largestSelectedRowIndex);
            smallestSelectedRowIndex = Math.Min(excelGridView.GetDataSourceRowIndex(cell.RowHandle), smallestSelectedRowIndex);
         });

         return new Rectangle(
            smallestSelectedColumnIndex, 
            smallestSelectedRowIndex, 
            largestSelectedColumnIndex - smallestSelectedColumnIndex+1, 
            largestSelectedRowIndex - smallestSelectedRowIndex+1);
      }

      public void AttachPresenter(IExcelPreviewPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(DataTable exportDataTable)
      {
         excelGridControl.DataSource = exportDataTable;
         excelGridView.PopulateColumns();
      }
   }
}
