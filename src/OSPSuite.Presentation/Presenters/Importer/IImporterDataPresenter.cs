using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class TabChangedEventArgs : EventArgs
   {
      public UnformattedData TabData { get; set; }
   }
   public class ImportSheetsEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }
   public class FormatChangedEventArgs : EventArgs
   {
      public IDataFormat Format { get; set; }
   }
   public interface IImporterDataPresenter : IPresenter<IImporterDataView>
   {
      void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats);

      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      event EventHandler<FormatChangedEventArgs> OnFormatChanged;

      event EventHandler<TabChangedEventArgs> OnTabChanged;

      event EventHandler<ImportSheetsEventArgs> OnImportSheets;

      event EventHandler<SourceFileChangedEventArgs> OnSourceFileChanged;

      void SetDataSource(string dataSourceFileName);
      void SelectTab(string tabName);
      void RemoveTab(string tabName);
      void RemoveAllButThisTab(string tabName);
      IEnumerable<string> GetNamingConventions();
      void ImportDataForConfirmation();
      void onMissingMapping();
      void onCompletedMapping();
      void ImportDataForConfirmation(string sheetName);
      void RefreshTabs();//should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
   }
}
