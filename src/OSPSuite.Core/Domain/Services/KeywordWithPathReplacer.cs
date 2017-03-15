using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///   Replaces a keyword in a PathEntries list of a objectPath. As replacement a list of strings is used. Representing a objectPath or, objectPath segment
   /// </summary>
   public class KeywordWithPathReplacer : IKeywordInObjectPathReplacer
   {
      private readonly string _keyword;
      private readonly IEnumerable<string> _replacingPath;

      /// <summary>
      ///   Initializes a new instance of the <see cref="KeywordWithPathReplacer" /> class.
      /// </summary>
      /// <param name="keyword"> The keyword to replace. </param>
      /// <param name="replacingPath"> The list of strings used to replace the keyword. </param>
      public KeywordWithPathReplacer(string keyword, IEnumerable<string> replacingPath)
      {
         _keyword = keyword;
         _replacingPath = replacingPath;
      }

      /// <summary>
      ///   Replaces the Keyword in the given <paramref name="objectPath" /> list.
      /// </summary>
      /// <param name="objectPath"> The objectPath entries where Keyword should be replaced. </param>
      public void ReplaceIn(IObjectPath objectPath)
      {
         objectPath.Replace(_keyword, _replacingPath);
      }
   }
}