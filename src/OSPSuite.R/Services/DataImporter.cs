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
   public class DataImporter : AbstractDataImporter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDimension _molarConcentrationDimension;
      private readonly IDimension _massConcentrationDimension;
      private readonly IOSPSuiteLogger _logger;

      public DataImporter(IImporter importer, IOSPSuiteLogger logger, IDimensionFactory dimensionFactory) : base(importer, dimensionFactory)
      {
         _logger = logger;
         _dimensionFactory = dimensionFactory;
         _molarConcentrationDimension = _dimensionFactory.Dimension("Concentration (molar)");
         _massConcentrationDimension = _dimensionFactory.Dimension("Concentration (mass)");
      }

      public override bool AreFromSameMetaDataCombination(
         DataRepository sourceDataRepository, 
         DataRepository targetDataRepository)
      {
         throw new NotImplementedException();
      }

      public override ReloadDataSets CalculateReloadDataSetsFromConfiguration(
         IReadOnlyList<DataRepository> dataSetsToImport, 
         IReadOnlyList<DataRepository> existingDataSets)
      {
         throw new NotImplementedException();
      }

 

      public override (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings,
         string dataFileName)
      {
         throw new NotImplementedException();
      }

      public override IReadOnlyList<DataRepository> ImportFromConfiguration(
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
   }
}
