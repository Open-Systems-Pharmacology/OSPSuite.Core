using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml.Linq;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterPresenter : AbstractDisposablePresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterDataPresenter _importerDataPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private DataImporterSettings _dataImporterSettings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly INanPresenter _nanPresenter;
      private readonly IDataSource _dataSource;
      private IDataSourceFile _dataSourceFile;
      private readonly Utility.Container.IContainer _container;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private OSPSuite.Core.Import.ImporterConfiguration _configuration = new OSPSuite.Core.Import.ImporterConfiguration();

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
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         Utility.Container.IContainer container
      ) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _sourceFilePresenter = sourceFilePresenter;
         _dataRepositoryMapper = dataRepositoryMapper;
         _dataSource = new DataSource(importer);
         _container = container;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;

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
         var dataRepository = _dataRepositoryMapper.ConvertImportDataSet(_dataSource, e.Index, e.Key);
         _confirmationPresenter.PlotDataRepository(dataRepository);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings)
      {
         _columnInfos = columnInfos;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerDataPresenter.SetSettings(metaDataCategories, columnInfos);
         _dataImporterSettings = dataImporterSettings;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;

         SetSourceFile(dataSourceFileName);
         _view.DisableConfirmationView();
      }

      public void ImportData(object sender, EventArgs e)
      {
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs {DataSource = _dataSource});
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
         catch (NanException e)
         {
            _view.ShowErrorMessage(e.Message);
            _view.DisableConfirmationView();
            foreach (var sheetName in args.Sheets.Keys)
            {
               _importerDataPresenter.Sheets.Remove(sheetName);
            }
         }
      }


      private void importSheets(IDataSourceFile dataSourceFile, Cache<string, IDataSheet> sheets, string filter)
      {
         if (!sheets.Any()) return;

         var mappings = dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
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
         _configuration.Parameters = e.Format.Parameters.ToList();
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
         catch (NanException e)
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

      public void SetSourceFile(string path)
      {
         _sourceFilePresenter.SetFilePath(path);
         _dataSourceFile = _importerDataPresenter.SetDataSource(path);
         _columnMappingPresenter.ValidateMapping();
         _configuration.FileName = path;
      }

      public void SaveConfiguration(string fileName)
      {
         
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor(_configuration);
            var element = serializer.Serialize(_configuration, serializationContext);
            element.Save(fileName);
         } 
      }

      public void LoadConfiguration(string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor(_configuration);
            var xel = XElement.Load(fileName);
            _configuration = serializer.Deserialize<OSPSuite.Core.Import.ImporterConfiguration>(xel, serializationContext);
            //broadcast...

            _sourceFilePresenter.SetFilePath(_configuration.FileName);
            _dataSourceFile = _importerDataPresenter.SetDataSource(_configuration.FileName);
            _dataSourceFile.Format.Parameters = _configuration.Parameters;
            _columnMappingPresenter.SetDataFormat(_dataSourceFile.Format);
            _columnMappingPresenter.ValidateMapping();
            _dataSource.SetNamingConvention(_configuration.NamingConventions);
            _confirmationPresenter.SetDataSetNames(_dataSource.NamesFromConvention());
            var sheets = new Cache<string, IDataSheet>();
            foreach (var element in _configuration.LoadedSheets)
            {
               sheets.Add(element, _dataSourceFile.DataSheets[element]);
            }
            importSheets(_dataSourceFile, sheets, _configuration.FilterString);
            _importerDataPresenter.DisableImportedSheets();
         }
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}