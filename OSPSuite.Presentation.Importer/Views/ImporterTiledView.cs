using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System;

namespace OSPSuite.Presentation.Importer.Views
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
         if (eventArgs.Item.Name == "dataMappingTile")
            _presenter.AddDataMappingView();
         else if (eventArgs.Item.Name == "confirmationTile")
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
   }
}