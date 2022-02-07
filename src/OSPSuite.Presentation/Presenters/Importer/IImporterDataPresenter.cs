using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
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
      public Cache<string, DataSheet> Sheets { get; set; }
      public string Filter { get; set; }
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

      event EventHandler<EventArgs> OnDataChanged;

      IDataSourceFile SetDataSource(string dataSourceFileName);
      bool SelectTab(string tabName);
      void RemoveTab(string tabName);
      void ReopenAllSheets();
      void RemoveAllButThisTab(string tabName);
      void ImportDataForConfirmation();
      void onMissingMapping();
      void onCompletedMapping();
      void DisableImportedSheets();
      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
      void ImportDataForConfirmation(string sheetName);

      //should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
      void RefreshTabs();
      Cache<string, DataSheet> Sheets { get; set; }
      string GetActiveFilterCriteria();
      string GetFilter();
      void TriggerOnDataChanged();
      void SetFilter(string FilterString);
      void GetFormatBasedOnCurrentSheet();
      void ResetLoadedSheets();
      void SetTabMarks(ParseErrors errors, Cache<string, IDataSet> loadedDataSets);
      void SetTabMarks(ParseErrors errors);
   }
}
