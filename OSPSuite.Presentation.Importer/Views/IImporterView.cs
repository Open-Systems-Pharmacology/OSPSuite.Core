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

      void SetNamingConventions(IEnumerable<string> options, string selected = null);

      event FormatChangedHandler OnFormatChanged;

      event NamingConventionChangedHandler OnNamingConventionChanged;

      event TabChangedHandler OnTabChanged;

      event ImportSingleSheetHandler OnImportSingleSheet;

      event ImportAllSheetsHandler OnImportAllSheets;

      void AddTabs(List<string> tabNames);
      void ClearTabs();
   }

   public delegate void ImportSingleSheetHandler(string sheetName);

   public delegate void ImportAllSheetsHandler();

   public delegate void TabChangedHandler(string format);

   public delegate void NamingConventionChangedHandler(string namingConvention);

}
