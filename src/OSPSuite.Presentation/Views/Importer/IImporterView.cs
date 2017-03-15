using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterView : IModalView<IImporterPresenter>
   {
      void StartImport(string sourceFile, ImportTableConfiguration importTableConfiguration, Mode mode);
      IList<ImportDataTable> Imports { get; }
      DialogResult ShowDialog();
      void SetIcon(ApplicationIcon icon);
      void SetNamingView(IView view);
   }
}