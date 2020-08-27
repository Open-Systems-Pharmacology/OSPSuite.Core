using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface ISourceFileControl : IView<ISourceFilePresenter>
   {
      void SetFilePath(string filePath);
   }
}