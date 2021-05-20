using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterView : IView<IImporterPresenter>
   {
      void AddImporterView(IImporterDataView importerDataView);
      void AddColumnMappingControl(IColumnMappingView columnMappingView);
      void AddConfirmationView(IImportConfirmationView confirmationView);
      void AddSourceFileControl(ISourceFileControl sourceFileControl);
      void EnableConfirmationView();
      void DisableConfirmationView();
      void AddNanView(INanView nanView);
   }
}
