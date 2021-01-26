
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization;
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
using System.Xml.Linq;

namespace OSPSuite.UI.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly Utility.Container.IContainer _container;
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporter _importer;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;

      public DataImporter(
         Utility.Container.IContainer container,
         IDialogCreator dialogCreator,
         IImporter importer,
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         IDataSetToDataRepositoryMapper dataRepositoryMapper
      )
      {
         _container = container;
         _dialogCreator = dialogCreator;
         _importer = importer;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {

         var path = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);

         if (string.IsNullOrEmpty(path))
            return new List<DataRepository>();

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

      public IEnumerable<DataRepository> ImportFromXml(
         string configurationFilePath,
         bool promptForConfirmation,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         if (string.IsNullOrEmpty(configurationFilePath)) return null;

         if (promptForConfirmation)
         {
            using (var importerPresenter = _container.Resolve<IImporterPresenter>())
            {
               importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
               importerPresenter.LoadConfiguration(configurationFilePath);
               using (var importerModalPresenter = _container.Resolve<IModalImporterPresenter>())
               {
                  return importerModalPresenter.ImportDataSets(importerPresenter, metaDataCategories, columnInfos, dataImporterSettings);
               }
            }
         }
         else
         {
            OSPSuite.Core.Import.ImporterConfiguration configuration = null;
            using (var serializationContext = SerializationTransaction.Create(_container))
            {
               var serializer = _modelingXmlSerializerRepository.SerializerFor<OSPSuite.Core.Import.ImporterConfiguration>();
               var xel = XElement.Load(configurationFilePath);
               configuration = serializer.Deserialize<OSPSuite.Core.Import.ImporterConfiguration>(xel, serializationContext);
            }

            var dataSource = new DataSource(_importer);
            var dataSourceFile = _importer.LoadFile(columnInfos, configuration.FileName, metaDataCategories);
            dataSourceFile.Format.Parameters = configuration.Parameters;
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
            dataSource.SetMappings(dataSourceFile.Path, mappings);
            dataSource.NanSettings = configuration.NanSettings;
            dataSource.SetDataFormat(dataSourceFile.Format);
            var sheets = new Cache<string, IDataSheet>();
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
                  result.Add(_dataRepositoryMapper.ConvertImportDataSet(dataSource, i++, pair.Key));
            }

            return result;
         }
      }
   }
}
