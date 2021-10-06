using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IModalImporterView : IModalView<IModalImporterPresenter>
   {
      void FillImporterPanel(IView view);
      void CloseOnImport();
   }
}
