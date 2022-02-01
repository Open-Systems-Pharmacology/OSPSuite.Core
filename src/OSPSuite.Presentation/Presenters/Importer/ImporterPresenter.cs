using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterPresenter : AbstractDisposablePresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterDataPresenter _importerDataPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private readonly IImporter _importer;
      private DataImporterSettings _dataImporterSettings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly INanPresenter _nanPresenter;
      protected IDataSource _dataSource;
      private IDataSourceFile _dataSourceFile;
      private ImporterConfiguration _configuration = new ImporterConfiguration();
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IDialogCreator _dialogCreator;
      private readonly IPKMLPersistor _pkmlPersistor;

      public ImporterPresenter(
         IImporterView view,
         IDataSetToDataRepositoryMapper dataRepositoryMapper,
         IImporter importer,
         INanPresenter nanPresenter,
         IImporterDataPresenter importerDataPresenter,
         IImportConfirmationPresenter confirmationPresenter,
         IColumnMappingPresenter columnMappingPresenter,
         ISourceFilePresenter sourceFilePresenter,
         IDialogCreator dialogCreator,
         IPKMLPersistor pkmlPersistor) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _sourceFilePresenter = sourceFilePresenter;
         _dataRepositoryMapper = dataRepositoryMapper;
         _dataSource = new DataSource(importer);
         _pkmlPersistor = pkmlPersistor;
         _importer = importer;
         _dialogCreator = dialogCreator;

         _sourceFilePresenter.Title = Captions.Importer.PleaseSelectDataFile;
         _sourceFilePresenter.Filter = Captions.Importer.ImportFileFilter;
         _sourceFilePresenter.DirectoryKey = Constants.DirectoryKey.OBSERVED_DATA;
         _sourceFilePresenter.OnSourceFileChanged += (s, e) => SetDataSource(e.FileName);
         _sourceFilePresenter.CheckBeforeSelectFile = () =>
            !_dataSource.DataSets.Any() || dialogCreator.MessageBoxYesNo(Captions.Importer.OpenFileConfirmation) == ViewResult.Yes;

         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);
         _view.AddNanView(nanPresenter.View);
         _confirmationPresenter.OnImportData += ImportData;
         _confirmationPresenter.OnDataSetSelected += plotDataSet;
         _confirmationPresenter.OnNamingConventionChanged += (s, a) =>
         {
            _dataSource.SetNamingConvention(a.NamingConvention);
            _confirmationPresenter.SetDataSetNames(_dataSource.NamesFromConvention());
            _configuration.NamingConventions = a.NamingConvention;
         };
         _importerDataPresenter.OnImportSheets += loadSheetsFromDataPresenter;
         _nanPresenter.OnNaNSettingsChanged += (s, a) =>
         {
            _columnMappingPresenter.ValidateMapping();
            _configuration.NanSettings = _nanPresenter.Settings;
         };
         _view.AddConfirmationView(_confirmationPresenter.View);
         _view.AddImporterView(_importerDataPresenter.View);
         AddSubPresenters(_importerDataPresenter, _confirmationPresenter, _columnMappingPresenter, _sourceFilePresenter);
         _importerDataPresenter.OnFormatChanged += onFormatChanged;
         _importerDataPresenter.OnTabChanged += onTabChanged;
         _importerDataPresenter.OnDataChanged += onImporterDataChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnResetMappingBasedOnCurrentSheet += onResetMappingBasedOnCurrentSheet;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
         View.DisableConfirmationView();
      }

      private void plotDataSet(object sender, DataSetSelectedEventArgs e)
      {
         try
         {
            _view.HideExtraErrors();
            var dataRepository = _dataRepositoryMapper.ConvertImportDataSet(_dataSource.DataSetAt(e.Index));
            _confirmationPresenter.PlotDataRepository(dataRepository.DataRepository);
         }
         catch (InvalidArgumentException invalidException)
         {
            _view.ShowExtraErrors(Error.ErrorWhenPlottingDataRepository(e.Index, invalidException.Message));
            _view.DisableConfirmationView();
         }
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings)
      {
         _columnInfos = columnInfos;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerDataPresenter.SetSettings(metaDataCategories, columnInfos);
         _dataImporterSettings = dataImporterSettings;
         _metaDataCategories = metaDataCategories;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;

         try
         {
            if (!SetSourceFile(dataSourceFileName)) return;
         }
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
            return;
         }

         _view.DisableConfirmationView();
      }

      public void ImportData(object sender, EventArgs e)
      {
         var id = Guid.NewGuid().ToString();
         IReadOnlyList<DataRepository> dataRepositories;
         try
         {
            var mappingResults = _importer.DataSourceToDataSets(_dataSource, _metaDataCategories, _dataImporterSettings, id);
            var messageMapping = mappingResults.FirstOrDefault(m => !string.IsNullOrEmpty(m.WarningMessage));
            if (messageMapping != null && _dialogCreator.MessageBoxYesNo(messageMapping.WarningMessage) == ViewResult.No)
               return;

            dataRepositories = mappingResults.Select(drm => drm.DataRepository).ToList();
         }
         catch (InconsistentMoleculeAndMolWeightException exception)
         {
            _dialogCreator.MessageBoxError(exception.Message);
            return;
         }

         var configuration = UpdateAndGetConfiguration();
         configuration.Id = id;
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs {DataRepositories = dataRepositories});
      }

      private void loadSheetsFromDataPresenter(object sender, ImportSheetsEventArgs args)
      {
         try
         {
            loadSheets(args.DataSourceFile, args.Sheets, args.Filter);
            _importerDataPresenter.DisableImportedSheets();
            args.Sheets.Keys.Each(_configuration.AddToLoadedSheets);
            _configuration.FilterString = args.Filter;
         }
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
            args.Sheets.Keys.Each(_importerDataPresenter.Sheets.Remove);
         }
      }

      private void validateDataSource(IDataSource dataSource)
      {
         dataSource.ValidateDataSourceUnits(_columnInfos);
      }

      private void loadSheets(IDataSourceFile dataSourceFile, Cache<string, DataSheet> sheets, string filter, string selectedNamingConvention = null)
      {
         if (!sheets.Any())
         {
            View.DisableConfirmationView();
            return;
         }

         var mappings = dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Where(p => p.ColumnName != null).Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => md.IsColumn ? dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index : -1
         }).Union
         (
            dataSourceFile.Format.Parameters.OfType<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
            {
               Id = md.ColumnName,
               Index = sheetName => dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
            })
         );

         _dataSource.SetMappings(dataSourceFile.Path, mappings);
         _dataSource.NanSettings = _nanPresenter.Settings;
         _dataSource.SetDataFormat(_columnMappingPresenter.GetDataFormat());
         _dataSource.AddSheets(sheets, _columnInfos, filter);

         validateDataSource(_dataSource);

         var keys = new List<string>()
         {
            Constants.FILE,
            Constants.SHEET
         };

         keys.AddRange(_dataSource.GetMappings().Select(m => m.Id));
         _confirmationPresenter.SetKeys(keys);
         View.EnableConfirmationView();
         _confirmationPresenter.SetNamingConventions(_dataImporterSettings.NamingConventions.ToList(), selectedNamingConvention);
      }

      private void onFormatChanged(object sender, FormatChangedEventArgs e)
      {
         _columnMappingPresenter.SetDataFormat(e.Format);
         _configuration.CloneParametersFrom(e.Format.Parameters.ToList());
      }

      private void onTabChanged(object sender, TabChangedEventArgs e)
      {
         _columnMappingPresenter.SetRawData(e.TabData);
      }

      protected virtual void onResetMappingBasedOnCurrentSheet(object sender, EventArgs e)
      {
         if (confirmDroppingOfLoadedSheets())
            return;

         try
         {
            _importerDataPresenter.GetFormatBasedOnCurrentSheet();
         }
         catch (UnsupportedFormatException)
         {
            _dialogCreator.MessageBoxError(Captions.Importer.SheetFormatNotSupported);
         }
         _view.DisableConfirmationView();
      }

      private void onMissingMapping(object sender, MissingMappingEventArgs missingMappingEventArgs)
      {
         _importerDataPresenter.onMissingMapping();
         View.DisableConfirmationView();
      }

      private void onImporterDataChanged(object sender, EventArgs args)
      {
         _dataSource.DataSets.Clear();
         try
         {
            loadSheets(_dataSourceFile, _importerDataPresenter.Sheets, _importerDataPresenter.GetActiveFilterCriteria());
         }
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
            if (e is NanException || e is ErrorUnitException)
               _view.DisableConfirmationView();
         }
      }

      private void onCompletedMapping(object sender, EventArgs args)
      {
         _importerDataPresenter.onCompletedMapping();
         onImporterDataChanged(this, args);
      }

      public bool SetSourceFile(string path)
      {
         _dataSourceFile = _importerDataPresenter.SetDataSource(path);

         if (_dataSourceFile == null)
            return false;

         _sourceFilePresenter.SetFilePath(path);
         _columnMappingPresenter.ValidateMapping();
         _configuration.FileName = path;

         return true;
      }

      public void SaveConfiguration()
      {
         var fileName = _dialogCreator.AskForFileToSave(Captions.Importer.SaveConfiguration, Constants.Filter.XML_FILE_FILTER, Constants.DirectoryKey.OBSERVED_DATA);

         if (string.IsNullOrEmpty(fileName))
            return;

         _configuration = UpdateAndGetConfiguration();
         _pkmlPersistor.SaveToPKML(_configuration, fileName);
      }

      public void LoadConfiguration(ImporterConfiguration configuration, string fileName)
      {
         openFile(fileName);
         applyConfiguration(configuration);
         loadImportedDataSetsFromConfiguration(configuration);
      }

      private void openFile(string configurationFileName)
      {
         _sourceFilePresenter.SetFilePath(configurationFileName);
         _dataSourceFile = _importerDataPresenter.SetDataSource(configurationFileName);
      }

      private void applyConfiguration(ImporterConfiguration configuration)
      {
         var excelColumnNames = _columnMappingPresenter.GetAllAvailableExcelColumns();
         var listOfNonExistingColumns = configuration.Parameters.Where(parameter => !excelColumnNames.Contains(parameter.ColumnName) && parameter.ComesFromColumn()).ToList();

         if (listOfNonExistingColumns.Any())
         {
            var confirm = _dialogCreator.MessageBoxYesNo(Captions.Importer.ConfirmDroppingExcelColumns(string.Join("\n", listOfNonExistingColumns.Select(x => x.ColumnName))));

            if (confirm == ViewResult.No)
               return;

            foreach (var element in listOfNonExistingColumns)
            {
               configuration.RemoveParameter(element);
            }
         }

         var mappings = configuration.Parameters.OfType<MappingDataFormatParameter>();
         var listOfNonExistingUnitColumns = mappings.Where(parameter => !parameter.MappedColumn.Unit.ColumnName.IsNullOrEmpty() && !excelColumnNames.Contains(parameter.MappedColumn.Unit.ColumnName)).ToList();
         foreach (var element in listOfNonExistingUnitColumns)
         {
            element.MappedColumn.Unit = new UnitDescription();
            element.MappedColumn.Dimension = null;
         }

         _importerDataPresenter.ResetLoadedSheets();
         _view.DisableConfirmationView();

         _configuration = configuration;

         if (!_configuration.NamingConventions.IsNullOrEmpty())
            _confirmationPresenter.TriggerNamingConventionChanged(_configuration.NamingConventions);

         _dataSourceFile.Format.CopyParametersFromConfiguration(_configuration);

         _columnMappingPresenter.SetDataFormat(_dataSourceFile.Format);
         _columnMappingPresenter.ValidateMapping();
         _nanPresenter.Settings = configuration.NanSettings;
         if (configuration.NanSettings != null)
            _nanPresenter.FillNaNSettings();
         _importerDataPresenter.SetFilter(configuration.FilterString);
      }

      private void loadImportedDataSetsFromConfiguration(ImporterConfiguration configuration)
      {
         _confirmationPresenter.SetDataSetNames(_dataSource.NamesFromConvention()); //this could probably be in the apply
         //About NanSettings: we do actually read the nanSettings in import dataSheets
         //we just never update the editor on the view, which actually is a problem
         var sheets = new Cache<string, DataSheet>();
         foreach (var element in _configuration.LoadedSheets)
         {
            sheets.Add(element, _dataSourceFile.DataSheets[element]);
         }

         foreach (var sheet in sheets.KeyValues)
         {
            _importerDataPresenter.Sheets.Add(sheet.Key, sheet.Value);
         }

         try
         {
            var namingConvention = configuration.NamingConventions;
            loadSheets(_dataSourceFile, _importerDataPresenter.Sheets, configuration.FilterString, namingConvention);
            _confirmationPresenter.TriggerNamingConventionChanged(namingConvention);
         }
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
         }

         _importerDataPresenter.DisableImportedSheets();
      }

      protected virtual bool confirmDroppingOfLoadedSheets()
      {
         return _dataSource.DataSets.Count != 0 && _dialogCreator.MessageBoxYesNo(Captions.Importer.ActionWillEraseLoadedData) != ViewResult.Yes;
      }

      public ImporterConfiguration UpdateAndGetConfiguration()
      {
         _configuration.CloneParametersFrom(_dataSourceFile.Format.Parameters.ToList());
         _configuration.FilterString = _importerDataPresenter.GetFilter();
         return _configuration;
      }

      public void LoadConfigurationWithoutImporting()
      {
         if (confirmDroppingOfLoadedSheets())
               return;

         onResetMappingBasedOnCurrentSheet(this, null);

         var fileName = _dialogCreator.AskForFileToOpen(Captions.Importer.ApplyConfiguration, Constants.Filter.XML_FILE_FILTER,
            Constants.DirectoryKey.OBSERVED_DATA);

         if (fileName.IsNullOrEmpty())
            return;
         
         var configuration = _pkmlPersistor.Load<ImporterConfiguration>(fileName);
         applyConfiguration(configuration);
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}