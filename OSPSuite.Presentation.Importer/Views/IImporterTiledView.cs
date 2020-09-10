using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Importer.Presenters;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImporterTiledView : IView<IImporterTiledPresenter>
   {
      void AddImporterView(IImporterView importerView);
   }
}
