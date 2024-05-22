using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Utility.Collections;
using static OSPSuite.Assets.Captions.Importer;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Services
{
   public class DataImporter : AbstractDataImporter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IApplicationController _applicationController;

      public DataImporter(
         IDialogCreator dialogCreator,
         IImporter importer,
         IApplicationController applicationController,
         IDimensionFactory dimensionFactory
      ) : base(importer, dimensionFactory)
      {
         _dialogCreator = dialogCreator;
         _applicationController = applicationController;
      }


      public override (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName
      )
      {
         if (string.IsNullOrEmpty(dataFileName) || !System.IO.File.Exists(dataFileName))
            return (Array.Empty<DataRepository>(), null);

         var columnInfoCache = new ColumnInfoCache(columnInfos);
         using (var importerModalPresenter = _applicationController.Start<IModalImporterPresenter>())
         {
            importerModalPresenter.SetCaption(dataImporterSettings.Caption);
            return importerModalPresenter.ImportDataSets(metaDataCategories, columnInfoCache, dataImporterSettings, dataFileName);
         }
      }

      public override IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName
      )
      {
         var columnInfoCache = new ColumnInfoCache(columnInfos);
         if (string.IsNullOrEmpty(dataFileName) || !System.IO.File.Exists(dataFileName))
            return Enumerable.Empty<DataRepository>().ToList();
         
         if (dataImporterSettings.PromptForConfirmation)
         {
            using (var importerModalPresenter = _applicationController.Start<IModalImporterPresenter>())
            {
               return importerModalPresenter.ImportDataSets(metaDataCategories, columnInfoCache, dataImporterSettings, dataFileName, configuration);
            }
         }

         try
         {
            var importedData = _importer.ImportFromConfiguration(configuration, columnInfoCache, dataFileName, metaDataCategories, dataImporterSettings);
            if (importedData.MissingSheets.Count != 0)
               _dialogCreator.MessageBoxError(SheetsNotFound(importedData.MissingSheets));
            return importedData.DataRepositories.Select(drm => drm.DataRepository).ToList();
         }
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
            return new List<DataRepository>();
         }
      }

      public override ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport,
         IReadOnlyList<DataRepository> existingDataSets)
      {
         var newDataSets = dataSetsToImport.Where(dataSet => !repositoryExistsInList(existingDataSets, dataSet)).ToList();
         var dataSetsToBeDeleted = existingDataSets.Where(dataSet => !repositoryExistsInList(dataSetsToImport, dataSet));
         var overwrittenDataSets = dataSetsToImport.Except(newDataSets);


         var result = new ReloadDataSets(newDataSets, overwrittenDataSets, dataSetsToBeDeleted);

         using (var reloadPresenter = _applicationController.Start<IImporterReloadPresenter>())
         {
            reloadPresenter.AddDeletedDataSets(result.DataSetsToBeDeleted.AllNames());
            reloadPresenter.AddNewDataSets(result.NewDataSets.AllNames());
            reloadPresenter.AddOverwrittenDataSets(result.OverwrittenDataSets.AllNames());
            reloadPresenter.Show();

            if (reloadPresenter.Canceled())
               return new ReloadDataSets();
         }

         return result;
      }

      public override bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository)
      {
         return compareMetaData(sourceDataRepository, targetDataRepository) && compareMetaData(targetDataRepository, sourceDataRepository);
      }

      private static bool compareMetaData(DataRepository firstRepository, DataRepository secondRepository)
      {
         // do not compare the file name when checking for equivalent metadata
         return secondRepository.ExtendedProperties.KeyValues.Where(x => !Equals(x.Key, Constants.FILE)).All(keyValuePair =>
            hasEquivalentMetaData(firstRepository.ExtendedProperties, keyValuePair)
         );
      }

      private static bool hasEquivalentMetaData(ExtendedProperties sourceExtendedProperties, KeyValuePair<string, IExtendedProperty> keyValuePair)
      {
         return sourceExtendedProperties.Contains(keyValuePair.Key) && Equals(sourceExtendedProperties[keyValuePair.Key].ValueAsObject, keyValuePair.Value.ValueAsObject);
      }

      private bool repositoryExistsInList(IEnumerable<DataRepository> dataRepositoryList, DataRepository targetDataRepository)
      {
         return dataRepositoryList.Any(dataRepo => AreFromSameMetaDataCombination(dataRepo, targetDataRepository));
      }
   }
}