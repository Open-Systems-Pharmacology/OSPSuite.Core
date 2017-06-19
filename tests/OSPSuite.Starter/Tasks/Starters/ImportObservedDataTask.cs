using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IImportObservedDataTask
   {
      DataRepository ImportObservedData();
   }


   public class ImportObservedDataTask : IImportObservedDataTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;

      public ImportObservedDataTask(IDataImporter dataImporter, IImporterConfigurationDataGenerator dataGenerator)
      {
         _dataImporter = dataImporter;
         _dataGenerator = dataGenerator;
      }

      public DataRepository ImportObservedData()
      {
         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         var dataSet = _dataImporter.ImportDataSet(
            _dataGenerator.DefaultMoBiMetaDataCategories(),
            _dataGenerator.DefaultMoBiConcentrationImportConfiguration(),
            dataImporterSettings);

         return dataSet;
      }
   }
}