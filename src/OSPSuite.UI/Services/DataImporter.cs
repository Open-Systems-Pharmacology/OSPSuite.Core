using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;


namespace OSPSuite.UI.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly Utility.Container.IContainer _container;
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporter _importer;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private readonly IApplicationController _applicationController;


      public DataImporter(
         Utility.Container.IContainer container,
         IDialogCreator dialogCreator,
         IImporter importer,
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         IDataSetToDataRepositoryMapper dataRepositoryMapper,
         IApplicationController applicationController
      )
      {
         _container = container;
         _dialogCreator = dialogCreator;
         _importer = importer;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
         _dataRepositoryMapper = dataRepositoryMapper;
         _applicationController = applicationController;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings
      )
      {

         var path = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);

         if (string.IsNullOrEmpty(path))
            return (new List<DataRepository>(), null);

         using (var importerPresenter = _container.Resolve<IImporterPresenter>())
         {
            importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
            importerPresenter.SetSourceFile(path);
            using (var importerModalPresenter = _container.Resolve<IModalImporterPresenter>())
            {
               return importerModalPresenter.ImportDataSets(importerPresenter, metaDataCategories, columnInfos, dataImporterSettings);
            }
         }
      }

      public IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         if (dataImporterSettings.PromptForConfirmation)
         {
            using (var importerPresenter = _container.Resolve<IImporterPresenter>())
            {
               importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
               importerPresenter.LoadConfiguration(configuration);
               using (var importerModalPresenter = _container.Resolve<IModalImporterPresenter>())
               {
                  return importerModalPresenter.ImportDataSets(importerPresenter, metaDataCategories, columnInfos, dataImporterSettings).DataRepositories;
               }
            }
         }
         else
         {
            var dataSource = new DataSource(_importer);
            var dataSourceFile = _importer.LoadFile(columnInfos, configuration.FileName, metaDataCategories);
            dataSourceFile.Format.CopyParametersFromConfiguration(configuration);
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
            dataSource.SetMappings(dataSourceFile.Path, mappings);
            dataSource.NanSettings = configuration.NanSettings;
            dataSource.SetDataFormat(dataSourceFile.Format);
            dataSource.SetNamingConvention(configuration.NamingConventions);
            var sheets = new Cache<string, DataSheet>();
            foreach (var key in configuration.LoadedSheets)
            {
               sheets.Add(key, dataSourceFile.DataSheets[key]);
            }

            dataSource.AddSheets(sheets, columnInfos, configuration.FilterString);

            var result = new List<DataRepository>();
            var i = 0;
            foreach (var pair in dataSource.DataSets.KeyValues)
            {
               foreach (var data in pair.Value.Data)
               {
                  var dataRepo = _dataRepositoryMapper.ConvertImportDataSet(dataSource.DataSetAt(i++));
                  dataRepo.ConfigurationId = configuration.Id;
                  result.Add(dataRepo);
               }
            }

            return result;
         }
      }

      public (IEnumerable<DataRepository> newDataSets, IEnumerable<DataRepository> overwrittenDataSets, IEnumerable<DataRepository> dataSetsToBeDeleted) 
         ReloadFromConfiguration(IEnumerable<DataRepository> dataSetsToImport, IEnumerable<DataRepository> existingDataSets)
      {
         var newDataSets = dataSetsToImport.Where(x => existingDataSets.Any(y => y.ExtendedProperties == x.ExtendedProperties));
         var dataSetsToBeDeleted = existingDataSets.Where(x => existingDataSets.Any(y => y.ExtendedProperties == x.ExtendedProperties));
         var overwrittenDataSets = dataSetsToImport.Except(newDataSets);
         

         using (var reloadPresenter = _applicationController.Start<IImporterReloadPresenter>())
         {
            reloadPresenter.AddDeletedDataSets(dataSetsToBeDeleted.AllNames());
            reloadPresenter.AddNewDataSets(newDataSets.AllNames());
            reloadPresenter.AddOverwrittenDataSets(overwrittenDataSets.AllNames());
            reloadPresenter.Show();

            if (reloadPresenter.Canceled())
               return (null, null, null);
         }

         return (newDataSets, overwrittenDataSets, dataSetsToBeDeleted);
      }
   }
}
