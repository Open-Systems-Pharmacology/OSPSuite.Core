using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;

namespace OSPSuite.Presentation.Services
{
   public interface IRepositoryNamingTask
   {
      /// <summary>
      /// Creates a new list of naming patterns from the given <paramref name="metaDataCategories"></paramref> and <paramref name="dataImporterSettings"></paramref>
      /// </summary>
      /// <param name="metaDataCategories">The meta data categories used for template generation</param>
      /// <param name="dataImporterSettings">The data importer settings used for template generation</param>
      /// <returns>A list of naming patterns</returns>
      IEnumerable<string> CreateNamingPatternsBasedOn(IReadOnlyList<MetaDataCategory> metaDataCategories, DataImporterSettings dataImporterSettings);
   }

   public class RepositoryNamingTask : IRepositoryNamingTask
   {
      private readonly IMetaDataCategoryToNamingPatternMapper _namePatternMapper;

      public RepositoryNamingTask(IMetaDataCategoryToNamingPatternMapper namePatternMapper)
      {
         _namePatternMapper = namePatternMapper;
      }

      public IEnumerable<string> CreateNamingPatternsBasedOn(IReadOnlyList<MetaDataCategory> metaDataCategories, DataImporterSettings dataImporterSettings)
      {
         //The first item should be the default
         foreach (var convention in dataImporterSettings.NamingConventions.Where(convention => !string.IsNullOrEmpty(convention)))
         {
            yield return convention;
         }

         var allMetaData = _namePatternMapper.AllMetaData(metaDataCategories, dataImporterSettings.Token, dataImporterSettings.Delimiter);
         if (!string.IsNullOrEmpty(allMetaData))
            yield return allMetaData;

         var requiredMetaData = _namePatternMapper.RequiredMetaData(metaDataCategories, dataImporterSettings.Token, dataImporterSettings.Delimiter);
         if (!string.IsNullOrEmpty(requiredMetaData))
            yield return requiredMetaData;

         yield return $"{string.Format(dataImporterSettings.Token, Constants.FILE)}{dataImporterSettings.Delimiter}{string.Format(dataImporterSettings.Token, Constants.SHEET)}";
      }
   }
}
