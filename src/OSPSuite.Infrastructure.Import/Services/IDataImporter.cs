using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Utility.Collections;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IDataImporter
   {
      /// <summary>
      ///    This function retrieves a list of imported DataRepositories.
      /// </summary>
      /// <param name="metaDataCategories">Specification of meta data of the table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="dataImporterSettings">Settings used to initialize the view</param>
      /// <param name="dataFileName">Path to the file containing the data</param>
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName
      );

      /// <summary>
      ///    This function retrieves a list of imported DataRepositories.
      /// </summary>
      /// <param name="configuration">Configuration to use</param>
      /// <param name="metaDataCategories">Specification of meta data of the table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="dataImporterSettings">Settings used to initialize the view</param>
      /// <param name="dataFileName">Path to the file containing the data</param>
      IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName
      );

      ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport,
         IReadOnlyList<DataRepository> existingDataSets);

      /// <summary>
      ///    Creates a default list of meta data categories that could still be modified by the caller
      /// </summary>
      /// <returns>a list of meta data categories</returns>
      IReadOnlyList<MetaDataCategory> DefaultMetaDataCategoriesForObservedData();

      /// <summary>
      ///    Creates a default list of ColumnInfos that could still be modified by the caller
      /// </summary>
      /// <returns>a list of meta data categories</returns>
      IReadOnlyList<ColumnInfo> ColumnInfosForObservedData();

      /// <summary>
      ///    Compares if two data repositories come from the same data
      /// </summary>
      /// <param name="sourceDataRepository">source DataRepository to compare with</param>
      /// <param name="targetDataRepository">target DataRepository to compare with</param>
      /// <returns></returns>
      bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository);

      /// <summary>
      ///    Returns a new Configuration auto discovered from the data contained in dataPath
      /// </summary>
      /// <param name="dataPath">File containing data</param>
      /// <param name="columnInfos">Column infos description</param>
      /// <param name="metaDataCategories">meta data description</param>
      /// <param name="sheetName">name of the sheet to base the configuration on</param>
      /// <returns></returns>
      ImporterConfiguration ConfigurationFromData(string dataPath, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories, string sheetName = null);
   }

   public abstract class AbstractDataImporter : IDataImporter
   {
      protected readonly IImporter _importer;
      private readonly IDimensionFactory _dimensionFactory;

      protected AbstractDataImporter(IImporter importer, IDimensionFactory dimensionFactory)
      {
         _importer = importer;
         _dimensionFactory = dimensionFactory;
      }

      public virtual IReadOnlyList<ColumnInfo> ColumnInfosForObservedData()
      {
         var columns = new List<ColumnInfo>();

         var timeDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME);
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = timeDimension,
            Name = Constants.Dimension.TIME,
            DisplayName = Constants.Dimension.TIME,
            IsMandatory = true,
         };

         timeColumn.SupportedDimensions.Add(timeDimension);
         columns.Add(timeColumn);

         var mainDimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var measurementInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = Constants.MEASUREMENT,
            DisplayName = Constants.MEASUREMENT,
            IsMandatory = true,
            BaseGridName = timeColumn.Name
         };

         addDimensionsTo(measurementInfo);
         columns.Add(measurementInfo);

         var errorInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = Constants.ERROR,
            DisplayName = Constants.ERROR,
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = measurementInfo.Name
         };

         addDimensionsTo(errorInfo);
         columns.Add(errorInfo);

         return columns;
      }

      public abstract bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository);

      public abstract ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport, IReadOnlyList<DataRepository> existingDataSets);

      public ImporterConfiguration ConfigurationFromData(string dataPath, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories, string sheetName = null)
      {
         var configuration = new ImporterConfiguration();
         var columnInfoCache = new ColumnInfoCache(columnInfos);
         var dataSourceFile = _importer.LoadFile(columnInfoCache, dataPath, metaDataCategories);
         if (!string.IsNullOrEmpty(sheetName))
         {
            dataSourceFile.AvailableFormats = _importer.CalculateFormat(dataSourceFile, columnInfoCache, metaDataCategories, sheetName).ToList();
         }

         configuration.CloneParametersFrom(dataSourceFile.Format.Parameters.ToList());
         configuration.FileName = dataPath;
         configuration.Id = Guid.NewGuid().ToString();
         configuration.NamingConventions = "";
         return configuration;
      }

      private void addDimensionsTo(ColumnInfo columnInfo)
      {
         foreach (var dimension in _dimensionFactory.DimensionsSortedByName)
         {
            columnInfo.SupportedDimensions.Add(dimension);
         }
      }

      public virtual IReadOnlyList<MetaDataCategory> DefaultMetaDataCategoriesForObservedData()
      {
         var categories = new List<MetaDataCategory>();

         var speciesCategory = CreateMetaDataCategory<string>(Constants.ObservedData.SPECIES, isMandatory: true, isListOfValuesFixed: true);
         categories.Add(speciesCategory);

         var organCategory = CreateMetaDataCategory<string>(Constants.ObservedData.ORGAN, isMandatory: true, isListOfValuesFixed: true);
         organCategory.Description = ObservedData.ObservedDataOrganDescription;
         organCategory.TopNames.Add(Constants.ObservedData.PERIPHERAL_VENOUS_BLOOD_ORGAN);
         organCategory.TopNames.Add(Constants.ObservedData.VENOUS_BLOOD_ORGAN);
         categories.Add(organCategory);

         var compCategory = CreateMetaDataCategory<string>(Constants.ObservedData.COMPARTMENT, isMandatory: true, isListOfValuesFixed: true);
         compCategory.Description = ObservedData.ObservedDataCompartmentDescription;
         compCategory.TopNames.Add(Constants.ObservedData.PLASMA_COMPARTMENT);
         categories.Add(compCategory);

         var moleculeCategory = CreateMetaDataCategory<string>(Constants.ObservedData.MOLECULE);
         moleculeCategory.Description = ObservedData.MoleculeNameDescription;
         moleculeCategory.AllowsManualInput = true;
         categories.Add(moleculeCategory);

         // Add non-mandatory metadata categories
         var molecularWeightCategory = CreateMetaDataCategory<double>(Constants.ObservedData.MOLECULAR_WEIGHT);
         molecularWeightCategory.MinValue = 0;
         molecularWeightCategory.MinValueAllowed = false;
         categories.Add(molecularWeightCategory);
         categories.Add(CreateMetaDataCategory<string>(Constants.ObservedData.STUDY_ID));
         categories.Add(CreateMetaDataCategory<string>(Constants.ObservedData.SUBJECT_ID));
         categories.Add(CreateMetaDataCategory<string>(Constants.ObservedData.GENDER, isListOfValuesFixed: true));
         categories.Add(CreateMetaDataCategory<string>(Constants.ObservedData.DOSE));
         categories.Add(CreateMetaDataCategory<string>(Constants.ObservedData.ROUTE));

         return categories;
      }

      protected static MetaDataCategory CreateMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false)
      {
         var category = new MetaDataCategory
         {
            Name = descriptiveName,
            DisplayName = descriptiveName,
            Description = descriptiveName,
            MetaDataType = typeof(T),
            IsMandatory = isMandatory,
            IsListOfValuesFixed = isListOfValuesFixed
         };

         return category;
      }

      public abstract (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, string dataFileName);
      public abstract IReadOnlyList<DataRepository> ImportFromConfiguration(ImporterConfiguration configuration, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, string dataFileName);
   }
}