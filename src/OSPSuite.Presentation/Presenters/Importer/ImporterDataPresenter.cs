using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{ 
   class ImporterDataPresenter : AbstractPresenter<IImporterDataView, IImporterDataPresenter>, IImporterDataPresenter
   {
      private readonly IImporter _importer;
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      public Cache<string, IDataSheet> Sheets { get; set; }

      public event EventHandler<FormatChangedEventArgs> OnFormatChanged = delegate { };
      public event EventHandler<TabChangedEventArgs> OnTabChanged;

      public event EventHandler<ImportSheetsEventArgs> OnImportSheets = delegate { };

      public ImporterDataPresenter
      (
         IImporterDataView dataView,
         IImporter importer
      ) : base(dataView)
      {
         _importer = importer;
         Sheets = new Cache<string, IDataSheet>();
      }

      public List<string> GetSheetNames()
      {
         return _dataSourceFile.DataSheets.Keys.ToList();
      }
      public DataTable GetSheet(string tabName)
      {
         return _dataSourceFile.DataSheets.Contains(tabName) ? _dataSourceFile.DataSheets[tabName].RawData.AsDataTable() : new DataTable();
      }
      public void ImportDataForConfirmation()
      {
         var sheets = new Cache<string, IDataSheet>();
         foreach (var element in _dataSourceFile.DataSheets.KeyValues)
         {
            if (!Sheets.Keys.Contains(element.Key))
            {
               Sheets.Add(element.Key, element.Value);
               sheets.Add(element.Key, element.Value);
            }
         }

         if (sheets.Count == 0) return;

         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets });
      }

      public void onMissingMapping()
      {
         View.DisableImportButtons();
      }

      public void onCompletedMapping()
      {
         View.EnableImportButtons();
      }

      public void ImportDataForConfirmation(string sheetName)
      {
         var sheets = new Cache<string, IDataSheet>();

         if (!Sheets.Keys.Contains(sheetName))
         {
            Sheets.Add(sheetName, getSingleSheet(sheetName));
            sheets.Add(sheetName, getSingleSheet(sheetName));
         }
         if (sheets.Count == 0) return;

         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets});
      }

      private IDataSheet getSingleSheet(string sheetName)
      {
         return _dataSourceFile.DataSheets[sheetName];
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         OnFormatChanged.Invoke(this, new FormatChangedEventArgs() {Format = format});
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos)
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      public IDataSourceFile SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return null;
         Sheets = new Cache<string, IDataSheet>();
         _dataSourceFile = _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);
         View.SetGridSource();
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(GetSheetNames());
         View.ResetImportButtons();
         return _dataSourceFile;
      }

      public void SelectTab(string tabName)
      {
         OnTabChanged.Invoke(this, new TabChangedEventArgs() { TabData = _dataSourceFile.DataSheets[tabName].RawData });
         View.SetGridSource(tabName);
      }

      public void RemoveTab(string tabName)
      {
         _dataSourceFile.DataSheets.Remove(tabName);
      }

      public void RemoveAllButThisTab(string tabName)
      {
         View.ClearTabs();
         var remainingSheet = _dataSourceFile.DataSheets[tabName];
         _dataSourceFile.DataSheets.Clear();
         _dataSourceFile.DataSheets.Add(tabName, remainingSheet);
         View.AddTabs(GetSheetNames());
      }

      public void RefreshTabs()
      {
         View.ClearTabs();
         View.AddTabs(GetSheetNames());
      }
      public void DisableImportedSheets()
      {
         View.DisableImportCurrentSheet();

         if (Sheets.Keys.All(GetSheetNames().Contains) && GetSheetNames().Count == Sheets.Keys.Count())
            View.DisableImportAllSheets();
      }
   }
}