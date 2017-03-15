using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Journal
{
   public class JournalSearch
   {
      public string Search { get; set; }
      public bool MatchAny { get; set; }
      public bool MatchWholePhrase { get; set; }
      public bool MatchCase { get; set; }

      public bool MatchAll
      {
         get { return !MatchAny; }
         set { MatchAny = !value; }
      }

      public IReadOnlyList<string> SearchTerms
      {
         get
         {
            if (string.IsNullOrEmpty(Search))
               return new List<string>();

            if (MatchWholePhrase)
               return new[] {Search};


            return Search.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
         }
      }
   }
}