using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Starter.Presenters
{
   public interface IImporterTestPresenter : IPresenter<IImporterTestView>
   {
      void StartWithTestForGroupBySettings();
      void StartWithTestSettings();
      void StartWithPKSimSettings();
      void StartPKSimSingleMode();
      void StartWithOntogenySettings();
      void StartWithMoBiSettings();
      void ReloadWithPKSimSettings();
      void LoadWithPKSimSettings();
   }

   public class ImporterTestPresenter : AbstractPresenter<IImporterTestView, IImporterTestPresenter>, IImporterTestPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;
      private readonly IDataImporter _dataImporter;
      private readonly IContainer _container;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      public ImporterTestPresenter(IImporterTestView view, IImporterConfigurationDataGenerator dataGenerator, IDialogCreator dialogCreator, IDataImporter dataImporter,
         IContainer container, IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository, IDimensionFactory dimensionFactory)
         : base(view)
      {
         _dataGenerator = dataGenerator;
         _dialogCreator = dialogCreator;
         _dataImporter = dataImporter;
         _container = container;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
         if(!dimensionFactory.Has("Input Dose"))
            dimensionFactory.AddDimension(new BaseDimensionRepresentation(), "Input Dose", "mg");
      }

      private void StartImporterExcelView(IReadOnlyList<MetaDataCategory> categories, IReadOnlyList<ColumnInfo> columns, DataImporterSettings settings)
      {
         settings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         settings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
         _dialogCreator.MessageBoxInfo(_dataImporter.ImportDataSets
         (
            categories,
            columns,
            settings,
            _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA)
         ).DataRepositories?.Count() + " data sets successfully imported");
      }

      public void StartWithTestForGroupBySettings()
      {
         StartImporterExcelView
         (
            _dataGenerator.DefaultGroupByTestMetaDataCategories(), 
            _dataGenerator.DefaultGroupByConcentrationImportConfiguration(),
            new DataImporterSettings()
         );
      }

      public void StartWithTestSettings()
      {
         StartImporterExcelView(
            _dataGenerator.DefaultTestMetaDataCategories(),
            _dataGenerator.DefaultTestConcentrationImportConfiguration(),
            new DataImporterSettings()
         );
      }

      public void StartWithMoBiSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE);

         StartImporterExcelView(
            _dataGenerator.DefaultMoBiMetaDataCategories(),
            _dataImporter.ColumnInfosForObservedData(),
            dataImporterSettings
         );

         //promptForImports(dataSets);
      }

      public void ReloadWithPKSimSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();

            var fileName = _dialogCreator.AskForFileToOpen("Save configuration", "xml files (*.xml)|*.xml|All files (*.*)|*.*",
               Constants.DirectoryKey.PROJECT);

            if (fileName.IsNullOrEmpty()) return;

            var xel = XElementSerializer.PermissiveLoad(fileName); // We have to correctly handle the case of cancellation
            var configuration = serializer.Deserialize<ImporterConfiguration>(xel, serializationContext);

            dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
            dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";

            _dialogCreator.MessageBoxInfo(_dataImporter.ImportFromConfiguration
            (
               configuration,
               _dataImporter.DefaultMetaDataCategoriesForObservedData(),
               _dataImporter.ColumnInfosForObservedData(),
               dataImporterSettings,
               _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA)
            )?.Count() + " data sets successfully imported");
         }
      }

      public void LoadWithPKSimSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");

         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();

            var fileName = _dialogCreator.AskForFileToOpen("Load configuration", "xml files (*.xml)|*.xml|All files (*.*)|*.*",
               Constants.DirectoryKey.PROJECT);

            if (fileName.IsNullOrEmpty()) return;

            var xel = XElementSerializer.PermissiveLoad(fileName); // We have to correctly handle the case of cancellation
            var configuration = serializer.Deserialize<ImporterConfiguration>(xel, serializationContext);

            dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
            dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
            dataImporterSettings.PromptForConfirmation = true;

            _dialogCreator.MessageBoxInfo(_dataImporter.ImportFromConfiguration
            (
               configuration,
               (IReadOnlyList<MetaDataCategory>) _dataImporter.DefaultMetaDataCategoriesForObservedData(),
               _dataImporter.ColumnInfosForObservedData(),
               dataImporterSettings,
               _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA)
            )?.Count() + " data sets successfully imported");
         }
      }

      public void StartWithPKSimSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         var metaDataCategories = _dataImporter.DefaultMetaDataCategoriesForObservedData().ToList();
         _dataGenerator.AddMoleculeValuesToMetaDataList(metaDataCategories);
         _dataGenerator.AddOrganValuesToMetaDataList(metaDataCategories);
         StartImporterExcelView
         (
            metaDataCategories,
            _dataImporter.ColumnInfosForObservedData(),
            dataImporterSettings
         );
      }

      public void StartPKSimSingleMode()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         var metaDataCategories = _dataImporter.DefaultMetaDataCategoriesForObservedData().ToList();
         _dataGenerator.AddMoleculeValuesToMetaDataList(metaDataCategories);
         StartImporterExcelView(
            metaDataCategories,
            _dataImporter.ColumnInfosForObservedData(),
            dataImporterSettings
         );
      }

      public void StartWithOntogenySettings()
      {
         var dataImporterSettings = new DataImporterSettings
         {
            Caption = "PK-Sim - LoadCurrentSheet Ontogeny"
         };

         StartImporterExcelView(
            new List<MetaDataCategory>(), 
            _dataGenerator.GetOntogenyColumnInfo(), 
            dataImporterSettings
         );
      }
   }
}