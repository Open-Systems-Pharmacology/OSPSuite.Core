using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using System.Collections.Generic;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class ReloadDataSets
   {
      public IEnumerable<DataRepository> NewDataSets;
      public IEnumerable<DataRepository> OverwrittenDataSets;
      public IEnumerable<DataRepository> DataSetsToBeDeleted;

      public ReloadDataSets()
      {
         NewDataSets = Enumerable.Empty<DataRepository>();
         OverwrittenDataSets = Enumerable.Empty<DataRepository>();
         DataSetsToBeDeleted = Enumerable.Empty<DataRepository>();
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
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings
      );

      IReadOnlyList<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport,
         IReadOnlyList<DataRepository> existingDataSets);
            
      /// <summary>
      /// Creates a default list of meta data categories that could still be modified by the caller
      /// </summary>
      /// <returns>a list of meta data categories</returns>
      IList<MetaDataCategory> DefaultMetaDataCategories();
   }
}
