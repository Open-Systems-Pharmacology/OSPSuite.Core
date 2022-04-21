using System.Data;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class DataRepositoryDataView : BaseDataRepositoryDataView<IDataRepositoryDataView, IDataRepositoryDataPresenter>,
      IDataRepositoryDataView
   {
      protected readonly GridViewColumnUnitsMenuBinder<int> _columnUnitsMenuBinder;
      private DataTable _dataTable;

      public DataRepositoryDataView(IToolTipCreator tooltipCreator) : base(tooltipCreator)
      {
         InitializeComponent();
         _columnUnitsMenuBinder = new GridViewColumnUnitsMenuBinder<int>(gridView, col => col.AbsoluteIndex);
         gridView.OptionsBehavior.Editable = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         gridView.ValidatingEditor += (o, e) => OnEvent(gridEditorValidating, e);
      }

      public override void BindTo(DataTable dataTable)
      {
         _dataTable = dataTable;
         base.BindTo(dataTable);
         _columnUnitsMenuBinder.BindTo(_presenter);
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
   }
}