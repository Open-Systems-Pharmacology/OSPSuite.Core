using System.Linq;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class DataViewingControl : BaseUserControl, IDataViewingControl
   {
      private IDataViewingPresenter _presenter;

      public DataViewingControl()
      {
         InitializeComponent();

      }

      public override void InitializeBinding()
      {
         base.InitializeResources();
      }
      public void AttachPresenter(IDataViewingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetGridSource(string tabName = null)
      {
         dataViewingGridControl.DataSource = null;
         dataViewingGridView.Columns.Clear();

         //should this logic be moved to presenter?
         //======= presentation notes
         if (tabName == null)
            tabName = _presenter.GetSheetNames().ElementAt(0);
         //========================================

         dataViewingGridControl.DataSource = _presenter.GetSheet(tabName);
      }
   }
}