using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class TabChangedEventArgs : EventArgs
   {
      public UnformattedData TabData { get; set; }
   }

   public class ImportSheetsEventArgs : EventArgs
   {
      public IDataSourceFile DataSourceFile { get; set; }
      public Cache<string, IDataSheet> Sheets { get; set; }
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
         IReadOnlyList<ColumnInfo> columnInfos
      );

      event EventHandler<FormatChangedEventArgs> OnFormatChanged;

      event EventHandler<TabChangedEventArgs> OnTabChanged;

      event EventHandler<ImportSheetsEventArgs> OnImportSheets;

      IDataSourceFile SetDataSource(string dataSourceFileName);
      void SelectTab(string tabName);
      void RemoveTab(string tabName);
      void RemoveAllButThisTab(string tabName);
      void ImportDataForConfirmation();
      void onMissingMapping();
      void onCompletedMapping();
      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
      void ImportDataForConfirmation(string sheetName);

      //should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
      void RefreshTabs(); 

      Cache<string, IDataSheet> Sheets { get; }
   }
}