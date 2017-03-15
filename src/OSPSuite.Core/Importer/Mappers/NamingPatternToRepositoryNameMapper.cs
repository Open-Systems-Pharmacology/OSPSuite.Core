using System.Collections.Generic;
using System.Text.RegularExpressions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Importer.Mappers
{
   public interface INamingPatternToRepositoryNameMapper
   {
      /// <summary>
      /// Renames repositories according to the settings of the importer. First the <paramref name="pattern">pattern</paramref> will be used, then any default pattern defined by <paramref name="dataImporterSettings">dataImporterSettings</paramref>
      /// </summary>
      /// <param name="dataRepositoriesToImport">The list of repositories to import</param>
      /// <param name="pattern">The first pattern to use when renaming</param>
      /// <param name="dataImporterSettings">The settings to use when renaming</param>
      void RenameRepositories(IReadOnlyList<DataRepository> dataRepositoriesToImport, string pattern, DataImporterSettings dataImporterSettings);
   }

   public class NamingPatternToRepositoryNameMapper : INamingPatternToRepositoryNameMapper
   {
      private string _token;
      private string _delimiter;

      private void renameDataRepositories(IEnumerable<DataRepository> dataRepositories, string pattern, string token, string delimiter)
      {
         _token = token;
         _delimiter = delimiter;
         dataRepositories.Each(repo => renameDataRepository(repo, pattern));
      }

      private void renameDataRepository(DataRepository dataRepository, string pattern)
      {
         string[] name = { pattern };

         dataRepository.ExtendedProperties.Each(x => name[0] = replaceTokens(x, name[0]));

         name[0] = cleanUnusedTokens(name[0]);

         dataRepository.Name = name[0];
      }

      private string cleanUnusedTokens(string name)
      {
         var regex = new Regex(string.Format(_delimiter + _token, ".*"));

         var newName = regex.Replace(name, "");

         regex = new Regex(string.Format(_token + _delimiter, ".*"));

         return regex.Replace(newName, "");
      }

      private string replaceTokens(IExtendedProperty x, string name)
      {
         return name.Replace(string.Format(_token, x.Name), x.ValueAsObject.ToString());
      }

      public void RenameRepositories(IReadOnlyList<DataRepository> dataRepositoriesToImport, string pattern, DataImporterSettings dataImporterSettings)
      {
         renameDataRepositories(
            dataRepositoriesToImport,
            pattern,
            dataImporterSettings.Token,
            dataImporterSettings.Delimiter);         
      }
   }
}