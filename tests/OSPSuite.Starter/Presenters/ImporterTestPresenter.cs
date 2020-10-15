using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Presenters;
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
      private readonly IImporterTiledPresenter _importer;
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;

      public ImporterTestPresenter(IImporterTestView view, IImporterTiledPresenter importer, IImporterConfigurationDataGenerator dataGenerator, IDialogCreator dialogCreator)
         : base(view)
      {
         _importer = importer;
         _dataGenerator = dataGenerator;
         _dialogCreator = dialogCreator;
      }

      private void StartImporterExcelView(IReadOnlyList<MetaDataCategory> categories, IReadOnlyList<ColumnInfo> columns, DataImporterSettings settings)
      {
         var starter = new TestStarter<IImporterTiledPresenter>();
         starter.Start(1000, 600);
         starter.Presenter.SetSettings(categories, columns, settings);
         starter.Presenter.OnTriggerImport += startImportSuccessDialog;
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

         //promptForImports(new List<DataRepository> { dataSet });
      }

      private static void promptForImports(List<DataRepository> dataSets)
      {
         if (dataSets.Any())
            XtraMessageBox.Show($"Transferred {dataSets.Count} Data Sets!");
      }

      private void startImportSuccessDialog(object sender, ImportTriggeredEventArgs importTriggeredEventArgs)
      {
         _dialogCreator.MessageBoxInfo(importTriggeredEventArgs.DataSource.DataSets.Select(d => d.Data.Count()).Sum() + " data sets successfully imported");
      }

      public void StartWithOntogenySettings()
      {
         var dataImporterSettings = new DataImporterSettings
         {
            Caption = "PK-Sim - Import Ontogeny"
         };

         StartImporterExcelView(
            new List<MetaDataCategory>(), 
            _dataGenerator.GetOntogenyColumnInfo(), 
            dataImporterSettings
         );
      }
   }
}