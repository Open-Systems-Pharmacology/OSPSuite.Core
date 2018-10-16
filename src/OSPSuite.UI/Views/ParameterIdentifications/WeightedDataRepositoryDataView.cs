using System.Data;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.ObservedData;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class WeightedDataRepositoryDataView : BaseDataRepositoryDataView<IWeightedDataRepositoryDataView, IWeightedDataRepositoryDataPresenter>, IWeightedDataRepositoryDataView
   {
      public WeightedDataRepositoryDataView(IToolTipCreator tooltipCreator) : base(tooltipCreator)
      {
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         gridView.CellValueChanged += (o, e) => OnEvent(gridViewOnCellValueChanged, e);
         gridView.ValidatingEditor += (o, e) => OnEvent(validateEditor, e);
         gridControl.Load += (o, e) => OnEvent(disableRepositoryColumns);
      }

      private void validateEditor(BaseContainerValidateEditorEventArgs e)
      {
         var validationMessages = _presenter.GetValidationMessagesForWeight(e.Value.ToString()).ToList();

         if (!validationMessages.Any()) return;

         e.Valid = false;
         e.ErrorText = validationMessages.First();
      }

      private void gridViewOnCellValueChanged(CellValueChangedEventArgs e)
      {
         _presenter.ChangeWeight(gridView.GetDataSourceRowIndex(e.RowHandle), e.Value.ConvertedTo<float>());
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemAddData.Visibility = LayoutVisibility.Never;
         emptySpaceItem.Visibility = LayoutVisibility.Never;

         gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         gridView.FocusRectStyle = DrawFocusRectStyle.RowFocus;
      }

      public override void BindTo(DataTable dataTable)
      {
         base.BindTo(dataTable);
         disableRepositoryColumns();
      }

      public void DisplayColumnReadOnly(DataColumn column)
      {
         displayColumnReadOnly(column);
      }

      public void SelectRow(int rowIndex)
      {
         gridView.Focus();
         gridView.FocusedRowHandle = gridView.GetRowHandle(rowIndex);
      }

      private void disableRepositoryColumns()
      {
         _presenter.DisableRepositoryColumns();
      }

      private void displayColumnReadOnly(DataColumn column)
      {
         column.ReadOnly = true;
         var gridViewColumn = gridViewColumnFromDataColumn(column);
         if (gridViewColumn == null)
            return;
         gridViewColumn.OptionsColumn.AllowEdit = false;
         gridViewColumn.AppearanceCell.BackColor = Colors.Disabled;
      }

      private GridColumn gridViewColumnFromDataColumn(DataColumn column)
      {
         return gridView.Columns[column.ColumnName];
      }
   }
}