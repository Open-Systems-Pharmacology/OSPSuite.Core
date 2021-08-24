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
using OSPSuite.Core.Import;
using System.Linq;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Extensions;

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
      MappingDataFormatParameter GetTime(ImporterConfiguration configuration);
      MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration);
      MappingDataFormatParameter GetError(ImporterConfiguration configuration);
      void AddError(ImporterConfiguration configuration);
      void RemoveError(ImporterConfiguration configuration);
      void SetIsUnitFromColumn(MappingDataFormatParameter parameter, bool isUnitFromColumn);
      string[] GetAllGroupingColumns(ImporterConfiguration configuration);
      void AddGroupingColumn(ImporterConfiguration configuration, string columnName);
      void RemoveGroupingColumn(ImporterConfiguration configuration, string columnName);
      string[] GetAllLoadedSheets(ImporterConfiguration configuration);
      void SetAllLoadedSheet(ImporterConfiguration configuration, string[] sheet);
      void SetAllLoadedSheet(ImporterConfiguration configuration, string sheet);
      bool IgnoreSheetNamesAtImport { get; set; }
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
         ICsvDynamicSeparatorSelector csvSeparatorSelector,
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
         _dataImporterSettings.IgnoreSheetNamesAtImport = true;
         _columnInfos = ((DataImporter)_dataImporter).DefaultPKSimImportConfiguration();
         _csvSeparatorSelector = csvSeparatorSelector;
      }

      public bool IgnoreSheetNamesAtImport {
         get => _dataImporterSettings.IgnoreSheetNamesAtImport;
         set => _dataImporterSettings.IgnoreSheetNamesAtImport = value;
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

      /// <summary>
      /// Creates an emtpy configuration with the columns "Time" and "Measurement".
      /// </summary>
      /// <returns>A new configuration object</returns>
      public ImporterConfiguration CreateConfiguration()
      {
         ImporterConfiguration configuration = new ImporterConfiguration();

         Column timeColumn = new Column()
         {
            Name = _columnInfos.First(ci => ci.IsBase()).DisplayName,
            Dimension = _dimensionFactory.Dimension("Time"),
            Unit = new UnitDescription("h")
         };
         configuration.AddParameter(new MappingDataFormatParameter("Time", timeColumn));

         Column measurementColumn = new Column()
         {
            Name = _columnInfos.First(ci => !(ci.IsAuxiliary() || ci.IsBase())).DisplayName,
            Dimension = _dimensionFactory.Dimension("Concentration (molar)"),
            Unit = new UnitDescription("µmol/l")
         };
         configuration.AddParameter(new MappingDataFormatParameter("Measurement", measurementColumn));

         return configuration;
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

      /// <summary>
      /// Add an error column to the configuration if no error column is present.
      /// If the configuration already has an error column, the mehtod does nothing.
      /// </summary>
      /// <param name="configuration">Configuration object</param>
      public void AddError(ImporterConfiguration configuration)
      {
         //Add a new error column only of the configuration does not have an error column yet
         var errorParameter = GetError(configuration);
         if (errorParameter == null)
         {
            Column errorColumn = new Column()
            {
               Name = _columnInfos.First(ci => ci.IsAuxiliary()).DisplayName,
               Dimension = _dimensionFactory.Dimension("Concentration (molar)"),
               Unit = new UnitDescription("µmol/l"),
               ErrorStdDev = "Arithmetic Standard Deviation"
            };
            configuration.AddParameter(new MappingDataFormatParameter("Error", errorColumn));
         }
      }

      public void AddGroupingColumn(ImporterConfiguration configuration, string columnName)
      {
            DataFormatParameter parameter = new GroupByDataFormatParameter(columnName);
            configuration.AddParameter(parameter);
      }

      public void SetAllLoadedSheet(ImporterConfiguration configuration, string[] sheets)
      {
         configuration.ClearLoadedSheets();
         sheets.Each(sheet => configuration.AddToLoadedSheets(sheet));
      }

      public void SetAllLoadedSheet(ImporterConfiguration configuration, string sheets)
      {
         configuration.ClearLoadedSheets();
         configuration.AddToLoadedSheets(sheets);
      }

      public string[] GetAllGroupingColumns(ImporterConfiguration configuration)
      {
         return configuration.Parameters.Where(p => (p is MetaDataFormatParameter) || (p is GroupByDataFormatParameter)).Select(p => p.ColumnName).ToArray();
      }

      public string[] GetAllLoadedSheets(ImporterConfiguration configuration)
      {
         return configuration.LoadedSheets.ToArray();
      }

      public MappingDataFormatParameter GetError(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsAuxiliary());
      }

      public MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p =>
         {
            var columnInfo = _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name);
            return !(columnInfo.IsAuxiliary() || columnInfo.IsBase());
         });
      }

      public MappingDataFormatParameter GetTime(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsBase());
      }

      public void RemoveError(ImporterConfiguration configuration)
      {
         var errorParameter = GetError(configuration);
         if (errorParameter != null)
            configuration.Parameters.Remove(errorParameter);
      }

      public void RemoveGroupingColumn(ImporterConfiguration configuration, string columnName)
      {
         configuration.Parameters.Remove(configuration.Parameters.First(p => p.ColumnName == columnName));
      }

      public void SetIsUnitFromColumn(MappingDataFormatParameter parameter, bool isUnitFromColumn)
      {
         if (isUnitFromColumn)
         {
            parameter.MappedColumn.Unit.ColumnName = parameter.MappedColumn.Unit.SelectedUnit;
            parameter.MappedColumn.Dimension = null;
         }
         else
         {
            parameter.MappedColumn.Unit.ColumnName = null;
            parameter.MappedColumn.Dimension = _dimensionFactory.DimensionForUnit(parameter.MappedColumn.Unit.SelectedUnit);
         }
      }
   }
}
