using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterView : IView<IImporterPresenter>
   {
      void AddDataViewingControl(IDataViewingControl dataViewingControl);
      void AddColumnMappingControl(IColumnMappingControl columnMappingControl);
      void AddSourceFileControl(ISourceFileControl sourceFileControl);
      void AddNanView(INanView nanView);
      void SetFormats(IEnumerable<string> options, string selected, string description);
      void EnableImportButtons();
      void DisableImportButtons();
      void AddTabs(List<string> tabNames);
      void ClearTabs();
   }
}
