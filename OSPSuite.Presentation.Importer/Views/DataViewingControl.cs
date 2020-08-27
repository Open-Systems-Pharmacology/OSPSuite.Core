using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;


namespace OSPSuite.Presentation.Importer.Views
{
   public partial class DataViewingControl : BaseUserControl, IDataViewingControl
   {
      private IDataViewingPresenter _presenter;
      //private readonly GridViewBinder<DataTable> _gridViewBinder;
      //private  DataTable _sheetToView = new DataTable();

      public DataViewingControl()
      {
         InitializeComponent();
         //_gridViewBinder = new GridViewBinder<DataTable>(gridView1);

      }

      public override void InitializeBinding()
      {
         base.InitializeResources();
         //_gridViewBinder.BindToSource(_sheetToView);
         //gridControl1.DataSource = _sheetToView;

      }
      public void AttachPresenter(IDataViewingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetGridSource(string tabName = null)
      {
         if (tabName == null)
            tabName = _presenter.GetSheetNames().ElementAt(0);
         //_sheetToView = _presenter.GetSheet(tabName); //in such a case do we really need to keep this as a member?
         gridControl1.DataSource = _presenter.GetSheet(tabName);
      }
   }
}