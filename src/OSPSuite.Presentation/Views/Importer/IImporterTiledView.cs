using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterTiledView : IView<IImporterTiledPresenter>
   {
      void AddImporterView(IImporterDataView importerDataView);
      void AddColumnMappingControl(IColumnMappingControl columnMappingControl);
      void AddConfirmationView(IImportConfirmationView confirmationView);
      void EnableConfirmationView();
      void DisableConfirmationView();
   }
}
