using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter;
      public ImporterView()
      {
         InitializeComponent();
      }

      public void AddColumnMappingControl(IColumnMappingControl columnMappingControl)
      {
         columnMappingPanelControl.FillWith(columnMappingControl);
      }

      public void AddDataViewingControl(IDataViewingControl dataViewingControl)
      {
         dataViewingPanelControl.FillWith(dataViewingControl);
      }

      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}