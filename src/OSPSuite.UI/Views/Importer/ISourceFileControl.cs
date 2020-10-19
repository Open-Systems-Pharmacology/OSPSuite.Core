using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views.Importer
{
   public interface ISourceFileControl : IView<ISourceFilePresenter>
   {
      void SetFilePath(string filePath);
   }
}