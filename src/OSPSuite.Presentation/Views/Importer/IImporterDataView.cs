using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterDataView : IView<IImporterDataPresenter>
   {
      void SetGridSource(string tabName = null);
      void EnableImportButtons();
      void DisableImportButtons();
      void AddTabs(List<string> tabNames);
      void ClearTabs();
      void DisableImportCurrentSheet();
      void DisableImportAllSheets();
      void ResetImportButtons();
      string GetActiveFilterCriteria();

      void SetFilter(string filter);
   }
}
