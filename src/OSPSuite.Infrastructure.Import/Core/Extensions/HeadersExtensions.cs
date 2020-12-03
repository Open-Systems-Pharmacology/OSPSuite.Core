using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core.Extensions
{
   public static class HeaderExtensions
   {
      public static string FindHeader(this IEnumerable<string> me, string match)
      {
         return me.Where(h => h.ToUpper().StartsWith(match.ToUpper())).OrderBy(h => h.Length).FirstOrDefault();
      }
   }
}
