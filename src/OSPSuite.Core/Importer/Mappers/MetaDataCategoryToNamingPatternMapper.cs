using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Importer.Mappers
{
   public interface IMetaDataCategoryToNamingPatternMapper
   {
      /// <summary>
      /// Gets a pattern based on only the required metadata from the supplied categories
      /// </summary>
      /// <param name="metaDataCategories">This is the list of categories</param>
      /// <param name="token">This is a pattern for the token being substituted. For example {{{0}}} with a Species catgory results in {Species}</param>
      /// <param name="delimiter">This delimits one item in the pattern from the next. The destinction between delimiter and token is that the final delimiter is stripped
      /// from the pattern on return</param>
      /// <returns>The pattern for only required metadata</returns>
      string RequiredMetaData(IEnumerable<MetaDataCategory> metaDataCategories, string token, string delimiter);

      /// <summary>
      /// Gets a pattern based on all the metadata from the supplied categories
      /// </summary>
      /// <param name="metaDataCategories">This is the list of categories</param>
      /// <param name="token">This is a pattern for the token being substituted. For example {{{0}}} with a Species catgory results in {Species}</param>
      /// <param name="delimiter">This delimits one item in the pattern from the next. The destinction between delimiter and token is that the final delimiter is stripped
      /// from the pattern on return</param>
      /// <returns>The pattern for all metadata</returns>
      string AllMetaData(IEnumerable<MetaDataCategory> metaDataCategories, string token, string delimiter);
   }

   public class MetaDataCategoryToNamingPatternMapper : IMetaDataCategoryToNamingPatternMapper
   {
      public string RequiredMetaData(IEnumerable<MetaDataCategory> metaDataCategories, string token, string delimiter)
      {
         return formatPatterns(metaDataCategories.Where(category => category.IsMandatory), token, delimiter);
      }

      public string AllMetaData(IEnumerable<MetaDataCategory> metaDataCategories, string token, string delimiter)
      {
         return formatPatterns(metaDataCategories, token, delimiter);
      }

      private static string formatPatterns(IEnumerable<MetaDataCategory> categories, string token, string delimiter)
      {
         var sb = new StringBuilder();

         categories.Each(x => formatPatternItem(sb, x, token+delimiter));
         return sb.ToString().TrimEnd('.');
      }

      private static StringBuilder formatPatternItem(StringBuilder sb, MetaDataCategory x, string token)
      {
         return sb.Append(string.Format(token, x.Name));
      }
   }
}