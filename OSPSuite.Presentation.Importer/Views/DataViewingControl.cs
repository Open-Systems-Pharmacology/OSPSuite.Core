using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class DataViewingControl : BaseUserControl, IDataViewingControl
   {
      public DataViewingControl()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IDataViewingPresenter presenter)
      {
         //throw new System.NotImplementedException();
      }
   }
}