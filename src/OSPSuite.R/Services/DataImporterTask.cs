using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using System.Collections.Generic;
using System.Xml.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R.Services
{
   public interface IDataImporterTask
   {
      ImporterConfiguration CreateConfiguration();
      ImporterConfiguration GetConfiguration(string fileName);
      ImporterConfiguration CreateConfigurationFor(string dataFileName);
      void SaveConfiguration(ImporterConfiguration configuration, string path);
      IReadOnlyList<DataRepository> ImportXslxFromConfiguration(string configurationFileName, string dataFileName);
      IReadOnlyList<DataRepository> ImportXslxFromConfiguration(ImporterConfiguration configuration, string dataFileName);
      IReadOnlyList<DataRepository> ImportCsvFromConfiguration(string configurationFileName, string dataFileName, char columnSeparator);
      IReadOnlyList<DataRepository> ImportCsvFromConfiguration(ImporterConfiguration configuration, string dataFileName, char columnSeparator);
   }

   public class DataImporterTask : IDataImporterTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private readonly IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly DataImporterSettings _dataImporterSettings;
      private readonly IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IContainer _container;
      private readonly ICsvDynamicSeparatorSelector _csvSeparatorSelector;

      public DataImporterTask(
         IDataImporter dataImporter, 
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         ICsvSeparatorSelector csvSeparatorSelector,
         IDimensionFactory dimensionFactory, 
         IContainer container
      )
      {
         _dataImporter = dataImporter;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
         _dimensionFactory = dimensionFactory;
         _container = container;
         _metaDataCategories = (IReadOnlyList<MetaDataCategory>)_dataImporter.DefaultMetaDataCategories();
         _dataImporterSettings = new DataImporterSettings();
         _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
         _columnInfos = ((DataImporter)_dataImporter).DefaultPKSimImportConfiguration();
         //Should actually ask for ICsvDynamicSeparatorSelector as a dependency but I do not currently know
         //how to serve two interfaces with the same singleton. If there is no way to do it with the current
         //implementation, then we need to extend but first I need to check if there is no support for it.
         _csvSeparatorSelector = csvSeparatorSelector as ICsvDynamicSeparatorSelector;
      }

      public IReadOnlyList<DataRepository> ImportXslxFromConfiguration(
         string configurationFileName,
         string dataFileName)
      {
         return _dataImporter.ImportFromConfiguration(
            GetConfiguration(configurationFileName), 
            _metaDataCategories, 
            _columnInfos, 
            _dataImporterSettings, 
            dataFileName
         );
      }

      public IReadOnlyList<DataRepository> ImportXslxFromConfiguration(
         ImporterConfiguration configuration,
         string dataFileName)
      {
         return _dataImporter.ImportFromConfiguration(
            configuration,
            _metaDataCategories,
            _columnInfos,
            _dataImporterSettings,
            dataFileName
         );
      }

      public IReadOnlyList<DataRepository> ImportCsvFromConfiguration(
         string configurationFileName,
         string dataFileName,
         char columnSeparator)
      {
         _csvSeparatorSelector.CsvSeparator = columnSeparator;
         return _dataImporter.ImportFromConfiguration(
            GetConfiguration(configurationFileName),
            _metaDataCategories,
            _columnInfos,
            _dataImporterSettings,
            dataFileName
         );
      }

      public IReadOnlyList<DataRepository> ImportCsvFromConfiguration(
         ImporterConfiguration configuration,
         string dataFileName,
         char columnSeparator)
      {
         _csvSeparatorSelector.CsvSeparator = columnSeparator;
         return _dataImporter.ImportFromConfiguration(
            configuration,
            _metaDataCategories,
            _columnInfos,
            _dataImporterSettings,
            dataFileName
         );
      }

      public ImporterConfiguration GetConfiguration(string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container, _dimensionFactory))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();

            var xel = XElement.Load(fileName);
            return serializer.Deserialize<ImporterConfiguration>(xel, serializationContext);
         }
      }

      public ImporterConfiguration CreateConfiguration()
      {
         return new ImporterConfiguration();
      }

      public ImporterConfiguration CreateConfigurationFor(string dataFileName)
      {
         return _dataImporter.ConfigurationFromData(dataFileName, _columnInfos, _metaDataCategories);
      }

      public void SaveConfiguration(ImporterConfiguration configuration, string path)
      {
         using (var serializationContext = SerializationTransaction.Create(_container, _dimensionFactory))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();

            serializer.Serialize(configuration, serializationContext).Save(path);
         }
      }
   }
}
