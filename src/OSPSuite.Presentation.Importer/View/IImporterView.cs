using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.View
{
   public interface IImporterView : IModalView<IImporterPresenter>
   {
      void StartImport(string sourceFile, ImportTableConfiguration importTableConfiguration, Mode mode);
      IList<ImportDataTable> Imports { get; }
      void SetIcon(ApplicationIcon icon);
      void SetNamingView(IView view);
   }
}