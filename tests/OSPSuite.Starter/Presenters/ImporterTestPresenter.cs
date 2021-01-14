using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;

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
   }

   public class ImporterTestPresenter : AbstractPresenter<IImporterTestView, IImporterTestPresenter>, IImporterTestPresenter
   {
      private readonly IImporterPresenter _importer;
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;
      private readonly IDataImporter _dataImporter;

      public ImporterTestPresenter(IImporterTestView view, IImporterPresenter importer, IImporterConfigurationDataGenerator dataGenerator, IDialogCreator dialogCreator, IDataImporter dataImporter)
         : base(view)
      {
         _importer = importer;
         _dataGenerator = dataGenerator;
         _dialogCreator = dialogCreator;
         _dataImporter = dataImporter;
      }

      private void StartImporterExcelView(IReadOnlyList<MetaDataCategory> categories, IReadOnlyList<ColumnInfo> columns, DataImporterSettings settings)
      {
         _dialogCreator.MessageBoxInfo(_dataImporter.ImportDataSets
         (
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
            settings
         ).Count() + " data sets successfully imported");
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

         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");

         StartImporterExcelView(
            _dataGenerator.DefaultMoBiMetaDataCategories(),
            _dataGenerator.DefaultMoBiConcentrationImportConfiguration(),
            dataImporterSettings
         );

         //promptForImports(dataSets);
      }

      public void StartWithPKSimSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         StartImporterExcelView
         (
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
            dataImporterSettings
         );
      }

      public void StartPKSimSingleMode()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         StartImporterExcelView(
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
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