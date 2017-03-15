using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Importer
{
   public enum Mode
   {
      MultipleRepositories,
      SingleRepository
   }

   public interface IDataImporter
   {
      /// <summary>
      ///    This function retrieves a list of imported DataRepositories.
      /// </summary>
      /// <param name="metaDataCategories">Specification of meta data of the table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="dataImporterSettings">Settings used to initialize the view</param>
      IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings);
      
      /// <summary>
      ///   This function retrieves only one DataRepository.
      /// </summary>
      DataRepository ImportDataSet(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings);

   }
}