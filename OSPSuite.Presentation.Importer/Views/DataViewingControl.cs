using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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

      public void SetGridSource()
      {
         var firstSheetName = _presenter.GetSheetNames().ElementAt(0);
         gridControl1.DataSource = _presenter.GetSheet(firstSheetName); 
      }
   }
}