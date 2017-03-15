using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   /// Replaces a keyword in a PathEntries list of a objectPath.
   /// As replacement a single string is used
   /// </summary>
   public class SimpleKeywordReplacer : IKeywordInTagsReplacer,IKeywordInObjectPathReplacer
   {
      private readonly string _keyword;
      private readonly string _replacement;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleKeywordReplacer"/> class.
      /// </summary>
      /// <param name="keyword">The keyword to replace.</param>
      /// <param name="replacement">The replacement for the keyword.</param>
      public SimpleKeywordReplacer(string keyword, string replacement)
      {
         _keyword = keyword;
         _replacement = replacement;
      }

      /// <summary>
      /// Replaces the Keyword in the given <paramref name="objectPath"/> list.
      /// </summary>
      /// <param name="objectPath">The objectPath entries.</param>
      public void ReplaceIn(IObjectPath objectPath)
      {
         objectPath.Replace(_keyword, _replacement);
      }

      public void ReplaceIn(Tags tags)
      {
         tags.Replace(_keyword, _replacement);
      }

      public void ReplaceIn(IDescriptorCondition descriptorCondition)
      {
         descriptorCondition.Replace(_keyword, _replacement);
      }
   }
}