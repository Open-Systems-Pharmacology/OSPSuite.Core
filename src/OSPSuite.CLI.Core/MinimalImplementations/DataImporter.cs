using OSPSuite.Assets;
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

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class DataImporter : AbstractDataImporter
   {
      private readonly IOSPSuiteLogger _logger;

      public DataImporter(IImporter importer, IOSPSuiteLogger logger, IDimensionFactory dimensionFactory) : base(importer, dimensionFactory)
      {
         _logger = logger;
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

         var columnInfoCache = new ColumnInfoCache(columnInfos);
         var importedData = _importer.ImportFromConfiguration(configuration, columnInfoCache, dataFileName, metaDataCategories, dataImporterSettings);
         if (importedData.MissingSheets.Count != 0)
            _logger.AddWarning(Captions.Importer.SheetsNotFound(importedData.MissingSheets));
         return importedData.DataRepositories.Select(drm => drm.DataRepository).ToList();
      }
   }
}