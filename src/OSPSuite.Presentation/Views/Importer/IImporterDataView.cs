using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterDataView : IView<IImporterDataPresenter>
   {
      void AddDataViewingControl(IDataViewingControl dataViewingControl);
      void AddSourceFileControl(ISourceFileControl sourceFileControl);
      void AddNanView(INanView nanView);
      void EnableImportButtons();
      void DisableImportButtons();
      void AddTabs(List<string> tabNames);
      void ClearTabs();
   }
}
