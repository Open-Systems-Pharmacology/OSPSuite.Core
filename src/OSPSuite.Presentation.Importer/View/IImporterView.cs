using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.DeprecatedImporter.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.DeprecatedImporter.View
{
   public interface IImporterView : IModalView<IImporterPresenter>
   {
      void StartImport(string sourceFile, ImportTableConfiguration importTableConfiguration, Mode mode);
      IList<ImportDataTable> Imports { get; }
      void SetIcon(ApplicationIcon icon);
      void SetNamingView(IView view);
   }
}