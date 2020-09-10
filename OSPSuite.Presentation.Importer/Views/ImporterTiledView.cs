using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterTiledView
   {
      private IImporterTiledPresenter _presenter;

      public ImporterTiledView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IImporterTiledPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddImporterView(IImporterView importerView)
      {
         centralPanelControl.FillWith(importerView);
      }
   }
}