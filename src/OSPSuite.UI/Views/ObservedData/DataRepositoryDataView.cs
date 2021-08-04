using System.Data;
using System.Linq;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Core.Commands;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class DataRepositoryDataView : BaseDataRepositoryDataView<IDataRepositoryDataView, IDataRepositoryDataPresenter>, IDataRepositoryDataView
   {
      protected readonly GridViewColumnUnitsMenuBinder<int> _columnUnitsMenuBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly GridColumnCreator _creator = new GridColumnCreator();
      private DataTable _dataTable;
      private GridColumn _removeColumn;
      private bool _editable = true;

      public DataRepositoryDataView(IToolTipCreator tooltipCreator) : base(tooltipCreator)
      {
         InitializeComponent();
         _columnUnitsMenuBinder = new GridViewColumnUnitsMenuBinder<int>(gridView, col => col.AbsoluteIndex);
      }

      public void DisableEdition()
      {
         btnAddData.Enabled = false;
         layoutItemAddData.Visibility = LayoutVisibility.Never;
         emptySpaceItem.Visibility = LayoutVisibility.Never;
         gridView.OptionsBehavior.Editable = false;
         if (_removeColumn != null)
         {
            gridView.Columns.Remove(_removeColumn);
         }
         gridView.OptionsView.ShowIndicator = false;

         _editable = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         gridView.ValidatingEditor += (o, e) => OnEvent(gridEditorValidating, e);
         gridView.CellValueChanged += (o, e) => OnEvent(gridViewOnCellValueChanged, e);
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(_presenter.RemoveData, gridView.GetDataSourceRowIndex(gridView.FocusedRowHandle));
         btnAddData.Click += (o, e) => OnEvent(_presenter.AddRow);
      }

      public override void BindTo(DataTable dataTable)
      {
         _dataTable = dataTable;
         base.BindTo(dataTable);
         _columnUnitsMenuBinder.BindTo(_presenter);
         createDataRemoveColumn();
      }

      private void createDataRemoveColumn()
      {
         if (!_editable)
            return;

         _removeColumn = _creator.CreateFor<DataRowData>(UIConstants.EMPTY_COLUMN, gridView);
         _removeColumn.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
         _removeColumn.ColumnEdit = _removeButtonRepository;
         _removeColumn.Width = UIConstants.Size.EMBEDDED_BUTTON_WIDTH;
         _removeColumn.OptionsColumn.FixedWidth = true;
      }

      private void gridEditorValidating(BaseContainerValidateEditorEventArgs e)
      {
         var rowIndex = gridView.GetFocusedDataSourceRowIndex();
         var columnIndex = gridView.FocusedColumn.AbsoluteIndex;

         var validationFailureMessages = _presenter.GetCellValidationErrorMessages(rowIndex, columnIndex, e.Value.ToString()).ToList();

         if (!validationFailureMessages.Any()) return;

         e.Valid = false;
         e.ErrorText = validationFailureMessages.First();
      }

      private void gridViewOnCellValueChanged(CellValueChangedEventArgs e)
      {
         if (_presenter.NumberOfObservations == _dataTable.Rows.Count)
            eventIsForSet(e);
         else
            eventIsForAdd(e);
      }

      private void eventIsForAdd(CellValueChangedEventArgs e)
      {
         var rowIndex = gridView.GetDataSourceRowIndex(e.RowHandle);
         _presenter.AddData(rowIndex);
      }

      private void eventIsForSet(CellValueChangedEventArgs e)
      {
         var oldValue = gridView.ActiveEditor.OldEditValue.ConvertedTo<float>();
         var newValue = e.Value.ConvertedTo<float>();
         _presenter.ValueIsSet(new CellValueChangedDTO
         {
            NewDisplayValue = newValue,
            OldDisplayValue = oldValue,
            RowIndex = gridView.GetDataSourceRowIndex(e.RowHandle),
            ColumnIndex = e.Column.AbsoluteIndex
         });
      }
   }
}
