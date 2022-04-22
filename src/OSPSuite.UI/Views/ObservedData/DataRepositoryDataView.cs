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

      public override void BindTo(DataTable dataTable)
      {
         _dataTable = dataTable;
         base.BindTo(dataTable);
         _columnUnitsMenuBinder.BindTo(_presenter);
      }
   }
}