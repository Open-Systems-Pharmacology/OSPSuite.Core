using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterView
   {
      private IImporterPresenter _presenter;

      public ImporterView()
      {
         InitializeComponent();
         previewXtraTabControl.SelectedPageChanged += (s, e) => OnEvent(onSelectedPageChanged, s, e);
         sourceFileLayoutControlItem.Name = Captions.Importer.SourceLayout;
         previewLayoutControlItem.Name = Captions.Importer.PreviewLayout;
         columnMappingLayoutControlItem.Name = Captions.Importer.MappingName;
         nanLayoutControlItem.AdjustControlHeight(80);
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (previewXtraTabControl.SelectedTabPage == sourceTabPage) //not the best solution in the world this check here....
            _presenter.AddDataMappingView();
         else
            _presenter.AddConfirmationView();
      }

      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddImporterView(IImporterDataView importerDataView)
      {
         previewXtraTabControl.FillWith(importerDataView);
      }

      public void AddSourceFileControl(ISourceFileControl sourceFileControl)
      {
         sourceFilePanelControl.FillWith(sourceFileControl);
      }

      public void AddColumnMappingControl(IColumnMappingControl columnMappingControl)
      {
         columnMappingPanelControl.FillWith(columnMappingControl);
      }
      public void AddConfirmationView(IImportConfirmationView confirmationView)
      {
         previewXtraTabControl.FillWith(confirmationView);
      }
      public void EnableConfirmationView()
      {
         if (!confirmationTabPage.PageEnabled)
         {
            confirmationTabPage.PageEnabled = true;
            confirmationTabPage.PageVisible = true;
            previewXtraTabControl.SelectedTabPage = confirmationTabPage;
         }
      }

      public void DisableConfirmationView()
      {
         confirmationTabPage.PageEnabled = false;
         confirmationTabPage.PageVisible = false;
      }

      public void AddNanView(INanView nanView)
      {
         nanPanelControl.FillWith(nanView);
      }
   }
}