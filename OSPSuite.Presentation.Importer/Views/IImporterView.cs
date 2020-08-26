using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImporterView : IView<IImporterPresenter>
   {
      void AddDataViewingControl(IDataViewingControl dataViewingControl);
      void AddColumnMappingControl(IColumnMappingControl columnMappingControl);
      void AddSourceFileControl(ISourceFileControl sourceFileControl);

      void SetFormats(IEnumerable<string> options, string selected);

      event FormatChangedHandler OnFormatChanged;

      event TabChangedHandler OnTabChanged;

      void AddTabs(List<string> tabNames);
   }

   public delegate void TabChangedHandler(string format);

}
