using System;
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
         navigationTileBar.ItemClick += onTileBarClicked;
      }

      private void onTileBarClicked(object sender, EventArgs e)
      {
         var eventArgs = e as DevExpress.XtraEditors.TileItemEventArgs;
         if (eventArgs.Item == dataMappingTile)
            _presenter.AddDataMappingView();
         else if (eventArgs.Item == confirmationTile)
            _presenter.AddConfirmationView();
      }

      public void AttachPresenter(IImporterTiledPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddImporterView(IImporterView importerView)
      {
         centralPanelControl.FillWith(importerView);
      }

      public void AddConfirmationView(IImportConfirmationView confirmationView)
      {
         centralPanelControl.FillWith(confirmationView);
      }

      public void EnableConfirmationView()
      {
         if (!confirmationTile.Enabled)
            confirmationTile.Enabled = true;
      }
   }
}