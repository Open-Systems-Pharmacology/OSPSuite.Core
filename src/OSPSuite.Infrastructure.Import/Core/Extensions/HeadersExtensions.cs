using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core.Extensions
{
   public static class HeaderExtensions
   {
      public static string FindHeader(this IEnumerable<string> me, string match)
      {
         return me.FirstOrDefault(h => h.ToUpper().StartsWith(match.ToUpper()));
      }
   }
}