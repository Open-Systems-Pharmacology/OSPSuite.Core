using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

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
      private readonly Utility.Container.IContainer _container;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private ImporterConfiguration _configuration = new ImporterConfiguration();
      private readonly IDimensionFactory _dimensionFactory;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IDialogCreator _dialogCreator;


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
         IDimensionFactory dimensionFactory,
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         Utility.Container.IContainer container
      ) : base(view)
      {
         _dimensionFactory = dimensionFactory;
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _sourceFilePresenter = sourceFilePresenter;
         _dataRepositoryMapper = dataRepositoryMapper;
         _dataSource = new DataSource(importer);
         _container = container;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
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
         _importerDataPresenter.OnImportSheets += ImportSheetsFromDataPresenter;
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
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
         View.DisableConfirmationView();
      }

      private void plotDataSet(object sender, DataSetSelectedEventArgs e)
      {
         var dataRepository = _dataRepositoryMapper.ConvertImportDataSet(_dataSource.DataSetAt(e.Index));
         _confirmationPresenter.PlotDataRepository(dataRepository);
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

         SetSourceFile(dataSourceFileName);
         _view.DisableConfirmationView();
      }

      public void ImportData(object sender, EventArgs e)
      {
         var id = Guid.NewGuid().ToString();
         IReadOnlyList<DataRepository> dataRepositories;
         try
         {
            dataRepositories = _importer.DataSourceToDataSets(_dataSource, _metaDataCategories, _dataImporterSettings, id);
         }
         catch (InconsistentMoleculeAndMolWeightException exception)
         {
            _view.ShowErrorMessage(exception.Message);
            return;
         }
         var configuration = GetConfiguration();
         configuration.Id = id;
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs { DataRepositories = dataRepositories });
      }

      private void ImportSheetsFromDataPresenter(object sender, ImportSheetsEventArgs args)
      {
         try
         {
            importSheets(args.DataSourceFile, args.Sheets, args.Filter);
            _importerDataPresenter.DisableImportedSheets();
            _configuration.LoadedSheets.AddRange(args.Sheets.Keys);
            _configuration.FilterString = args.Filter;
         }
         catch (Exception e) when (e is NanException || e is ErrorUnitException)
         {
            {
               _view.ShowErrorMessage(e.Message);
               _view.DisableConfirmationView();
               foreach (var sheetName in args.Sheets.Keys)
               {
                  _importerDataPresenter.Sheets.Remove(sheetName);
                  _view.DisableConfirmationView();
               }
            }
         }
      }

      private void validateDataSource(IDataSource dataSource)
      {
         foreach (var column in _columnInfos.Where(c => !c.IsAuxiliary()))
         {
            foreach (var relatedColumn in _columnInfos.Where(c => c.IsAuxiliary() && c.RelatedColumnOf == column.Name))
            {
               foreach (var dataSet in dataSource.DataSets)
               {
                  foreach (var set in dataSet.Data)
                  {
                     var measurementColumn = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == column.Name);
                     var errorColumn = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == relatedColumn.Name);

                     if (errorColumn.Key == null)
                        return;

                     if (_dimensionFactory.DimensionForUnit(errorColumn.Value.ElementAt(0).Unit) == Constants.Dimension.NO_DIMENSION
                         || _dimensionFactory.DimensionForUnit(errorColumn.Value.ElementAt(0).Unit) == null)
                        continue;

                     for (var i = 0; i < measurementColumn.Value.Count(); i++)
                     {
                        if (double.IsNaN(errorColumn.Value.ElementAt(i).Measurement))
                           continue;

                        if (_dimensionFactory.DimensionForUnit(measurementColumn.Value.ElementAt(i).Unit) !=
                            _dimensionFactory.DimensionForUnit(errorColumn.Value.ElementAt(i).Unit))
                           throw new ErrorUnitException();
                     }
                  }
               }
            }
         }
      }

      private void importSheets(IDataSourceFile dataSourceFile, Cache<string, DataSheet> sheets, string filter)
      {
         if (!sheets.Any()) return;

         var mappings = dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
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
         _confirmationPresenter.SetNamingConventions(_dataImporterSettings.NamingConventions);
         View.EnableConfirmationView();
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

      private void onMissingMapping(object sender, MissingMappingEventArgs missingMappingEventArgs)
      {
         _importerDataPresenter.onMissingMapping();
      }

      private void onImporterDataChanged(object sender, EventArgs args)
      {
         _dataSource.DataSets.Clear();
         try
         {
            importSheets(_dataSourceFile, _importerDataPresenter.Sheets, _importerDataPresenter.GetActiveFilterCriteria());
         }
         catch (Exception e) when (e is NanException || e is ErrorUnitException)
         {
            _view.ShowErrorMessage(e.Message);
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
         _sourceFilePresenter.SetFilePath(path);
         _dataSourceFile = _importerDataPresenter.SetDataSource(path);

         if (_dataSourceFile == null)
            return false;

         _columnMappingPresenter.ValidateMapping();
         _configuration.FileName = path;

         return true;
      }

      public void SaveConfiguration()
      {
         var fileName = _dialogCreator.AskForFileToSave(Captions.Importer.SaveConfiguration, Constants.Filter.XML_FILE_FILTER, Constants.DirectoryKey.OBSERVED_DATA);

         if (string.IsNullOrEmpty(fileName)) return;

         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            _configuration = GetConfiguration();
            var serializer = _modelingXmlSerializerRepository.SerializerFor(_configuration);
            var element = serializer.Serialize(_configuration, serializationContext);
            element.Save(fileName);
         }
      }

      public void LoadConfiguration(ImporterConfiguration configuration)
      {
         openFile(configuration.FileName);
         applyConfiguration(configuration);
         loadImportedDataSetsFromConfiguration(configuration.FilterString);
      }

      private void openFile(string configurationFileName)
      {
         _sourceFilePresenter.SetFilePath(configurationFileName);
         _dataSourceFile = _importerDataPresenter.SetDataSource(configurationFileName);
      }

      private void applyConfiguration(ImporterConfiguration configuration)
      {
         var excelColumnNames = _columnMappingPresenter.GetAllAvailableExcelColumns();
         var mappings = configuration.Parameters.OfType<MappingDataFormatParameter>();
         var listOfNonExistingColumns = mappings.Where(parameter => !excelColumnNames.Contains(parameter.ColumnName)).ToList();

         if (listOfNonExistingColumns.Any())
         {
            var confirm = _dialogCreator.MessageBoxYesNo(Captions.Importer.ConfirmDroppingExcelColumns( string.Join("\n", listOfNonExistingColumns.Select(x => x.ColumnName))));

            if (confirm == ViewResult.No)
               return;

            foreach (var element in listOfNonExistingColumns)
            {
               configuration.Parameters.Remove(element);
            }
         }

         _configuration = configuration;
         _dataSourceFile.Format.CopyParametersFromConfiguration(_configuration);

         _columnMappingPresenter.SetDataFormat(_dataSourceFile.Format);
         _columnMappingPresenter.ValidateMapping();
         _dataSource.SetNamingConvention(_configuration.NamingConventions);
         _nanPresenter.Settings = configuration.NanSettings;
         if (configuration.NanSettings != null)
            _nanPresenter.FillNaNSettings();
         _importerDataPresenter.SetFilter(configuration.FilterString);
      }

      private void loadImportedDataSetsFromConfiguration(string filterString)
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
            importSheets(_dataSourceFile, _importerDataPresenter.Sheets, filterString);
         }
         catch (Exception e) when (e is NanException || e is ErrorUnitException)
         {
            _view.ShowErrorMessage(e.Message);
         }

         _importerDataPresenter.DisableImportedSheets();
      }

      public ImporterConfiguration GetConfiguration() {
         _configuration.CloneParametersFrom(_dataSourceFile.Format.Parameters.ToList());
         _configuration.FilterString = _importerDataPresenter.GetFilter();
         return _configuration;
      }

      public void LoadConfigurationWithoutImporting()
      {
         var fileName = _dialogCreator.AskForFileToOpen(Captions.Importer.ApplyConfiguration, Constants.Filter.XML_FILE_FILTER, Constants.DirectoryKey.OBSERVED_DATA);

         if (fileName.IsNullOrEmpty()) return;
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();
            var xel = XElement.Load(fileName);
            var configuration = serializer.Deserialize<ImporterConfiguration>(xel, serializationContext);

            applyConfiguration(configuration);
         }
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}