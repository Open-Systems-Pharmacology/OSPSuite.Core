using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Infrastructure.Import.Services
{

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

      Cache<string, IEnumerable<DataRepository>> ReloadFromConfiguration(IEnumerable<DataRepository> dataSetsToImport,
            IEnumerable<DataRepository> existingDataSets);
            
      /// <summary>
      /// Creates a default list of meta data categories that could still be modified by the caller
      /// </summary>
      /// <returns>a list of meta data categories</returns>
      IList<MetaDataCategory> DefaultMetaDataCategories();
   }
}
