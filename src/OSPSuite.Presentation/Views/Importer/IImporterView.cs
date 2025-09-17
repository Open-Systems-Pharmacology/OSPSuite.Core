using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterView : IView<IImporterPresenter>
   {
      void AddImporterView(IImporterDataView importerDataView);
      void AddColumnMappingControl(IColumnMappingView columnMappingView);
      void AddPreviewView(IImportPreviewView previewView);
      void AddSourceFileControl(ISourceFileControl sourceFileControl);
      void EnablePreviewView();
      void DisableConfirmationView();
      void AddNanView(INanView nanView);
   }
}
