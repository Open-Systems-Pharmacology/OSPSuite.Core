using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
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
      private ColumnInfoCache _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly Cache<string, DataTable> _sheetsForViewing;
      private string _currentSheetName;
      private Cache<string, IDataSet> _lastLoadedDataSets = new Cache<string, IDataSet>();
      public DataSheetCollection ImportedSheets { get; set; }

      public event EventHandler<FormatChangedEventArgs> OnFormatChanged = delegate { };
      public event EventHandler<TabChangedEventArgs> OnTabChanged = delegate { };

      public event EventHandler<ImportSheetsEventArgs> OnImportSheets = delegate { };
      public event EventHandler<EventArgs> OnDataChanged = delegate { };

      public ImporterDataPresenter(IImporterDataView dataView, IImporter importer) : base(dataView)
      {
         _importer = importer;
         _sheetsForViewing = new Cache<string, DataTable>();
         ImportedSheets = new DataSheetCollection();
         _currentSheetName = string.Empty;
      }

      public List<string> GetSheetNames()
      {
         return _dataSourceFile.DataSheets.GetDataSheetNames().ToList();
      }

      public DataTable GetSheet(string tabName)
      {
         return _sheetsForViewing.Contains(tabName) ? _sheetsForViewing[tabName] : new DataTable();
      }

      public void ImportDataForConfirmation()
      {
         var sheets = ImportedSheets.AddNotExistingSheets(_dataSourceFile.DataSheets);

         if (sheets.Count == 0)
            return;

         OnImportSheets.Invoke(this,
            new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, SheetNames = sheets, Filter = GetActiveFilterCriteria() });
      }

      public void OnMissingMapping()
      {
         View.DisableImportButtons();
      }

      public void OnCompletedMapping()
      {
         View.EnableImportButtons();
      }

      public void ImportDataForConfirmation(string sheetName)
      {
         var sheets = new DataSheetCollection();
         if (!ImportedSheets.Contains(sheetName))
         {
            ImportedSheets.AddSheet(getSingleSheet(sheetName));
            sheets.AddSheet(getSingleSheet(sheetName));
         }

         if (!sheets.Any())
            return;

         OnImportSheets.Invoke(this,
            new ImportSheetsEventArgs
               { DataSourceFile = _dataSourceFile, SheetNames = sheets.GetDataSheetNames(), Filter = GetActiveFilterCriteria() });
      }

      public string GetFilter()
      {
         return _view.GetFilter();
      }

      public void TriggerOnDataChanged()
      {
         OnDataChanged.Invoke(this, null);
      }

      public void SetFilter(string filterString)
      {
         _view.SetFilter(filterString);
      }

      private DataSheet getSingleSheet(string sheetName)
      {
         return _dataSourceFile.DataSheets.GetDataSheetByName(sheetName);
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         OnFormatChanged.Invoke(this, new FormatChangedEventArgs() { Format = format });
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, ColumnInfoCache columnInfos)
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      public IDataSourceFile SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName))
            return null;

         ImportedSheets = new DataSheetCollection();
         _dataSourceFile = _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);

         if (_dataSourceFile == null)
            return null;

         setDefaultMetaData();
         setMetaDataWithManualInput();
         createSheetsForViewing();
         View.SetGridSource();
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(GetSheetNames());
         View.ResetImportButtons();

         View.SelectTab(_dataSourceFile.FormatCalculatedFrom);
         return _dataSourceFile;
      }

      private void setMetaDataWithManualInput()
      {
         foreach (var metaData in _metaDataCategories)
         {
            if (!metaData.AllowsManualInput)
               continue;

            var parameter = _dataSourceFile.Format.GetColumnByName<MetaDataFormatParameter>(metaData.Name);

            if (parameter != null)
               continue;

            parameter = new MetaDataFormatParameter(null, metaData.Name, false);

            if (_dataSourceFile.Format.GetParameters<MetaDataFormatParameter>().Any(p => p.MetaDataId == parameter.MetaDataId))
               continue;

            _dataSourceFile.Format.Parameters.Add(parameter);
            return;
         }
      }

      private void setDefaultMetaData()
      {
         foreach (var metaData in _metaDataCategories)
         {
            if (!metaData.SelectDefaultValue || metaData.DefaultValue == null) continue;
            var parameter = _dataSourceFile.Format.GetColumnByName<MetaDataFormatParameter>(metaData.Name);
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
         foreach (var sheet in _dataSourceFile.DataSheets)
         {
            _sheetsForViewing[sheet.SheetName] = sheet.ToDataTable();
         }
      }

      public bool SelectTab(string tabName)
      {
         if (!_dataSourceFile.DataSheets.Contains(tabName))
            return false;

         var activeFilter = GetActiveFilterCriteria();
         OnTabChanged.Invoke(this, new TabChangedEventArgs() { TabSheet = _dataSourceFile.DataSheets.GetDataSheetByName(tabName) });
         View.SetGridSource(tabName);
         View.SetFilter(activeFilter);
         _currentSheetName = tabName;
         return true;
      }

      public void RemoveTab(string tabName)
      {
         _dataSourceFile.DataSheets.Remove(tabName);

         if (!ImportedSheets.Contains(tabName))
            return;

         ImportedSheets.Remove(tabName);
         TriggerOnDataChanged();
      }

      public void ReopenAllSheets()
      {
         _dataSourceFile.Path = _dataSourceFile.Path;
         RefreshTabs();
      }

      public void RemoveAllButThisTab(string tabName)
      {
         View.ClearTabs();
         var remainingSheet = _dataSourceFile.DataSheets.GetDataSheetByName(tabName);
         _dataSourceFile.DataSheets.Clear();
         _dataSourceFile.DataSheets.AddSheet(remainingSheet);
         View.AddTabs(GetSheetNames());

         if (ImportedSheets.All(k => k.SheetName == tabName))
            return;

         DataSheet currentAlreadyLoaded = null;

         if (ImportedSheets.Contains(tabName))
            currentAlreadyLoaded = ImportedSheets.GetDataSheetByName(tabName);

         ImportedSheets.Clear();

         if (currentAlreadyLoaded != null)
            ImportedSheets.AddSheet(currentAlreadyLoaded);

         TriggerOnDataChanged();
      }

      public void RefreshTabs()
      {
         View.ClearTabs();
         View.AddTabs(GetSheetNames());
      }

      public void DisableImportedSheets()
      {
         if (ImportedSheets.Any(x => x.SheetName == View.SelectedTab))
            View.DisableImportCurrentSheet();

         var sheetNames = GetSheetNames();
         var importedSheetsNames = ImportedSheets.GetDataSheetNames();
         if (importedSheetsNames.ContainsAll(sheetNames) && sheetNames.Count == importedSheetsNames.Count())
            View.DisableImportAllSheets();
      }

      public string GetActiveFilterCriteria()
      {
         return View.GetActiveFilterCriteria();
      }

      public void GetFormatBasedOnCurrentSheet()
      {
         var availableFormats = _importer.CalculateFormat(_dataSourceFile, _columnInfos, _metaDataCategories, _currentSheetName).ToList();

         if (!availableFormats.Any())
            throw new UnsupportedFormatException(_dataSourceFile.Path);

         _dataSourceFile.AvailableFormats = availableFormats;
         ResetLoadedSheets();
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.SetTabMarks(new Cache<string, TabMarkInfo>(onMissingKey: _ => new TabMarkInfo(errorMessage: null, isLoaded: false)));
      }

      public void ResetLoadedSheets()
      {
         ImportedSheets.Clear();
         View.ResetImportButtons();
      }

      public void SetTabMarks(ParseErrors errors, Cache<string, IDataSet> loadedDataSets)
      {
         _lastLoadedDataSets = loadedDataSets;
         var tabMarkInfos = new Cache<string, TabMarkInfo>(onMissingKey: _ => new TabMarkInfo(errorMessage: null, isLoaded: false));
         foreach (var loadedDataSet in loadedDataSets.KeyValues)
         {
            var errorsForDataSet = errors.ErrorsFor(loadedDataSet.Value);
            var errorMessage = errorsForDataSet.Any() ? Error.ParseErrorMessage(errorsForDataSet.Select(x => x.Message)) : null;
            var info = new TabMarkInfo(errorMessage: errorMessage, isLoaded: true);
            tabMarkInfos.Add(loadedDataSet.Key, info);
         }

         View.SetTabMarks(tabMarkInfos);
      }

      public void SetTabMarks(ParseErrors errors)
      {
         SetTabMarks(errors, _lastLoadedDataSets);
      }
   }
}