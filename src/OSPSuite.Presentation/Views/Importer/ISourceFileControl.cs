using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ISourceFileControl : IView<ISourceFilePresenter>
   {
      void SetFilePath(string filePath);
   }
}