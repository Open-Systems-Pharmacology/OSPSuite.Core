using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Importer;
using static OSPSuite.Assets.Captions.Importer;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporter _importer;
      private readonly IApplicationController _applicationController;

      public DataImporter(
         IDialogCreator dialogCreator,
         IImporter importer,
         IApplicationController applicationController
      )
      {
         _dialogCreator = dialogCreator;
         _importer = importer;
         _applicationController = applicationController;
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
         var molecularWeightCategory = createMetaDataCategory<double>(Constants.ObservedData.MOLECULARWEIGHT);
         molecularWeightCategory.MinValue = 0;
         molecularWeightCategory.MinValueAllowed = false;
         categories.Add(molecularWeightCategory);
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.STUDY_ID));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.GENDER, isListOfValuesFixed: true));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.DOSE));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.ROUTE));
         categories.Add(createMetaDataCategory<string>(Constants.ObservedData.GROUP_ID));

         return categories;
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

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         var path = _dialogCreator.AskForFileToOpen(PleaseSelectDataFile, ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);

         if (string.IsNullOrEmpty(path))
            return (Array.Empty<DataRepository>(), null);

         using (var importerModalPresenter = _applicationController.Start<IModalImporterPresenter>())
         {
            return importerModalPresenter.ImportDataSets(metaDataCategories, columnInfos, dataImporterSettings, path);
         }
      }

      public IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         var fileName = _dialogCreator.AskForFileToOpen(OpenFile, ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);
         
         if (string.IsNullOrEmpty(fileName))
            return Enumerable.Empty<DataRepository>().ToList();
         
         if (dataImporterSettings.PromptForConfirmation)
         {
            using (var importerModalPresenter = _applicationController.Start<IModalImporterPresenter>())
            {
               return importerModalPresenter.ImportDataSets(metaDataCategories, columnInfos, dataImporterSettings, fileName, configuration);
            }
         }

         try
         {
            var importedData = _importer.ImportFromConfiguration(configuration, columnInfos, fileName, metaDataCategories, dataImporterSettings);
            if (importedData.MissingSheets.Count != 0)
               _dialogCreator.MessageBoxError(SheetsNotFound(importedData.MissingSheets));
            return importedData.DataRepositories.Select(drm => drm.DataRepository).ToList();
         }
         catch (Exception e) when (e is UnsupportedFormatException || e is UnsupportedFileTypeException)
         {
            _dialogCreator.MessageBoxError(e.Message);
            return new List<DataRepository>();
         }
      }

      public ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport,
         IReadOnlyList<DataRepository> existingDataSets)
      {
         var newDataSets = dataSetsToImport.Where(dataSet => !repositoryExistsInList(existingDataSets, dataSet));
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

      public bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository)
      {
         return targetDataRepository.ExtendedProperties.KeyValues.All(keyValuePair =>
            keyValuePair.Key == Constants.FILE || //Ignore source file
            Equals(sourceDataRepository.ExtendedProperties[keyValuePair.Key].ValueAsObject, keyValuePair.Value.ValueAsObject)
         );
      }

      private bool repositoryExistsInList(IEnumerable<DataRepository> dataRepositoryList, DataRepository targetDataRepository)
      {
         return dataRepositoryList.Any(dataRepo => AreFromSameMetaDataCombination(dataRepo, targetDataRepository));
      }
   }
}