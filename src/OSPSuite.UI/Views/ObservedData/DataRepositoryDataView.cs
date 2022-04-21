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
      private readonly GridColumnCreator _creator = new GridColumnCreator();
      private DataTable _dataTable;

      public DataRepositoryDataView(IToolTipCreator tooltipCreator) : base(tooltipCreator)
      {
         InitializeComponent();
         _columnUnitsMenuBinder = new GridViewColumnUnitsMenuBinder<int>(gridView, col => col.AbsoluteIndex);
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
