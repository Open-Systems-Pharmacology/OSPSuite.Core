using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.R.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporter _importer;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDimension _molarConcentrationDimension;
      private readonly IDimension _massConcentrationDimension;
      private readonly IOSPSuiteLogger _logger;

      public DataImporter(IDialogCreator dialogCreator, IImporter importer, IOSPSuiteLogger logger, IDimensionFactory dimensionFactory)
      {
         _logger = logger;
         _dialogCreator = dialogCreator;
         _importer = importer;
         _dimensionFactory = dimensionFactory;
         _molarConcentrationDimension = _dimensionFactory.Dimension("Concentration (molar)");
         _massConcentrationDimension = _dimensionFactory.Dimension("Concentration (mass)");
      }

      public bool AreFromSameMetaDataCombination(
         DataRepository sourceDataRepository, 
         DataRepository targetDataRepository)
      {
         throw new NotImplementedException();
      }

      public ReloadDataSets CalculateReloadDataSetsFromConfiguration(
         IReadOnlyList<DataRepository> dataSetsToImport, 
         IReadOnlyList<DataRepository> existingDataSets)
      {
         throw new NotImplementedException();
      }

      private static MetaDataCategory createMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false)
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

      public IList<MetaDataCategory> DefaultMetaDataCategories()
      {
         var categories = new List<MetaDataCategory>();

         var speciesCategory = createMetaDataCategory<string>(Constants.ObservedData.SPECIES, isMandatory: true, isListOfValuesFixed: true);
         categories.Add(speciesCategory);

         var organCategory = createMetaDataCategory<string>(Constants.ObservedData.ORGAN, isMandatory: true, isListOfValuesFixed: true);
         organCategory.Description = ObservedData.ObservedDataOrganDescription;
         organCategory.TopNames.Add(Constants.ObservedData.PERIPHERAL_VENOUS_BLOOD_ORGAN);
         organCategory.TopNames.Add(Constants.ObservedData.VENOUS_BLOOD_ORGAN);
         categories.Add(organCategory);

         var compCategory = createMetaDataCategory<string>(Constants.ObservedData.COMPARTMENT, isMandatory: true, isListOfValuesFixed: true);
         compCategory.Description = ObservedData.ObservedDataCompartmentDescription;
         compCategory.TopNames.Add(Constants.ObservedData.PLASMA_COMPARTMENT);
         categories.Add(compCategory);

         var moleculeCategory = createMetaDataCategory<string>(Constants.ObservedData.MOLECULE);
         moleculeCategory.Description = ObservedData.MoleculeNameDescription;
         moleculeCategory.AllowsManualInput = true;
         categories.Add(moleculeCategory);

         // Add non-mandatory metadata categories
         var molecularWeightCategory = createMetaDataCategory<double>(Constants.ObservedData.MOLECULAR_WEIGHT);
         molecularWeightCategory.MinValue = 0;
         molecularWeightCategory.MinValueAllowed = false;
         categories.Add(molecularWeightCategory);
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.STUDY_ID));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.SUBJECT_ID));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.GENDER, isListOfValuesFixed: true));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.DOSE));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.ROUTE));

         return categories;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings,
         string dataFileName)
      {
         throw new NotImplementedException();
      }

      public IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName)
      {
         if (string.IsNullOrEmpty(dataFileName) || !File.Exists(dataFileName))
            throw new OSPSuiteException(Error.InvalidFile);


         var importedData = _importer.ImportFromConfiguration(configuration, columnInfos, dataFileName, metaDataCategories, dataImporterSettings);
         if (importedData.MissingSheets.Count != 0)
            _logger.AddWarning(Captions.Importer.SheetsNotFound(importedData.MissingSheets));
         return importedData.DataRepositories.Select(drm => drm.DataRepository).ToList();
      }

      public IReadOnlyList<ColumnInfo> DefaultPKSimImportConfiguration()
      {
         var columns = new List<ColumnInfo>();
         var timeColumn = createTimeColumn();

         columns.Add(timeColumn);

         var concentrationInfo = createConcentrationColumn(timeColumn);

         columns.Add(concentrationInfo);

         var errorInfo = createErrorColumn(timeColumn, concentrationInfo);

         columns.Add(errorInfo);

         return columns;
      }

      private ColumnInfo createTimeColumn()
      {
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME),
            Name = Constants.TIME,
            DisplayName = Constants.TIME,
            IsMandatory = true,
         };

         timeColumn.SupportedDimensions.Add(_dimensionFactory.Dimension(Constants.Dimension.TIME));
         return timeColumn;
      }

      private ColumnInfo createConcentrationColumn(ColumnInfo timeColumn)
      {
         var concentrationInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = Constants.MEASUREMENT,
            DisplayName = Constants.MEASUREMENT,
            IsMandatory = true,
            BaseGridName = timeColumn.Name
         };

         concentrationInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         concentrationInfo.SupportedDimensions.Add(_massConcentrationDimension);
         return concentrationInfo;
      }

      private ColumnInfo createErrorColumn(ColumnInfo timeColumn, ColumnInfo concentrationInfo)
      {
         var errorInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = Constants.ERROR,
            DisplayName = Constants.ERROR,
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = concentrationInfo.Name
         };

         errorInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_massConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_dimensionFactory.NoDimension);
         return errorInfo;
      }

      public ImporterConfiguration ConfigurationFromData(string dataPath, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         var configuration = new ImporterConfiguration();
         configuration.CloneParametersFrom(_importer.LoadFile(columnInfos, dataPath, metaDataCategories).Format.Parameters.ToList());
         configuration.FileName = dataPath;
         configuration.Id = Guid.NewGuid().ToString();
         configuration.NamingConventions = "";
         return configuration;
      }
   }
}
