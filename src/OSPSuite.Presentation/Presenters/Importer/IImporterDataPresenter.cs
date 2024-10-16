﻿using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class TabChangedEventArgs : EventArgs
   {
      public DataSheet TabSheet { get; set; }
   }

   public class ImportSheetsEventArgs : EventArgs
   {
      public IDataSourceFile DataSourceFile { get; set; }
      public IReadOnlyList<string> SheetNames { get; set; }
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
         ColumnInfoCache columnInfos
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
      void OnMissingMapping();
      void OnCompletedMapping();
      void DisableImportedSheets();
      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
      void ImportDataForConfirmation(string sheetName);

      //should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
      void RefreshTabs();
      string GetActiveFilterCriteria();
      string GetFilter();
      void TriggerOnDataChanged();
      void SetFilter(string filterString);
      void GetFormatBasedOnCurrentSheet();
      void ResetLoadedSheets();
      void SetTabMarks(ParseErrors errors, Cache<string, IDataSet> loadedDataSets);
      void SetTabMarks(ParseErrors errors);
      DataSheetCollection ImportedSheets { get; set; }
   }

   public class TabMarkInfo
   {
      public string ErrorMessage { get; }
      public bool IsLoaded { get; }

      public bool ContainsError => ErrorMessage != null;

      public TabMarkInfo(string errorMessage, bool isLoaded)
      {
         ErrorMessage = errorMessage;
         IsLoaded = isLoaded;
      }
   }
}