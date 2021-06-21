using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Importer
{ 
   public class ImporterDataPresenter : AbstractPresenter<IImporterDataView, IImporterDataPresenter>, IImporterDataPresenter
   {
      private readonly IImporter _importer;
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly Cache<string, DataTable> _sheetsForViewing;
      public Cache<string, DataSheet> Sheets { get; set; }

      public event EventHandler<FormatChangedEventArgs> OnFormatChanged = delegate { };
      public event EventHandler<TabChangedEventArgs> OnTabChanged = delegate { };

      public event EventHandler<ImportSheetsEventArgs> OnImportSheets = delegate { };
      public event EventHandler<EventArgs> OnDataChanged = delegate { };

      public ImporterDataPresenter
      (
         IImporterDataView dataView,
         IImporter importer) : base(dataView)
      {
         _importer = importer;
         _sheetsForViewing = new Cache<string, DataTable>();
         Sheets = new Cache<string, DataSheet>();
      }

      public List<string> GetSheetNames()
      {
         return _dataSourceFile.DataSheets.Keys.ToList();
      }
      public DataTable GetSheet(string tabName)
      {
         return _sheetsForViewing.Contains(tabName) ? _sheetsForViewing[tabName] : new DataTable();
      }
      public void ImportDataForConfirmation()
      {
         var sheets = new Cache<string, DataSheet>();
         foreach (var element in _dataSourceFile.DataSheets.KeyValues)
         {
            if (Sheets.Keys.Contains(element.Key)) 
               continue;

            Sheets.Add(element.Key, element.Value);
            sheets.Add(element.Key, element.Value);
         }

         if (sheets.Count == 0) 
            return;

         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets, Filter = GetActiveFilterCriteria() });
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
         var sheets = new Cache<string, DataSheet>();
         if (!Sheets.Keys.Contains(sheetName))
         {
            Sheets.Add(sheetName, getSingleSheet(sheetName));
            sheets.Add(sheetName, getSingleSheet(sheetName));
         }
         if (sheets.Count == 0) 
            return;

         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets, Filter = GetActiveFilterCriteria()});
      }

      public string GetFilter()
      {
         return  _view.GetFilter();
      }

      public void TriggerOnDataChanged()
      {
         OnDataChanged.Invoke(this, null);
      }

      public void SetFilter(string FilterString)
      {
         _view.SetFilter(FilterString);
      }

      private DataSheet getSingleSheet(string sheetName)
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
         Sheets = new Cache<string, DataSheet>();
         _dataSourceFile = _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);

         if (_dataSourceFile == null)
            return null;
         
         setDefaultMetaData();
         createSheetsForViewing();
         View.SetGridSource();
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(GetSheetNames());
         View.ResetImportButtons();

         return _dataSourceFile;
      }

      private void setDefaultMetaData()
      {
         foreach (var metaData in _metaDataCategories)
         {
            if (!metaData.SelectDefaultValue || metaData.DefaultValue == null) continue;
            var parameter = _dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().FirstOrDefault(p => p.ColumnName == metaData.Name);
            if (parameter == null)
            {
               parameter = new MetaDataFormatParameter(metaData.DefaultValue.ToString(), metaData.Name, false);
               _dataSourceFile.Format.Parameters.Add(parameter);
               return;
            }
            parameter.ColumnName = metaData.DefaultValue.ToString();
            parameter.IsColumn = false;
         }
      }

      private void createSheetsForViewing()
      {
         foreach (var sheet in _dataSourceFile.DataSheets.KeyValues)
         {
            _sheetsForViewing[sheet.Key] = sheet.Value.RawData.AsDataTable();
         }
      }

      public bool SelectTab(string tabName)
      {
         if (!_dataSourceFile.DataSheets.Contains(tabName))
            return false;

         var activeFilter = GetActiveFilterCriteria();
         OnTabChanged.Invoke(this, new TabChangedEventArgs() { TabData = _dataSourceFile.DataSheets[tabName].RawData });
         View.SetGridSource(tabName);
         View.SetFilter(activeFilter);

         return true;
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
         if (Sheets.Keys.Any(x => x ==View.SelectedTab))
            View.DisableImportCurrentSheet();

         if (Sheets.Keys.All(GetSheetNames().Contains) && GetSheetNames().Count == Sheets.Keys.Count())
            View.DisableImportAllSheets();
      }

      public string GetActiveFilterCriteria()
      {
         return View.GetActiveFilterCriteria();
      }
   }
}