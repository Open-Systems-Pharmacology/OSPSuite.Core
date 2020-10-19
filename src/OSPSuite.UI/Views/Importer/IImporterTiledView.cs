using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public interface IImporterTiledView : IView<IImporterTiledPresenter>
   {
      void AddImporterView(IImporterView importerView);
      void AddConfirmationView(IImportConfirmationView confirmationView);
      void EnableConfirmationView();
   }
}
