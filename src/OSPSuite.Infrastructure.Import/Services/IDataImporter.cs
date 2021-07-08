using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using System.Collections.Generic;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class ReloadDataSets
   {
      public IEnumerable<DataRepository> NewDataSets { get; }
      public IEnumerable<DataRepository> OverwrittenDataSets{ get; }
      public IEnumerable<DataRepository> DataSetsToBeDeleted { get; }

      public ReloadDataSets()
      {
         NewDataSets = Enumerable.Empty<DataRepository>();
         OverwrittenDataSets = Enumerable.Empty<DataRepository>();
         DataSetsToBeDeleted = Enumerable.Empty<DataRepository>();
      }

      public ReloadDataSets( IEnumerable<DataRepository> newDataSets, IEnumerable<DataRepository> overwrittenDataSets, IEnumerable<DataRepository> dataSetsToBeDeleted )
      {
         NewDataSets = newDataSets;
         OverwrittenDataSets = overwrittenDataSets;
         DataSetsToBeDeleted = dataSetsToBeDeleted;
      }
   }

   public interface IDataImporter
   {
      /// <summary>
      ///    This function retrieves a list of imported DataRepositories.
      /// </summary>
      /// <param name="metaDataCategories">Specification of meta data of the table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="dataImporterSettings">Settings used to initialize the view</param>
      /// <param name="dataFileName">Path to the file containing the data</param>
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings,
         string dataFileName
      );

      /// <summary>
      ///    This function retrieves a list of imported DataRepositories.
      /// </summary>
      /// <param name="configuration">Configuration to use</param>
      /// <param name="metaDataCategories">Specification of meta data of the table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="dataImporterSettings">Settings used to initialize the view</param>
      /// <param name="dataFileName">Path to the file containing the data</param>
      IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string dataFileName
      );

      ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport,
         IReadOnlyList<DataRepository> existingDataSets);
            
      /// <summary>
      /// Creates a default list of meta data categories that could still be modified by the caller
      /// </summary>
      /// <returns>a list of meta data categories</returns>
      IList<MetaDataCategory> DefaultMetaDataCategories();

      /// <summary>
      /// Compares if two data repositories come from the same data
      /// </summary>
      /// <param name="sourceDataRepository">source DataRepository to compare with</param>
      /// <param name="targetDataRepository">target DataRepository to compare with</param>
      /// <returns></returns>
      bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository);
   }
}
