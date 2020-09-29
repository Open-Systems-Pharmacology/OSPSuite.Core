using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Importer.Core;
using System.Collections.Generic;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImporterPresenter : IPresenter<IImporterView>
   {
      void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats);

      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      event FormatChangedHandler OnFormatChanged;

      event ImportSingleSheetHandler OnImportSingleSheet;
      
      event ImportAllSheetsHandler OnImportAllSheets;

      event ImportTriggeredHandler OnTriggerImport;
      void SetDataSource(string dataSourceFileName);
      void SelectTab(string tabName);
      void RemoveTab(string tabName);
      void RemoveAllButThisTab(string tabName);
      void GetDataForImport(out string fileName, out IDataFormat format, out IReadOnlyList<ColumnInfo> columnInfos, out IEnumerable<string> namingConventions, out IEnumerable<MetaDataMappingConverter> mappings);
      IReadOnlyDictionary<string, IDataSheet> GetAllSheets();
      IDataSheet GetSingleSheet(string sheetName);
      void ImportDataForConfirmation();
      void ImportDataForConfirmation(string sheetName);
      void SetNewFormat(string formatName);
      void RefreshTabs();//should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
   }

   public delegate void FormatChangedHandler(string format);

   public delegate void ImportTriggeredHandler( IDataSource dataSource);

   public delegate void ImportSingleSheetHandler(string sheetName);

   public delegate void ImportAllSheetsHandler();
}
