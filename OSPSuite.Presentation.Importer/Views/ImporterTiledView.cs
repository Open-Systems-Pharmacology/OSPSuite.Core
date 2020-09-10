using OSPSuite.Presentation.Importer.Presenters;

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
   }
}