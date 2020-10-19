using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterTiledView : IView<IImporterTiledPresenter>
   {
      void AddImporterView(IImporterView importerView);
      void AddConfirmationView(IImportConfirmationView confirmationView);
      void EnableConfirmationView();
   }
}
