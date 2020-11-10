using System;
using DevExpress.XtraTab;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterTiledView
   {
      private IImporterTiledPresenter _presenter;

      public ImporterTiledView()
      {
         InitializeComponent();
         previewXtraTabControl.SelectedPageChanged += (s, e) => OnEvent(onSelectedPageChanged, s, e);
         //xtraTabControl.SelectedTabPage.Appearance.Header.Font = Fonts.SelectedTabHeaderFont; -- WE COULD ACTUALLY KEEP THE LOGIC OF HAVING A FONT HERE
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (previewXtraTabControl.SelectedTabPage == sourceTabPage) //not the best solution in the world this check here....
            _presenter.AddDataMappingView();
         else
            _presenter.AddConfirmationView();
      }

      public void AttachPresenter(IImporterTiledPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddImporterView(IImporterDataView importerDataView)
      {
         previewXtraTabControl.FillWith(importerDataView);
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
         if (!importTabPage.PageEnabled)
         {
            importTabPage.PageEnabled = true;
            importTabPage.PageVisible = true;
         }
      }

      public void DisableConfirmationView()
      {
         importTabPage.PageEnabled = false;
         importTabPage.PageVisible = false;
      }
   }
}