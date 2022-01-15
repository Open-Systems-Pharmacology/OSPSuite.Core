using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Utility.Collections;

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
      string SelectedTab { get; set; }
      string GetFilter();
      void SetTabMarks(Cache<IDataSet, List<ParseErrorDescription>> errors, IEnumerable<string> loadedSheets);
   }
}
