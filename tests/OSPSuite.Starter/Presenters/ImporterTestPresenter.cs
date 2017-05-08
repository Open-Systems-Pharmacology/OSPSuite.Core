using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks;
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
      private readonly IDataImporter _importer;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;

      public ImporterTestPresenter(IImporterTestView view, IDataImporter importer, IImporterConfigurationDataGenerator dataGenerator)
         : base(view)
      {
         _importer = importer;
         _dataGenerator = dataGenerator;
      }

      public void StartWithTestForGroupBySettings()
      {
         promptForImports(_importer.ImportDataSets(
            _dataGenerator.DefaultGroupByTestMetaDataCategories(),
            _dataGenerator.DefaultGroupByConcentrationImportConfiguration(),
            new DataImporterSettings()).ToList());
      }

      public void StartWithTestSettings()
      {
         promptForImports(_importer.ImportDataSets(
            _dataGenerator.DefaultTestMetaDataCategories(),
            _dataGenerator.DefaultTestConcentrationImportConfiguration(),
            new DataImporterSettings()).ToList());
      }

      public void StartWithMoBiSettings()
      {
         var dataImporterSettings = new DataImporterSettings();

         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");

         var dataSets = _importer.ImportDataSets(
            _dataGenerator.DefaultMoBiMetaDataCategories(),
            _dataGenerator.DefaultMoBiConcentrationImportConfiguration(),
            dataImporterSettings).ToList();

         promptForImports(dataSets);
      }

      public void StartWithPKSimSettings()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         var dataSets = _importer.ImportDataSets(
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
            dataImporterSettings).ToList();

         promptForImports(dataSets);
      }

      public void StartPKSimSingleMode()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         var dataSet = _importer.ImportDataSet(
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
            dataImporterSettings);

         promptForImports(new List<DataRepository> { dataSet });
      }

      private static void promptForImports(List<DataRepository> dataSets)
      {
         if (dataSets.Any())
            XtraMessageBox.Show($"Transferred {dataSets.Count} Data Sets!");
      }


      public void StartWithOntogenySettings()
      {
         var dataImporterSettings = new DataImporterSettings
         {
            Caption = "PK-Sim - Import Ontogeny"
         };

         var dataSets = _importer.ImportDataSets(new List<MetaDataCategory>(), _dataGenerator.GetOntogenyColumnInfo(), dataImporterSettings);
         promptForImports(dataSets.ToList());
      }
   }
}