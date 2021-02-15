using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using System.Collections.Generic;
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
      IEnumerable<DataRepository> ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings
      );

      IEnumerable<DataRepository> ImportFromXml(
         string configurationFilePath,
         bool promptForConfirmation,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      IEnumerable<DataRepository> ImportFromConfiguration(
         ImporterConfiguration configuration,
         bool promptForConfirmation,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );
   }
}
